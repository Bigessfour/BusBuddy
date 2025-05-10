using Xunit;
using BusBuddy.Data;
using BusBuddy.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusBuddy.Tests
{
    public class DeleteDriverTests
    {
        [Fact]
        public async Task DeleteDriverSafely_ShouldNullifyRouteDataReferences_WhenDeletingDriver()
        {            // Arrange
            var options = new DbContextOptionsBuilder<BusBuddyContext>()
                .UseInMemoryDatabase(databaseName: $"DeleteDriverTest_Nullify_{Guid.NewGuid()}")
                .Options;

            // Set up test data
            using (var context = new BusBuddyContext(options))
            {
                // Create a driver to delete
                var driver = new Driver
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    LicenseNumber = "DL12345"
                };
                context.Drivers.Add(driver);

                // Create route data referencing this driver
                var routeData = new RouteData
                {
                    Id = 1,
                    RouteId = 1,
                    Date = DateTime.Today,
                    AMDriverId = 1,
                    PMDriverId = 1
                };
                context.RouteData.Add(routeData);

                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new BusBuddyContext(options))
            {
                var mockLogger = new Mock<ILogger<BusBuddyContext>>();
                var result = await context.DeleteDriverSafelyAsync(1, null, mockLogger.Object);

                // Assert
                Assert.True(result);
                Assert.Null(await context.Drivers.FindAsync(1));
                
                // Verify route data references are nullified
                var routeData = await context.RouteData.FirstAsync();
                Assert.Null(routeData.AMDriverId);
                Assert.Null(routeData.PMDriverId);
            }
        }

        [Fact]
        public async Task DeleteDriverSafely_ShouldReassignRouteDataReferences_WhenReassignmentDriverSpecified()
        {            // Arrange
            var options = new DbContextOptionsBuilder<BusBuddyContext>()
                .UseInMemoryDatabase(databaseName: $"DeleteDriverTest_Reassign_{Guid.NewGuid()}")
                .Options;

            // Set up test data
            using (var context = new BusBuddyContext(options))
            {
                // Create a driver to delete
                var driver1 = new Driver
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    LicenseNumber = "DL12345"
                };
                
                // Create a driver to reassign to
                var driver2 = new Driver
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    LicenseNumber = "DL67890"
                };
                
                context.Drivers.AddRange(driver1, driver2);

                // Create route data referencing the driver to delete
                var routeData = new RouteData
                {
                    Id = 1,
                    RouteId = 1,
                    Date = DateTime.Today,
                    AMDriverId = 1,
                    PMDriverId = 1
                };
                context.RouteData.Add(routeData);

                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new BusBuddyContext(options))
            {
                var mockLogger = new Mock<ILogger<BusBuddyContext>>();
                var result = await context.DeleteDriverSafelyAsync(1, 2, mockLogger.Object);

                // Assert
                Assert.True(result);
                Assert.Null(await context.Drivers.FindAsync(1));
                
                // Verify route data references are reassigned to driver 2
                var routeData = await context.RouteData.FirstAsync();
                Assert.Equal(2, routeData.AMDriverId);
                Assert.Equal(2, routeData.PMDriverId);
            }
        }

        [Fact]
        public async Task DeleteDriverSafely_ShouldReturnFalse_WhenDriverNotFound()
        {            // Arrange
            var options = new DbContextOptionsBuilder<BusBuddyContext>()
                .UseInMemoryDatabase(databaseName: $"DeleteDriverTest_NotFound_{Guid.NewGuid()}")
                .Options;

            // Act
            using (var context = new BusBuddyContext(options))
            {
                var mockLogger = new Mock<ILogger<BusBuddyContext>>();
                var result = await context.DeleteDriverSafelyAsync(999, null, mockLogger.Object);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task DeleteDriverSafely_ShouldHandleActivityTrips_WhenDeletingDriver()
        {            // Arrange
            var options = new DbContextOptionsBuilder<BusBuddyContext>()
                .UseInMemoryDatabase(databaseName: $"DeleteDriverTest_ActivityTrips_{Guid.NewGuid()}")
                .Options;

            // Set up test data
            using (var context = new BusBuddyContext(options))
            {
                // Create a driver to delete
                var driver = new Driver
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    LicenseNumber = "DL12345"
                };
                
                // Create a vehicle for the activity trip
                var vehicle = new Vehicle
                {
                    Id = 1,
                    VehicleNumber = "Bus-001",
                    Make = "Blue Bird",
                    Model = "School Bus",
                    Year = 2023,
                    VIN = "12345678901234567",
                    LicensePlate = "SCHOOL1"
                };
                
                context.Drivers.Add(driver);
                context.Vehicles.Add(vehicle);                // Create an activity trip referencing this driver
                var activityTrip = new ActivityTrip
                {
                    Id = 1,
                    ActivityName = "Field Trip",
                    DepartureTime = DateTime.Today,
                    ReturnTime = DateTime.Today.AddHours(6),
                    DriverId = 1,
                    VehicleId = 1
                };
                context.ActivityTrips.Add(activityTrip);

                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new BusBuddyContext(options))
            {
                var mockLogger = new Mock<ILogger<BusBuddyContext>>();
                var result = await context.DeleteDriverSafelyAsync(1, null, mockLogger.Object);

                // Assert
                Assert.True(result);
                Assert.Null(await context.Drivers.FindAsync(1));
                
                // Verify activity trip was deleted due to cascade delete
                Assert.Empty(await context.ActivityTrips.ToListAsync());
            }
        }
    }
}
