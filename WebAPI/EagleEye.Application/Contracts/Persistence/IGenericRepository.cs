using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.EagleEye.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(string collectionName);
        Task<T> GetByIdAsync(string collectionName, Guid id);
        Task CreateAsync(string collectionName, T data);
        Task<bool> UpdateAsync(string collectionName, T data);
        Task<bool> DeleteAsync(string collectionName, Guid Id);
    }
}