using System;
using System.Collections.Generic;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for dashboard metrics
    /// </summary>
    public class DashboardDto : IDashboardData
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
        /// Number of active alerts
        /// </summary>
        public int ActiveAlertCount { get; set; }
        
        /// <summary>
        /// On-time performance percentage
        /// </summary>
        public decimal OnTimePerformance { get; set; }
        
        /// <summary>
        /// Route status counts for pie chart
        /// </summary>
        public Dictionary<string, int> RouteStatusCounts { get; set; } = new();
        
        /// <summary>
        /// List of recent activity
        /// </summary>
        public List<ActivitySummaryDto> RecentActivity { get; set; } = new();
        
        /// <summary>
        /// Gets a key metric value for dashboard display
        /// </summary>
        /// <param name="metricName">Name of the metric to retrieve</param>
        /// <returns>The metric value as an object that can be cast to appropriate type</returns>
        public object GetKeyMetric(string metricName)
        {
            return metricName switch
            {
                "TotalRoutes" => TotalRoutes,
                "TotalDrivers" => TotalDrivers,
                "TotalVehicles" => TotalVehicles,
                "TotalMileage" => TotalMileage,
                "TripsToday" => TripsToday,
                "ActiveAlertCount" => ActiveAlertCount,
                "OnTimePerformance" => OnTimePerformance,
                _ => throw new ArgumentException($"Unknown metric name: {metricName}")
            };
        }
    }
    
    /// <summary>
    /// Summary of an activity for dashboard display
    /// </summary>
    public class ActivitySummaryDto
    {
        /// <summary>
        /// Activity ID
        /// </summary>
        public int ActivityId { get; set; }
        
        /// <summary>
        /// Type of activity
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;
        
        /// <summary>
        /// Description of the activity
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// When the activity occurred
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
