using ICTAZEVoting.Core.Extensions;
using ICTAZEVoting.Shared.Constants;
using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;

using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;

using System.Net.Http.Json;

namespace ICTAZEVoting.Services.Identity
{
    public class FacialRecognitionService : IFacialRecognitionService
    {
        readonly HttpClient httpClient;
        public FacialRecognitionService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IResult> VerifyAsync(VerifyRequest request)
        {
            var req = await httpClient.PostAsJsonAsync(ApiEndpoints.VerifyFace, request);
            return await req.ToResult();
        }

        public Task<IResult> VerifyAsync(MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
