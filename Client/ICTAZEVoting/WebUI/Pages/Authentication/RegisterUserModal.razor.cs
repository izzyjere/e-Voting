
using ICTAZEVoting.Shared.Requests;
using ICTAZEVoting.Shared.Responses.Identity;

using Microsoft.AspNetCore.Components;

using MudBlazor;


namespace ICTAZEVoting.WebUI.Pages.Authentication;

public partial class RegisterUserModal
{


    private readonly RegisterRequest _registerUserModel = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    List<RoleResponse> roleResponses = new List<RoleResponse>();
    [Inject] IRoleManager roleManager { get; set; }
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    protected override async Task OnInitializedAsync()
    {
        var res = await roleManager.GetRolesAsync();
        roleResponses = res.Data;
    }

    private async Task SubmitAsync()
    {

    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}