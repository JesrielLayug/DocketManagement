using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketFavoriteService
    {
        Task<IEnumerable<Favorite>> GetAll();
        Task<IEnumerable<Favorite>> GetByUserId(string userId);
        Task<Favorite> GetExisting(string userId, string docketId);
        Task Add(Favorite favorite);
        Task Update(Favorite favorite);
        Task Remove(string docketId, string userId);
    }
}
