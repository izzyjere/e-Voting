global using ICTAZEVoting.Shared.Enums;
global using System.IO;


namespace ICTAZEVoting.Shared.Requests
{
    public class UploadRequest
    {
        public string FileName { get; set; }    
        public UploadType Type { get; set; }
        public byte[] Data { get; set; }
    }
    
}
