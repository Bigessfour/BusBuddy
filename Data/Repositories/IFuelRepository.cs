// BusBuddy/Data/Repositories/IFuelRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface IFuelRepository : IRepository<FuelRecord, int>
    {
        /// <summary>
        /// Gets fuel records by bus number
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<FuelRecord>> GetFuelRecordsByBusAsync(int busNumber);
#pragma warning restore SA1611 // Element parameters should be documented

        /// <summary>
        /// Gets all bus numbers
        /// </summary>
        List<int> GetBusNumbers();
        
        /// <summary>
        /// Gets all fuel records (synchronous version)
        /// </summary>
        List<FuelRecord> GetAll();
        
        /// <summary>
        /// Adds a new fuel record (synchronous version)
        /// </summary>
        int Add(FuelRecord entity);
        
        /// <summary>
        /// Updates an existing fuel record
        /// </summary>
        bool Update(FuelRecord entity);
        
        /// <summary>
        /// Deletes a fuel record by ID
        /// </summary>
        bool Delete(int id);
    }
}