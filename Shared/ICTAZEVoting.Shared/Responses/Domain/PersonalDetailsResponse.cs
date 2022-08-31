using ICTAZEVoting.Shared.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Responses.Domain;

public class PersonalDetailsResponse
{
    public string FirstName { get; set; }
    public Guid UserId { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }         
    public string NRC { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    public string PictureUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }       
    public string Email { get; set; }
    public Guid OwnerId { get; set; }
    public string FullName => FirstName + " " + LastName;
}
