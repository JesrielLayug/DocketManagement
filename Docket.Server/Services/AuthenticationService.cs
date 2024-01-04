using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using MongoDB.Driver;

namespace Docket.Server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMongoCollection<User> users;

        public AuthenticationService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task Register(User user)
        {
            await users.InsertOneAsync(user);
        }
    }
}
