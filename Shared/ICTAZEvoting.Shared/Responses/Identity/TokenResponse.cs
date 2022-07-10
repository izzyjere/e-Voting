using System;

namespace ICTAZEVoting.Shared.Responses.Identity
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public Guid TokenKey { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}