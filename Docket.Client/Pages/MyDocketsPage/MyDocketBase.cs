using Docket.Client.Components;
using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Docket.Client.Pages.MyDocketsPage
{
    public class MyDocketBase : ComponentBase
    {
        [Inject] IDocketService DocketService { get; set; }
        [Inject] private IDialogService dialogService { get; set; }

        public IEnumerable<DTODocket> Dockets;

        public bool isLoading = true;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Dockets = await DocketService.GetUserDocket();
                isLoading = false;
                StateHasChanged();
            }
        }

        public void OpenEditDocketForm()
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

            dialogService.Show<EditDocket>("EditDocket", parameters, options);
        }
    }
}
