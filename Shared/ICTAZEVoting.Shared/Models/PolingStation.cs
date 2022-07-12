﻿using ICTAZEVoting.Shared.Contracts;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Models
{
    public class PolingStation : Entity<Guid>
    {
        public string Name { get; set; }
        public Guid ConstituencyId { get; set; }
        public List<Voter> Voters { get; set; }
        public Constituency Constituency { get; set; }
    }
}