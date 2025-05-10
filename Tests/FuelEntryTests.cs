using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class FuelEntryTests
    {
        [Fact]
        public void FuelEntry_Properties_AreSetCorrectly()
        {
            var purchaseDate = DateTime.Now;
            var entry = new FuelEntry
            {
                Id = 1,
                VehicleId = 1,
                PurchaseDate = purchaseDate,
                FuelAmount = 50.5m,
                PricePerGallon = 3.25m,
                TotalCost = 164.13m,
                Mileage = 12500.5m
            };
            
            Assert.Equal(1, entry.Id);
            Assert.Equal(50.5m, entry.FuelAmount);
            Assert.Equal(3.25m, entry.PricePerGallon);
            Assert.Equal(164.13m, entry.TotalCost);
            Assert.Equal(purchaseDate, entry.PurchaseDate);
        }
    }
}
