using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using Docket.Shared;
using MongoDB.Driver;

namespace Docket.Server.Services
{
    public class DocketRateService : IDocketRateService
    {
        private readonly IMongoCollection<Favorite> _docketFavorite;
        private readonly IMongoCollection<Rate> _docketRate;
        public DocketRateService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _docketFavorite = database.GetCollection<Favorite>(settings.DocketFavoriteCollectionName);
            _docketRate = database.GetCollection<Rate>(settings.DocketRateCollectionName);
        }


        public async Task Add(Rate rate)
        {
            await _docketRate.InsertOneAsync(rate);
        }

        public async Task<IEnumerable<Rate>> GetAll()
        {
            return await _docketRate.Find(d => true).ToListAsync();
        }

        public async Task Update(Rate rate)
        {
            await _docketRate.ReplaceOneAsync(d => d.DocketId == rate.DocketId && d.UserId == rate.UserId, rate);
        }

        public async Task<IEnumerable<Rate>> GetByUserId(string userId)
        {
            return await _docketRate.Find(d => d.UserId == userId).ToListAsync();
        }

        public async Task<Rate> GetExisting(string userId, string docketId)
        {
            return await _docketRate.Find(d => d.UserId == userId && d.DocketId == docketId).FirstOrDefaultAsync();
        }
    }
}
