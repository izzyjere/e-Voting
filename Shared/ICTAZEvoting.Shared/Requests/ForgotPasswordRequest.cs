using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}