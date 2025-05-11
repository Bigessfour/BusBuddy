using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Represents a bus driver
    /// </summary>
    public class Driver : BaseEntity
    {
        /// <summary>
        /// Driver's full name
        /// </summary>
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        // Additional properties not included for brevity
    }
}
