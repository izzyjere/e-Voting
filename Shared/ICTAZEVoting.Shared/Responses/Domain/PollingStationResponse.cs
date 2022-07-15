using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Responses.Domain
{
    public class PollingStationResponse
    {   public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ConstituencyId { get; set; }
        public ConstituencyResponse Constituency { get; set; }
    }
}
