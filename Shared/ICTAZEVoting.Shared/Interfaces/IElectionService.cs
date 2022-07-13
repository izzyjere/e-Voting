global using ICTAZEVoting.Shared.Models;
global using ICTAZEVoting.Shared.Requests;
global using ICTAZEVoting.Shared.Wrapper;
global using System.Collections.Generic;
global using System.Threading.Tasks;

using ICTAZEVoting.Shared.Responses.Domain;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IElectionService
    {
        Task<Election> GetCurrentElection();
        Task<List<ElectionType>> GetElectionTypes();
        Task<ElectionType> GetElectionType(string id);
        Task<Candidate> GetCandidate(string id);
        Task<IResult> DeleteCandidate(string id);
        Task<IResult> AddCandidate(Candidate candidate);
        Task<List<Candidate>> GetCandidateList();
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
        Task<IResult> AddElection(Election election);
        Task<IResult> UpdateElection(Election election);
        Task<IResult> DeleteElection(string id);
        Task<List<ElectionResponse>> GetElectionList();
        Task<Election> GetElection(string Id);
        Task<IResult> AddConstituency(Constituency election);
        Task<IResult> UpdateConstituency(Constituency election);
        Task<IResult> DeleteConstituency(string id);
        Task<List<Constituency>> GetConstituencyList();
        Task<Election> GetConstituency(string Id);
        Task<IResult> AddPolingStation(PollingStation polingStation);
        Task<IResult> UpdatePolingStation(PollingStation polingStation);
        Task<IResult> DeletePolingStation(string id);
        Task<List<PollingStation>> GetPolingStationList();
        Task<PollingStation> GetPolingStation(string Id);

    }
}
