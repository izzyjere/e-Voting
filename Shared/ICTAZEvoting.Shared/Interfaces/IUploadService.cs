using System.IO;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IUploadService
    {
        Task<IResult> DeleteFileAsync(string filePath);
        Task<IResult<string>> UploadFileAsync(UploadRequest request);
    }
}
