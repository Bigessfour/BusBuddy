namespace BusBuddy.Tests;

using BusBuddy.Models;
using System;
using Xunit;

public class BasicModelTests
{
    [Fact]
    public void Trip_Properties_WorkCorrectly()
    {
        // Arrange
        var trip = new Trip
        {
            TripID = 1,
            TripType = "Field Trip",
            Date = DateOnly.Parse("2025-04-20"),
            BusNumber = 42,
            DriverName = "Jane Smith",
            StartTime = TimeOnly.Parse("09:00"),
            EndTime = TimeOnly.Parse("15:00"),
            TotalHoursDriven = 6.0,
            Destination = "Science Museum"
        };

        // Act & Assert
        Assert.Equal(1, trip.TripID);
        Assert.Equal("Field Trip", trip.TripType);
        Assert.Equal(DateOnly.Parse("2025-04-20"), trip.Date);
        Assert.Equal(42, trip.BusNumber);
        Assert.Equal("Jane Smith", trip.DriverName);
        Assert.Equal(TimeOnly.Parse("09:00"), trip.StartTime);
        Assert.Equal(TimeOnly.Parse("15:00"), trip.EndTime);
        Assert.Equal(6.0, trip.TotalHoursDriven);
        Assert.Equal("Science Museum", trip.Destination);
    }
}
