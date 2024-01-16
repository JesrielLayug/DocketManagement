using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Docket.Client.Components
{
    public class EditDocketBase : ComponentBase
    {
        [Inject] private ISnackbar Snackbar { get; set; }
        [CascadingParameter] MudDialogInstance Dialog { get; set; }
        [Inject] public IDocketService DocketService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
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

            Response response = new Response();

            if(DocketUpdate != null)
            {
                DocketUpdate.Id = Docket.Id;
                DocketUpdate.Title = Docket.Title;
                DocketUpdate.Body = Docket.Body;
                DocketUpdate.IsPublic = Docket.IsPublic;

                response = await DocketService.Update(DocketUpdate);
            }
            else
            {
                DocketCreate.Title = Docket.Title;
                DocketCreate.Body = Docket.Body;
                DocketCreate.IsPublic = Docket.IsPublic;

                response = await DocketService.Add(DocketCreate);
            }

            if (response.isSuccess)
            {
                isLoading = false;

                Dialog.Close(DialogResult.Ok(true));

                NavigationManager.NavigateTo(NavigationManager.Uri, true);

                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add(response.message, Severity.Success);
            }
            else
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add(response.message, Severity.Error);
            }
        }
    }
}
