using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
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

        public async Task<IEnumerable<DocketFavorite>> GetAllFavorites()
        {
             return await _docketFavorite.Find(d => true).ToListAsync();
        }


        public async Task AddDocketToFavorite(DocketFavorite favorite)
        {
            await _docketFavorite.InsertOneAsync(favorite);
        }

        public async Task<DocketFavorite> GetByDocketIdFromFavorite(string id)
        {
            return await _docketFavorite.Find(docket => docket.DocketId == id).FirstOrDefaultAsync();
        }

        public async Task AddRateToDocket(DocketRate rate)
        {
            await _docketRate.InsertOneAsync(rate);
        }

        public async Task<IEnumerable<DocketRate>> GetAllRates()
        {
            return await _docketRate.Find(d => true).ToListAsync();
        }

        public async Task UpdateRateToDocket(string docketId, DocketRate rate)
        {
            await _docketRate.ReplaceOneAsync(d => d.DocketId == docketId, rate);
        }

        public async Task<DocketRate> GetByDocketIdFromRate(string id)
        {
            return await _docketRate.Find(d => d.DocketId == id).FirstOrDefaultAsync();
        }
    }
}
