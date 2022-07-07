using ICTAZEvoting.Shared.Requests;
using ICTAZEvoting.Shared.Responses.Identity;
using ICTAZEvoting.Shared.Wrapper;

using ICTAZEVoting.Core.Models;

namespace ICTAZEVoting.Core.Services.Identity;

public interface IUserService 
{
    Task<IResult<int>> ConfirmEmailAsync(int userId, string code);
    Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<Result<List<UserResponse>>> GetAllAsync();
    Task<IResult<UserResponse>> GetByIdAsync(int userId);
    Task<int> GetCountAsync();
    Task<bool> Delete(Guid guid);
    Task<IResult<UserRolesResponse>> GetRolesAsync(int userId);
    Task<IResult<Guid>> RegisterAsync(RegisterRequest request);
    Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
    Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);    
}
