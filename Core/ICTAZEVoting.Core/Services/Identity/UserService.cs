
using AutoMapper;

using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

using System.Text;
using System.Text.Encodings.Web;

namespace ICTAZEVoting.Core.Services.Identity
{
    public class UserService : IUserService
    {
        readonly UserManager<User> _userManager;
        readonly RoleManager<Role> _roleManager;
        readonly IMapper _mapper;
        readonly ICurrentUserService _currentUserService;

        public UserService(UserManager<User> userManager,
                           RoleManager<Role> roleManager
                          , ICurrentUserService currentUserService
                          , IMapper mapper
                           )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _mapper = mapper;

        }

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            var users = new List<User> { };
            users = await _userManager.Users.Include(u => u.Roles).ThenInclude(ur => ur.Role).ToListAsync();

            var result = _mapper.Map<List<UserResponse>>(users);
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }
        public async Task<IResult<UserResponse>> GetByIdAsync(int userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            return await Result<UserResponse>.SuccessAsync(result);
        }
        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                return await Result.FailAsync("User Not Found.");
            }
            var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
            if (isAdmin)
            {
                return await Result.FailAsync("Administrators Profile's Status cannot be toggled");
            }


            user.IsActive = request.ActivateUser;
            var identityResult = await _userManager.UpdateAsync(user);

            return await Result.SuccessAsync();
        }
        public async Task<IResult<Guid>> RegisterAsync(RegisterRequest request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);


            if (userWithSameUserName != null)
            {
                return await Result<Guid>.FailAsync($"Username {request.UserName} is already taken");
            }
            var user = new User()
            {

                Email = request.Email,                  
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoConfirmEmail
                
            };
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    return await Result<Guid>.FailAsync(string.Format("Phone number {0} is already registered.", request.PhoneNumber));
                }
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(request.Role);
                    if (role == null)
                    {
                        role = new Role(request.Role, request.Role);

                        await _roleManager.CreateAsync(role);
                    }
                    if (!await _userManager.IsInRoleAsync(user, request.Role))
                    {
                        await _userManager.AddToRoleAsync(user, request.Role);
                    }

                    if (!request.AutoConfirmEmail)
                    {
                        ///Code For mail verification here
                    }
                    return await Result<Guid>.SuccessAsync(user.PersonId, string.Format("User {0} Registered.", user.UserName));

                }
                else
                {
                    return await Result<Guid>.FailAsync(result.Errors.Select(a => a.Description.ToString()).ToList());
                }
            }
            else
            {
                return await Result<Guid>.FailAsync($"Email address {request.Email} is already taken");
            }


        }
        public async Task<IResult<UserRolesResponse>> GetRolesAsync(int userId)
        {
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleName = role.Name,
                    RoleDescription = role.Description
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var result = new UserRolesResponse { UserRoles = viewModel };
            return await Result<UserRolesResponse>.SuccessAsync(result);
        }
       
        /// <summary>
        ///  Will probably use it if there would be need to confirm emails.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="EmailConfirmationException"></exception>
        public async Task<IResult<int>> ConfirmEmailAsync(int userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Result<int>.SuccessAsync(user.Id, string.Format("Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT.", user.Email));
            }
            else
            {
                throw new Exception(string.Format("An error occurred while confirming {0}", user.Email));
            }
        }
        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return await Result.FailAsync("An Error has occurred!");
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new MailRequest
            {
                Body = string.Format("Please reset your password by <a href='{0}>clicking here</a>.", HtmlEncoder.Default.Encode(passwordResetURL)),
                Subject = "Reset Password",
                To = request.Email
            };
            //BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
            return await Result.SuccessAsync("Password Reset Mail has been sent to your authorized Email.");
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync("An Error has occured!");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync("Password Reset Successful!");
            }
            else
            {
                return await Result.FailAsync("An Error has occured!");
            }
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _userManager.Users.CountAsync();
            return count;
        }

        public async Task<bool> Delete(Guid guid)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PersonId== guid);
            if (user == null)
            { return false; }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public class UpdateUserRolesRequest
        {
        }
    }
}
