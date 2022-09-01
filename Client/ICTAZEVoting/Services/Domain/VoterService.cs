using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses;
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
        public async Task<IResult> Delete(string id)
        {
            var delete = await httpClient.DeleteAsync($"{ApiEndpoints.DeleteVoter}/{id}");
            if (delete.IsSuccessStatusCode)
            {
                return await delete.ToResult();
            }
            return Result.Fail("An error occured. Check your internet connection.");
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

        public async Task<VoterResponse> GetByUserId(string id)
        {
            var get = await httpClient.GetAsync(ApiEndpoints.GetVoterByUserId +$"/{id}");
            if (get.IsSuccessStatusCode)
            {
                var res = await get.ToResult<VoterResponse>();
                return res.Data;
            }
            return null;
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

        public async Task<IResult<VoterVerificationResponse>> VerifyVoter(VoterVerificationRequest request)
        {
            var post = await httpClient.PostAsJsonAsync(ApiEndpoints.VerifyVoter, request);
            if(post.IsSuccessStatusCode)
            {
                return await post.ToResult<VoterVerificationResponse>();
            }
            return Result<VoterVerificationResponse>.Fail("Could not connect to the server. Try again");
        }
    }
}
