using Docket.Client.Utilities;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Win32;
using MudBlazor;

namespace Docket.Client.Pages.Login
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        private IDialogService DialogService { get; set; }

        public DTOUserLogin user = new DTOUserLogin();

        public UtilityShowPassword showPassword = new UtilityShowPassword();

        public void Login()
        {
            throw new NotImplementedException();
        }

        public void OpenRegisterForm()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };
            //DialogService.Show<Register>("Register", options);
        }

        public void ShowPasswordClick()
        {
            showPassword.Toggle();
        }
    }
}
