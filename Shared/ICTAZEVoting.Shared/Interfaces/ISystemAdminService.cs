using ICTAZEVoting.Shared.Responses.Audit;

using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface ISystemAdminService
    {
        Task<SystemAdmin> GetSystemAdminAsync(string userId);
        Task<List<SystemAdmin>> GetSystemAdmins();
        Task<List<SystemAdmin>> GetSystemAdmins(string constituencyId);
        Task<List<IResult>> DeleteAdmin(string id);
        Task<List<IResult>> UpdateAdmin(SystemAdmin entity);
        Task<List<PollingStation>> GetPollingStations(string userId);
        Task<List<AuditResponse>> GetAuditsAsync();
    }
}
