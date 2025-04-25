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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Trip> GetTripByIdAsync(int id);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets trips by date
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Trip>> GetTripsByDateAsync(DateOnly date);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets trips by driver
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Trip>> GetTripsByDriverAsync(string driverName);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets trips by bus number
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Trip>> GetTripsByBusAsync(int busNumber);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new trip
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int TripId)> AddTripAsync(Trip trip);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Checks for scheduling conflicts
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool HasConflict, string ConflictDetails)> CheckSchedulingConflictsAsync(Trip newTrip);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}