using Docket.Client.Services.Contracts;
using Docket.Shared;
using System.Net.Http.Json;

namespace Docket.Client.Services
{
    public class DocketService : IDocketService
    {
        private readonly HttpClient httpClient;

        public DocketService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<DTODocket>> GetAll()
        {
            try
            {
                var dockets = await httpClient.GetFromJsonAsync<IEnumerable<DTODocket>>("api/Docket/GetAll");
                return dockets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
