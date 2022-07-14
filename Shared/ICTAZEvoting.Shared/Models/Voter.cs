using ICTAZEVoting.Shared.Contracts;

using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Models
{
    public class Voter : AuditableEntity<Guid>
    {
        public string VoterNumber { get; set; }
        [ValidateComplexType]
        public PersonalDetails PersonalDetails { get; set; }
        public SecreteKey SecreteKey {get; set; } 
        public Guid PolingStationId { get; set; }
        public virtual PollingStation PolingStation { get; set; }
    }
}
