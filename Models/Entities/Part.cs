using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a vehicle part
    /// </summary>
    public class Part
    {
        /// <summary>
        /// The unique identifier for the part
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Part name
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Part number
        /// </summary>
        [Required]
        public string PartNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Unit price of the part
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
