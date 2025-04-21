// BusBuddy/Data/Repositories/FuelRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Data.Repositories
{
    public class FuelRepository : IFuelRepository
    {
        private readonly IDatabaseManager _dbManager;
        private readonly ILogger _logger;
        
        public FuelRepository(IDatabaseManager dbManager, ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<FuelRecord>> GetAllAsync()
        {
            try
            {
                return await Task.FromResult(_dbManager.GetFuelRecords());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all fuel records");
                throw;
            }
        }
        
        public async Task<FuelRecord> GetByIdAsync(int id)
        {
            try
            {
                var fuelRecords = _dbManager.GetFuelRecords();
                return await Task.FromResult(fuelRecords.FirstOrDefault(f => f.Fuel_ID == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting fuel record by ID {FuelId}", id);
                throw;
            }
        }
        
        public async Task<int> AddAsync(FuelRecord entity)
        {
            try
            {
                _dbManager.AddFuelRecord(entity);
                return await Task.FromResult(entity.Fuel_ID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record");
                throw;
            }
        }
        
        public async Task<bool> UpdateAsync(FuelRecord entity)
        {
            try
            {
                // Implement update logic when available in DatabaseManager
                _logger.Warning("Fuel record update not implemented in DatabaseManager");
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating fuel record");
                throw;
            }
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // Implement delete logic when available in DatabaseManager
                _logger.Warning("Fuel record deletion not implemented in DatabaseManager");
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting fuel record");
                throw;
            }
        }
        
        public async Task<IEnumerable<FuelRecord>> GetFuelRecordsByBusAsync(int busNumber)
        {
            try
            {
                var fuelRecords = _dbManager.GetFuelRecords();
                return await Task.FromResult(fuelRecords.Where(f => f.Bus_Number == busNumber));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting fuel records by bus {BusNumber}", busNumber);
                throw;
            }
        }
    }
}