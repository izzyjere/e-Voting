using ICTAZEVoting.Shared.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Services.Utility
{
    public class FileUploadService : IFileService
    {
        readonly HttpClient httpClient;
        public FileUploadService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IResult<string>> UploadFile(UploadRequest uploadRequest)
        {   
            var response = await httpClient.PostAsJsonAsync(ApiEndpoints.FileUpload, uploadRequest);
            if(response.IsSuccessStatusCode)
            {
                var res = await response.ToResult<string>();
                return res;
            }
            else
            {
            
                return Result<string>.Fail("An error occured. Try again.");
            }
           
        }
    }
}
