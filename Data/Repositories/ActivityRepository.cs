// BusBuddy/Data/Repositories/ActivityRepository.cs
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using BusBuddy.Models;
using Dapper;
using Serilog;

namespace BusBuddy.Data.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IDatabaseManager _dbManager;
        private readonly ILogger _logger;

        public ActivityRepository(IDatabaseManager dbManager, ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
#pragma warning disable CS8603 // Possible null reference return.
        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            try
            {
                return await Task.FromResult(_dbManager.GetActivities());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all activities");
                throw;
            }
        }
        
        public async Task<Activity> GetByIdAsync(int id)
        {
            try
            {
                var activities = _dbManager.GetActivities();
                return await Task.FromResult(activities.FirstOrDefault(a => a.ActivityID == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting activity by ID {ActivityId}", id);
                throw;
            }
        }
        
        public async Task<int> AddAsync(Activity entity)
        {
            try
            {
                _dbManager.AddActivity(entity);
                return await Task.FromResult(entity.ActivityID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding activity");
                throw;
            }
        }
        
        public async Task<bool> UpdateAsync(Activity entity)
        {
            try
            {
                // Implement update logic when available in DatabaseManager
                _logger.Warning("Activity update not implemented in DatabaseManager");
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating activity");
                throw;
            }
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // Implement delete logic when available in DatabaseManager
                _logger.Warning("Activity deletion not implemented in DatabaseManager");
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting activity");
                throw;
            }
        }
        
        public async Task<IEnumerable<Activity>> GetActivitiesByDateAsync(string date)
        {
            try
            {
                var activities = _dbManager.GetActivities();
                return await Task.FromResult(activities.Where(a => a.ActivityDate == date));
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
                var activities = _dbManager.GetActivities();
                return await Task.FromResult(activities.Where(a => a.BusNumber == busNumber));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting activities by bus {BusNumber}", busNumber);
                throw;
            }
        }
#pragma warning restore CS8603
    }
}
#pragma warning restore CS1591