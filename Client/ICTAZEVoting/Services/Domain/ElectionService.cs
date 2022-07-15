global using ICTAZEVoting.Shared.Interfaces;
global using ICTAZEVoting.Shared.Models;
global using ICTAZEVoting.Shared.Wrapper;
global using ICTAZEVoting.Shared.Constants;
global using System.Net.Http.Json;
global using ICTAZEVoting.Extensions;
using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Domain;
using Newtonsoft.Json;

namespace ICTAZEVoting.Services.Domain
{
    public class ElectionService : IElectionService
    {
        readonly HttpClient httpClient;
        public ElectionService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public Task<IResult> AddElectionPosition(ElectionPosition position)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> AddElectionType(string type)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.AddElectionType, type);
            return await add.ToResult();
        }
        public async Task<IResult<string[]>> AddCandidate(Candidate candidate)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.AddCandidate, candidate);
            return await add.ToResult<string[]>();
        }
        public async Task<List<CandidateResponse>> GetCandidateList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetCandidates);
            var list = new List<CandidateResponse>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<CandidateResponse>>();
                list = res.Data;
            }
            return list;
        }
        public async Task<IResult> AddPoliticalParty(PoliticalParty politicalParty)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.AddPoliticalParty, politicalParty);
            return await add.ToResult();
        }

        public Task<IResult> DeleteCandidate(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteElectionPosition(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<IResult> DeletePoliticalParty(string id)
        {
            var delete = await httpClient.DeleteAsync($"{ApiEndpoints.DeletePoliticalParty}/{id}");
            if (delete.IsSuccessStatusCode)
            {
                return await delete.ToResult();
            }
            return Result.Fail("An error occured. Check your internet connection.");
        }

        public async Task<IResult> DeleteElectionType(string id)
        {
            var delete = await httpClient.DeleteAsync($"{ApiEndpoints.DeleteElectionType}/{id}");
            if (delete.IsSuccessStatusCode)
            {
                return await delete.ToResult();
            }
            return Result.Fail("An error occured. Check your internet connection.");
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

        public async Task<List<ElectionType>> GetElectionTypes()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetElectionTypes);
            var list = new List<ElectionType>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<ElectionType>>();
                list = res.Data;
            }
            return list;
        }

        public Task<PoliticalParty> GetPoliticalParty(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PoliticalPartyResponse>> GetPoliticalPartyList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetPoliticalParties);
            var list = new List<PoliticalPartyResponse>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<PoliticalPartyResponse>>();
                list = res.Data;
            }
            return list;
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

        public async Task<IResult> UpdateElectionType(ElectionType entity)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.EditElectionType, entity);
            return await add.ToResult();
        }

        public async Task<IResult> UpdatePoliticalParty(PoliticalPartyUpdateRequest politicalParty)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.EditPoliticalParty, politicalParty);
            return await add.ToResult();
        }

        public async Task<IResult> AddElection(Election election)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.AddElection, election);
            return await add.ToResult();
        }

        public async Task<IResult> UpdateElection(Election type)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.EditElection, type);
            return await add.ToResult();
        }

        public async Task<IResult> DeleteElection(string id)
        {
            var delete = await httpClient.DeleteAsync($"{ApiEndpoints.DeleteElection}/{id}");
            if (delete.IsSuccessStatusCode)
            {
                return await delete.ToResult();
            }
            return Result.Fail("An error occured. Check your internet connection.");
        }

        public async Task<List<ElectionResponse>> GetElectionList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetElections);
            var list = new List<ElectionResponse>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<ElectionResponse>>();
                list = res.Data;
            }
            return list;
        }

        public async Task<IResult> AddConstituency(Constituency constituency)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.AddConstituency, constituency);
            return await add.ToResult();
        }

        public async Task<IResult> UpdateConstituency(Constituency constituency)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.EditConstituency, constituency);
            return await add.ToResult();
        }


        public Task<Election> GetConstituency(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> DeleteConstituency(string id)
        {
            var delete = await httpClient.DeleteAsync($"{ApiEndpoints.DeleteConstituency}/{id}");
            if (delete.IsSuccessStatusCode)
            {
                return await delete.ToResult();
            }
            return Result.Fail("An error occured. Check your internet connection.");
        }
        /*
         public async Task<List<ElectionType>> GetElectionTypes()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetElectionTypes);
            var list = new List<ElectionType>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<ElectionType>>();
                list = res.Data;
            }
            return list;
        }
         */
        public async Task<List<Constituency>> GetConstituencyList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetConstituencies);
            var list = new List<Constituency>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<Constituency>>();
                list = res.Data;
            }
            return list;
        }

        public async Task<IResult> AddPolingStation(PollingStation polingStation)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.AddPollingStation, polingStation);
            return await add.ToResult();
        }

        public async Task<IResult> UpdatePolingStation(PollingStation polingStation)
        {
            var add = await httpClient.PostAsJsonAsync(ApiEndpoints.EditPollingStation, polingStation);
            return await add.ToResult();
        }

        public async Task<IResult> DeletePolingStation(string id)
        {
            var delete = await httpClient.DeleteAsync($"{ApiEndpoints.DeletePolingStation}/{id}");
            if (delete.IsSuccessStatusCode)
            {
                return await delete.ToResult();
            }
            return Result.Fail("An error occured. Check your internet connection.");
        }

        public async Task<List<PollingStationResponse>> GetPollingStationList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetPollingStations);
            var list = new List<PollingStationResponse>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<PollingStationResponse>>();
                list = res.Data;
            }
            return list;
        }

        public Task<PollingStation> GetPollingStation(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ElectionResponse>> GetPendingElections()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetPendingElections);
            var list = new List<ElectionResponse>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<ElectionResponse>>();
                list = res.Data;
            }
            return list;
        }

        public async Task<int> GetNumberOfCandidates(string electionId)
        {
            var get = await httpClient.GetAsync(ApiEndpoints.CountCandidates+$"/{electionId}");
            return JsonConvert.DeserializeObject<int>(await get.Content.ReadAsStringAsync());

        }

        public async Task<int> GetNumberOfVoters(string electionId)
        {
            var get = await httpClient.GetAsync(ApiEndpoints.CountVoters + $"/{electionId}");
            return JsonConvert.DeserializeObject<int>(await get.Content.ReadAsStringAsync());
        }

        public Task<int> GetnumberOfPollingbooths(string elecitonId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumberOfconstituencies(string electionId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumberOfConstituencies(string elecitonId)
        {
            throw new NotImplementedException();
        }
    }
}
