using ICTAZEVoting.Shared.Contracts;

using System;

namespace ICTAZEVoting.Shared.Models
{
    public class Voter: AuditableEntity<Guid>
    {
        public string VoterNumber { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public string VerificationCipher { get; set; }         
    }
}
