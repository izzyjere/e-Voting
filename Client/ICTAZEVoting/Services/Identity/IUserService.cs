using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;

using System.Threading.Tasks;


namespace ICTAZEVoting.Services.Identity;

public interface IUserService
{
    Task<IResult> SignUp(RegisterRequest request);
    Task<IResult> ChangePassword(ChangePasswordRequest request);
    Task<IResult> ForgotPassword(ForgotPasswordRequest request);
}
