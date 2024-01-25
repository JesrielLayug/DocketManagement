using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketRateService : IDocketRateService
    {
        private readonly HttpClient httpClient;

        public DocketRateService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Response> Add(DTOFeatureRate rating)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/Rate/AddRate", rating);
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

        public async Task<DTOFeatureRate> GetExisting(string docketId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<DTOFeatureRate>($"api/Rate/GetExisting/{docketId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
} 
