// BusBuddy/Data/Repositories/TripRepository.cs
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Data.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly IDatabaseManager _dbManager;
        private readonly ILogger _logger;
        
        public TripRepository(IDatabaseManager dbManager, ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
#pragma warning disable CS8603 // Possible null reference return.
        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            try
            {
                return await Task.FromResult(_dbManager.GetTrips());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all trips");
                throw;
            }
        }
        
        public async Task<Trip> GetByIdAsync(int id)
        {
            try
            {
                var trips = _dbManager.GetTrips();
                return await Task.FromResult(trips.FirstOrDefault(t => t.TripID == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trip by ID {TripId}", id);
                throw;
            }
        }
        
        public async Task<int> AddAsync(Trip entity)
        {
            try
            {
                _dbManager.AddTrip(entity);
                return await Task.FromResult(entity.TripID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding trip");
                throw;
            }
        }
        
        public async Task<bool> UpdateAsync(Trip entity)
        {
            try
            {
                // Implement update logic when available in DatabaseManager
                _logger.Warning("Trip update not implemented in DatabaseManager");
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating trip");
                throw;
            }
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // Implement delete logic when available in DatabaseManager
                _logger.Warning("Trip deletion not implemented in DatabaseManager");
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting trip");
                throw;
            }
        }
        
        public async Task<IEnumerable<Trip>> GetTripsByDateAsync(DateOnly date)
        {
            try
            {
                return await Task.FromResult(_dbManager.GetTripsByDate(date));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trips by date {Date}", date);
                throw;
            }
        }
        
        public async Task<IEnumerable<Trip>> GetTripsByDriverAsync(string driverName)
        {
            try
            {
                var trips = _dbManager.GetTrips();
                return await Task.FromResult(trips.Where(t => t.DriverName == driverName));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trips by driver {DriverName}", driverName);
                throw;
            }
        }
        
        public async Task<IEnumerable<Trip>> GetTripsByBusAsync(int busNumber)
        {
            try
            {
                var trips = _dbManager.GetTrips();
                return await Task.FromResult(trips.Where(t => t.BusNumber == busNumber));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trips by bus {BusNumber}", busNumber);
                throw;
            }
        }
#pragma warning restore CS8603
    }
}
#pragma warning restore CS1591