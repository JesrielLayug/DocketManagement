using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketFeatureService
    {
        Task<IEnumerable<DocketFavorite>> GetUserFavoriteDockets(string userId);
        Task<DocketFavorite> GetExistingFavoriteDocket(string userId, string docketId);
        Task AddDocketToFavorite(DocketFavorite favorite);

        Task<IEnumerable<DocketRate>> GetAllRates();
        Task<IEnumerable<DocketRate>> GetUserRatedDocket(string userId);
        Task<DocketRate> ExistingUserRated(string userId, string docketId);
        Task<DocketRate> GetExistingRatedOfUser(string userId, string docketId);
        Task AddRateToDocket(DocketRate rate);
        Task UpdateRateToDocket(DocketRate rate);
    }
}
