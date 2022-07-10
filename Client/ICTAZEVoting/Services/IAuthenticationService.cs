
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;



namespace ICTAZEVoting.Services
{
    public interface IAuthenticationService
    {
        Task<IResult> SignIn(TokenRequest request);
        Task<IResult> SignOut();
        Task<AuthenticationState> GetAuthenticationState();
    }
}
