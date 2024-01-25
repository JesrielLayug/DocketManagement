using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using MongoDB.Driver;

namespace Docket.Server.Services
{
    public class DocketFavoriteService : IDocketFavoriteService
    {
        private readonly IMongoCollection<Favorite> _docketFavorite;
        public DocketFavoriteService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _docketFavorite = database.GetCollection<Favorite>(settings.DocketFavoriteCollectionName);
        }

        public async Task<IEnumerable<Favorite>> GetAll()
        {
            return await _docketFavorite.Find(f => true).ToListAsync();
        }

        public async Task<IEnumerable<Favorite>> GetByUserId(string userId)
        {
            return await _docketFavorite.Find(d => d.UserId == userId).ToListAsync();
        }

        public async Task<Favorite> GetExisting(string userId, string docketId)
        {
            return await _docketFavorite.Find(docket => docket.UserId == userId && docket.DocketId == docketId).FirstOrDefaultAsync();
        }

        public async Task Add(Favorite favorite)
        {
            await _docketFavorite.InsertOneAsync(favorite);
        }

        public async Task Remove(string docketId, string userId)
        {
            await _docketFavorite.DeleteOneAsync(f => f.DocketId == docketId && f.UserId == userId);
        }
    }
}
