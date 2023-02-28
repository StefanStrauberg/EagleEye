using EagleEye.Domain;
using EagleEye.Infrastructure.DatabaseConfig;
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

        public Task<bool> CreateAsync(string collectionName, object model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string collectionName, Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<object>> GetAllAsync(string collectionName)
            => await _database.GetCollection<DomainBaseEntity>(collectionName)
                              .Find(x => true)
                              .ToListAsync();

        public async Task<object> GetByIdAsync(string collectionName, Guid id)
            => await _database.GetCollection<DomainBaseEntity>(collectionName)
                              .Find(x => x.Id == id)
                              .FirstOrDefaultAsync();

        public Task<bool> UpdateAsync(string collectionName, object model)
        {
            throw new NotImplementedException();
        }
    }
}
