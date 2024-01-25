using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketFeatureService : IDocketFeatureService
    {
        private readonly HttpClient httpClient;

        public DocketFeatureService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Response> AddRating(DTOFeatureRate rating)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/DocketFeature/AddRateToDocket", rating);
                if (response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = true,
                        message = "Rate successfully added."
                    };
                }

                return new Response
                {
                    isSuccess = false,
                    message = "Failed to add rating to docket."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return new Response { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<Response> AddToFavorite(DTOFeatureFavorite favorite)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/DocketFeature/AddDocketToFavorite", favorite);
                if (response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = true,
                        message = "Docket successfully added to favorite"
                    };
                }
                return new Response
                {
                    isSuccess = false,
                    message = "Failed adding the docket to favorite"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response { isSuccess = false, message = e.Message };
            }
        }

        public async Task<DTOFeatureRate> GetCurrentDocketRate(DTOFeatureRate request)
        {
            try
            {
                var docketWithRate = await httpClient.GetFromJsonAsync<DTOFeatureRate>($"api/DocketFeature/GetDocketRate/{request.DocketId}");
                return docketWithRate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<DTOFeatureRate> GetUserCurrentRateToDocket(string docketId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<DTOFeatureRate>($"api/DocketFeature/GetUserCurrentRateToDocket/{docketId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
} 
