using ICTAZEVoting.Shared.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Core.Models;

public class SecreteKey
{
   
    public Guid VoterId { get; set; }
    public string EncryptedKey { get; set; }        
    public string IV { get; set; }
    [ForeignKey(nameof(VoterId))]
    public virtual Voter Voter { get; set; }
}
