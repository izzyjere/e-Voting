using ICTAZEVoting.Shared.Contracts;

using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Models;

public class Voter : AuditableEntity<Guid>
{
   
    [ValidateComplexType]
    public PersonalDetails PersonalDetails { get; set; }
    public SecreteKey SecreteKey {get; set; } 
    [Required]
    public Guid PolingStationId { get; set; }
    public virtual PollingStation PolingStation { get; set; }
}
