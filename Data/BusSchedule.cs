using System;

namespace BusBuddy.Data
{
    public class BusSchedule
    {
        public int Id { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public DateTime Time { get; set; }
    }
}
