using ICTAZEvoting.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEvoting.Shared.Models
{
    public class PersonalDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        [Required,RegularExpression(@"^\d{6}\/\d{2}\/\d{1}$", ErrorMessage ="Please enter a valid NRC. xxxxxx/xx/x")]
        public string NRC { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string PictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }         
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
