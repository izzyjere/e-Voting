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
        public string ConfirmPassword { get; set; }
        public string? PictureUrl { get; set; }
        [Required, RegularExpression(@"^\d{6}\/\d{2}\/\d{1}$", ErrorMessage = "Please enter a valid NRC. xxxxxx/xx/x")]
        public string NRC { get; set; }
        public string Role { get; set; }
        public string? PhoneNumber { get; set; }
        public bool ActivateUser { get; set; } = false;
        public bool AutoConfirmEmail { get; set; } = false;
    }
}