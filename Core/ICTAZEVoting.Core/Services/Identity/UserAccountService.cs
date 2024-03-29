﻿using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;

using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;

namespace ICTAZEVoting.Core.Services.Identity
{
    public class UserAccountService : IUserAccountService
    {
        readonly UserManager<User> userManager;       
    
        public UserAccountService(UserManager<User> _userManager)
        {
            userManager = _userManager;
           
          
        }
        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return await Result.FailAsync("User not Found");
            }
            var identityResult = await userManager.ChangePasswordAsync(
               user,
               model.Password,
               model.NewPassword);
            var errors = identityResult.Errors.Select(e => e.Description.ToString()).ToList();
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);

        }         
        public async Task<IResult<string>> GetProfilePictureAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return await Result<string>.FailAsync("User Not Found");
            }
            return await Result<string>.SuccessAsync(data: user.PictureUrl);
        } 

    }
}
