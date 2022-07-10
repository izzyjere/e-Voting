using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Wrapper;

using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;

namespace ICTAZEVoting.Core.Services.Identity
{
    public class FacialRecognitionService : IFacialRecognitionService
    {
        public Task<IResult> VerifyAsync(string imagePath)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> VerifyAsync(MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
