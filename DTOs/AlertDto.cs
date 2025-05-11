using System;
using BusBuddy.Entities;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for alert information
    /// </summary>
    public class AlertDto : IDashboardData
    {
        /// <summary>
        /// Unique identifier for the alert
        /// </summary>
        public int AlertId { get; set; }

        /// <summary>
        /// The route ID for this alert
        /// </summary>
        public int RouteId { get; set; }
        
        /// <summary>
        /// The route associated with this alert
        /// </summary>
        public RouteDto? Route { get; set; }
        
        /// <summary>
        /// The alert message
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// The severity level (Info, Warning, Critical)
        /// </summary>
        public string Severity { get; set; } = string.Empty;
        
        /// <summary>
        /// Whether the alert is active
        /// </summary>
        public bool IsActive { get; set; }
          /// <summary>
        /// When the alert was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets a key metric value for dashboard display
        /// </summary>
        /// <param name="metricName">Name of the metric to retrieve</param>
        /// <returns>The metric value as an object that can be cast to appropriate type</returns>
        public object GetKeyMetric(string metricName)
        {
            return metricName switch
            {
                "AlertId" => AlertId,
                "RouteId" => RouteId,
                "RouteName" => Route?.RouteName ?? string.Empty,
                "Message" => Message,
                "Severity" => Severity,
                "IsActive" => IsActive,
                "CreatedAt" => CreatedAt,
                _ => throw new ArgumentException($"Unknown metric name: {metricName}")
            };
        }
    }
}
