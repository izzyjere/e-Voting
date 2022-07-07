
using System.Collections.Generic;

namespace ICTAZEvoting.Shared.Responses.Identity
{
    public class PermissionResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleClaimResponse> RoleClaims { get; set; }
    }
}