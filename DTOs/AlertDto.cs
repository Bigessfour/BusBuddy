using System;
using BusBuddy.Models.Entities;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for alert information
    /// </summary>
    public class AlertDto
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
    }
}
