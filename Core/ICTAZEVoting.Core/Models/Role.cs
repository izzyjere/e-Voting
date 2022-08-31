using Microsoft.AspNetCore.Identity;

namespace ICTAZEVoting.Core.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public Role() : base() { }
        public Role(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
    }
}
