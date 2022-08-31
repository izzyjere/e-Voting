namespace ICTAZEVoting.Shared.Requests;

public class UpdateProfilePictureRequest
{
    public byte[] Data { get; set; }
    public string FileName { get; set; }
    public string Extension { get; set; }
    public object UploadType { get; set; }
}