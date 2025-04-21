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
                if (fuelRecord.Bus_Number <= 0)
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
        
        public async Task<(bool Success, string Message, int FuelId)> AddFuelRecordFromFuelAsync(Fuel fuel)
        {
            try
            {
                // Convert Fuel to FuelRecord
                var fuelRecord = FuelRecord.FromFuel(fuel);
                
                // Add the fuel record
                return await AddFuelRecordAsync(fuelRecord);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record from fuel");
                return (false, $"Error adding fuel record from fuel: {ex.Message}", 0);
            }
        }
    }
}