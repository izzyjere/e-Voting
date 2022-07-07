using ICTAZEvoting.Shared.Requests;
using ICTAZEvoting.Shared.Responses.Identity;
using ICTAZEvoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity;

public interface IRoleService
{
    Task<Result<string>> AddEditAsync(RoleRequest request);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<List<RoleResponse>>> GetAllAsync();
    Task<Result<RoleResponse>> GetByIdAsync(int id);
    Task<int> GetCountAsync();     
}