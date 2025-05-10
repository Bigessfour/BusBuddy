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
            var driver = new Driver
            {
                Id = 1,
                Name = "John Doe",
                LicenseExpiry = DateTime.Now.AddYears(1)
            };
            Assert.Equal(1, driver.Id);
            Assert.Equal("John Doe", driver.Name);
        }
    }
}
