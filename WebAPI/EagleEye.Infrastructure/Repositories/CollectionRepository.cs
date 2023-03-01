using EagleEye.Infrastructure.DatabaseConfig;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<bool> DeleteAsync(string collectionName, string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var result = await _database.GetCollection<BsonDocument>(collectionName)
                              .DeleteOneAsync(filter);
            return (result.IsAcknowledged && result.DeletedCount > 0);
        }

        public async Task<IReadOnlyList<BsonDocument>> FilterBy(string collectionName,
                                                                Expression<Func<BsonDocument, bool>> filterExpression)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(filterExpression)
                              .ToListAsync();

        public async Task<IReadOnlyList<BsonDocument>> GetAllAsync(string collectionName)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(x => true)
                              .ToListAsync();

        public async Task<BsonDocument> GetByIdAsync(string collectionName, string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            return await _database.GetCollection<BsonDocument>(collectionName)
                                  .Find(filter)
                                  .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string collectionName, string id, BsonDocument data)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var result = await _database.GetCollection<BsonDocument>(collectionName)
                                        .ReplaceOneAsync(filter, data);
            return (result.IsAcknowledged && result.ModifiedCount > 0);
        }
    }
}
