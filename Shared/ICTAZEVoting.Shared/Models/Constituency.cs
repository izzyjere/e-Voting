using ICTAZEVoting.Shared.Contracts;

using System;

namespace ICTAZEVoting.Shared.Models
{
    public class Constituency  : Entity<Guid>
    {
        public string Name { get; set; }
        public List<PolingStation> PolingStations { get; set; }
    }
}