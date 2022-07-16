namespace ICTAZEVoting.Shared.Requests
{
    public class VoterVerificationRequest
    {
        public string Key { get; set; }
        public Guid UserId { get; set; }
    }
}
