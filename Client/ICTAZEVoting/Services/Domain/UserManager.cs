using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Services.Domain;

internal class UserManager : IUserManager
{      
    readonly HttpClient _httpClient;
    public UserManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public Task<IResult> Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IResult<string>> GetProfilePictureAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult<UserResponse>> GetUser(string id)
    {
        var get = await _httpClient.GetAsync(ApiEndpoints.GetUsers + $"/{id}");
        return await get.ToResult<UserResponse>();
    }

    public async Task<List<UserRoleModel>> GetUserRoles(string id)
    {
        var get = await _httpClient.GetAsync(ApiEndpoints.GetUserRoles+$"/{id}");            
        var res = await get.ToResult<UserRolesResponse>();
        return res.Data.UserRoles;

    }

    public async Task<IResult<List<UserResponse>>> GetUsers()
{
        var get = await _httpClient.GetAsync(ApiEndpoints.GetUsers);
        return await get.ToResult<List<UserResponse>>();
    }

    public Task<IResult> RegisterUser(RegisterRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> Update(UserResponse record)
    {
        throw new NotImplementedException();
    }
}
