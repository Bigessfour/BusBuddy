namespace BusBuddy.Models
{
    public class ActivityTrip
    {
        public int ActivityID { get; set; }
        public string Date { get; set; } = string.Empty;
        public int BusNumber { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string LeaveTime { get; set; } = string.Empty;
        public string Driver { get; set; } = string.Empty;
        public string HoursDriven { get; set; } = string.Empty;
        public int StudentsDriven { get; set; }
    }
}