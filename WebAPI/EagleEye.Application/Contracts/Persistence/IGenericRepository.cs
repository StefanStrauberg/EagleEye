using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Models.RequestFeatures;

namespace WebAPI.EagleEye.Application.Contracts.Persistence
{
    public interface IGenericRepository<TDocument> where TDocument : class
    {
        Task<List<TDocument>> FilterBy(string collectionName, Expression<Func<TDocument, bool>> filterExpression);
        Task<List<TDocument>> GetAllAsync(string collectionName);
        Task<(List<TDocument> data, long countItemsByFilter)> GetAllAsync(string collectionName, QueryParameters parameters);
        Task<TDocument> GetByIdAsync(string collectionName, string id);
        Task InsertOneAsync(string collectionName, TDocument data);
        Task InsertManyAsync(string collectionName, ICollection<TDocument> documents);
        Task<bool> UpdateAsync(string collectionName, string id, TDocument data);
        Task<bool> DeleteAsync(string collectionName, string id);
    }
}