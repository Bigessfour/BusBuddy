using System.Collections.Generic;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for dashboard metrics
    /// </summary>
    public class DashboardMetricsDto
    {
        /// <summary>
        /// Total number of active routes
        /// </summary>
        public int TotalRoutes { get; set; }
        
        /// <summary>
        /// Total number of drivers
        /// </summary>
        public int TotalDrivers { get; set; }
        
        /// <summary>
        /// Total number of vehicles
        /// </summary>
        public int TotalVehicles { get; set; }
        
        /// <summary>
        /// Total mileage across all routes
        /// </summary>
        public decimal TotalMileage { get; set; }
        
        /// <summary>
        /// Number of trips scheduled for today
        /// </summary>
        public int TripsToday { get; set; }
        
        /// <summary>
        /// List of recent activity
        /// </summary>
        public List<ActivitySummaryDto> RecentActivity { get; set; } = new();
    }

    /// <summary>
    /// Summary of recent activity
    /// </summary>
    public class ActivitySummaryDto
    {
        /// <summary>
        /// Activity ID
        /// </summary>
        public int ActivityId { get; set; }
        
        /// <summary>
        /// Activity type (e.g., "Trip", "Maintenance", "Fuel")
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;
        
        /// <summary>
        /// Brief description of the activity
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// When the activity occurred
        /// </summary>
        public System.DateTime Timestamp { get; set; }
    }
}
