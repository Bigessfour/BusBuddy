// BusBuddy/Services/IFuelService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Services
{
    public interface IFuelService
    {
        /// <summary>
        /// Gets all fuel records
        /// </summary>
        Task<IEnumerable<FuelRecord>> GetAllFuelRecordsAsync();
        
        /// <summary>
        /// Gets a fuel record by ID
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<FuelRecord> GetFuelRecordByIdAsync(int id);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets fuel records by bus number
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<FuelRecord>> GetFuelRecordsByBusAsync(int busNumber);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new fuel record
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int FuelId)> AddFuelRecordAsync(FuelRecord fuelRecord);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new fuel record from a fuel object
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int FuelId)> AddFuelRecordFromFuelAsync(FuelRecord fuel);
#pragma warning restore SA1611 // Element parameters should be documented

        /// <summary>
        /// Gets all bus numbers
        /// </summary>
        List<int> GetBusNumbers();
        
        /// <summary>
        /// Gets all fuel records (synchronous version)
        /// </summary>
        List<FuelRecord> GetFuelRecords();
        
        /// <summary>
        /// Adds a new fuel record (synchronous version)
        /// </summary>
        bool AddFuelRecord(FuelRecord fuelRecord);
        
        /// <summary>
        /// Updates an existing fuel record
        /// </summary>
        bool UpdateFuelRecord(FuelRecord fuelRecord);
        
        /// <summary>
        /// Deletes a fuel record by ID
        /// </summary>
        bool DeleteFuelRecord(int fuelId);
    }
}