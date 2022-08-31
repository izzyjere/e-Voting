using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity;

public interface ITokenService 
{
    Task<IResult<TokenResponse>> LoginAsync(TokenRequest model);

    Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
}
