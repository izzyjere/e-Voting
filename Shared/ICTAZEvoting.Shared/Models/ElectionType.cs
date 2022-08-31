using ICTAZEVoting.Shared.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Models;

public class ElectionType : Entity<Guid>
{
    [Required]
    public string Name { get; set; }
    public ElectionType()
    {
        Id = Guid.NewGuid();
    }
}