using System;
// Add this to use RouteEntity
using RouteEntity = BusBuddy.Models.Entities.Route;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a driver's scheduled assignment
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Gets or sets the schedule ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the driver ID
        /// </summary>
        public int DriverId { get; set; }
        
        /// <summary>
        /// Gets or sets the related driver
        /// </summary>
        public Driver Driver { get; set; }
        
        /// <summary>
        /// Gets or sets the vehicle ID (optional)
        /// </summary>
        public int? VehicleId { get; set; }
        
        /// <summary>
        /// Gets or sets the related vehicle
        /// </summary>
        public Vehicle Vehicle { get; set; }
        
        /// <summary>
        /// Gets or sets the route ID (optional)
        /// </summary>
        public int? RouteId { get; set; }
        
        /// <summary>
        /// Gets or sets the related route
        /// </summary>
        public RouteEntity Route { get; set; }
        
        /// <summary>
        /// Gets or sets the schedule start time
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// Gets or sets the schedule end time
        /// </summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>
        /// Gets or sets the schedule title or description
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets notes about this schedule
        /// </summary>
        public string Notes { get; set; }
        
        /// <summary>
        /// Gets or sets the status of this schedule
        /// </summary>
        public string Status { get; set; }
    }
}
