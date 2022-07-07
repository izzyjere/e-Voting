﻿using System.ComponentModel.DataAnnotations;

namespace ICTAZEvoting.Shared.Requests
{
    public class RoleRequest
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}