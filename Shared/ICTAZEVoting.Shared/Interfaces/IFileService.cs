using ICTAZEVoting.Shared.Responses;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Interfaces;

public interface IFileService
{
    Task<IResult<UploadResponse>> UploadFile(UploadRequest uploadRequest);
    string GetFileUrl(string filePath);
}
