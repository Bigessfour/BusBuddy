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
                RouteName = "Route 1",
                StartLocation = "School",
                EndLocation = "Downtown",
                Distance = 10.5m
            };
            Assert.Equal(1, route.Id);
            Assert.Equal("Route 1", route.RouteName);
            Assert.Equal("School", route.StartLocation);
            Assert.Equal("Downtown", route.EndLocation);
            Assert.Equal(10.5m, route.Distance);
        }
    }
}
