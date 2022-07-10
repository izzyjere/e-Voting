using ICTAZEVoting.Shared.Contracts;

using System;

namespace ICTAZEVoting.Shared.Models
{
    public class Candidate : AuditableEntity<Guid>
    {
        public string CandidateNumber { get; set; }
        public Guid CandidatePositionId { get; set; }
        public virtual CandidatePosition Position { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
    }
}
