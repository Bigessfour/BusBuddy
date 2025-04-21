// File Path: BusBuddy/Models/Trip.cs
#nullable enable
using System;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a scheduled trip or activity.
    /// </summary>
    public class Trip
    {
        /// <summary>
        /// Unique identifier for the trip.
        /// </summary>
        public int TripID { get; set; }

        /// <summary>
        /// Type of trip (e.g., "Morning Route", "Afternoon Route", "Field Trip", "Sports").
        /// </summary>
        public string TripType { get; set; } = string.Empty;

        /// <summary>
        /// Date of the trip.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Start time of the trip.
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// End time of the trip.
        /// </summary>
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// Destination or activity description.
        /// </summary>
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Bus number assigned to the trip.
        /// </summary>
        public int BusNumber { get; set; }

        /// <summary>
        /// Name of the driver assigned to the trip.
        /// </summary>
        public string DriverName { get; set; } = string.Empty;

        /// <summary>
        /// For activity trips: Total hours driven for the trip.
        /// </summary>
        public double TotalHoursDriven { get; set; }
        
        /// <summary>
        /// String representation of total hours driven (for compatibility)
        /// </summary>
        public string Total_Hours_Driven { get; set; } = string.Empty;
        
        /// <summary>
        /// For activity trips with stipend drivers: Total miles driven.
        /// </summary>
        public double MilesDriven { get; set; }
        
        /// <summary>
        /// For route trips: Whether this is a CDL route (higher pay) or not.
        /// </summary>
        public bool IsCDLRoute { get; set; }
        
        /// <summary>
        /// Category of the trip: "Route" or "Activity"
        /// </summary>
        public string TripCategory { get; set; } = "Route";
    }
}