using Docket.Client.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace Docket.Client.Pages.MyCollectionPage
{
    public class MyCollectionBase
    {
        [Inject] IDocketRateService RateService { get; set; }
        [Inject] IDocketFavoriteService FavoriteService { get; set; }
    }
}
