using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using MongoDB.Driver;

namespace Docket.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> users;

        public UserService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task Create(User user)
        {
            await users.InsertOneAsync(user);
        }

        public async Task<List<User>> GetAll()
        {
            return await users.Find(user => true).ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await users.Find(user => user.id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByName(string name)
        {
            return await users.Find(user => user.name == name).FirstOrDefaultAsync();
        }

        public async Task Remove(string id)
        {
            await users.DeleteOneAsync(user => user.id == id);
        }

        public async Task Update(string id, User user)
        {
            await users.ReplaceOneAsync(user => user.id == id, user);
        }
    }
}
