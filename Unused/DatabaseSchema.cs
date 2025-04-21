namespace BusBuddy.Data
{
    public static class DatabaseSchema
    {
        public static class Trips
        {
            public const string TableName = "Trips";
            public const string TripID = "TripID";
            public const string TripType = "TripType";
            public const string Date = "Date";
            public const string BusNumber = "BusNumber";
            public const string DriverName = "DriverName";
            public const string StartTime = "StartTime";
            public const string EndTime = "EndTime";
            public const string TotalHoursDriven = "Total_Hours_Driven";
            public const string Destination = "Destination";
        }
    }
}