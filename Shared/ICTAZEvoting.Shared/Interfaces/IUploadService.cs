using ICTAZEVoting.Shared.Responses;

using System.IO;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IUploadService
    {
        Task<IResult> DeleteFileAsync(string filePath);
        Task<IResult<UploadResponse>> UploadFileAsync(UploadRequest request);
    }
}
