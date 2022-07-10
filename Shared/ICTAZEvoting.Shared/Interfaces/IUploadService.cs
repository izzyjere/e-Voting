using System.IO;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IUploadService
    {
        Task<bool> DeleteFileAsync(string filePath);
        Task<string> UploadFileAsync(string filePath, MemoryStream data);
    }
}
