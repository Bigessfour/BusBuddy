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
}
