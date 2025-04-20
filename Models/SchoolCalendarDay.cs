// BusBuddy/Models/SchoolCalendarDay.cs
using System;
using System.Collections.Generic;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a day in the school calendar
    /// </summary>
    public class SchoolCalendarDay
    {
        /// <summary>
        /// Unique identifier for the calendar day
        /// </summary>
        public int CalendarDayId { get; set; }

        /// <summary>
        /// The date of this calendar entry
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Whether this is a school day (buses run)
        /// </summary>
        public bool IsSchoolDay { get; set; }

        /// <summary>
        /// Description of the day (Regular, Early Release, etc.)
        /// </summary>
        public string DayType { get; set; }

        /// <summary>
        /// Notes about this calendar day
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// List of route IDs that run on this day
        /// </summary>
        public List<int> ActiveRouteIds { get; set; }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public SchoolCalendarDay(int calendarDayId, DateTime date, bool isSchoolDay, string dayType, string notes)
        {
            CalendarDayId = calendarDayId;
            Date = date;
            IsSchoolDay = isSchoolDay;
            DayType = dayType;
            Notes = notes;
            ActiveRouteIds = new List<int>();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SchoolCalendarDay()
        {
            Date = DateTime.Now;
            DayType = "Regular";
            Notes = string.Empty;
            ActiveRouteIds = new List<int>();
        }

        /// <summary>
        /// Helper to determine if the Truck Plaza route should run
        /// </summary>
        public bool IsRunningTruckPlazaRoute => IsSchoolDay && ActiveRouteIds.Contains(1);

        /// <summary>
        /// Helper to determine if the East route should run
        /// </summary>
        public bool IsRunningEastRoute => IsSchoolDay && ActiveRouteIds.Contains(2);

        /// <summary>
        /// Helper to determine if the West route should run
        /// </summary>
        public bool IsRunningWestRoute => IsSchoolDay && ActiveRouteIds.Contains(3);

        /// <summary>
        /// Helper to determine if the SPED route should run
        /// </summary>
        public bool IsRunningSPEDRoute => IsSchoolDay && ActiveRouteIds.Contains(4);
    }
}