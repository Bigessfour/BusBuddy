// BusBuddy/Models/Activity.cs
using System;

namespace BusBuddy.Models
{
    public class Activity
    {
        public string Title { get; set; } = string.Empty;

        public int ActivityID { get; set; }
        public string ActivityDate { get; set; } = string.Empty; // 'YYYY-MM-DD'
        public int BusNumber { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string LeaveTime { get; set; } = string.Empty;
        public int DriverID { get; set; }
        public double HoursDriven { get; set; }
        public int StudentsDriven { get; set; }
        public string? Name { get; internal set; }

        // Conversion methods
        public static Activity FromActivityTrip(ActivityTrip trip)
        {
            return new Activity
            {
                ActivityID = trip.ActivityID,
                ActivityDate = trip.ActivityDate,
                BusNumber = trip.BusNumber,
                Destination = trip.Destination,
                LeaveTime = trip.LeaveTime,
                DriverID = trip.DriverID,
                HoursDriven = trip.HoursDriven,
                StudentsDriven = trip.StudentsDriven
            };
        }
    }
}