using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class PartTests
    {
        [Fact]
        public void Part_Properties_AreSetCorrectly()
        {
            var part = new Part
            {
                Id = 1,
                Name = "Brake Pad"
            };
            Assert.Equal(1, part.Id);
            Assert.Equal("Brake Pad", part.Name);
        }
    }
}
