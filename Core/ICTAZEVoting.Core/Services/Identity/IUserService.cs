using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

using ICTAZEVoting.Core.Models;

namespace ICTAZEVoting.Core.Services.Identity;

public interface IUserService 
{
    Task<IResult<Guid>> ConfirmEmailAsync(Guid userId, string code);
    Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<Result<List<UserResponse>>> GetAllAsync();
    Task<IResult<UserResponse>> GetByIdAsync(Guid userId);
    Task<int> GetCountAsync();
    Task<bool> Delete(Guid guid);
    Task<IResult<UserRolesResponse>> GetRolesAsync(Guid userId);
    Task<IResult<Guid>> RegisterAsync(RegisterRequest request);
    Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
    Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);    
}
