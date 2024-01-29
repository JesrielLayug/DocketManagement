using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class DocketCardBase : ComponentBase
    {
        [Parameter] public EventCallback OnFavoriteChanged { get; set; }
        [Parameter] public EventCallback OnRatingChanged { get; set; }
        [Parameter] public IEnumerable<DTODocket> Dockets { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] IDocketFavoriteService FavoriteService { get; set; }


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
                await OnRatingChanged.InvokeAsync(null);
        }


        public async Task AddToFavorite(DTOFeatureFavorite favorite)
        {
            favorite.IsFavorite = !favorite.IsFavorite;

            Response response;

            if (favorite.IsFavorite)
                response = await FavoriteService.Add(favorite);
            else
                response = await FavoriteService.Remove(favorite.DocketId);

            await OnFavoriteChanged.InvokeAsync(null);

            if (response.isSuccess)
                Response(response.message, Severity.Success);
            else
                Response(response.message, Severity.Error);
        }

        public Color SetIconColor(bool isFavorite)
        {
            return isFavorite ? Color.Warning : Color.Inherit;
        }

        private void Response(string message, Severity severity)
        {
            StateHasChanged();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
            Snackbar.Add(message, severity);
        }

    }
}
