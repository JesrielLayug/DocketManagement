using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketFeatureService
    {
        Task<IEnumerable<DocketFavorite>> GetAllFavorites();
        Task<IEnumerable<DocketRate>> GetAllRates();
        Task<IEnumerable<DocketRate>> GetUserRatedDocket(string userId);
        Task<DocketFavorite> GetByDocketIdFromFavorite(string id);
        Task<DocketRate> GetByDocketIdFromRate(string id);
        Task AddDocketToFavorite(DocketFavorite favorite);
        Task AddRateToDocket(DocketRate rate);
        Task UpdateRateToDocket(string docketId, DocketRate rate);
    }
}
