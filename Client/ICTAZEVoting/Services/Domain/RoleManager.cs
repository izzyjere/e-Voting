using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Services.Domain
{
    internal class RoleManager : IRoleManager
    {
        readonly HttpClient httpClient;
        public RoleManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public Task<IResult> DeleteRoleAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<RoleResponse>> GetRoleAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<List<RoleResponse>>> GetRolesAsync()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetRoles);
            return await get.ToResult<List<RoleResponse>>();
        }

        public Task<IResult> SaveRole(RoleRequest roleRequest)
        {
            throw new NotImplementedException();
        }
    }
}
