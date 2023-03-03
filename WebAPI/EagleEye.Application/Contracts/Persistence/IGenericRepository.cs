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
        Task<(List<TDocument> items, long countOfItemsByFilter)> GetAllAsync(string collectionName, QueryParameters parameters);
        Task<TDocument> GetByIdAsync(string collectionName, string id);
        Task CreateAsync(string collectionName, TDocument data);
        Task<bool> UpdateAsync(string collectionName, string id, TDocument data);
        Task<bool> DeleteAsync(string collectionName, string id);
    }
}