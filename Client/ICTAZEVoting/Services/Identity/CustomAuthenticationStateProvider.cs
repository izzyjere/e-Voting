
using Microsoft.AspNetCore.Components.Authorization;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using System.Linq;
using ICTAZEVoting.Extensions;
using ICTAZEVoting.Shared.Responses.Identity;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace ICTAZEVoting.Services.Identity
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        HttpClient Client;
        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {

            Client = httpClient;
        }

        public async Task StateChangedAsync()
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());

            NotifyAuthenticationStateChanged(authState);

        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
        {
            var state = await GetAuthenticationStateAsync();
            var authenticationStateProviderUser = state.User;
            return authenticationStateProviderUser;
        }

        public ClaimsPrincipal AuthenticationStateUser { get; set; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await SessionStorage.ReadEncryptedItemAsync<TokenResponse>("UserToken");
            if (savedToken == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken.Token);
            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetAllClaims(savedToken.Token), "jwt")));
            AuthenticationStateUser = state.User;
            return state;
        }

        IEnumerable<Claim> GetAllClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            return securityToken.Claims.ToList();

        }         
       
    }
}


