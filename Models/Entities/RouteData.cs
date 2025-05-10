using System;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents route data for a specific day
    /// </summary>
    public class RouteData
    {
        /// <summary>
        /// The unique identifier for the route data
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The ID of the route this data belongs to
        /// </summary>
        public int RouteId { get; set; }
        
        /// <summary>
        /// The date this route data applies to
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// AM driver ID
        /// </summary>
        public int? AMDriverId { get; set; }
        
        /// <summary>
        /// AM driver
        /// </summary>
        public Driver? AMDriver { get; set; }
        
        /// <summary>
        /// PM driver ID
        /// </summary>
        public int? PMDriverId { get; set; }
        
        /// <summary>
        /// PM driver
        /// </summary>
        public Driver? PMDriver { get; set; }
        
        /// <summary>
        /// Starting mileage for morning route
        /// </summary>
        public decimal AMStartMileage { get; set; }
        
        /// <summary>
        /// Ending mileage for morning route
        /// </summary>
        public decimal AMEndMileage { get; set; }
        
        /// <summary>
        /// Starting mileage for afternoon route
        /// </summary>
        public decimal PMStartMileage { get; set; }
        
        /// <summary>
        /// Ending mileage for afternoon route
        /// </summary>
        public decimal PMEndMileage { get; set; }
    }
}
