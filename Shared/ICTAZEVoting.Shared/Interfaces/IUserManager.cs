using ICTAZEVoting.Shared.Responses.Identity;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IUserManager
    {
        Task<IResult<List<UserResponse>>> GetUsers();
        Task<IResult> RegisterUser(RegisterRequest request);
        Task<IResult> Delete(string id);
        Task<IResult> Update(UserResponse record);
        Task<List<UserRoleModel>> GetUserRoles(string id);
    }
}
