using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace ICTAZEVoting.Shared.Models
{
    public class ElectionPosition:Entity<Guid> 
    {                            
        public string Position { get; set; }
        public Guid ElectionId { get; set; }
        public List<Candidate> Candidates { get; set; }
     
        public virtual Election Election { get; set; }
        public ElectionPosition()
        {
            Id = Guid.NewGuid();
        }
    }
}
