using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.EagleEye.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(string collectionName);
        Task<T> GetByIdAsync(string collectionName, Guid id);
        Task<bool> CreateAsync(string collectionName, T model);
        Task<bool> UpdateAsync(string collectionName, T model);
        Task<bool> DeleteAsync(string collectionName, Guid Id);
    }
}