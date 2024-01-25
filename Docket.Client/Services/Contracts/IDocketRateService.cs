using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketRateService
    {
        Task<DTOFeatureRate> GetExisting(string docketId);
        Task<Response> Add(DTOFeatureRate rating);
    }
}
