using ICTAZEVoting.Shared.Contracts;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICTAZEVoting.Shared.Models
{
    public class Candidate : AuditableEntity<Guid>
    {
        public string CandidateNumber { get; set; }  
        public Guid PoliticalPartyId { get; set; }
        public Guid ElectionPositionId { get; set; }
        [ForeignKey(nameof(ElectionPositionId))]
        public virtual ElectionPosition Position { get; set; }
        public PersonalDetails PersonalDetails { get; set; }
        public PoliticalParty PoliticalParty { get; set; }
    }
   
}
