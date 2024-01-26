using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Docket.Client.Components
{
    public class EditDocketBase : ComponentBase
    {
        [Inject] ISnackbar Snackbar { get; set; }
        [CascadingParameter] MudDialogInstance Dialog { get; set; }
        [Inject] IDocketService DocketService { get; set; }

        [Parameter] public DTODocketUpdate DocketUpdate { get; set; } = new DTODocketUpdate();

        public DTODocketCreate DocketCreate = new DTODocketCreate();

        public DTODocket Docket = new DTODocket();

        public bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            if(DocketUpdate != null)
            {
                Docket.Id = DocketUpdate.Id;
                Docket.Title = DocketUpdate.Title;
                Docket.Body = DocketUpdate.Body;
                Docket.IsPublic = DocketUpdate.IsPublic;
            }
        }

        public async Task SaveOnClick()
        {
            isLoading = true;

            if(DocketUpdate != null)
            {
                DocketUpdate.Id = Docket.Id;
                DocketUpdate.Title = Docket.Title;
                DocketUpdate.Body = Docket.Body;
                DocketUpdate.IsPublic = Docket.IsPublic;

                await Update(DocketUpdate);
            }
            else
            {
                DocketCreate.Title = Docket.Title;
                DocketCreate.Body = Docket.Body;
                DocketCreate.IsPublic = Docket.IsPublic;

                await Create(DocketCreate);
            }
        }

        private async Task Create(DTODocketCreate docket)
        {
            var response = await DocketService.Add(docket);

            if (response.isSuccess)
            {
                isLoading = false;

                Dialog.Close(DialogResult.Ok(true));

                Response(response.message, Severity.Success);
            }
            else
            {
                Response(response.message, Severity.Error);
            }
        }

        private async Task Update(DTODocketUpdate docket)
        {
            var response = await DocketService.Update(docket);

            if (response.isSuccess)
            {
                isLoading = false;

                Dialog.Close(DialogResult.Ok(true));

                Response(response.message, Severity.Success);
            }
            else
            {
                Response(response.message, Severity.Error);
            }
        }

        private void Response(string message, Severity severity)
        {
            StateHasChanged();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
            Snackbar.Add(message, severity);
        }
    }
}
