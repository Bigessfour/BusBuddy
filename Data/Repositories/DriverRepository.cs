// BusBuddy/Data/Repositories/DriverRepository.cs
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Data.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IDatabaseManager _dbManager;
        private readonly ILogger _logger;
        
        public DriverRepository(IDatabaseManager dbManager, ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
#pragma warning disable CS8603 // Possible null reference return.
        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            try
            {
                return await Task.FromResult(_dbManager.GetDrivers());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all drivers");
                throw;
            }
        }
        
        public async Task<Driver> GetByIdAsync(int id)
        {
            try
            {
                var drivers = _dbManager.GetDrivers();
                return await Task.FromResult(drivers.FirstOrDefault(d => d.DriverID == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver by ID {DriverId}", id);
                throw;
            }
        }
        
        public async Task<int> AddAsync(Driver entity)
        {
            try
            {
                _dbManager.AddDriver(entity);
                return await Task.FromResult(entity.DriverID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding driver");
                throw;
            }
        }
        
        public async Task<bool> UpdateAsync(Driver entity)
        {
            try
            {
                _logger.Information("Updating driver with ID {DriverId}", entity.DriverID);
                var result = _dbManager.UpdateDriver(entity);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating driver");
                throw;
            }
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.Information("Deleting driver with ID {DriverId}", id);
                var result = _dbManager.DeleteDriver(id);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting driver");
                throw;
            }
        }
        
        public async Task<IEnumerable<string>> GetDriverNamesAsync()
        {
            try
            {
                return await Task.FromResult(_dbManager.GetDriverNames());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver names");
                throw;
            }
        }
        
        public async Task<Driver> GetDriverByNameAsync(string name)
        {
            try
            {
                var drivers = _dbManager.GetDrivers();
                // Support both DriverName and Name properties
                return await Task.FromResult(drivers.FirstOrDefault(d => 
                    (d.Name != null && d.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) || 
                    (d.DriverName != null && d.DriverName.Equals(name, StringComparison.OrdinalIgnoreCase))
                ));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver by name {Name}", name);
                throw;
            }
        }
#pragma warning restore CS8603
    }
}
#pragma warning restore CS1591