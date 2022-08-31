using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Wrapper;
namespace ICTAZEVoting.Shared.Interfaces;

public interface IFacialRecognitionService
{
    Task<IResult> VerifyAsync(VerifyRequest request);
    Task<IResult> VerifyAsync(MemoryStream memoryStream);
}
