namespace ICTAZEVoting.Shared.Requests
{
    public class ToggleUserStatusRequest
    {
        public bool ActivateUser { get; set; }
        public int UserId { get; set; }
    }
}