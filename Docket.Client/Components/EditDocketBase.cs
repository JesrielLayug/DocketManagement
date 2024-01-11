using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Docket.Client.Components
{
    public class EditDocketBase : ComponentBase
    {
        [Inject] private IDialogService dialogService { get; set; }
        [CascadingParameter] MudDialogInstance dialog { get; set; }
        [Inject] public IAuthenticationService authenticationService { get; set; }

        public DTODocket Docket = new DTODocket();
        public bool isLoading = false;

        public async Task SaveOnClick(EditContext context)
        {
            throw new NotImplementedException();
        }
    }
}
