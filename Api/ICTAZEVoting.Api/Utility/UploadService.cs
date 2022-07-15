using ICTAZEVoting.Shared.Enums;
using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses;
using ICTAZEVoting.Shared.Wrapper;

using IResult = ICTAZEVoting.Shared.Wrapper.IResult;
namespace ICTAZEVoting.Api.Utility
{
    public class UploadService : IUploadService
    {
        readonly IWebHostEnvironment webHostEnvironment;
        public UploadService(IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task<IResult> DeleteFileAsync(string filePath)
        {

            try
            {
                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));

                    return Result.Success();
                }
                else
                {
                    return Result.Fail("Could not delete file.");
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return Result.Fail("Could not delete file.");
            }

        }


        public async Task<IResult<UploadResponse>> UploadFileAsync(UploadRequest request)
        {
            var newName = Path.GetRandomFileName().Replace(".", "_") + Path.GetExtension(request.FileName);

            var path = Path.Combine(GetPath(request.Type), newName);
            try
            {

                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write);
                var ms = new MemoryStream();
                ms.Write(request.Data, 0, request.Data.Length);
                await Task.Run(() => ms.WriteTo(fileStream));
                return await Result<UploadResponse>.SuccessAsync(new UploadResponse { Path = getFileName(request.Type, newName) }, "Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);                 
                return Result<UploadResponse>.Fail("Could not upload file.");
                
            }             
        }
        string GetPath(UploadType uploadType) => uploadType switch
        {
            UploadType.Biometric => Path.Combine(GetWebRootPath(),"biometrics"),
            UploadType.ProfilePicture => Path.Combine(GetWebRootPath(), "_dhhfhffg"),
            UploadType.Other=> Path.Combine(GetWebRootPath(), "fileUploads"),
            _=> string.Empty
        };
        string getFileName(UploadType uploadType,string fn) => uploadType switch
        {
            UploadType.Biometric => $"biometrics/{fn}",
            UploadType.ProfilePicture => $"_dhhfhffg/{fn}",
            UploadType.Other => $"fileUploads/{fn}",
            _ => string.Empty
        };

        private string GetWebRootPath() =>Path.Combine(webHostEnvironment.ContentRootPath, "Files");

    }

}
