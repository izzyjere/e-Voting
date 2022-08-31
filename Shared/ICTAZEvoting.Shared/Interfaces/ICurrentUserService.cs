
using System;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces;

public interface ICurrentUserService 
{
    Task<string> GetUserName();
    Task<Guid> GetUserId();           
    Task<string> GetRemoteIP();
}
