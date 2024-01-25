using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class RateDocketDialogBase : ComponentBase
    {
        [Parameter] public DTOFeatureRate Rating {  get; set; }
        [CascadingParameter] MudDialogInstance Dialog { get; set; }
        [Inject] private IDocketRateService FeatureService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Rating = await FeatureService.GetExisting(Rating.DocketId);
            StateHasChanged();
        }

        public async Task Submit(DTOFeatureRate rate)
        {
            rate.DocketId = Rating.DocketId;
            var response = await FeatureService.Add(rate);

            if (response.isSuccess)
            {

                Dialog.Close(DialogResult.Ok(true));

                Response(response.message, Severity.Success);
            }
            else
            {
                Response(response.message, Severity.Error);
            }
        }

        private void Response(string message, Severity severity)
        {
            StateHasChanged();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
            Snackbar.Add(message, severity);
        }
    }
}
