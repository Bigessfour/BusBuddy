using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Represents a trip for an activity (field trip, sporting event, etc.)
    /// </summary>
    public class ActivityTrip : BaseEntity
    {
        /// <summary>
        /// Name or title of the activity
        /// </summary>
        [Required]
        public string ActivityName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the activity
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Date and time of departure
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Date and time of return
        /// </summary>
        public DateTime ReturnTime { get; set; }
        
        /// <summary>
        /// Date of the activity trip
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Number of students on the trip
        /// </summary>
        public int StudentCount { get; set; }

        /// <summary>
        /// Number of adult chaperones
        /// </summary>
        public int ChaperonCount { get; set; }

        /// <summary>
        /// ID of the driver assigned to this trip
        /// </summary>
        public int DriverId { get; set; }

        /// <summary>
        /// Navigation property for the driver
        /// </summary>
        public Driver Driver { get; set; } = null!;

        /// <summary>
        /// ID of the vehicle used for this trip
        /// </summary>
        public int VehicleId { get; set; }

        /// <summary>
        /// Navigation property for the vehicle
        /// </summary>
        public Vehicle Vehicle { get; set; } = null!;

        /// <summary>
        /// ID of the route, if this trip follows a predefined route
        /// </summary>
        public int? RouteId { get; set; }

        /// <summary>
        /// Navigation property for the route
        /// </summary>
        public Route? Route { get; set; }

        /// <summary>
        /// ID of the destination
        /// </summary>
        public int? DestinationId { get; set; }

        /// <summary>
        /// Navigation property for the destination
        /// </summary>
        public Destination? Destination { get; set; }
    }
}
