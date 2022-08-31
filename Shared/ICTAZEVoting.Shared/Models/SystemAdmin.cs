using ICTAZEVoting.Shared.Contracts;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICTAZEVoting.Shared.Models;

public class SystemAdmin : AuditableEntity<Guid> 
{
    [ValidateComplexType]
    public  PersonalDetails PersonalDetails { get; set; }
    public Guid ConstituencyId { get; set; }

    [ForeignKey(nameof(ConstituencyId))]
    public Constituency Constituency { get; set; }

}
