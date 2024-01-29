using Docket.Client.Pages.UserPage;
using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<DTOUser>> GetAll()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IEnumerable<DTOUser>>("api/User/GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
