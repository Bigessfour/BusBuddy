using System;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Models.Entities
{
    /// <summary>
    /// Represents a bus driver
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// The unique identifier for the driver
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the driver
        /// </summary>
        [Required]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the driver
        /// </summary>
        [Required]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Driver's license number
        /// </summary>
        [Required]
        public string LicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// License expiration date
        /// </summary>
        public DateTime LicenseExpiration { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Contact email
        /// </summary>
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Date and time the driver was added to the system
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Full name of the driver (calculated property)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
}
