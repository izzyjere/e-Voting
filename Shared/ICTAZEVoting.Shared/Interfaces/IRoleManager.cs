using ICTAZEVoting.Shared.Responses.Identity;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IRoleManager
    {
        Task<IResult<List<RoleResponse>>> GetRolesAsync();
        Task<IResult<RoleResponse>> GetRoleAsync(string roleId);
        Task<IResult> SaveRole(RoleRequest roleRequest);
        Task<IResult> DeleteRoleAsync(string roleId);
    }
}