using Docket.Server.Data;
using Docket.Server.Models;
using Docket.Server.Services.Contracts;
using MongoDB.Driver;

namespace Docket.Server.Services
{
    public class DocketFeatureService : IDocketFeatureService
    {
        private readonly IMongoCollection<DocketRate> _docketRate;
        private readonly IMongoCollection<DocketFavorite> _docketFavorite;
        public DocketFeatureService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _docketRate = database.GetCollection<DocketRate>(settings.DocketRateCollectionName);
            _docketFavorite = database.GetCollection<DocketFavorite>(settings.DocketFavoriteCollectionName);
        }

        public async Task<IEnumerable<DocketFavorite>> GetDocketFavorites()
        {
             return await _docketFavorite.Find(d => true).ToListAsync();
        }

        public async Task<IEnumerable<DocketRate>> GetDocketRates()
        {
            return await _docketRate.Find(d => true).ToListAsync();
        }

        public async Task AddDocketToFavorite(DocketFavorite favorite)
        {
            await _docketFavorite.InsertOneAsync(favorite);
        }

        public async Task RateDocket(DocketRate docketRate)
        {
            await _docketRate.InsertOneAsync(docketRate);
        }
    }
}
