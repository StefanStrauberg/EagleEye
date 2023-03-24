using EagleEye.Infrastructure.DatabaseConfig;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;
using WebAPI.EagleEye.Application.Models.RequestFeatures;

namespace EagleEye.Infrastructure.Repositories
{
    internal class CollectionRepository : ICollectionRepository
    {
        readonly IMongoDatabase _database;

        public CollectionRepository(IMongoDBConnection connection)
            => _database = new MongoClient(connection.ConnectionString).GetDatabase(connection.DatabaseName);

        async Task ICollectionRepository.InsertOneAsync(string collectionName,
                                                        BsonDocument data)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .InsertOneAsync(data);

        async Task<bool> ICollectionRepository.DeleteAsync(string collectionName,
                                                           string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var result = await _database.GetCollection<BsonDocument>(collectionName)
                              .DeleteOneAsync(filter);

            return (result.IsAcknowledged && result.DeletedCount > 0);
        }

        async Task<List<BsonDocument>> ICollectionRepository.FilterBy(string collectionName,
                                                                      Expression<Func<BsonDocument, bool>> filterExpression)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(filterExpression)
                              .ToListAsync();

        async Task<List<BsonDocument>> ICollectionRepository.GetAllAsync(string collectionName)
            => await _database.GetCollection<BsonDocument>(collectionName)
                              .Find(x => true)
                              .ToListAsync();

        async Task<(List<BsonDocument> data , long countItemsByFilter)> ICollectionRepository.GetAllAsync(string collectionName,
                                                                                                          QueryParameters parameters)
        {
            var minDate = Builders<BsonDocument>.Filter.Gte("date", parameters.MinSearchDate);
            var maxDate = Builders<BsonDocument>.Filter.Lte("date", parameters.MaxSearchDate);
            var combineFilters = Builders<BsonDocument>.Filter.And(minDate, maxDate);

            var countItemsByFilter = await _database.GetCollection<BsonDocument>(collectionName)
                                                    .Find(combineFilters)
                                                    .CountDocumentsAsync();

            var data = await _database.GetCollection<BsonDocument>(collectionName)
                                      .Find(combineFilters)
                                      .SortBy(x => x["date"])
                                      .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                      .Limit(parameters.PageSize)
                                      .ToListAsync();

            return (data, countItemsByFilter);
        }

        async Task<BsonDocument> ICollectionRepository.GetByIdAsync(string collectionName,
                                                                    string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            return await _database.GetCollection<BsonDocument>(collectionName)
                                  .Find(filter)
                                  .FirstOrDefaultAsync();
        }

        void ICollectionRepository.InsertMany(string collectionName,
                                              ICollection<BsonDocument> documents)
            => _database.GetCollection<BsonDocument>(collectionName)
                        .InsertMany(documents);

        async Task<bool> ICollectionRepository.UpdateAsync(string collectionName,
                                                           string id,
                                                           BsonDocument data)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var result = await _database.GetCollection<BsonDocument>(collectionName)
                                        .ReplaceOneAsync(filter, data);

            return (result.IsAcknowledged && result.ModifiedCount > 0);
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}