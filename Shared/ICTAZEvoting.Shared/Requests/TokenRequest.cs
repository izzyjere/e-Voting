using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEvoting.Shared.Requests
{
    public class TokenRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string TwoFactorCode { get; set; }
        public bool RemberMachine { get; set; }
        public DateTime LoginStarted { get; set; }
        public bool RememberMe { get; set; }
    }
}
