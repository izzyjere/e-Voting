using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Wrapper;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IVotingService
    {
        Task<IResult> Vote(string secretKey, string electionId, Vote vote);
        Task<List<Candidate>> GetCandidates(string electionId);
        Task<List<ElectionResult>> GetElectionResults(string electionId);
        Task<ElectionResult> GetElectionResult(string candidateId);
        Task<int> GetVoterCount(int electionId);
    }
}
