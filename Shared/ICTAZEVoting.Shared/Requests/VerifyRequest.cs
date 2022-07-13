using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Requests
{
    public class VerifyRequest
    {
        public string VoterId { get; set; }
        public byte[] Data { get; set; }
        public List<int> Dimensions { get; set; }
    }
}
