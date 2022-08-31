using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

using ICTAZEVoting.Core.Middleware;
using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ICTAZEVoting.Core.Services.Identity;

public class TokenService : ITokenService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly AppConfiguration appConfig;
    private readonly SignInManager<User> signInManager;
    private readonly ILogger<TokenService> logger;
    public TokenService(UserManager<User> _userManager,
                           RoleManager<Role> _roleManager,
                           IOptions<AppConfiguration> appConfigurationOptions,
                           SignInManager<User> _signInManager, ILogger<TokenService> _logger)

    {
        logger = _logger;
        userManager = _userManager;
        roleManager = _roleManager;
        appConfig = appConfigurationOptions.Value;
        signInManager = _signInManager;
    }

    public async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
    {
        if (model is null)
        {
            return await Result<TokenResponse>.FailAsync("Invalid Client Token");
        }
        var claimsPrincipal = GetPrincipalFromExpiredToken(model.Token);
        var userEmail = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        var user = await userManager.FindByIdAsync(userEmail);
        if (user == null)
        {
            return await Result<TokenResponse>.FailAsync("User not found.");
        }
        if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return await Result<TokenResponse>.FailAsync("Invalid Client Token.");
        }
        var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
        user.RefreshToken = GenerateRefreshToken();
        await userManager.UpdateAsync(user);
        var response = new TokenResponse() { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
        return await Result<TokenResponse>.SuccessAsync(response);
    }
    private async Task<string> GenerateJwtAsync(User user)
    {
        var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
        return token;
    }
    private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));
            var thisRole = await roleManager.FindByNameAsync(role);
            var allPermissionsForThisRoles = await roleManager.GetClaimsAsync(thisRole);
            permissionClaims.AddRange(allPermissionsForThisRoles);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
    }

    public async Task<IResult<TokenResponse>> LoginAsync(TokenRequest tokenRequest)
    {
        try
        {
            var user = await userManager.FindByNameAsync(tokenRequest.UserName);
            if (user == null)
            {
                return await Result<TokenResponse>.FailAsync("User Not Found.");
            }
            if (!user.IsActive)
            {
                return await Result<TokenResponse>.FailAsync("User Not Active. Please contact the administrator.");
            }
            if (!user.EmailConfirmed)
            {
                return await Result<TokenResponse>.FailAsync("Email not confirmed");
            }
            if (await signInManager.CanSignInAsync(user))
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, tokenRequest.Password, true);
                if (result.Succeeded)
                {
                    var key = SignInMiddleware<User>.AnnounceLogin(tokenRequest);
                    user.RefreshToken = GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                    await userManager.UpdateAsync(user);
                    var token = await GenerateJwtAsync(user);
                    var response = new TokenResponse { Token = token, TokenKey = key, RefreshToken = user.RefreshToken };
                    logger.LogInformation("User Logged in..");
                    return await Result<TokenResponse>.SuccessAsync(response, $"Logged in as {user.UserName}.");
                }
                else
                {
                    return await Result<TokenResponse>.FailAsync("Incorrect Credentials.");
                }
            }
            else
            {
                return await Result<TokenResponse>.FailAsync("Your Account Is Blocked. Please contact your group admin");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e.Message + e.StackTrace);
            return await Result<TokenResponse>.FailAsync("An Error Occured. Please Try Again");
        }




    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(appConfig.Secret);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddDays(2),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

}


