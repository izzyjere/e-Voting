using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string? PhoneNumber { get; set; }

        public bool ActivateUser { get; set; } = false;
        public bool AutoConfirmEmail { get; set; } = false;
    }
}