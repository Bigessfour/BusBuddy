using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                var availableDrivers = new List<string>();
                var availableBusNumbers = new List<int>();

                var (isValid, errors) = DataValidator.ValidateTrip(trip, availableDrivers, availableBusNumbers);
                if (!isValid)
                {
                    string errorMessage = string.Join(", ", errors);
                    _logger.Warning("Trip validation failed: {ErrorMessage}", errorMessage);
                    return (false, errorMessage, 0);
                }

                var (hasConflict, conflictDetails) = await CheckSchedulingConflictsAsync(trip);
                if (hasConflict)
                {
                    _logger.Warning("Scheduling conflict detected: {ConflictDetails}", conflictDetails);
                    return (false, conflictDetails, 0);
                }

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
                // Fetch all existing trips
                var existingTrips = await _tripRepository.GetAllAsync();
                
                // Implement conflict detection logic
                foreach (var existingTrip in existingTrips)
                {
                    // Skip comparing with itself (if updating)
                    if (existingTrip.TripID == newTrip.TripID) continue;
                    
                    // Only check trips on the same date
                    if (existingTrip.Date != newTrip.Date) continue;
                    
                    // Check for Driver conflict
                    if (existingTrip.DriverName == newTrip.DriverName &&
                        TimesOverlap(existingTrip.StartTime, existingTrip.EndTime, newTrip.StartTime, newTrip.EndTime))
                    {
                        return (true, $"Driver '{newTrip.DriverName}' is already scheduled for trip {existingTrip.TripID} at that time.");
                    }

                    // Check for Bus conflict
                    if (existingTrip.BusNumber == newTrip.BusNumber &&
                        TimesOverlap(existingTrip.StartTime, existingTrip.EndTime, newTrip.StartTime, newTrip.EndTime))
                    {
                        return (true, $"Bus #{newTrip.BusNumber} is already scheduled for trip {existingTrip.TripID} at that time.");
                    }
                }
                
                // No conflicts found
                return (false, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error checking scheduling conflicts");
                return (false, $"Error checking scheduling conflicts: {ex.Message}");
            }
        }
        
        // Helper method for checking time overlaps
        private bool TimesOverlap(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
        {
            // Ensure end is after start for comparison logic (handle overnight trips)
            if (end1 < start1) end1 = new TimeOnly(end1.Hour + 24, end1.Minute);
            if (end2 < start2) end2 = new TimeOnly(end2.Hour + 24, end2.Minute);
            
            // Overlap occurs when one range starts before the other ends
            return start1 < end2 && start2 < end1;
        }
    }
}