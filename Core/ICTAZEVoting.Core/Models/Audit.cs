﻿using ICTAZEVoting.Shared.Contracts;

namespace ICTAZEVoting.Core.Models
{
    public class Audit : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public string? Type { get; set; }
        public string? TableName { get; set; }
        public string UserName { get; set; }
        public string RemoteIpAddress { get; set; }
        public DateTime DateTime { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
        public string? PrimaryKey { get; set; }


    }
}
