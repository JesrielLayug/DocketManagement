using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketFeatureService
    {
        Task<IEnumerable<DocketFavorite>> GetAllFavorites();
        Task<DocketFavorite> GetByDocketId(string id);
        Task AddDocketToFavorite(DocketFavorite favorite);
    }
}
