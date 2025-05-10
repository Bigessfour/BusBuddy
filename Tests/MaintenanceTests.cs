using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class MaintenanceTests
    {
        [Fact]
        public void Maintenance_Properties_AreSetCorrectly()
        {
            var maintenance = new Maintenance
            {
                Id = 1,
                Description = "Oil Change"
            };
            Assert.Equal(1, maintenance.Id);
            Assert.Equal("Oil Change", maintenance.Description);
        }
    }
}
