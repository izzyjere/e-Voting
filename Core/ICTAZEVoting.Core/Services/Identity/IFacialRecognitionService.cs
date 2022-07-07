using ICTAZEvoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity
{
    public interface IFacialRecognitionService
    {
        Task<IResult> VerifyAsync(string imagePath);
    }
}
