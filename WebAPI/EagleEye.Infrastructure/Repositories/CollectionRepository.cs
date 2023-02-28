using EagleEye.Infrastructure.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.Infrastructure.Repositories
{
    internal class CollectionRepository : ICollectionRepository
    {
        private readonly IMongoDBConnection _connection;

        public CollectionRepository(IMongoDBConnection connection)
            => _connection = connection;

        public Task<bool> CreateAsync(string collectionName, object model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string collectionName, Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<object>> GetAllAsync(string collectionName)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetByIdAsync(string collectionName, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string collectionName, object model)
        {
            throw new NotImplementedException();
        }
    }
}
