using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient httpClient;

        public RegisterService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
