global using System;

namespace ICTAZEVoting.Shared.Responses.Identity;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }      
    public string PictureUrl { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public List<UserRoleModel> Roles { get; set; }
    public string? MiddleName { get; set; }
    public string NRC { get; set; }
}