using ICTAZEVoting.Shared.Contracts;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Models
{
    public class SystemAdmin : AuditableEntity<Guid> 
    {
        public  PersonalDetails PersonalDetails { get; set; }
    }
}
