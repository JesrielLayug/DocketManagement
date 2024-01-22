using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketFeatureService
    {
        Task<IEnumerable<DocketRate>> GetDocketRates();
        Task<IEnumerable<DocketFavorite>> GetDocketFavorites();
        Task AddDocketToFavorite(DocketFavorite favorite);
        Task RateDocket(DocketRate docketRate);
    }
}
