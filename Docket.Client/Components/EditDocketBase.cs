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
        [Inject] private ISnackbar Snackbar { get; set; }
        [CascadingParameter] MudDialogInstance Dialog { get; set; }
        [Inject] public IDocketService DocketService { get; set; }


        public DTODocketCreate Docket = new DTODocketCreate();
        public bool isLoading = false;

        public async Task SaveOnClick(EditContext context)
        {
            isLoading = true;

            var response = await DocketService.Add(Docket);

            if (response.isSuccess)
            {
                isLoading = false;

                Dialog.Close(DialogResult.Ok(true));

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
