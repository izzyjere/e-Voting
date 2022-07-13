using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IFileService
    {
        Task<IResult<string>> UploadFile(UploadRequest uploadRequest);
    }
}
