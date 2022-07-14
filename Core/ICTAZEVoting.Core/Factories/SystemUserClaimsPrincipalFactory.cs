using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using System.Security.Claims;

namespace ICTAZEVoting.Core.Factories
{
    public class SystemUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {

        public SystemUserClaimsPrincipalFactory(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {

            var identity = await base.GenerateClaimsAsync(user);           
            identity.AddClaim(new Claim("UserId", user.Id.ToString()));
            identity.AddClaim(new Claim("FullName", user.FirstName+" "+ user.FirstName));
            identity.AddClaim(new Claim("Picture", user.PictureUrl));
            return identity;
        }
    }
}
