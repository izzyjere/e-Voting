using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Enums;

using ICTAZEVoting.Shared.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Models
{
    public class Election : Entity<Guid>
    {
        public DateTime? ElectionDate { get; set; }
        [Required(ErrorMessage ="Please provide a description for the election.")]
        public string Name { get; set; }         
        public Guid ElectionTypeId { get; set; }
        [Required,Range(1,48)]
        public double Duration { get; set; }
        public ElectionStatus Status { get; set; } 
        public List<ElectionVoter> Voters { get; set; }
        public List<ElectionPosition> Positions { get; set; }
        public DateTime ClosingTime => ElectionDate.Value.AddHours(Duration);
        public ElectionType Type { get; set; }
        public Election()
        {
            Id = Guid.NewGuid();
            ElectionDate = DateTime.Today.AddDays(1);
            Name = $"{DateTime.Today.Year} General Elections";
            ElectionTypeId = default;
        }
    }
}
