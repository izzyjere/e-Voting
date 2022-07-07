
using System;

namespace ICTAZEvoting.Shared.Models
{
    public class Vote 
    {
        public DateTime TimeStamp { get; set; }
        public Guid VoterId { get; set; }
        public Guid CandidateId { get; set; }  


    }
}
