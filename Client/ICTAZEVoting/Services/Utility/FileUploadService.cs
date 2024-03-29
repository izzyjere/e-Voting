﻿using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses;

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

        public string GetFileUrl(string filePath)
        {
            var apiDomain = httpClient?.BaseAddress?.ToString();
            return string.IsNullOrEmpty(apiDomain)?string.Empty: $"{apiDomain}files/{filePath}";
        }

        public async Task<IResult<UploadResponse>> UploadFile(UploadRequest uploadRequest)
        {   
            var response = await httpClient.PostAsJsonAsync(ApiEndpoints.FileUpload, uploadRequest);
            if(response.IsSuccessStatusCode)
            {
                var res = await response.ToResult<UploadResponse>();
                return res;
            }
            else
            {
            
                return Result<UploadResponse>.Fail("An error occured. Try again.");
            }
           
        }
    }
}
