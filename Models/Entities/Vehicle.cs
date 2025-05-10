using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a bus vehicle
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// The unique identifier for the vehicle
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The vehicle number or identification
        /// </summary>
        [Required]
        public string VehicleNumber { get; set; } = string.Empty;

        /// <summary>
        /// The make of the vehicle
        /// </summary>
        [Required]
        public string Make { get; set; } = string.Empty;

        /// <summary>
        /// The model of the vehicle
        /// </summary>
        [Required]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// The year the vehicle was manufactured
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Vehicle identification number
        /// </summary>
        [Required]
        public string VIN { get; set; } = string.Empty;

        /// <summary>
        /// License plate number
        /// </summary>
        [Required]
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Current odometer reading in miles
        /// </summary>
        public decimal Odometer { get; set; }

        /// <summary>
        /// Seating capacity of the vehicle
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Date and time the vehicle was added to the system
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// ID of the driver assigned to this vehicle, if any
        /// </summary>
        public int? AssignedDriverId { get; set; }

        /// <summary>
        /// Navigation property for the assigned driver
        /// </summary>
        public Driver? AssignedDriver { get; set; }
    }
}
