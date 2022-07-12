using Microsoft.AspNetCore.Components;

using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Requests;

namespace ICTAZEVoting.WebUI.Pages.Authentication
{
    public partial class Roles
    {
        private List<RoleResponse> _roleList = new();
        private RoleResponse _role = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _loaded = false;

        private ClaimsPrincipal _currentUser;        

        protected override async Task OnInitializedAsync()
        {
          
            await GetRolesAsync();
            
        }

        private async Task GetRolesAsync()
        {
           
        }

        private async Task Delete(string id)
        {
            
        }

        private async Task InvokeModal(string id = null)
        {
            var parameters = new DialogParameters();
            if (id != null)
            {
                _role = _roleList.FirstOrDefault(c => c.Id == Guid.Parse(id));
                if (_role != null)
                {
                    parameters.Add(nameof(RoleModal.RoleModel), new RoleRequest
                    {
                        Id = _role.Id.ToString(),
                        Name = _role.Name,
                        Description = _role.Description
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = dialogService.Show<RoleModal>(id == null ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _role = new RoleResponse();
            await GetRolesAsync();
        }

        private bool Search(RoleResponse role)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (role.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (role.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }        
    }
}