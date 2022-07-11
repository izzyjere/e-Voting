using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Wrapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Services.Domain
{
    public class VoterService : IVoterService
    {
        public Task<IResult> Delete(Voter entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Voter>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Voter> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Voter> GetByUserId(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<string>> Register(Voter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Update(Voter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> VerifyVoter(byte[] facialData)
        {
            throw new NotImplementedException();
        }
    }
}
