
using ICTAZEVoting.Shared.Requests;

using Microsoft.AspNetCore.Components;

using MudBlazor;


namespace ICTAZEVoting.WebUI.Pages.Authentication;

public partial class RoleModal
{
    [Inject] private IRoleManager RoleManager { get; set; }

    [Parameter] public RoleRequest RoleModel { get; set; } = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }       

    public void Cancel()
    {
        MudDialog.Cancel();
    }
    private async Task SaveAsync()
    {
        var result = await RoleManager.SaveRole(RoleModel);
        if(result.Succeeded)
        {
            snackBar.Add(result.Messages.First(), Severity.Success);
            MudDialog.Close();
        }
        else
        {
            snackBar.Add(result.Messages.First(), Severity.Error);
        }
    }
}