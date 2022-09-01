using ICTAZEVoting.Core.Extensions;
using ICTAZEVoting.Shared.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

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
            var context = httpContextAccessor.HttpContext;
            string? ip;
            if (!string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-For"]))
            {
                ip = context.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ip = context.Request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            }
            return Task.FromResult(ip);
        }

        public Task<Guid> GetUserId()
        {
            var context = httpContextAccessor.HttpContext;
            var id = Guid.Empty;
            if (context != null && context.User!=null && !string.IsNullOrEmpty(context.User.Identity.Name))
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
