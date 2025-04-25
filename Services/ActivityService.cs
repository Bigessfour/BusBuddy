// BusBuddy/Services/ActivityService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Data.Repositories;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ILogger _logger;
        
        public ActivityService(IActivityRepository activityRepository, ILogger logger)
        {
            _activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            try
            {
                return await _activityRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all activities");
                throw;
            }
        }
        
        public async Task<Activity> GetActivityByIdAsync(int id)
        {
            try
            {
                return await _activityRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting activity by ID {ActivityId}", id);
                throw;
            }
        }
        
        public async Task<IEnumerable<Activity>> GetActivitiesByDateAsync(string date)
        {
            try
            {
                return await _activityRepository.GetActivitiesByDateAsync(date);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting activities by date {Date}", date);
                throw;
            }
        }
        
        public async Task<IEnumerable<Activity>> GetActivitiesByBusAsync(int busNumber)
        {
            try
            {
                return await _activityRepository.GetActivitiesByBusAsync(busNumber);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting activities by bus {BusNumber}", busNumber);
                throw;
            }
        }
        
        public async Task<(bool Success, string Message, int ActivityId)> AddActivityAsync(Activity activity)
        {
            try
            {
                // Validate activity data
                if (string.IsNullOrWhiteSpace(activity.Title))
                {
                    _logger.Warning("Activity validation failed: Title is required");
                    return (false, "Activity title is required", 0);
                }
                
                // Add the activity
                int activityId = await _activityRepository.AddAsync(activity);
                _logger.Information("Activity added successfully with ID {ActivityId}", activityId);
                return (true, "Activity added successfully", activityId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding activity");
                return (false, $"Error adding activity: {ex.Message}", 0);
            }
        }
        
        public async Task<(bool Success, string Message, int ActivityId)> AddActivityFromTripAsync(ActivityTrip activityTrip)
        {
            try
            {
                // Convert ActivityTrip to Activity
                var activity = Activity.FromActivityTrip(activityTrip);
                
                // Add the activity
                return await AddActivityAsync(activity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding activity from trip");
                return (false, $"Error adding activity from trip: {ex.Message}", 0);
            }
        }
    }
}