namespace BusBuddy.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum SchoolDayType
    {
        Regular,
        EarlyRelease,
        LateStart,
        HalfDay,
        NoSchool
    }

    public sealed class SchoolCalendarDay
    {
        private const int TruckPlazaRouteId = 1;
        private const int EastRouteId = 2;
        private const int WestRouteId = 3;
        private const int SPEDRouteId = 4;

        public SchoolCalendarDay()
        {
            this.CalendarDate = DateTime.Now.ToString("yyyy-MM-dd");
            this.DayType = SchoolDayType.Regular;
            this.Notes = string.Empty;
            this.ActiveRouteIds = new List<int>();
            this.ScheduledRoutes = new List<ScheduledRoute>();
        }

        public SchoolCalendarDay(int calendarDayID, string calendarDate, bool isSchoolDay, string dayType, string notes)
        {
            this.CalendarDayID = calendarDayID;
            this.CalendarDate = calendarDate;
            this.IsSchoolDay = isSchoolDay;

            if (!Enum.TryParse<SchoolDayType>(dayType, true, out var parsedDayType))
            {
                parsedDayType = SchoolDayType.Regular;
            }

            this.DayType = parsedDayType;
            this.Notes = notes ?? string.Empty;
            this.ActiveRouteIds = new List<int>();
            this.ScheduledRoutes = new List<ScheduledRoute>();
        }

        public int CalendarDayID { get; set; }
        public string CalendarDate { get; set; } // 'YYYY-MM-DD'
        public bool IsSchoolDay { get; set; }
        public bool IsTeacherWorkDay { get; set; }
        public SchoolDayType DayType { get; set; } = SchoolDayType.Regular;
        public string Notes { get; set; } = string.Empty;
        public List<int> ActiveRouteIds { get; set; } = new List<int>();
        public List<ScheduledRoute> ScheduledRoutes { get; set; } = new List<ScheduledRoute>();
        
        // Add Date property that converts CalendarDate string to DateTime for DatabaseManager compatibility
        public DateTime Date
        {
            get => DateTime.Parse(CalendarDate);
            set => CalendarDate = value.ToString("yyyy-MM-dd");
        }

        public int CalendarDayId
        {
            get => CalendarDayID;
            set => CalendarDayID = value;
        }

        public bool IsRunningTruckPlazaRoute => this.IsSchoolDay &&
            (this.ScheduledRoutes.Any(sr => sr.RouteID == TruckPlazaRouteId) ||
             this.ActiveRouteIds.Contains(TruckPlazaRouteId));

        public bool IsRunningEastRoute => this.IsSchoolDay &&
            (this.ScheduledRoutes.Any(sr => sr.RouteID == EastRouteId) ||
             this.ActiveRouteIds.Contains(EastRouteId));

        public bool IsRunningWestRoute => this.IsSchoolDay &&
            (this.ScheduledRoutes.Any(sr => sr.RouteID == WestRouteId) ||
             this.ActiveRouteIds.Contains(WestRouteId));

        public bool IsRunningSPEDRoute => this.IsSchoolDay &&
            (this.ScheduledRoutes.Any(sr => sr.RouteID == SPEDRouteId) ||
             this.ActiveRouteIds.Contains(SPEDRouteId));

        public override string ToString()
        {
            return $"{DateTime.Parse(CalendarDate):d} - {(this.IsSchoolDay ? this.DayType.ToString() + " School Day" : "No School")}";
        }
    }
}