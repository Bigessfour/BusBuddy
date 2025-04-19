// BusBuddy/Models/Driver.cs
namespace BusBuddy.Models
{
    public class Driver
    {
        public int DriverID { get; set; }
        public string Driver_Name { get; set; } = string.Empty; // Initialize to avoid null
        public string Address { get; set; } = string.Empty; // Initialize to avoid null
        public string City { get; set; } = string.Empty; // Initialize to avoid null
        public string State { get; set; } = string.Empty; // Initialize to avoid null
        public string Zip_Code { get; set; } = string.Empty; // Initialize to avoid null
        public string Phone_Number { get; set; } = string.Empty; // Initialize to avoid null
        public string Email_Address { get; set; } = string.Empty; // Initialize to avoid null
        public string Is_Stipend_Paid { get; set; } = string.Empty; // Initialize to avoid null
        public string DL_Type { get; set; } = string.Empty; // Initialize to avoid null
    }
}