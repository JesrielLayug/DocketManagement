using Docket.Client.Components;
using Docket.Client.Pages.RegisterPage;
using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Pages.PublicPage
{
    public class PublicBase : ComponentBase
    {
        [Inject] IDocketService DocketService { get; set; }
        [Inject] private IDialogService dialogService { get; set; }

        public IEnumerable<DTODocket> Dockets;
        public bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Dockets = await DocketService.GetAll();
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
