// BusBuddy/Models/FuelRecord.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a fuel purchase record for a vehicle in the fleet.
    /// </summary>
    public class FuelRecord
    {
        [JsonPropertyName("FuelID")]
        public int FuelID { get; set; }
        
        [Required]
        [Range(1, 9999)]
        [JsonPropertyName("BusNumber")]
        public int BusNumber { get; set; }
        
        [Required]
        [JsonPropertyName("FuelDate")]
        public string FuelDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        
        [Required]
        [Range(0.1, 1000)]
        [JsonPropertyName("FuelGallons")]
        public double Gallons { get; set; }
        
        [Required]
        [JsonPropertyName("FuelType")]
        public string FuelType { get; set; } = "Diesel";
        
        [Required]
        [Range(0, 999999)]
        [JsonPropertyName("OdometerReading")]
        public int Odometer { get; set; }
        
        [JsonPropertyName("Notes")]
        public string Notes { get; set; } = string.Empty;
        
        [JsonPropertyName("Cost")]
        public double? Cost { get; set; }
        
        // Date property with conversion logic
        [JsonIgnore]
        public DateTime Date 
        { 
            get => DateTime.TryParse(FuelDate, out DateTime dt) ? dt : DateTime.Now; 
            set => FuelDate = value.ToString("yyyy-MM-dd"); 
        }
        
        /// <summary>
        /// Validates that this fuel record has all required fields.
        /// </summary>
        /// <returns>True if the record is valid, false otherwise.</returns>
        public bool IsValid()
        {
            return BusNumber > 0 
                && Gallons > 0
                && Odometer >= 0
                && !string.IsNullOrWhiteSpace(FuelDate);
        }
        
        /// <summary>
        /// Creates a deep copy of this fuel record.
        /// </summary>
        /// <returns>A new FuelRecord object with the same values.</returns>
        public FuelRecord Clone()
        {
            return new FuelRecord
            {
                FuelID = this.FuelID,
                BusNumber = this.BusNumber,
                FuelDate = this.FuelDate,
                Gallons = this.Gallons,
                FuelType = this.FuelType,
                Odometer = this.Odometer,
                Notes = this.Notes,
                Cost = this.Cost
            };
        }
    }
}