// BusBuddy/Data/Repositories/DriverRepository.cs
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
                // Implement update logic when available in DatabaseManager
                _logger.Warning("Driver update not implemented in DatabaseManager");
                return await Task.FromResult(false);
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
                // Implement delete logic when available in DatabaseManager
                _logger.Warning("Driver deletion not implemented in DatabaseManager");
                return await Task.FromResult(false);
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
                return await Task.FromResult(drivers.FirstOrDefault(d => d.Driver_Name == name));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting driver by name {Name}", name);
                throw;
            }
        }
    }
}