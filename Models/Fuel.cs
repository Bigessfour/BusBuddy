// BusBuddy/Models/Fuel.cs
namespace BusBuddy.Models
{
    public class Fuel
    {
        public int Fuel_ID { get; set; }
        public int Bus_Number { get; set; }
        public int Fuel_Gallons { get; set; }
        public string Fuel_Date { get; set; } = string.Empty; // Initialize to avoid null
        public string Fuel_Type { get; set; } = string.Empty; // Initialize to avoid null
        public int Odometer_Reading { get; set; }
    }
}