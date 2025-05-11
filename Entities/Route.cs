using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Represents a bus route
    /// </summary>
    public class Route : BaseEntity
    {
        /// <summary>
        /// The name of the route
        /// </summary>
        [Required]
        public string RouteName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the route
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Starting location
        /// </summary>
        [Required]
        public string StartLocation { get; set; } = string.Empty;

        /// <summary>
        /// Ending location
        /// </summary>
        [Required]
        public string EndLocation { get; set; } = string.Empty;

        /// <summary>
        /// The distance of the route in miles
        /// </summary>
        public decimal Distance { get; set; }

        /// <summary>
        /// Date and time the route was last modified
        /// </summary>
        public DateTime LastModified { get; set; } = DateTime.Now;

        /// <summary>
        /// Destinations associated with this route
        /// </summary>
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
        
        /// <summary>
        /// Trips associated with this route
        /// </summary>
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
        
        /// <summary>
        /// Alerts associated with this route
        /// </summary>
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    }
}
