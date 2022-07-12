using ICTAZEVoting.Shared.Responses.Identity;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IRoleManager
    {
        Task<IResult<List<RoleResponse>>> GetRolesAsync();
    }
}