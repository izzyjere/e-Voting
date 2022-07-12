using ICTAZEVoting.Shared.Contracts;

using System;

namespace ICTAZEVoting.Shared.Models
{
    public class Voter : AuditableEntity<Guid>
    {
        public string VoterNumber { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public SecreteKey SecreteKey {get; set; } 
        public Guid PolingStationId { get; set; }
        public virtual PollingStation PolingStation { get; set; }
    }
}
