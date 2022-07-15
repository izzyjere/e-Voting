using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Responses.Domain;
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
        readonly HttpClient httpClient;
        public VoterService(HttpClient httpClient)
        {
            this.httpClient = httpClient;                                                                                                                                                     
        }
        public Task<IResult> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VoterResponse>> GetAll()
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetVoters);
            if(get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<List<VoterResponse>>();
                return res.Data;
            }
            return new();
        }

        public Task<Voter> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Voter> GetByUserId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<List<string>>> Register(Voter entity)
        {
            var result = await httpClient.PostAsJsonAsync(ApiEndpoints.AddVoter,entity);
            if(result.IsSuccessStatusCode)
            {
                return await result.ToResult<List<string>>();               
            }
            else
            {
                  return Result<List<string>>.Fail("An error occured. Try again");
            }
           
        }

        public async Task<IResult> Update(Voter entity)
        {
            var result = await httpClient.PostAsJsonAsync(ApiEndpoints.UpdateVoter, entity);
            if (result.IsSuccessStatusCode)
            {
                return await result.ToResult();
            }
            else
            {
                return Result.Fail("An error occured.");
            }
            
        }

        public Task<IResult> VerifyVoter(byte[] facialData)
        {
            throw new NotImplementedException();
        }
    }
}
