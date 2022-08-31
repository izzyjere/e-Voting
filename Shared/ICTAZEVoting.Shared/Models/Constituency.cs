using ICTAZEVoting.Shared.Contracts;

using System;

namespace ICTAZEVoting.Shared.Models;

public class Constituency  : Entity<Guid>
{
    public string Name { get; set; }
    public List<PollingStation> PolingStations { get; set; }
    public Constituency()
    {
        Id = new();
        PolingStations = new();
    }
}