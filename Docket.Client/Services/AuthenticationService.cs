using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorageService;

        public AuthenticationService(
            HttpClient httpClient, 
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorageService
            )
        {
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorageService = localStorageService;
        }

        public async Task<Response> Login(DTOUserLogin request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/Authentication/login", request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    if(token != null)
                    {
                        await localStorageService.SetItemAsync("token", token);
                        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
                        if(state != null)
                        {
                            return new Response
                            {
                                isSuccess = true,
                                message = "Successfully logged in user"
                            };
                        }
                    }
                }
                return new Response
                {
                    isSuccess = false,
                    message = "Wrong email or password."
                };
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<Response> Register(DTOUserRegister request)
        {
            try
            {
                var response = await this.httpClient.PostAsJsonAsync("api/Authentication/register", request);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new Response
                    {
                        isSuccess = true,
                        message = "Successfully register user",
                        statusCode = response.StatusCode
                    };
                }

                return new Response
                {
                    isSuccess = false,
                    message = "Failed to register user",
                    statusCode = response.StatusCode
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
