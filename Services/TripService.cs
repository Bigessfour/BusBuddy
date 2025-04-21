// BusBuddy/Services/TripService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBuddy.API;
using BusBuddy.Data.Repositories;
using BusBuddy.Models;
using BusBuddy.Utilities;
using Serilog;

namespace BusBuddy.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ILogger _logger;
        
        public TripService(ITripRepository tripRepository, ILogger logger)
        {
            _tripRepository = tripRepository ?? throw new ArgumentNullException(nameof(tripRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<Trip>> GetAllTripsAsync()
        {
            try
            {
                return await _tripRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all trips");
                throw;
            }
        }
        
        public async Task<Trip> GetTripByIdAsync(int id)
        {
            try
            {
                return await _tripRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trip by ID {TripId}", id);
                throw;
            }
        }
        
        public async Task<IEnumerable<Trip>> GetTripsByDateAsync(DateOnly date)
        {
            try
            {
                return await _tripRepository.GetTripsByDateAsync(date);
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
                return await _tripRepository.GetTripsByDriverAsync(driverName);
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
                return await _tripRepository.GetTripsByBusAsync(busNumber);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting trips by bus {BusNumber}", busNumber);
                throw;
            }
        }
        
        public async Task<(bool Success, string Message, int TripId)> AddTripAsync(Trip trip)
        {
            try
            {
                // Validate trip data
                var (isValid, errors) = DataValidator.ValidateTrip(trip);
                if (!isValid)
                {
                    string errorMessage = string.Join(", ", errors);
                    _logger.Warning("Trip validation failed: {ErrorMessage}", errorMessage);
                    return (false, errorMessage, 0);
                }
                
                // Check for scheduling conflicts
                var (hasConflict, conflictDetails) = await CheckSchedulingConflictsAsync(trip);
                if (hasConflict)
                {
                    _logger.Warning("Scheduling conflict detected: {ConflictDetails}", conflictDetails);
                    return (false, conflictDetails, 0);
                }
                
                // Add the trip
                int tripId = await _tripRepository.AddAsync(trip);
                _logger.Information("Trip added successfully with ID {TripId}", tripId);
                return (true, "Trip added successfully", tripId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding trip");
                return (false, $"Error adding trip: {ex.Message}", 0);
            }
        }
        
        public async Task<(bool HasConflict, string ConflictDetails)> CheckSchedulingConflictsAsync(Trip newTrip)
        {
            try
            {
                var existingTrips = await _tripRepository.GetAllAsync();
                return await ApiClient.DetectSchedulingConflictsAsync(newTrip, existingTrips.ToList());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error checking scheduling conflicts");
                return (false, $"Error checking scheduling conflicts: {ex.Message}");
            }
        }
    }
}