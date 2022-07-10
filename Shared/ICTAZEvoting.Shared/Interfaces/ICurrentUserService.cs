
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface ICurrentUserService 
    {
        Task<string> GetUserName();
        Task<int> GetUserId();           
        Task<string> GetRemoteIP();
    }
}
