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
            var entry = new FuelEntry
            {
                Id = 1,
                Amount = 50
            };
            Assert.Equal(1, entry.Id);
            Assert.Equal(50, entry.Amount);
        }
    }
}
