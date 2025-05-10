using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class DriverTests
    {
        [Fact]
        public void Driver_Properties_AreSetCorrectly()
        {
            var date = DateTime.Now.AddYears(1);
            var driver = new Driver
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                LicenseNumber = "DL12345678",
                LicenseExpiration = date,
                PhoneNumber = "555-123-4567",
                Email = "john.doe@example.com"
            };
            Assert.Equal(1, driver.Id);
            Assert.Equal("John", driver.FirstName);
            Assert.Equal("Doe", driver.LastName);
            Assert.Equal("John Doe", driver.FullName);
            Assert.Equal(date, driver.LicenseExpiration);
        }
    }
}
