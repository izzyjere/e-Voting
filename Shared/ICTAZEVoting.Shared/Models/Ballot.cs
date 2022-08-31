using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICTAZEVoting.Shared.Models
{
    public class Ballot 
    {
        public Guid PollingStationId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid ElectionId { get; set; }
        public List<Vote> Votes { get; set; }
        public Ballot()
        {
            TimeStamp = DateTime.Now;
        }
        
    }
}
