using ICTAZEvoting.Shared.Contracts;
using ICTAZEvoting.Shared.Wrapper;

using Microsoft.Extensions.Options;
using Microsoft.ProjectOxford.Face;

namespace ICTAZEVoting.Core.Services.Identity
{
    public class FacialRecognitionService : IFacialRecognitionService
    {
       
        readonly FaceServiceClient faceServiceClient;
        public FacialRecognitionService(IOptions<AppConfiguration> options)
        {
            var appConfig = options.Value;
            faceServiceClient = new FaceServiceClient(appConfig.FacialApiKey, appConfig.FacialApiRoute);
        }

        public async Task<IResult> VerifyAsync(string imagePath)
        {
            try
            {
                using var fileStream = File.OpenRead(imagePath);
                var faces = await faceServiceClient.DetectAsync(fileStream);
                if (!faces.Any())
                {
                    return await Result.FailAsync("Verification Failed. Try Again");
                }
                else
                {
                    return await Result.SuccessAsync($"Welcome.");
                }

            }
            catch (Exception)
            {

                return await Result.FailAsync("An error occured. Try Again");
            }
        }
    }
}
