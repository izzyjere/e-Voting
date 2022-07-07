using ICTAZEvoting.Shared.Requests;
using ICTAZEvoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity
{
    public interface IUserAccountService
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, int userId);

        Task<IResult<string>> GetProfilePictureAsync(int userId);        
    }
}
