
using System;

namespace ICTAZEVoting.Shared.Models
{
    public class Vote 
    {
        public DateTime TimeStamp { get; set; }
        public Guid PositionId { get; set; }      
        public Guid PollingStationId { get; set; }
        public string NodeAddress { get; set; }
        public Guid CandidateId { get; set; }  


    }
}
