using ICTAZEVoting.Shared.Interfaces;

using Microsoft.AspNetCore.Http;

namespace ICTAZEVoting.Core.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            httpContextAccessor = contextAccessor;
        }

        public Task<string> GetRemoteIP()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetUserId()
        {
            var context = httpContextAccessor.HttpContext;
            var id = Guid.Empty;
            if (context != null)
            {
                id = context.User.GetId();
            }
            return Task.FromResult(id);
        }

        public Task<string> GetUserName()
        {
            var context = httpContextAccessor.HttpContext;
            return Task.FromResult(context?.User?.Identity?.Name ?? "SYSTEM");
        }
    }
}
