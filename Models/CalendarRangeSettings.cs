// Copyright (c) YourCompanyName. All rights reserved.
namespace BusBuddy.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents settings for a range of calendar days.
    /// </summary>
    public class CalendarRangeSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRangeSettings"/> class.
        /// </summary>
        public CalendarRangeSettings()
        {
            this.Notes = string.Empty;
            this.ActiveRouteIds = new List<int>();
        }

        /// <summary>
        /// Gets or sets the day type for the range.
        /// </summary>
        public SchoolDayType DayType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the days in the range are school days.
        /// </summary>
        public bool IsSchoolDay { get; set; }

        /// <summary>
        /// Gets or sets the notes for the days in the range.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the active route IDs for the days in the range.
        /// </summary>
        public List<int> ActiveRouteIds { get; set; }
    }
}