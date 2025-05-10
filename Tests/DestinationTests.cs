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
                Name = "Elementary School",
                Address = "123 Education St",
                City = "Springfield",
                State = "IL",
                ZipCode = "62701",
                ContactName = "Principal Smith",
                ContactPhone = "555-123-4567",
                TotalMiles = 15.5m
            };
            
            Assert.Equal(1, destination.Id);
            Assert.Equal("Elementary School", destination.Name);
            Assert.Equal("Springfield", destination.City);
            Assert.Equal("123 Education St", destination.Address);
            Assert.Equal(15.5m, destination.TotalMiles);
        }
    }
}
