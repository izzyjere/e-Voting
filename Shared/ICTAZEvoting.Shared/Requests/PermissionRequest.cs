using System.Collections.Generic;

namespace ICTAZEVoting.Shared.Requests;

public class PermissionRequest
{
    public int RoleId { get; set; }
    public IList<RoleClaimRequest> RoleClaims { get; set; }
}
