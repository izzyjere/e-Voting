﻿using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Models;

using System;
using System.Collections.Generic;

namespace ICTAZEVoting.Shared.Models
{
    public class CandidatePosition : Entity<Guid>
    {
        public string Position { get; set; }
        public Guid ElectionId { get; set; }
        public List<Candidate> Candidates { get; set; }
        public virtual Election Election { get; set; }
    }
}
