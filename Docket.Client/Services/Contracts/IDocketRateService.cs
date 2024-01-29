using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketRateService
    {
        Task<IEnumerable<DTODocket>> GetAllRated();
        Task<DTOFeatureRate> GetExisting(string docketId);
        Task<Response> Add(DTOFeatureRate rating);
    }
}
