using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketFavoriteService
    {
        Task<IEnumerable<DTODocket>> GetAllFavorites();
        Task<Response> Add(DTOFeatureFavorite favorite);
        Task<Response> Remove(string docketId);
    }
}
