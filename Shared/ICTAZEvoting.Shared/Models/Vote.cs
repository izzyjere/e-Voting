
using System;

namespace ICTAZEVoting.Shared.Models
{
    public class Vote 
    {
        public DateTime TimeStamp { get; set; }
        public Guid VoterId { get; set; }
        public string NodeAddress { get; set; }
        public Guid CandidateId { get; set; }  


    }
}
