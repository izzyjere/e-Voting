namespace ICTAZEVoting.Shared.Models;

public class ElectionResult
{
    public Election Election { get; set; }

    public string Position { get; set; }

    public Candidate Candidate { get; set; }

    public int VoteCount { get; set; }
}