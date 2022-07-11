global using ICTAZEVoting.Shared.Models;
global using ICTAZEVoting.Shared.Requests;
global using ICTAZEVoting.Shared.Wrapper;
global using System.Collections.Generic;
global using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IElectionService
    {
        Task<Election> GetElection(string Id);
        Task<Election> GetCurrentElection();
        Task<List<ElectionType>> GetElectionTypes();
        Task<ElectionType> GetElectionType(string id);
        Task<Candidate> GetCandidate(string id);
        Task<IResult> DeleteCandidate(string id);
        Task<IResult> RegisterCandidate(Candidate candidate);
        Task<IResult> DisqualifyCandidate(string id);
        Task<IResult> UpdateCandidate(Candidate candidate);
        Task<IResult> AddPoliticalParty(PoliticalParty politicalParty);
        Task<IResult> RemovePoliticalParty(PoliticalParty politicalParty);
        Task<IResult> UpdatePoliticalParty(PoliticalPartyUpdateRequest politicalParty);
        Task<List<PoliticalParty>> GetPoliticalPartyList();
        Task<PoliticalParty> GetPoliticalParty(string id);
        Task<IResult> AddElectionType(string type);
        Task<IResult> UpdateElectionType(ElectionType type);
        Task<IResult> DeleteElectionType(string id);
        Task<IResult> AddElectionPosition(ElectionPosition position);
        Task<IResult> UpdateElectionPosition(ElectionPosition position);
        Task<IResult> DeleteElectionPosition(string id);
        Task<List<ElectionPosition>> GetElectionPositionList(string electionId);
        Task<List<ElectionPosition>> GetElectionPositionList();
        Task<IResult> DeletePoliticalParty(string id);
    }
}
