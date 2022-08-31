using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Extensions;

namespace ICTAZEVoting.Services.Identity;

public class AuthenticationHeaderHandler : DelegatingHandler
{

    protected override async Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            var savedToken = await SessionStorage.ReadEncryptedItemAsync<TokenResponse>("UserToken");
            if (savedToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken.Token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
