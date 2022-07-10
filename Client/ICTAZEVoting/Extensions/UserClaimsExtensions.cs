
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ICTAZEVoting.Extensions
{
    public static class UserClaimsExtensions
    {
        internal static string GetEmail(this ClaimsPrincipal claimsPrincipal)
           => claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        internal static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        internal static string GetRole(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        internal static string GetLastName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);
        internal static string GetFullName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue("FullName");
        internal static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

        internal static string GetUserId(this ClaimsPrincipal claimsPrincipal)
           => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        internal static string FindFirstValue(this ClaimsPrincipal claimsPrincipal, string claimType)
            => claimsPrincipal.FindFirst(claimType).Value;
    }
}
