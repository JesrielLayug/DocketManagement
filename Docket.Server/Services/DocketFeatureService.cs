using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using MongoDB.Driver;

namespace Docket.Server.Services
{
    public class DocketFeatureService : IDocketFeatureService
    {
        private readonly IMongoCollection<DocketFavorite> _docketFavorite;
        private readonly IMongoCollection<DocketRate> _docketRate;
        public DocketFeatureService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _docketFavorite = database.GetCollection<DocketFavorite>(settings.DocketFavoriteCollectionName);
            _docketRate = database.GetCollection<DocketRate>(settings.DocketRateCollectionName);
        }

        public async Task<IEnumerable<DocketFavorite>> GetUserFavoriteDockets(string userId)
        {
             return await _docketFavorite.Find(d => d.UserId == userId).ToListAsync();
        }

        public async Task AddDocketToFavorite(DocketFavorite favorite)
        {
            await _docketFavorite.InsertOneAsync(favorite);
        }

        public async Task<DocketFavorite> GetExistingFavoriteDocket(string userId, string docketId)
        {
            return await _docketFavorite.Find(docket => docket.UserId == userId && docket.DocketId == docketId).FirstOrDefaultAsync();
        }

        public async Task AddRateToDocket(DocketRate rate)
        {
            await _docketRate.InsertOneAsync(rate);
        }

        public async Task<IEnumerable<DocketRate>> GetAllRates()
        {
            return await _docketRate.Find(d => true).ToListAsync();
        }

        public async Task UpdateRateToDocket(DocketRate rate)
        {
            await _docketRate.ReplaceOneAsync(d => d.DocketId == rate.Id, rate);
        }

        public async Task<IEnumerable<DocketRate>> GetUserRatedDocket(string userId)
        {
            return await _docketRate.Find(d => d.UserId == userId).ToListAsync();
        }

        public async Task<DocketRate> ExistingUserRated(string userId, string docketId)
        {
            var userRated = await _docketRate.Find(d => d.UserId == userId && d.DocketId == docketId).FirstOrDefaultAsync();
            return userRated;
        }

        public async Task<DocketRate> GetExistingRatedOfUser(string userId, string docketId)
        {
            return await _docketRate.Find(d => d.UserId == userId && d.DocketId == docketId).FirstOrDefaultAsync();
        }
    }
}
