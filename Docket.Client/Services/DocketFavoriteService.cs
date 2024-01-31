using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketFavoriteService : IDocketFavoriteService
    {
        private readonly HttpClient httpClient;

        public DocketFavoriteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response> Add(DTOFeatureFavorite favorite)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/Favorite/Add", favorite);
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

        public async Task<IEnumerable<DTODocket>> GetAllFavorites()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IEnumerable<DTODocket>>("api/Favorite/GetByCurrentUser");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<DTOFavoriteReport>> GetAverageFavorite(string date)
        {
            try
            {
                var dateFormat = WebUtility.UrlEncode(date);

                return await httpClient.GetFromJsonAsync<IEnumerable<DTOFavoriteReport>>($"api/Favorite/GetAverageFavorite/{dateFormat}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine("API endpoint not found");
                return Enumerable.Empty<DTOFavoriteReport>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Enumerable.Empty<DTOFavoriteReport>();
            }
        }

        public async Task<IEnumerable<DTOFeatureFavorite>> GetByDocketId(string docketId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IEnumerable<DTOFeatureFavorite>>($"api/Favorite/GetByDocketId/{docketId}");
            }
            catch 
            {
                throw;
            }
        }

        public async Task<Response> Remove(string docketId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Favorite/Delete/{docketId}");
                if (response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = true,
                        message = "Docket successfully remove to favorite"
                    };
                }
                return new Response
                {
                    isSuccess = false,
                    message = "Failed removing the docket to favorite"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Response { isSuccess = false, message = e.Message };
            }
        }
    }
}
