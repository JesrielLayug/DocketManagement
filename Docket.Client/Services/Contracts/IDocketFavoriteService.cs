using Docket.Shared;

namespace Docket.Client.Services.Contracts
{
    public interface IDocketFavoriteService
    {
        Task<Response> Add(DTOFeatureFavorite favorite);
        Task<Response> Remove(string docketId);
    }
}
