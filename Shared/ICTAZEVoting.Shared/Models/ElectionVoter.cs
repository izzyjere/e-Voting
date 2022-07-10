using System;

namespace ICTAZEVoting.Shared.Models
{
    public class ElectionVoter
    {
        public Guid VoterId { get; set; }
        public Guid ElectionId { get; set; }
        public bool HasVoted { get; set; }
        public virtual Voter Voter { get; set; }
        public virtual Election Election { get; set; }
    }
}