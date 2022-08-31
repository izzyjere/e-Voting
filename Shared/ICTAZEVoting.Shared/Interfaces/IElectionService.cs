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
        Task<ElectionResponse> GetCurrentElection();
        Task<List<ElectionType>> GetElectionTypes();
        Task<List<ElectionResponse>> GetPendingElections();
        Task<ElectionType> GetElectionType(string id);
        Task<Candidate> GetCandidate(string id);
        Task<List<CandidateResponse>> GetCandidates(string electionId);
        Task<IResult> DeleteCandidate(string id);
        Task<IResult<string[]>> AddCandidate(Candidate candidate);
        Task<List<CandidateResponse>> GetCandidateList();
        Task<IResult> DisqualifyCandidate(string id);
        Task<IResult> UpdateCandidate(Candidate candidate);
        Task<IResult> AddPoliticalParty(PoliticalParty politicalParty);
        Task<IResult> RemovePoliticalParty(PoliticalParty politicalParty);
        Task<IResult> UpdatePoliticalParty(PoliticalPartyUpdateRequest politicalParty);
        Task<List<PoliticalPartyResponse>> GetPoliticalPartyList();
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
        Task<ElectionResponse> GetElection(string Id);
        Task<IResult> AddConstituency(Constituency election);
        Task<IResult> UpdateConstituency(Constituency election);
        Task<IResult> DeleteConstituency(string id);
        Task<List<Constituency>> GetConstituencyList();
        Task<Election> GetConstituency(string Id);
        Task<IResult> AddPollingStation(PollingStation pollingStation);
        Task<IResult> UpdatePolingStation(PollingStation polingStation);
        Task<IResult> DeletePolingStation(string id);
        Task<List<PollingStationResponse>> GetPollingStationList();
        Task<PollingStation> GetPollingStation(string Id);
        Task<int> GetNumberOfCandidates(string electionId);
        Task<int> GetNumberOfVoters(string electionId);
        Task<int> GetnumberOfPollingbooths(string elecitonId);
        Task<int> GetNumberOfconstituencies(string electionId);
        Task<int> GetNumberOfConstituencies(string elecitonId);

    }
}
