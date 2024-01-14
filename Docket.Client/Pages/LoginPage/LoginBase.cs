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

        public bool isLoading = false;

        public string responseMessage = string.Empty;
        public bool showAlert = false;

        public async Task LoginOnClick(EditContext context)
        {
            isLoading = true;

            var response = await authService.Login(user);
            if (response.isSuccess)
            {
                isLoading = false;
                navigationManager.NavigateTo("/");
            }
            else
            {
                isLoading = false;
                showAlert = true;
                responseMessage = response.message;
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
