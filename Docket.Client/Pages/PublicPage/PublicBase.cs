using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;

namespace Docket.Client.Pages.PublicPage
{
    public class PublicBase : ComponentBase
    {
        [Inject] IDocketService DocketService { get; set; }

        public IEnumerable<DTODocket> Dockets { get; set; }

        private string searchString = null;

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetAll();
        }

        public void OnSearch(string text)
        {
            searchString = text;
        }
    }
}
