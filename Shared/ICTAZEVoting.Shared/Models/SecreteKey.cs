
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Models;

public class SecreteKey
{
    [Key]
    public Guid VoterId { get; set; }
    public string EncryptedKey { get; set; }
    public string IV { get; set; }
    
}
