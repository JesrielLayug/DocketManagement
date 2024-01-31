using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketFavoriteService
    {
        Task<IEnumerable<DTODocket>> GetAllFavorites();
        Task<IEnumerable<DTOFeatureFavorite>> GetByDocketId(string docketId);
        Task<IEnumerable<DTOFavoriteReport>> GetAverageFavorite(string date);
        Task<Response> Add(DTOFeatureFavorite favorite);
        Task<Response> Remove(string docketId);
    }
}
