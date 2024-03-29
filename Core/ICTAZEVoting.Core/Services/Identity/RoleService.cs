﻿using AutoMapper;

using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Wrapper;

using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ICTAZEVoting.Shared.Constants;

namespace ICTAZEVoting.Core.Services.Identity;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public RoleService(
        RoleManager<Role> roleManager,
        IMapper mapper,
        UserManager<User> userManager        )
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _userManager = userManager;        
       
    }
    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        var existingRole = await _roleManager.FindByIdAsync(id.ToString());
        if (existingRole.Name != RoleConstants.AdministratorRole && existingRole.Name != RoleConstants.BasicRole)
        {
            bool roleIsNotUsed = true;
            var allUsers = await _userManager.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                {
                    roleIsNotUsed = false;
                }
            }
            if (roleIsNotUsed)
            {
                await _roleManager.DeleteAsync(existingRole);
                return await Result<string>.SuccessAsync(string.Format("Role {0} Deleted.", existingRole.Name));
            }
            else
            {
                return await Result<string>.SuccessAsync(string.Format("Not allowed to delete {0} Role as it is being used.", existingRole.Name));
            }
        }
        else
        {
            return await Result<string>.SuccessAsync(string.Format("Not allowed to delete {0} Role.", existingRole.Name));
        }
    }
    public async Task<Result<List<RoleResponse>>> GetAllAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
        return await Result<List<RoleResponse>>.SuccessAsync(rolesResponse);
    }
    
   public async Task<Result<RoleResponse>> GetByIdAsync(Guid id)
    {
        var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
        var rolesResponse = _mapper.Map<RoleResponse>(roles);
        return await Result<RoleResponse>.SuccessAsync(rolesResponse);
    }

    public async Task<Result<string>> AddEditAsync(RoleRequest request)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            var existingRole = await _roleManager.FindByNameAsync(request.Name);
            if (existingRole != null) return await Result<string>.FailAsync("Similar Role already exists.");
            var response = await _roleManager.CreateAsync(new Role(request.Name, request.Description));
            if (response.Succeeded)
            {
                return await Result<string>.SuccessAsync(string.Format("Role {0} Created.", request.Name));
            }
            else
            {
                return await Result<string>.FailAsync(response.Errors.Select(e => e.Description.ToString()).ToList());
            }
        }
        else
        {
            var existingRole = await _roleManager.FindByIdAsync(request.Id);
            if (existingRole.Name == RoleConstants.AdministratorRole || existingRole.Name == RoleConstants.BasicRole)
            {
                return await Result<string>.FailAsync(string.Format("Not allowed to modify {0} Role.", existingRole.Name));
            }
            existingRole.Name = request.Name;
            existingRole.NormalizedName = request.Name.ToUpper();
            existingRole.Description = request.Description;
            await _roleManager.UpdateAsync(existingRole);
            return await Result<string>.SuccessAsync(string.Format("Role {0} Updated.", existingRole.Name));
        }
    }

    
    public async Task<int> GetCountAsync()
    {
        var count = await _roleManager.Roles.CountAsync();
        return count;
    }
}
