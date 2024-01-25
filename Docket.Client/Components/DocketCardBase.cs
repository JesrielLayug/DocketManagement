using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Docket.Client.Components
{
    public class DocketCardBase : ComponentBase
    {
        [Parameter] public IEnumerable<DTODocket> Dockets { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] IDocketService DocketService { get; set; }
        [Inject] IDocketFavoriteService FavoriteService { get; set; }

        public bool isFavorite { get; set; } = false;


        public static char GetFirstLetterOFUser(string name)
        {
            string fiteredName = new string(name
                .Where(char.IsLetter)
                .ToArray());

            return char.ToUpper(fiteredName[0]);
        }

        public async Task RefreshCard()
        {
            Dockets = await DocketService.GetAll();
            StateHasChanged();
        }

        #region AddRating Click

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

        #endregion

        #region AddFavorite Click


        public async Task AddToFavorite(DTOFeatureFavorite favorite)
        {
            isFavorite = !isFavorite;
            favorite.IsFavorite = isFavorite;

            Response response;

            if (favorite.IsFavorite == true)
            {
                response = await FavoriteService.Add(favorite);
                await RefreshCard();
            }
            else
            {
                response = await FavoriteService.Remove(favorite.DocketId);
                await RefreshCard();
            }
            

            if (response.isSuccess)
                Response(response.message, Severity.Success);
            else
                Response(response.message, Severity.Error);
        }

        public Color SetIconColor(bool isFavorite)
        {
            return isFavorite ? Color.Warning : Color.Inherit;
        }


        #endregion

        #region Response

        private void Response(string message, Severity severity)
        {
            StateHasChanged();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
            Snackbar.Add(message, severity);
        }

        #endregion
    }
}
