// new-project-files/Models/Trip.cs
namespace BusBuddy.Models
{
    public class Trip
    {
        public int TripID { get; set; }
        public string TripType { get; set; } = string.Empty; // Initialize to avoid null
        public string Date { get; set; } = string.Empty; // Initialize to avoid null
        public int BusNumber { get; set; }
        public string DriverName { get; set; } = string.Empty; // Initialize to avoid null
        public string StartTime { get; set; } = string.Empty; // Initialize to avoid null
        public string EndTime { get; set; } = string.Empty; // Initialize to avoid null
        public string Total_Hours_Driven { get; set; } = string.Empty; // Initialize to avoid null
        public string Destination { get; set; } = string.Empty; // Added missing property
    }
}