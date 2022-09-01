using ICTAZEVoting.Shared.Responses.Audit;
using ICTAZEVoting.Shared.Wrapper;

namespace ICTAZEVoting.Core.Services.Utility
{
    public interface IAuditService
    {
        Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(Guid userId);
        Task<IResult<IEnumerable<AuditResponse>>> GetTrailsAsync();
    }
}
