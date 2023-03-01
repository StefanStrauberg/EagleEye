using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.EagleEye.Application.Contracts.Persistence
{
    public interface IGenericRepository<TDocument> where TDocument : class
    {
        Task<IReadOnlyList<TDocument>> FilterBy(string collectionName, Expression<Func<TDocument, bool>> filterExpression);
        Task<IReadOnlyList<TDocument>> GetAllAsync(string collectionName);
        Task<TDocument> GetByIdAsync(string collectionName, string id);
        Task CreateAsync(string collectionName, TDocument data);
        Task<bool> UpdateAsync(string collectionName, string id, TDocument data);
        Task<bool> DeleteAsync(string collectionName, string id);
    }
}