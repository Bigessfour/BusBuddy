using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a fuel purchase record
    /// </summary>
    public class FuelEntry
    {
        /// <summary>
        /// The unique identifier for the fuel entry
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the vehicle being fueled
        /// </summary>
        public int VehicleId { get; set; }

        /// <summary>
        /// Navigation property for the vehicle
        /// </summary>
        public Vehicle Vehicle { get; set; } = null!;

        /// <summary>
        /// Date and time of the fuel purchase
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Amount of fuel purchased in gallons
        /// </summary>
        public decimal FuelAmount { get; set; }

        /// <summary>
        /// Price per gallon paid
        /// </summary>
        public decimal PricePerGallon { get; set; }

        /// <summary>
        /// Total cost of the fuel purchase
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Odometer reading at time of fueling
        /// </summary>
        public decimal Mileage { get; set; }

        /// <summary>
        /// ID of the driver who purchased the fuel
        /// </summary>
        public int? DriverId { get; set; }

        /// <summary>
        /// Navigation property for the driver
        /// </summary>
        public Driver? Driver { get; set; }

        /// <summary>
        /// Notes about the fuel purchase
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Date and time the fuel entry was added to the system
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
