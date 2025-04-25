namespace BusBuddy.Models
{
    public class Maintenance
    {
        public int MaintenanceID { get; set; }
        public int BusNumber { get; set; }
        public string DatePerformed { get; set; } = string.Empty; // 'YYYY-MM-DD'
        public string Description { get; set; } = string.Empty;
        public double? Cost { get; set; }
        public int? OdometerReading { get; set; }
    }
}