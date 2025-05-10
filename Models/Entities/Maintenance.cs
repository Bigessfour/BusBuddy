using System;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a maintenance record for a vehicle
    /// </summary>
    public class Maintenance
    {
        /// <summary>
        /// The unique identifier for the maintenance record
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The ID of the vehicle this maintenance was performed on
        /// </summary>
        public int VehicleId { get; set; }
        
        /// <summary>
        /// The vehicle this maintenance was performed on
        /// </summary>
        public Vehicle? Vehicle { get; set; }
        
        /// <summary>
        /// Date maintenance was performed
        /// </summary>
        public DateTime MaintenanceDate { get; set; }
        
        /// <summary>
        /// Description of maintenance performed
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
