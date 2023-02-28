using EagleEye.Infrastructure.DatabaseConfig;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
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

        public Task<bool> DeleteAsync(string collectionName, Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<BsonDocument>> GetAllAsync(string collectionName)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(x => true)
                              .ToListAsync();

        public async Task<BsonDocument> GetByIdAsync(string collectionName, Guid id)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(new BsonDocument { { "id", id.ToString() } })
                              .FirstOrDefaultAsync();

        public Task<bool> UpdateAsync(string collectionName, BsonDocument data)
        {
            throw new NotImplementedException();
        }
    }
}
