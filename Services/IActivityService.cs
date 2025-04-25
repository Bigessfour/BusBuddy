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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Activity> GetActivityByIdAsync(int id);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets activities by date
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Activity>> GetActivitiesByDateAsync(string date);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets activities by bus number
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Activity>> GetActivitiesByBusAsync(int busNumber);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new activity
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int ActivityId)> AddActivityAsync(Activity activity);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new activity from an activity trip
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int ActivityId)> AddActivityFromTripAsync(ActivityTrip activityTrip);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}