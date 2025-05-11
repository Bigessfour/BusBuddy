using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents an alert for a route
    /// </summary>
    public class Alert
    {
        /// <summary>
        /// The unique identifier for the alert
        /// </summary>
        public int AlertId { get; set; }
        
        /// <summary>
        /// The route this alert applies to
        /// </summary>
        public int RouteId { get; set; }
        
        /// <summary>
        /// Navigation property for the route
        /// </summary>
        public Route? Route { get; set; }
        
        /// <summary>
        /// The message content of the alert
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// The severity level of the alert (e.g., "Info", "Warning", "Critical")
        /// </summary>
        [Required]
        public string Severity { get; set; } = string.Empty;
        
        /// <summary>
        /// Whether the alert is currently active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// When the alert was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
