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
        [Inject] private IDialogService dialogService { get; set; }

        public IEnumerable<DTODocket> Dockets = new List<DTODocket>();

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetUserDocket();
        }

        public void OpenEditDocketForm()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };
            dialogService.Show<EditDocket>("EditDocket", options);
        }
    }
}
