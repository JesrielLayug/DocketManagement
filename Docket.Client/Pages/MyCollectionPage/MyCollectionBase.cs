using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;

namespace Docket.Client.Pages.MyCollectionPage
{
    public class MyCollectionBase : ComponentBase
    {
        [Inject] IDocketRateService RateService { get; set; }
        [Inject] IDocketFavoriteService FavoriteService { get; set; }

        public IEnumerable<DTODocket> RatedDockets;
        public IEnumerable<DTODocket> FavoriteDockets;

        public bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            RatedDockets = await RateService.GetAllRated();
            FavoriteDockets = await FavoriteService.GetAllFavorites();
            isLoading = false;
            StateHasChanged();
        }

        public async Task HandleFavoriteChanged()
        {
            RatedDockets = await RateService.GetAllRated();
            FavoriteDockets = await FavoriteService.GetAllFavorites();
        }

        public async Task HandleRatingChanged()
        {
            RatedDockets = await RateService.GetAllRated();
            FavoriteDockets = await FavoriteService.GetAllFavorites();
        }
    }
}
