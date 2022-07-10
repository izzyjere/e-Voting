using ICTAZEVoting.Shared.Wrapper;
namespace ICTAZEVoting.Core.Services.Identity
{
    public interface IFacialRecognitionService
    {
        Task<IResult> VerifyAsync(string imagePath);
        Task<IResult> VerifyAsync(MemoryStream memoryStream);
    }
}
