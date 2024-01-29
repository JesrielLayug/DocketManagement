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
        [Inject] IDialogService DialogService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        public IEnumerable<DTODocket> Dockets;

        public bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetUserDocket();
            isLoading = false;
            StateHasChanged();
        }

        public void ViewDocket(DTODocket docket)
        {
            var parameters = new DialogParameters<ViewDocketDialog>();
            parameters.Add(x => x.Docket, docket);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, NoHeader = true, FullWidth = false };

            DialogService.Show<ViewDocketDialog>("Docket", parameters, options);
        }

        public async Task AddDocket()
        {
            var parameters = new DialogParameters<EditDocket>();
            parameters.Add(x => x.DocketUpdate, null);

            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };

            var dialog = DialogService.Show<EditDocket>("EditDocket", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await RefreshTable();
            }
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

            var dialog = DialogService.Show<EditDocket>("EditDocket", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
               await RefreshTable();
            }
        }

        public async Task DeleteDocket(string docketId)
        {
            bool? result = await DialogService.ShowMessageBox(
                "Delete",
                "Deleting docket cannot be undone!",
                yesText: "Delete", cancelText: "Cancel"
                );

            if(result == true)
            {
                await DocketService.Delete(docketId);

                await RefreshTable();

                Response("Docket Deleted", Severity.Error);

            }


            StateHasChanged();
        }

        private async Task RefreshTable()
        {
            Dockets = await DocketService.GetUserDocket();
            StateHasChanged();
        }

        private void Response(string message, Severity severity)
        {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
            Snackbar.Add(message, severity);
        }

    }
}
