using System;
using BusBuddy.Models.Entities;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for trip information
    /// </summary>
    public class TripDto
    {
        /// <summary>
        /// Unique identifier for the trip
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        /// The route ID for this trip
        /// </summary>
        public int RouteId { get; set; }
        
        /// <summary>
        /// The name of the route
        /// </summary>
        public string RouteName { get; set; } = string.Empty;

        /// <summary>
        /// Number of passengers
        /// </summary>
        public int PassengerCount { get; set; }
        
        /// <summary>
        /// Delay in minutes
        /// </summary>
        public int DelayMinutes { get; set; }
        
        /// <summary>
        /// Status of the trip (OnTime, Delayed, Cancelled)
        /// </summary>
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// Whether the trip is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// When the trip was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
