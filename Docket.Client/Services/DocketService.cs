using Blazored.LocalStorage;
using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketService : IDocketService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorageService;

        public DocketService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorageService = localStorageService;
        }

        public async Task<Response> Add(DTODocket docket)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<DTODocket>("api/Docket/Create", docket);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return new Response
                    {
                        isSuccess = true,
                        statusCode = response.StatusCode,
                        message = "Successfully added the docket."
                    };
                }

                return new Response
                {
                    isSuccess = false,
                    statusCode = response.StatusCode,
                    message = "Failed to add the docket."
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Response
                {
                    isSuccess = false,
                    message = "Internal server error."
                };
            }
        }

        public async Task<IEnumerable<DTODocket>> GetAll()
        {
            try
            {
                var dockets = await httpClient.GetFromJsonAsync<IEnumerable<DTODocket>>("api/Docket/GetAllPublic");
                return dockets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<DTODocket>> GetUserDocket()
        {
            try
            {
                 return await httpClient.GetFromJsonAsync<IEnumerable<DTODocket>>("api/Docket/GetUserDockets");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
