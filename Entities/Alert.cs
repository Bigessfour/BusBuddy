using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Represents an alert for a route
    /// </summary>
    public class Alert : BaseEntity
    {
        /// <summary>
        /// The unique identifier for the alert (retained for compatibility)
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
    }
}
