﻿using ICTAZEVoting.Shared.Responses.Identity;

namespace ICTAZEVoting.Core.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }         
        public Guid UserGuid { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserRoleModel> Roles { get; set; }
    }
}