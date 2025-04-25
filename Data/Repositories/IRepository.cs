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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<T> GetByIdAsync(TKey id);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new entity
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<TKey> AddAsync(T entity);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Updates an existing entity
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<bool> UpdateAsync(T entity);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<bool> DeleteAsync(TKey id);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}