using ICTAZEVoting.Shared.Enums;
using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Requests;
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


        public async Task<IResult<string>> UploadFileAsync(UploadRequest request)
        {
            var newName = Path.GetRandomFileName().Replace(".", "_") + Path.GetExtension(request.FileName);

            var path = Path.Combine(GetWebRootPath(), "_dhhfhffg", newName);
            try
            {

                using FileStream fileStream = new(path, FileMode.CreateNew, FileAccess.Write);
                var ms = new MemoryStream();
                ms.Write(request.Data, 0, request.Data.Length);
                await Task.Run(() => ms.WriteTo(fileStream));
                return await Result<string>.SuccessAsync($"_dhhfhffg/{newName}", "Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return Result<string>.Fail("Could not upload file.");
            }             
        }

        private string GetWebRootPath() => webHostEnvironment.WebRootPath;

    }

}
