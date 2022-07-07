using ICTAZEvoting.Shared.Contracts;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEvoting.Shared.Models
{
    public class Vote 
    {
        public Guid VoterId { get; set; }
        public Guid CandidateId { get; set; }

        public override string ToString()
        {
            return VoterId.ToString() + "_" + CandidateId.ToString();
        }

    }
}
