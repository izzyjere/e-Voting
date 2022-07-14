using ICTAZEVoting.Extensions;
using ICTAZEVoting.Shared.Constants;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

using Microsoft.AspNetCore.Components.Authorization;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ICTAZEVoting.Services.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        AuthenticationStateProvider authenticationStateProvider;
        HttpClient Client;

        public AuthenticationService(AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            Client = httpClient;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            return (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
        }

        public async Task<AuthenticationState> GetAuthenticationState()
        {
            return await authenticationStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<IResult> SignIn(TokenRequest request)
        {
            var response = await Client.PostAsJsonAsync(ApiEndpoints.Login, request);
            if (response.IsSuccessStatusCode || response.StatusCode==System.Net.HttpStatusCode.Found)
            {
                var result = await response.ToResult<TokenResponse>();
                if (result.Succeeded)
                {
                    await SessionStorage.SaveItemEncryptedAsync("UserToken", result.Data);

                    await ((CustomAuthenticationStateProvider)authenticationStateProvider).StateChangedAsync();
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data.Token);
                    await Client.GetAsync($"login/?key={result.Data.TokenKey}");
                    return await Result.SuccessAsync($"Welcome!");
                }
                return await Result.FailAsync("Invalid Username or Password");
            }
            else
            {
                return await Result.FailAsync("An error occured. Try again");
            }

        }

        public async Task<IResult> SignOut()
        {
            await SessionStorage.RemoveItemAsync("UserToken");
            ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();
            Client.DefaultRequestHeaders.Authorization = null;
            return await Result.SuccessAsync();
        }
    }
}
