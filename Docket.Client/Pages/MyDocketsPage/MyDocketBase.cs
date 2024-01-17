using Docket.Client.Components;
using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace Docket.Client.Pages.MyDocketsPage
{
    public class MyDocketBase : ComponentBase
    {
        [Inject] IDocketService DocketService { get; set; }
        [Inject] IDialogService dialogService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        public IEnumerable<DTODocket> Dockets;

        public bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetUserDocket();
            isLoading = false;
            StateHasChanged();
        }

        public void AddDocket()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };
            dialogService.Show<EditDocket>("EditDocket", options);
        }

        public async Task UpdateDocket(DTODocket docket)
        {
            var docketTobeUpdate = new DTODocketUpdate
            {
                Id = docket.Id,
                Title = docket.Title,
                Body = docket.Body,
                IsPublic = docket.IsPublic
            };

            var parameters = new DialogParameters<EditDocket>();
            parameters.Add(x=> x.DocketUpdate, docketTobeUpdate);

            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };

            var dialog = dialogService.Show<EditDocket>("EditDocket", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Dockets = await DocketService.GetUserDocket();
                StateHasChanged();
            }
        }

        public async Task DeleteDocket(string docketId)
        {
            bool? result = await dialogService.ShowMessageBox(
                "Delete",
                "Deleting docket cannot be undone!",
                yesText: "Delete", cancelText: "Cancel"
                );

            if(result.Value == true)
            {
                await DocketService.Delete(docketId);
                Dockets = await DocketService.GetUserDocket();
                StateHasChanged();
            }
        }
    }
}
