using ICTAZEVoting.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Models;

public class PersonalDetails
{
    [Required]
    public string FirstName { get; set; }
    public Guid UserId { get; set; }
    [Required]
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    [Required,RegularExpression(@"^\d{6}\/\d{2}\/\d{1}$", ErrorMessage ="Please enter a valid NRC. xxxxxx/xx/x")]
    public string NRC { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    public string PictureUrl { get; set; }
    [Required]
    public DateTime? DateOfBirth { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public Guid OwnerId { get; set; }
    public string FullName => FirstName + " " + LastName;
}
