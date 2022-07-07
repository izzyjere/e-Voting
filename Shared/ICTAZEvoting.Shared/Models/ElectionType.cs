using ICTAZEvoting.Shared.Contracts;
using System;

namespace ICTAZEvoting.Shared.Models
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