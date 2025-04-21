// BusBuddy/Models/FuelRecord.cs
using System;

namespace BusBuddy.Models
{
    public class FuelRecord
    {
        // Primary properties
        public int Fuel_ID { get; set; }
        public int Bus_Number { get; set; }
        public string Fuel_Date { get; set; } = string.Empty;
        public double Fuel_Gallons { get; set; }
        public double Cost { get; set; }
        public string Fuel_Type { get; set; } = string.Empty;
        public int Odometer_Reading { get; set; }
        
        // Aliases for DatabaseManager
        public int RecordId { get => Fuel_ID; set => Fuel_ID = value; }
        public int BusNumber { get => Bus_Number; set => Bus_Number = value; }
        public double Gallons { get => Fuel_Gallons; set => Fuel_Gallons = value; }
        
        // Conversion methods
        public static FuelRecord FromFuel(Fuel fuel)
        {
            return new FuelRecord
            {
                Fuel_ID = fuel.Fuel_ID,
                Bus_Number = fuel.Bus_Number,
                Fuel_Date = fuel.Fuel_Date,
                Fuel_Gallons = fuel.Fuel_Gallons,
                Fuel_Type = fuel.Fuel_Type,
                Odometer_Reading = fuel.Odometer_Reading
            };
        }
    }
}