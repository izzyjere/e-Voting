using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity
{
    public interface IUserAccountService
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, Guid userId);

        Task<IResult<string>> GetProfilePictureAsync(Guid userId);        
    }
}
