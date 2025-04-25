namespace BusBuddy.Models
{
    public class Driver
    {
        public int DriverID { get; set; }
        public string Name { get => DriverName; set => DriverName = value; }
        public string DriverName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool IsStipendPaid { get; set; }
        public string DLType { get; set; }
    }
}