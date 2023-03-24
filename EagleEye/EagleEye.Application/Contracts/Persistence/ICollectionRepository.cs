using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using WebAPI.EagleEye.Application.Models.RequestFeatures;

namespace WebAPI.EagleEye.Application.Contracts.Persistence
{
    public interface ICollectionRepository : IDisposable
    {
        Task<List<BsonDocument>> FilterBy(string collectionName, Expression<Func<BsonDocument, bool>> filterExpression);
        Task<List<BsonDocument>> GetAllAsync(string collectionName);
        Task<(List<BsonDocument> data, long countItemsByFilter)> GetAllAsync(string collectionName, QueryParameters parameters);
        Task<BsonDocument> GetByIdAsync(string collectionName, string id);
        Task InsertOneAsync(string collectionName, BsonDocument data);
        void InsertMany(string collectionName, ICollection<BsonDocument> documents);
        Task<bool> UpdateAsync(string collectionName, string id, BsonDocument data);
        Task<bool> DeleteAsync(string collectionName, string id);
    }
}