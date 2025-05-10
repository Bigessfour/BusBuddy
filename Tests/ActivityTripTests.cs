using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class ActivityTripTests
    {
        [Fact]
        public void ActivityTrip_Properties_AreSetCorrectly()
        {
            var trip = new ActivityTrip
            {
                Id = 1,
                ActivityName = "Field Trip"
            };
            Assert.Equal(1, trip.Id);
            Assert.Equal("Field Trip", trip.ActivityName);
        }
    }
}
