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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Driver> GetDriverByIdAsync(int id);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets a driver by name
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Driver> GetDriverByNameAsync(string name);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets all driver names
        /// </summary>
        Task<IEnumerable<string>> GetDriverNamesAsync();
        
        /// <summary>
        /// Adds a new driver
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int DriverId)> AddDriverAsync(Driver driver);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}