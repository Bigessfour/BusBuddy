// BusBuddy/Data/Repositories/FuelRepository.cs
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
        private readonly Serilog.ILogger _logger;
        
        public FuelRepository(IDatabaseManager dbManager, Serilog.ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        #region Asynchronous Methods
        #pragma warning disable CS8603 // Possible null reference return.
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
                return await Task.FromResult(fuelRecords.FirstOrDefault(f => f.FuelID == id));
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
                return await Task.FromResult(entity.FuelID);
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
                return await Task.FromResult(_dbManager.UpdateFuelRecord(entity));
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
                return await Task.FromResult(_dbManager.DeleteFuelRecord(id));
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
                return await Task.FromResult(fuelRecords.Where(f => f.BusNumber == busNumber));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting fuel records by bus {BusNumber}", busNumber);
                throw;
            }
        }
        #pragma warning restore CS8603
        #endregion

        #region Synchronous Methods
        public List<int> GetBusNumbers()
        {
            try
            {
                return _dbManager.GetBusNumbers();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting bus numbers");
                throw;
            }
        }
        
        public List<FuelRecord> GetAll()
        {
            try
            {
                return _dbManager.GetFuelRecords();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all fuel records");
                throw;
            }
        }
        
        public int Add(FuelRecord entity)
        {
            try
            {
                bool success = _dbManager.AddFuelRecord(entity);
                return success ? entity.FuelID : 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record");
                throw;
            }
        }
        
        public bool Update(FuelRecord entity)
        {
            try
            {
                return _dbManager.UpdateFuelRecord(entity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating fuel record");
                throw;
            }
        }
        
        public bool Delete(int id)
        {
            try
            {
                return _dbManager.DeleteFuelRecord(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting fuel record");
                throw;
            }
        }
        #endregion
    }
}
#pragma warning restore CS1591