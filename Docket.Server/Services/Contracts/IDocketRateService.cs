using Docket.Server.Models;
using Docket.Shared;

namespace Docket.Server.Services.Contracts
{
    public interface IDocketRateService
    {
        Task<IEnumerable<Rate>> GetAll();
        Task<IEnumerable<Rate>> GetByUserId(string userId);
        Task<Rate> GetExisting(string userId, string docketId);
        Task Add(Rate rate);
        Task Update(Rate rate);
    }
}
