using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketFeatureService
    {
        Task<IEnumerable<DocketFavorite>> GetUserFavoriteDockets(string userId);
        Task<DocketFavorite> GetExistingFavoriteDocket(string userId, string docketId);

        Task<IEnumerable<DocketRate>> GetAllRates();
        Task<IEnumerable<DocketRate>> GetUserRatedDocket(string userId);
        Task<DocketRate> GetByDocketIdFromRate(string id);
        Task AddDocketToFavorite(DocketFavorite favorite);
        Task AddRateToDocket(DocketRate rate);
        Task UpdateRateToDocket(string docketId, DocketRate rate);
    }
}
