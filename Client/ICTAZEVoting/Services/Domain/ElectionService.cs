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
    public class ElectionService : IElectionService
    {
        public Task<IResult> AddElectionPosition(ElectionPosition position)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> AddElectionType()
        {
            throw new NotImplementedException();
        }

        public Task<IResult> AddPoliticalParty(PoliticalParty politicalParty)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteCandidate(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteElectionPosition(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteElectionType(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DisqualifyCandidate(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Candidate> GetCandidate(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Election> GetCurrentElection()
        {
            throw new NotImplementedException();
        }

        public Task<Election> GetElection(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ElectionPosition>> GetElectionPositionList(string electionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ElectionPosition>> GetElectionPositionList()
        {
            throw new NotImplementedException();
        }

        public Task<ElectionType> GetElectionType(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ElectionType>> GetElectionTypes()
        {
            throw new NotImplementedException();
        }

        public Task<PoliticalParty> GetPoliticalParty(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PoliticalParty>> GetPoliticalPartyList()
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RegisterCandidate(Candidate candidate)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RemovePoliticalParty(PoliticalParty politicalParty)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateCandidate(Candidate candidate)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateElectionPosition(ElectionPosition position)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateElectionType(ElectionType type)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdatePoliticalParty(PoliticalParty politicalParty)
        {
            throw new NotImplementedException();
        }
    }
}
