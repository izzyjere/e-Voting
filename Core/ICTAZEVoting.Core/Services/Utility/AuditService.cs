using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Shared.Responses.Audit;
using ICTAZEVoting.Shared.Wrapper;

using Microsoft.EntityFrameworkCore;

namespace ICTAZEVoting.Core.Services.Utility
{
    public class AuditService : IAuditService
    {
        private readonly SystemDbContext _context;
        private readonly IMapper _mapper;



        public AuditService(
            IMapper mapper,
            SystemDbContext context)

        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(Guid userId)
        {
            var trails = await _context.AuditTrails.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            var mappedLogs = _mapper.Map<List<AuditResponse>>(trails);
            return await Result<IEnumerable<AuditResponse>>.SuccessAsync(mappedLogs);
        }

        public async Task<IResult<IEnumerable<AuditResponse>>> GetTrailsAsync()
        {
            var trails = await _context.AuditTrails.OrderByDescending(a => a.Id).Take(250).ToListAsync();
            var mappedLogs = _mapper.Map<List<AuditResponse>>(trails);
            return await Result<IEnumerable<AuditResponse>>.SuccessAsync(mappedLogs);
        }
    }
}
