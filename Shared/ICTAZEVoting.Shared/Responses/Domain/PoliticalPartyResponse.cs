
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Responses.Domain
{
    public class PoliticalPartyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Slogan { get; set; }
        public string? Manifesto { get; set; }
        public string? LogoUrl { get; set; }
    }
}
