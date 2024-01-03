using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IUserService
    {
        Task<List<User>> Get();
        Task<User> Get(string id);
        Task Create(User user);
        Task Update(string id, User user);
        Task Remove(string id);
    }
}
