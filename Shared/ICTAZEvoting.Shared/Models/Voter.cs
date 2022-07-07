using ICTAZEvoting.Shared.Contracts;
using ICTAZEvoting.Shared.Enums;

using System;

namespace ICTAZEvoting.Shared.Models
{
    public class Voter: AuditableEntity<Guid>
    {
        public string VoterNumber { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public string VerificationCipher { get; set; }
        public Voter()
        {
            Id = Guid.NewGuid();             
        }

    }
}
