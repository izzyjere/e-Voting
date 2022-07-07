using ICTAZEvoting.Shared.Requests;
using ICTAZEvoting.Shared.Responses.Identity;
using ICTAZEvoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity
{
    public interface ITokenService 
    {
        Task<IResult<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}
