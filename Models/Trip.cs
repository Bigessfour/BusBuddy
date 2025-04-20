#nullable enable
using System;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a scheduled trip or activity.
    /// </summary>
    public class Trip
    {
        public int TripID { get; set; } // Assuming an ID might be useful
        public string TripType { get; set; } = string.Empty; // e.g., "Route", "Activity"
        public string Date { get; set; } = string.Empty; // Format "yyyy-MM-dd"
        public string StartTime { get; set; } = string.Empty; // Format "hh:mm"
        public string EndTime { get; set; } = string.Empty; // Format "hh:mm"
        public string Destination { get; set; } = string.Empty; // Route name or activity description
        public int BusNumber { get; set; } // Changed to int to match database and usage
        public string DriverName { get; set; } = string.Empty;
        public string Total_Hours_Driven { get; set; } = string.Empty; // Added to match database schema
    }
}