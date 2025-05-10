using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class VehicleTests
    {
        [Fact]
        public void Vehicle_Properties_AreSetCorrectly()
        {
            var vehicle = new Vehicle
            {
                Id = 1,
                LicensePlate = "ABC123",
                InsuranceExpiry = DateTime.Now.AddYears(1)
            };
            Assert.Equal(1, vehicle.Id);
            Assert.Equal("ABC123", vehicle.LicensePlate);
        }
    }
}
