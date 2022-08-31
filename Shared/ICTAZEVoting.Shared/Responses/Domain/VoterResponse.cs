using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ICTAZEVoting.Shared.Responses.Domain
{
    public class VoterResponse
    {
        public Guid Id { get; set; }
        public PersonalDetailsResponse PersonalDetails { get; set; }          
        public Guid PolingStationId { get; set; }
        public PollingStation PolingStation { get; set; }
    }
}
