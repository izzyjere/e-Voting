using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace ICTAZEVoting.Core.Models
{
    public class User : IdentityUser<Guid>
    {
        public virtual List<UserRole> Roles { get; set; }
        public Guid PersonId { get; set; }    
        [Column(TypeName = "text")]
        public string? PictureUrl { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        
    }
}
