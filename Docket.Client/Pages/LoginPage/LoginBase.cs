using Docket.Client.Pages.RegisterPage;
using Docket.Client.Services.Contracts;
using Docket.Client.Utilities;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Docket.Client.Pages.LoginPage
{
    public class LoginBase : ComponentBase
    {
        [Inject] public IAuthenticationService authService { get; set; }
        [Inject] private IDialogService dialogService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }

        public DTOUserLogin user = new DTOUserLogin();

        public UtilityShowPassword showPassword = new UtilityShowPassword();

        public async Task LoginOnClick(EditContext context)
        {
            var response = await authService.Login(user);
            if (response.isSuccess)
            {
                navigationManager.NavigateTo("/");
            }
            else
            {
                await dialogService.ShowMessageBox(
                    "Warning",
                    (MarkupString)response.message,
                    yesText: "Ok"
                    );
            }
        }

        public void OpenRegisterForm()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };
            dialogService.Show<Register>("Register", options);
        }

        public void ShowPasswordClick()
        {
            showPassword.Toggle();
        }
    }
}
