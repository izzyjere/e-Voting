
using System.Threading.Tasks;

namespace ICTAZEvoting.Shared.Interfaces
{
    public interface ICurrentUserService 
    {
        Task<string> GetUserName();
        Task<int> GetUserId();           
        Task<string> GetRemoteIP();
    }
}
