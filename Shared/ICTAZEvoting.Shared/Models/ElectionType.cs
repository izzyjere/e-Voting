using ICTAZEVoting.Shared.Contracts;
using System;

namespace ICTAZEVoting.Shared.Models
{
    public class ElectionType : Entity<Guid>
    {
        public string Name { get; set; }
        public ElectionType()
        {
            Id = Guid.NewGuid();
        }
    }
}