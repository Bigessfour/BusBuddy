namespace BusBuddy.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int BusNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int ModelYear { get; set; } // Maps to Year in UI
        public string VIN { get; set; }
        public string PlateNumber { get; set; }
        public int SeatingCapacity { get; set; } // Maps to Capacity in UI
        public int IsOperational { get; set; }
        public string PurchaseDate { get; set; } // 'YYYY-MM-DD'
        public string LastInspectionDate { get; set; } // 'YYYY-MM-DD'
        public int CurrentOdometer { get; set; }
        public double? PurchasePrice { get; set; }
        public string AnnualInspection { get; set; }
    }
}