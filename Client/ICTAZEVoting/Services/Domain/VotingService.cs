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
    public class VotingService : IVotingService
    {
        public Task<List<Candidate>> GetCandidates(string electionId)
        {
            throw new NotImplementedException();
        }

        public Task<ElectionResult> GetElectionResult(string candidateId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ElectionResult>> GetElectionResults(string electionId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetVoterCount(int electionId)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Vote(string secretKey, string electionId, Vote vote)
        {
            throw new NotImplementedException();
        }
    }
}
