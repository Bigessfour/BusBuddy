using Xunit;
using BusBuddy.Models.Entities;
using System;

namespace BusBuddy.Tests
{
    public class RouteEntityTests
    {
        [Fact]
        public void Route_Properties_AreSetCorrectly()
        {
            var route = new Route
            {
                Id = 1,
                Name = "Route 1"
            };
            Assert.Equal(1, route.Id);
            Assert.Equal("Route 1", route.Name);
        }
    }
}
