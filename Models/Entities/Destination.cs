using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a destination for trips
    /// </summary>
    public class Destination
    {
        /// <summary>
        /// The unique identifier for the destination
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the destination
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Full address of the destination
        /// </summary>
        [Required]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City where the destination is located
        /// </summary>
        [Required]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State where the destination is located
        /// </summary>
        [Required]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// ZIP code of the destination
        /// </summary>
        [Required]
        public string ZipCode { get; set; } = string.Empty;

        /// <summary>
        /// Contact person at the destination
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Total miles from the school to this destination (one way)
        /// </summary>
        public decimal TotalMiles { get; set; }

        /// <summary>
        /// Date and time the destination was added to the system
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Activity trips that have this destination
        /// </summary>
        public List<ActivityTrip> ActivityTrips { get; set; } = new List<ActivityTrip>();

        /// <summary>
        /// The route associated with this destination
        /// </summary>
        public int? RouteId { get; set; }
        public Route? Route { get; set; }
    }
}
