// BusBuddy/Data/Repositories/IRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusBuddy.Data.Repositories
{
    /// <summary>
    /// Generic repository interface for data access operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key</typeparam>
    public interface IRepository<T, TKey> where T : class
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        Task<T> GetByIdAsync(TKey id);
        
        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<TKey> AddAsync(T entity);
        
        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<bool> UpdateAsync(T entity);
        
        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        Task<bool> DeleteAsync(TKey id);
    }
}