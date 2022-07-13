using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Responses.Domain
{
    public class CandidateResponse
    {
        public Guid Id { get; set; }
        public Guid PoliticalPartyId { get; set; }
        public Guid ElectionPositionId { get; set; }
        public PersonalDetailsResponse PersonalDetails { get; set; }
        public ElectionPositionResponse Position { get; set; }
        public PoliticalPartyResponse PoliticalParty { get; set; }

    }
    public class PoliticalPartyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Slogan { get; set; }
        public string? Manifesto { get; set; }
        public string? LogoUrl { get; set; }
    }
}
