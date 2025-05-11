using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Represents a vehicle/bus
    /// </summary>
    public class Vehicle : BaseEntity
    {
        /// <summary>
        /// Vehicle identification number
        /// </summary>
        public string VehicleNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Current odometer reading
        /// </summary>
        public decimal Odometer { get; set; }
        
        /// <summary>
        /// ID of the driver assigned to this vehicle
        /// </summary>
        public int? AssignedDriverId { get; set; }
        
        /// <summary>
        /// Driver assigned to this vehicle
        /// </summary>
        public Driver? AssignedDriver { get; set; }
    }
}
