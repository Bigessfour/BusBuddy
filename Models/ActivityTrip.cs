namespace BusBuddy.Models
{
    public class ActivityTrip
    {
        public int ActivityID { get; set; }
        public string ActivityDate { get; set; } = string.Empty; // 'YYYY-MM-DD'
        public int BusNumber { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string LeaveTime { get; set; } = string.Empty;
        public int DriverID { get; set; }
        public double HoursDriven { get; set; }
        public int StudentsDriven { get; set; }
    }
}