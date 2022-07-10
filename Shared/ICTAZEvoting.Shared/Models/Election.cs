using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Enums;

using ICTAZEVoting.Shared.Models;

using System;
using System.Collections.Generic;

namespace ICTAZEVoting.Shared.Models
{
    public class Election : Entity<Guid>
    {
        public DateTime ElectionDate { get; set; }
        public string Name { get; set; }         
        public Guid ElectionTypeId { get; set; }
        public double Duration { get; set; }
        public ElectionStatus Status { get; set; } 
        public List<ElectionVoter> Voters { get; set; }
        public List<ElectionPosition> Positions { get; set; }            
        public ElectionType Type { get; set; }
        public Election()
        {
            Id = Guid.NewGuid();
        }
    }
}
