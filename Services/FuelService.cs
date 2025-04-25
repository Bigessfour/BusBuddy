// BusBuddy/Services/FuelService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Data.Repositories;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Services
{
    public class FuelService : IFuelService
    {
        private readonly IFuelRepository _fuelRepository;
        private readonly ILogger _logger;
        
        public FuelService(IFuelRepository fuelRepository, ILogger logger)
        {
            _fuelRepository = fuelRepository ?? throw new ArgumentNullException(nameof(fuelRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<FuelRecord>> GetAllFuelRecordsAsync()
        {
            try
            {
                return await _fuelRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all fuel records");
                throw;
            }
        }
        
        public async Task<FuelRecord> GetFuelRecordByIdAsync(int id)
        {
            try
            {
                return await _fuelRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting fuel record by ID {FuelId}", id);
                throw;
            }
        }
        
        public async Task<IEnumerable<FuelRecord>> GetFuelRecordsByBusAsync(int busNumber)
        {
            try
            {
                return await _fuelRepository.GetFuelRecordsByBusAsync(busNumber);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting fuel records by bus {BusNumber}", busNumber);
                throw;
            }
        }
        
        public async Task<(bool Success, string Message, int FuelId)> AddFuelRecordAsync(FuelRecord fuelRecord)
        {
            try
            {
                // Validate fuel record data
                if (fuelRecord.BusNumber <= 0)
                {
                    _logger.Warning("Fuel record validation failed: Bus number is required");
                    return (false, "Bus number is required", 0);
                }
                
                // Add the fuel record
                int fuelId = await _fuelRepository.AddAsync(fuelRecord);
                _logger.Information("Fuel record added successfully with ID {FuelId}", fuelId);
                return (true, "Fuel record added successfully", fuelId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record");
                return (false, $"Error adding fuel record: {ex.Message}", 0);
            }
        }
        
        public async Task<(bool Success, string Message, int FuelId)> AddFuelRecordFromFuelAsync(FuelRecord fuel)
        {
            try
            {
                // No need to convert since we're already using FuelRecord
                var fuelRecord = fuel.Clone();
                
                // Add the fuel record
                return await AddFuelRecordAsync(fuelRecord);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record from fuel");
                return (false, $"Error adding fuel record from fuel: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Gets all bus numbers
        /// </summary>
        public List<int> GetBusNumbers()
        {
            try
            {
                return _fuelRepository.GetBusNumbers();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting bus numbers");
                throw;
            }
        }
        
        /// <summary>
        /// Gets all fuel records (synchronous version)
        /// </summary>
        public List<FuelRecord> GetFuelRecords()
        {
            try
            {
                return _fuelRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all fuel records");
                throw;
            }
        }
        
        /// <summary>
        /// Adds a new fuel record (synchronous version)
        /// </summary>
        public bool AddFuelRecord(FuelRecord fuelRecord)
        {
            try
            {
                // Validate fuel record data
                if (fuelRecord.BusNumber <= 0)
                {
                    _logger.Warning("Fuel record validation failed: Bus number is required");
                    return false;
                }
                
                // Add the fuel record
                int fuelId = _fuelRepository.Add(fuelRecord);
                _logger.Information("Fuel record added successfully with ID {FuelId}", fuelId);
                return fuelId > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record");
                return false;
            }
        }
        
        /// <summary>
        /// Updates an existing fuel record
        /// </summary>
        public bool UpdateFuelRecord(FuelRecord fuelRecord)
        {
            try
            {
                // Validate fuel record data
                if (fuelRecord.FuelID <= 0)
                {
                    _logger.Warning("Fuel record validation failed: Fuel ID is required for updates");
                    return false;
                }
                
                if (fuelRecord.BusNumber <= 0)
                {
                    _logger.Warning("Fuel record validation failed: Bus number is required");
                    return false;
                }
                
                // Update the fuel record
                bool result = _fuelRepository.Update(fuelRecord);
                if (result)
                {
                    _logger.Information("Fuel record updated successfully with ID {FuelId}", fuelRecord.FuelID);
                }
                else
                {
                    _logger.Warning("Failed to update fuel record with ID {FuelId}", fuelRecord.FuelID);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating fuel record with ID {FuelId}", fuelRecord.FuelID);
                return false;
            }
        }
        
        /// <summary>
        /// Deletes a fuel record by ID
        /// </summary>
        public bool DeleteFuelRecord(int fuelId)
        {
            try
            {
                // Validate fuel ID
                if (fuelId <= 0)
                {
                    _logger.Warning("Fuel record validation failed: Valid Fuel ID is required for deletion");
                    return false;
                }
                
                // Delete the fuel record
                bool result = _fuelRepository.Delete(fuelId);
                if (result)
                {
                    _logger.Information("Fuel record deleted successfully with ID {FuelId}", fuelId);
                }
                else
                {
                    _logger.Warning("Failed to delete fuel record with ID {FuelId}", fuelId);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting fuel record with ID {FuelId}", fuelId);
                return false;
            }
        }
    }
}