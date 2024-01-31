using Docket.Client.Services.Contracts;
using Docket.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Collections;

namespace Docket.Client.Pages.UserPage
{
    public class UserBase : ComponentBase
    {
        [Inject] IUserService UserService { get; set; }
        [Inject] IDocketFavoriteService FavoriteService { get; set; }
        [Inject] IDocketRateService RateService { get; set; }
        [Inject] IDocketService DocketService { get; set; }


        public IEnumerable<DTOUser> Users;
        public IEnumerable<DTODocket> FavoriteDockets;
        public IEnumerable<DTODocket> RatedDockets;
        public IEnumerable<DTODocket> Dockets;


        public bool isLoading = true;

        public readonly ChartOptions _options = new();

        protected async override Task OnInitializedAsync()
        {
            await InitializeAllDockets();

            InitializeDonutChart();

            await InitializeLineChart();

            _options.YAxisTicks = 1;
            _options.InterpolationOption = InterpolationOption.EndSlope;

            isLoading = false;
            StateHasChanged();
        }

        private async Task InitializeAllDockets()
        {
            Users = await UserService.GetAll();
            FavoriteDockets = await FavoriteService.GetAllFavorites();
            RatedDockets = await RateService.GetAllRated();
            Dockets = await DocketService.GetUserDocket();
        }



        #region Line Chart

        public List<ChartSeries> Series = new List<ChartSeries>();
        public string[] XAxisLabels = new string[7];

        private async Task InitializeLineChart()
        {
            DateTime previousDay;

            // Create a dictionary to store data for each docket
            Dictionary<string, List<double>> docketDataMapping = new Dictionary<string, List<double>>();

            for (int i = 6; i >= 0; i--)
            {
                previousDay = DateTime.Now.AddDays(-i);
                XAxisLabels[i] = previousDay.ToString("MMM d");

                IEnumerable<DTOFavoriteReport> docketsFrom7Days = await FavoriteService.GetAverageFavorite(previousDay.ToString("MM/dd/yyyy"));

                foreach (var docket in docketsFrom7Days)
                {
                    if (!docketDataMapping.ContainsKey(docket.Title))
                    {
                        docketDataMapping[docket.Title] = new List<double>();
                    }

                    docketDataMapping[docket.Title].Add(docket.SumOfFavoritesForDay);
                }
            }

            // Convert the dictionary to the Series list
            foreach (var kvp in docketDataMapping)
            {
                Series.Add(new ChartSeries
                {
                    Name = kvp.Key,
                    Data = kvp.Value.ToArray()
                });
            }
        }


        #endregion

        #region Donut Chart

        public double[] data;
        public string[] labels;

        private void InitializeDonutChart()
        {
            labels = RatedDockets.Select(r => r.Title).ToArray();
            data = RatedDockets.Select(r => r.AverageRating).ToArray();
        }

        #endregion
    }
}
