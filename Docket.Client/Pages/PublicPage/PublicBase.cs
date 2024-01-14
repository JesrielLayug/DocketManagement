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

        public IEnumerable<DTODocket> Dockets { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Dockets = await DocketService.GetAll();
        }

        //public void OpenEditDocketForm()
        //{
        //    var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader=true };
        //    dialogService.Show<EditDocket>("EditDocket", options);
        //}
    }
}
