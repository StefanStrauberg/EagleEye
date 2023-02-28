using EagleEye.Infrastructure.DatabaseConfig;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.Infrastructure.Repositories
{
    internal class CollectionRepository : ICollectionRepository
    {
        private readonly IMongoDatabase _database;

        public CollectionRepository(IMongoDBConnection connection)
            => _database = new MongoClient(connection.ConnectionString).GetDatabase(connection.DatabaseName);

        public async Task CreateAsync(string collectionName, BsonDocument data)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .InsertOneAsync(data);

        public async Task<bool> DeleteAsync(string collectionName, ObjectId id)
        {
            var result = await _database.GetCollection<BsonDocument>(collectionName)
                              .DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("_id", id));
            return (result.IsAcknowledged && result.DeletedCount > 0);
        }

        public async Task<IReadOnlyList<BsonDocument>> GetAllAsync(string collectionName)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(x => true)
                              .ToListAsync();

        public async Task<BsonDocument> GetByIdAsync(string collectionName, ObjectId id)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(Builders<BsonDocument>.Filter.Eq("_id", id))
                              .FirstOrDefaultAsync();

        public async Task<bool> UpdateAsync(string collectionName, ObjectId id, BsonDocument data)
        {
            var result = await _database.GetCollection<BsonDocument>(collectionName)
                                        .ReplaceOneAsync(Builders<BsonDocument>.Filter.Eq("_id", id), data);
            return (result.IsModifiedCountAvailable && result.ModifiedCount > 0);
        }
    }
}
