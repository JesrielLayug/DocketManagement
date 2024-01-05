using Docket.Server.Models;

namespace Docket.Server.Services.Contracts
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> GetById(string id);
        Task<User> GetByName(string name);
        Task Create(User user);
        Task Update(string id, User user);
        Task Remove(string id);
    }
}
