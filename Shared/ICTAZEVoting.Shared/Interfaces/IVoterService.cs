using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Wrapper;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IVoterService
    {
        Task<IResult<string[]>> Register(Voter entity);
        Task<IResult> Update(Voter entity);
        Task<List<Voter>> GetAll();
        Task<Voter>GetByUserId(string id);
        Task<Voter> GetById(string id);
        Task<IResult> Delete(string id);
        Task<IResult> VerifyVoter(byte[] facialData);
    }
}
