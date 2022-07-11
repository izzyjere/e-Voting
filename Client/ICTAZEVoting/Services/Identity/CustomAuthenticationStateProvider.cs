
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using System.Linq;
using ICTAZEVoting.Extensions;
using ICTAZEVoting.Shared.Responses.Identity;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace ICTAZEVoting.Services.Identity
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        HttpClient Client;
        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {

            Client = httpClient;
        }

        public async Task StateChangedAsync()
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());

            NotifyAuthenticationStateChanged(authState);

        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
        {
            var state = await GetAuthenticationStateAsync();
            var authenticationStateProviderUser = state.User;
            return authenticationStateProviderUser;
        }

        public ClaimsPrincipal AuthenticationStateUser { get; set; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await SessionStorage.ReadEncryptedItemAsync<TokenResponse>("UserToken");
            if (savedToken == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken.Token);
            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetAllClaims(savedToken.Token), "jwt")));
            AuthenticationStateUser = state.User;
            return state;
        }

        IEnumerable<Claim> GetAllClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            return securityToken.Claims.ToList();

        }
        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles != null)
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                        claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }
            }

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string payload)
        {
            payload = payload.Trim().Replace('-', '+').Replace('_', '/');
            var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            return Convert.FromBase64String(base64);
        }
    }
}


