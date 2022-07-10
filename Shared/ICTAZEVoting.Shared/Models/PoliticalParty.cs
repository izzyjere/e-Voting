﻿using ICTAZEVoting.Shared.Contracts;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Models
{
    public class PoliticalParty : Entity<Guid>
    {
        public string Name { get; set; }
        public string? Slogan { get; set; }
        public string? Manifesto { get; set; }
        public string? LogoUrl { get; set; }
    }
}
