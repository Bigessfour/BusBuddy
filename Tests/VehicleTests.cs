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
            var date = DateTime.Now.AddYears(1);
            var vehicle = new Vehicle
            {
                Id = 1,
                VehicleNumber = "Bus-01",
                Make = "Blue Bird",
                Model = "Vision",
                Year = 2023,
                LicensePlate = "ABC123",
                VIN = "1HGCM82633A123456",
                InsuranceExpiration = date
            };
            Assert.Equal(1, vehicle.Id);
            Assert.Equal("ABC123", vehicle.LicensePlate);
            Assert.Equal("Bus-01", vehicle.VehicleNumber);
            Assert.Equal(date, vehicle.InsuranceExpiration);
        }
    }
}
