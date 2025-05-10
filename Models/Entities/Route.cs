using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using BusBuddy.Models;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a bus route
    /// </summary>
    public class Route
    {
        /// <summary>
        /// The unique identifier for the route
        /// </summary>
        public int Id { get; set; }

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
        /// Date and time the route was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date and time the route was last modified
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Destinations associated with this route
        /// </summary>
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
