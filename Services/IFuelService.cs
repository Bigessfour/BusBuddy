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
        Task<FuelRecord> GetFuelRecordByIdAsync(int id);
        
        /// <summary>
        /// Gets fuel records by bus number
        /// </summary>
        Task<IEnumerable<FuelRecord>> GetFuelRecordsByBusAsync(int busNumber);
        
        /// <summary>
        /// Adds a new fuel record
        /// </summary>
        Task<(bool Success, string Message, int FuelId)> AddFuelRecordAsync(FuelRecord fuelRecord);
        
        /// <summary>
        /// Adds a new fuel record from a fuel object
        /// </summary>
        Task<(bool Success, string Message, int FuelId)> AddFuelRecordFromFuelAsync(Fuel fuel);
    }
}