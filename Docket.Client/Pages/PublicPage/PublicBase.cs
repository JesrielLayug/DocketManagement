using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;

namespace Docket.Client.Pages.PublicPage
{
    public class PublicBase : ComponentBase
    {
        [Inject] IDocketService DocketService { get; set; }

        public IEnumerable<DTODocket> Dockets { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetAll();
        }
    }
}
