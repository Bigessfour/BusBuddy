using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusBuddy.Data;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;

namespace BusBuddy.Services
{
    /// <summary>
    /// Service for managing drivers
    /// </summary>
    public class DriverService
    {
        private readonly IDatabaseHelper _dbHelper;
        private readonly ILogger<DriverService> _logger;

        /// <summary>
        /// Initializes a new instance of the DriverService class
        /// </summary>
        /// <param name="dbHelper">The database helper</param>
        /// <param name="logger">The logger</param>
        public DriverService(IDatabaseHelper dbHelper, ILogger<DriverService> logger)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all drivers
        /// </summary>
        /// <returns>List of drivers</returns>
        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _dbHelper.GetDriversAsync();
        }

        /// <summary>
        /// Gets a driver by ID
        /// </summary>
        /// <param name="id">The driver ID</param>
        /// <returns>The driver if found, null otherwise</returns>
        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            return await _dbHelper.GetDriverByIdAsync(id);
        }

        /// <summary>
        /// Adds a new driver
        /// </summary>
        /// <param name="driver">The driver to add</param>
        /// <returns>The added driver with ID populated</returns>
        public async Task<Driver> AddDriverAsync(Driver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            // Default CreatedDate to now if not set
            if (driver.CreatedDate == default)
                driver.CreatedDate = DateTime.Now;

            return await _dbHelper.AddDriverAsync(driver);
        }

        /// <summary>
        /// Updates an existing driver
        /// </summary>
        /// <param name="driver">The driver with updated information</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> UpdateDriverAsync(Driver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            return await _dbHelper.UpdateDriverAsync(driver);
        }

        /// <summary>
        /// Safely deletes a driver by first handling any dependent records
        /// </summary>
        /// <param name="driverId">The ID of the driver to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            _logger.LogInformation("Deleting driver with ID {DriverId}", driverId);
            return await _dbHelper.DeleteDriverSafelyAsync(driverId);
        }

        /// <summary>
        /// Safely deletes a driver with the option to reassign related records to another driver
        /// </summary>
        /// <param name="driverId">The ID of the driver to delete</param>
        /// <param name="reassignToDriverId">The ID of the driver to reassign related records to</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteDriverAndReassignAsync(int driverId, int reassignToDriverId)
        {
            if (driverId == reassignToDriverId)
                throw new ArgumentException("Cannot reassign to the same driver being deleted", nameof(reassignToDriverId));

            _logger.LogInformation("Deleting driver {DriverId} and reassigning records to driver {ReassignToDriverId}", 
                driverId, reassignToDriverId);
                
            return await _dbHelper.DeleteDriverSafelyAsync(driverId, reassignToDriverId);
        }
    }
}
