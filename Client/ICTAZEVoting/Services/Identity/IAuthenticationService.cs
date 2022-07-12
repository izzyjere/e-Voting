using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;

using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;
using System.Threading.Tasks;



namespace ICTAZEVoting.Services.Identity
{
    public interface IAuthenticationService
    {
        Task<IResult> SignIn(TokenRequest request);
        Task<IResult> SignOut();
        Task<AuthenticationState> GetAuthenticationState();
        Task<ClaimsPrincipal> CurrentUser();
    }
}
