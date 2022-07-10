using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Identity;

public interface IRoleService
{
    Task<Result<string>> AddEditAsync(RoleRequest request);
    Task<Result<string>> DeleteAsync(Guid id);
    Task<Result<List<RoleResponse>>> GetAllAsync();
    Task<Result<RoleResponse>> GetByIdAsync(Guid id);
    Task<int> GetCountAsync();     
}