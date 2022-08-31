
using System.Collections.Generic;

namespace ICTAZEVoting.Shared.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}