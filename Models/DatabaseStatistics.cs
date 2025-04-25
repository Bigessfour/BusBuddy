// BusBuddy/Models/DatabaseStatistics.cs
using System;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents statistics about the database
    /// </summary>
    public class DatabaseStatistics
    {
        /// <summary>
        /// The total number of trips in the database
        /// </summary>
        public int TotalTrips { get; set; }
        
        /// <summary>
        /// The total number of drivers in the database
        /// </summary>
        public int TotalDrivers { get; set; }
        
        /// <summary>
        /// The total number of routes in the database
        /// </summary>
        public int TotalRoutes { get; set; }
        
        /// <summary>
        /// The total number of fuel records in the database
        /// </summary>
        public int TotalFuelRecords { get; set; }
        
        /// <summary>
        /// The total number of maintenance records in the database
        /// </summary>
        public int TotalMaintenanceRecords { get; set; }
        
        /// <summary>
        /// The total number of activities in the database
        /// </summary>
        public int TotalActivities { get; set; }
        
        /// <summary>
        /// The timestamp of when these statistics were collected
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}