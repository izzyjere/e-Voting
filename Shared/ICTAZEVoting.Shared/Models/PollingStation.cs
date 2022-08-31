using ICTAZEVoting.Shared.Contracts;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Models;

public class PollingStation : Entity<Guid>
{
    public string Name { get; set; }
    public Guid ConstituencyId { get; set; }       
    public Constituency Constituency { get; set; }
    public PollingStation()
    {
        Id = new();
    }
}
