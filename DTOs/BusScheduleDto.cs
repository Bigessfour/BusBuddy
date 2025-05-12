using System;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Data transfer object for bus schedule information displayed on the dashboard
    /// </summary>
    public class BusScheduleDto
    {
        /// <summary>
        /// Gets or sets the route name
        /// </summary>
        public string RouteName { get; set; }
        
        /// <summary>
        /// Gets or sets the driver name
        /// </summary>
        public string DriverName { get; set; }
        
        /// <summary>
        /// Gets or sets the vehicle information
        /// </summary>
        public string VehicleInfo { get; set; }
        
        /// <summary>
        /// Gets or sets the current status (e.g., On Time, Delayed)
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Gets or sets the last updated timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
