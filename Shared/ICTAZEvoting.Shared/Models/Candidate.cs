using ICTAZEvoting.Shared.Contracts;
using System;

namespace ICTAZEvoting.Shared.Models
{
    public class Candidate : AuditableEntity<Guid>
    {
        public string CandidateNumber { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
    }
}
