﻿using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Responses.Identity
{
    public class RoleResponse
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}