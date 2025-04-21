// BusBuddy/Services/IActivityService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Services
{
    public interface IActivityService
    {
        /// <summary>
        /// Gets all activities
        /// </summary>
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        
        /// <summary>
        /// Gets an activity by ID
        /// </summary>
        Task<Activity> GetActivityByIdAsync(int id);
        
        /// <summary>
        /// Gets activities by date
        /// </summary>
        Task<IEnumerable<Activity>> GetActivitiesByDateAsync(string date);
        
        /// <summary>
        /// Gets activities by bus number
        /// </summary>
        Task<IEnumerable<Activity>> GetActivitiesByBusAsync(int busNumber);
        
        /// <summary>
        /// Adds a new activity
        /// </summary>
        Task<(bool Success, string Message, int ActivityId)> AddActivityAsync(Activity activity);
        
        /// <summary>
        /// Adds a new activity from an activity trip
        /// </summary>
        Task<(bool Success, string Message, int ActivityId)> AddActivityFromTripAsync(ActivityTrip activityTrip);
    }
}