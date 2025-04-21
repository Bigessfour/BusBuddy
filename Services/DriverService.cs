// BusBuddy/Services/DriverService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Data.Repositories;
using BusBuddy.Models;
using BusBuddy.Utilities;
using Serilog;

namespace BusBuddy.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ILogger _logger;
        
        public DriverService(IDriverRepository driverRepository, ILogger logger)
        {
            _driverRepository = driverRepository ?? throw new ArgumentNullException(nameof(driverRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            try
            {
                return await _driverRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all drivers");
                throw;
            }
        }
        
        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            try
            {
                return await _driverRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver by ID {DriverId}", id);
                throw;
            }
        }
        
        public async Task<Driver> GetDriverByNameAsync(string name)
        {
            try
            {
                return await _driverRepository.GetDriverByNameAsync(name);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver by name {Name}", name);
                throw;
            }
        }
        
        public async Task<IEnumerable<string>> GetDriverNamesAsync()
        {
            try
            {
                return await _driverRepository.GetDriverNamesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver names");
                throw;
            }
        }
        
        public async Task<(bool Success, string Message, int DriverId)> AddDriverAsync(Driver driver)
        {
            try
            {
                // Validate driver data
                if (string.IsNullOrWhiteSpace(driver.Name))
                {
                    _logger.Warning("Driver validation failed: Name is required");
                    return (false, "Driver name is required", 0);
                }
                
                // Add the driver
                int driverId = await _driverRepository.AddAsync(driver);
                _logger.Information("Driver added successfully with ID {DriverId}", driverId);
                return (true, "Driver added successfully", driverId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding driver");
                return (false, $"Error adding driver: {ex.Message}", 0);
            }
        }
    }
}