using Docket.Server.Data;
using Docket.Server.Services.Contracts;
using MongoDB.Driver;
using System;

namespace Docket.Server.Services
{
    public class DocketService : IDocketService
    {
        private readonly IMongoCollection<Models.Docket> dockets;

        public DocketService(IDocketDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            dockets = database.GetCollection<Models.Docket>(settings.DocketCollectionName);
        }

        public async Task<string> Add(Models.Docket request)
        {
            await dockets.InsertOneAsync(request);

            var docketId = request.Id.ToString();

            return docketId;
        }

        public async Task Delete(string id)
        {
            await dockets.DeleteOneAsync(docket => docket.Id == id);
        }

        public async Task<IEnumerable<Models.Docket>> GetAll()
        {
            return await dockets.Find(docket => true).ToListAsync();
        }

        public async Task<IEnumerable<Models.Docket>> GetAllPublic()
        {
            return await dockets.Find(docket => docket.IsPublic == true).ToListAsync();
        }

        public async Task<Models.Docket> GetById(string id)
        {
            return await dockets.Find(docket => docket.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Models.Docket>> GetByUserId(string userId)
        {
            return await dockets.Find(docket => docket.UserId == userId).ToListAsync();
        }

        public async Task Update(string id, Models.Docket docket)
        {
            await dockets.ReplaceOneAsync(docket => docket.Id == id, docket);
        }
    }
}
