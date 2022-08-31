using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using ICTAZEVoting.Core.Models;
using ICTAZEVoting.Shared.Responses.Identity;
using Microsoft.AspNetCore.Components;

namespace ICTAZEVoting.WebUI.Pages.Authentication
{
    public partial class Users
    {
        [Inject] IUserManager userManager { get; set; }
        private List<UserResponse> _userList = new();
        private UserResponse _user = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;       
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await authenticationService.CurrentUser();
            

            await GetUsersAsync();
            _loaded = true;
        }

        private async Task GetUsersAsync()
        {
            var result = await userManager.GetUsers();
            if(result.Succeeded)
            {
                _userList = result.Data;
            }
        }

        private bool Search(UserResponse user)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (user.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        void ManageRoles(string id)
        {
            Navigation.NavigateTo($"user-roles/{id}");
        }  
        void ViewProfile(string id)
        {
            Navigation.NavigateTo($"user-profile/{id}");
        }
        private async Task ExportToExcel()
        {
            
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = dialogService.Show<RegisterUserModal>("Register New User", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetUsersAsync();
            }
        }        
    }
}