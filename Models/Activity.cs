// BusBuddy/Models/Activity.cs
using System;

namespace BusBuddy.Models
{
    public class Activity
    {
        // Primary properties
        public int ActivityID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int BusNumber { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string LeaveTime { get; set; } = string.Empty;
        public string Driver { get; set; } = string.Empty;
        public double HoursDriven { get; set; }
        public int StudentsDriven { get; set; }
        
        // Aliases for DatabaseManager
        public int ActivityId { get => ActivityID; set => ActivityID = value; }
        
        // Conversion methods
        public static Activity FromActivityTrip(ActivityTrip trip)
        {
            return new Activity
            {
                ActivityID = trip.ActivityID,
                Name = trip.Destination,
                Description = $"Bus: {trip.BusNumber}, Driver: {trip.Driver}",
                Date = trip.Date,
                BusNumber = trip.BusNumber,
                Destination = trip.Destination,
                LeaveTime = trip.LeaveTime,
                Driver = trip.Driver,
                HoursDriven = Convert.ToDouble(trip.HoursDriven),
                StudentsDriven = trip.StudentsDriven
            };
        }
    }
}