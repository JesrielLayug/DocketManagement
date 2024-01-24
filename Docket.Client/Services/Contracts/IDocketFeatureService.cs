using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketFeatureService
    {
        Task<DTOFeatureRate> GetCurrentDocketRate(DTOFeatureRate request);
        Task<DTOFeatureRate> GetUserCurrentRateToDocket(string docketId);
        Task<Response> AddRating(DTOFeatureRate rating);
    }
}
