﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Services.Domain
{
    internal class SystemAdminService : ISystemAdminService
    {
        readonly HttpClient httpClient;
        public SystemAdminService(HttpClient httpClient)
        {
             this.httpClient = httpClient;
        }
        public Task<List<IResult>> DeleteAdmin(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SystemAdmin> GetSystemAdminAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SystemAdmin>> GetSystemAdmins()
        {
            throw new NotImplementedException();
        }

        public Task<List<SystemAdmin>> GetSystemAdmins(string constituencyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<IResult>> UpdateAdmin(SystemAdmin entity)
        {
            throw new NotImplementedException();
        }
    }
}
