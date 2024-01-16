using Docket.Client.Services.Contracts;
using Docket.Client.Utilities;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Docket.Client.Pages.RegisterPage
{
    public class RegisterBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [CascadingParameter] MudDialogInstance dialog { get; set; }
        [Inject] public IAuthenticationService authService { get; set; }

        public static DTOUserRegister user = new DTOUserRegister();
        public UtilityShowPassword showPassword = new UtilityShowPassword();
        public bool isLoading = false;


        public void ShowPasswordOnClick() =>  showPassword.Toggle();

        public async Task RegisterOnClick(EditContext context)
        {
            isLoading = true;
            var response = await authService.Register(user);
            if(response.isSuccess)
            {
                isLoading = false;

                dialog.Close(DialogResult.Ok(true));

                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add(response.message, Severity.Success);

                StateHasChanged();
            }
            else
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add(response.message, Severity.Error);
            }
        }
    }
}
