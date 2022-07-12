global using ICTAZEVoting.Shared.Interfaces;
global using ICTAZEVoting.Shared.Models;
global using ICTAZEVoting.Shared.Wrapper;
global using ICTAZEVoting.Shared.Constants;
global using System.Net.Http.Json;
global using ICTAZEVoting.Extensions;
using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Requests;

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

        public async Task<List<PoliticalParty>> GetPoliticalPartyList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetPoliticalParties);
            var list = new List<PoliticalParty>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<PoliticalParty>>();
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

        public async Task<List<Election>> GetElectionList()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetElection);
            var list = new List<Election>();
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<Election>>();
                list = res.Data;
            }
            return list;
        }

        public Task<IResult> AddConstituency(Election election)
        {
            throw new NotImplementedException();
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

        public Task<IResult> UpdateConstituency(Election election)
        {
            throw new NotImplementedException();
        }

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
    }
}
