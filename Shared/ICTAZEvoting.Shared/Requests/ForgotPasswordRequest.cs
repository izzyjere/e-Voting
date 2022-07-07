using System.ComponentModel.DataAnnotations;

namespace ICTAZEvoting.Shared.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}