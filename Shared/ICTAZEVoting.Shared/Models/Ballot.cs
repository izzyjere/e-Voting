using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICTAZEVoting.Shared.Models
{
    internal class Ballot 
    {
        public Guid ElectionId { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
