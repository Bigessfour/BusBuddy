// BusBuddy/Services/IDriverService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Services
{
    public interface IDriverService
    {
        /// <summary>
        /// Gets all drivers
        /// </summary>
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        
        /// <summary>
        /// Gets a driver by ID
        /// </summary>
        Task<Driver> GetDriverByIdAsync(int id);
        
        /// <summary>
        /// Gets a driver by name
        /// </summary>
        Task<Driver> GetDriverByNameAsync(string name);
        
        /// <summary>
        /// Gets all driver names
        /// </summary>
        Task<IEnumerable<string>> GetDriverNamesAsync();
        
        /// <summary>
        /// Adds a new driver
        /// </summary>
        Task<(bool Success, string Message, int DriverId)> AddDriverAsync(Driver driver);
    }
}