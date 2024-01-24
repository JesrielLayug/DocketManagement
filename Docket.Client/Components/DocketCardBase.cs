using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class DocketCardBase : ComponentBase
    {
        [Parameter] public IEnumerable<DTODocket> Dockets { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] IDocketService DocketService { get; set; }

        public static char GetFirstLetterOFUser(string name)
        {
            string fiteredName = new string(name
                .Where(char.IsLetter)
                .ToArray());

            return char.ToUpper(fiteredName[0]);
        }

        public async void AddRating(DTOFeatureRate rating)
        {
            var parameters = new DialogParameters<RatedDocketDialog>();
            parameters.Add(x => x.Rating, rating);

            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, NoHeader = true };

            var dialog = DialogService.Show<RatedDocketDialog>("RateDocket", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await RefreshCard();
            }
        }

        public async Task RefreshCard()
        {
            Dockets = await DocketService.GetAll();
            StateHasChanged();
        }
    }
}
