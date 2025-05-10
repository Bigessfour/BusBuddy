using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class DestinationTests
    {
        [Fact]
        public void Destination_Properties_AreSetCorrectly()
        {
            var destination = new Destination
            {
                Id = 1,
                Name = "School"
            };
            Assert.Equal(1, destination.Id);
            Assert.Equal("School", destination.Name);
        }
    }
}
