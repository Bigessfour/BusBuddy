using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Represents a bus trip
    /// </summary>
    public class Trip : BaseEntity
    {
        /// <summary>
        /// The unique identifier for the trip (retained for compatibility)
        /// </summary>
        public int TripId { get; set; }
        
        /// <summary>
        /// The route this trip follows
        /// </summary>
        public int RouteId { get; set; }
        
        /// <summary>
        /// Navigation property for the route
        /// </summary>
        public Route? Route { get; set; }
        
        /// <summary>
        /// Number of passengers on this trip
        /// </summary>
        public int PassengerCount { get; set; }
        
        /// <summary>
        /// Delay in minutes, if any
        /// </summary>
        public int DelayMinutes { get; set; }
        
        /// <summary>
        /// Status of the trip (e.g., "OnTime", "Delayed", "Cancelled")
        /// </summary>
        [Required]
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// Whether the trip is currently active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// When the trip was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
