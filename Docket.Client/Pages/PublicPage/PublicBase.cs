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

        public IEnumerable<DTODocket> Dockets;

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetAll();
            StateHasChanged();
        }
    }
}
