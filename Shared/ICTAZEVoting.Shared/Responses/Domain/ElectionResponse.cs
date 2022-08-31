using ICTAZEVoting.Shared.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICTAZEVoting.Shared.Responses.Domain
{
    public class ElectionResponse
    {
        public Guid Id { get; set; }
        public DateTime ElectionDate { get; set; }
        public Guid ElectionTypeId { get; set; }
        public string Name { get; set; }       
        public ElectionType  Type { get; set; }
        public double Duration { get; set; }
        public ElectionStatus Status { get; set; }
        public DateTime ClosingTime { get; set; }
        public List<ElectionPositionResponse> Positions { get; set; }
        public int VoterCount { get; set; }
        public int CandidateCount => Positions.Sum(prop =>prop.CandidateCount);
    }
    public class ElectionPositionResponse
    {
        public Guid Id { get; set; }
        public Guid ElectionId { get; set; }
        public string Position { get; set; }
        public int CandidateCount { get; set; }
    }
}
