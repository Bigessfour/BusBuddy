// BusBuddy/Services/ITripService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Services
{
    public interface ITripService
    {
        /// <summary>
        /// Gets all trips
        /// </summary>
        Task<IEnumerable<Trip>> GetAllTripsAsync();
        
        /// <summary>
        /// Gets a trip by ID
        /// </summary>
        Task<Trip> GetTripByIdAsync(int id);
        
        /// <summary>
        /// Gets trips by date
        /// </summary>
        Task<IEnumerable<Trip>> GetTripsByDateAsync(DateOnly date);
        
        /// <summary>
        /// Gets trips by driver
        /// </summary>
        Task<IEnumerable<Trip>> GetTripsByDriverAsync(string driverName);
        
        /// <summary>
        /// Gets trips by bus number
        /// </summary>
        Task<IEnumerable<Trip>> GetTripsByBusAsync(int busNumber);
        
        /// <summary>
        /// Adds a new trip
        /// </summary>
        Task<(bool Success, string Message, int TripId)> AddTripAsync(Trip trip);
        
        /// <summary>
        /// Checks for scheduling conflicts
        /// </summary>
        Task<(bool HasConflict, string ConflictDetails)> CheckSchedulingConflictsAsync(Trip newTrip);
    }
}