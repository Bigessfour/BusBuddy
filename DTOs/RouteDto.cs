using System;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for bus route information
    /// </summary>
    public class RouteDto
    {
        /// <summary>
        /// Unique identifier for the route
        /// </summary>
        public int RouteID { get; set; }

        /// <summary>
        /// Name of the route
        /// </summary>
        public string RouteName { get; set; } = string.Empty;

        /// <summary>
        /// Starting location of the route
        /// </summary>
        public string StartLocation { get; set; } = string.Empty;

        /// <summary>
        /// Ending location of the route
        /// </summary>
        public string EndLocation { get; set; } = string.Empty;

        /// <summary>
        /// Distance of the route in miles
        /// </summary>
        public decimal Distance { get; set; }

        /// <summary>
        /// When the route was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
        
        /// <summary>
        /// Current status of the route
        /// </summary>
        public string Status { get; set; } = "Inactive";
    }
}
