using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.ValueObjects
{
    /// <summary>
    /// Represents an address value object
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Street address
        /// </summary>
        [Required]
        public string Street { get; set; } = string.Empty;
        
        /// <summary>
        /// City
        /// </summary>
        [Required]
        public string City { get; set; } = string.Empty;
        
        /// <summary>
        /// State
        /// </summary>
        [Required]
        public string State { get; set; } = string.Empty;
        
        /// <summary>
        /// ZIP Code
        /// </summary>
        [Required]
        public string ZipCode { get; set; } = string.Empty;
    }
}
