using ICTAZEvoting.Shared.Contracts;
using ICTAZEvoting.Shared.Enums;

using System;
using System.Collections.Generic;

namespace ICTAZEvoting.Shared.Models
{
    public class Election : Entity<Guid>
    {
        public DateTime ElectionDate { get; set; }
        public string Name { get; set; }         
        public Guid ElectionTypeId { get; set; }
        public double Duration { get; set; }
        public ElectionStatus Status { get; set; }
        public List<Voter> Voters { get; set; }
        public List<Candidate> Candidates { get; set; }      
        public ElectionType Type { get; set; }
        public Election()
        {
            Id = Guid.NewGuid();
        }
    }
}
