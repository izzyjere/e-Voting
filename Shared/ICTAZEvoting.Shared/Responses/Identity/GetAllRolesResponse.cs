
using System.Collections.Generic;

namespace ICTAZEvoting.Shared.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}