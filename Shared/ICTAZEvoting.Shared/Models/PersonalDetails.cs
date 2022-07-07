using ICTAZEvoting.Shared.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEvoting.Shared.Models
{
    public class PersonalDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string NRC { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }         
        public string Address { get; set; }
    }
}
