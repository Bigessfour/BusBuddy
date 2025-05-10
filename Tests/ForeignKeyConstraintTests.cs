using System;
using System.Threading.Tasks;
using BusBuddy.Data;
using BusBuddy.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BusBuddy.Tests
{
    public class ForeignKeyConstraintTests
    {
        [Fact]
        public async Task DeleteDriver_WithBothAMAndPMReferences_ShouldSucceed()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BusBuddyContext>()
                .UseInMemoryDatabase(databaseName: "DeleteDriverWithMultipleReferences_Test")
                .Options;

            // Create test data
            using (var context = new BusBuddyContext(options))
            {
                // Add two drivers
                var driver1 = new Driver
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    LicenseNumber = "DL12345",
                    Email = "john@example.com",
                    PhoneNumber = "555-1234"
                };

                var driver2 = new Driver
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    LicenseNumber = "DL67890",
                    Email = "jane@example.com",
                    PhoneNumber = "555-5678"
                };
                
                context.Drivers.AddRange(driver1, driver2);
                
                // Add a route
                var route = new Route
                {
                    Id = 1,
                    RouteName = "Test Route",
                    StartLocation = "Start",
                    EndLocation = "End",
                    Distance = 10.5m,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                };
                
                context.Routes.Add(route);
                
                // Add route data with both AM and PM drivers set to driver1
                var routeData = new RouteData
                {
                    Id = 1,
                    RouteId = 1,
                    Date = DateTime.Today,
                    AMDriverId = 1, // Driver1
                    PMDriverId = 1, // Driver1 also as PM driver
                    AMStartMileage = 100,
                    AMEndMileage = 150,
                    PMStartMileage = 150,
                    PMEndMileage = 200
                };
                
                context.RouteData.Add(routeData);
                
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new BusBuddyContext(options))
            {
                var mockLogger = new Mock<ILogger<BusBuddyContext>>();
                
                // Test deletion with reassignment
                var result = await context.DeleteDriverSafelyAsync(1, 2, mockLogger.Object);
                
                // Verify deletion was successful
                Assert.True(result);
                Assert.Null(await context.Drivers.FindAsync(1)); // Driver1 should be deleted
                
                // Verify RouteData references are updated correctly
                var routeData = await context.RouteData.FirstAsync();
                Assert.Equal(2, routeData.AMDriverId); // Should be reassigned to driver2
                Assert.Equal(2, routeData.PMDriverId); // Should be reassigned to driver2
            }
            
            // Test deletion without reassignment
            using (var context = new BusBuddyContext(options))
            {
                // Reset the test data
                context.Database.EnsureDeleted();
                
                // Recreate test data
                var driver1 = new Driver
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    LicenseNumber = "DL12345",
                    Email = "john@example.com",
                    PhoneNumber = "555-1234"
                };
                
                context.Drivers.Add(driver1);
                
                var route = new Route
                {
                    Id = 1,
                    RouteName = "Test Route",
                    StartLocation = "Start",
                    EndLocation = "End",
                    Distance = 10.5m,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now
                };
                
                context.Routes.Add(route);
                
                var routeData = new RouteData
                {
                    Id = 1,
                    RouteId = 1,
                    Date = DateTime.Today,
                    AMDriverId = 1,
                    PMDriverId = 1,
                    AMStartMileage = 100,
                    AMEndMileage = 150,
                    PMStartMileage = 150,
                    PMEndMileage = 200
                };
                
                context.RouteData.Add(routeData);
                
                await context.SaveChangesAsync();
                
                // Test deletion without reassignment (should set to null)
                var mockLogger = new Mock<ILogger<BusBuddyContext>>();
                var result = await context.DeleteDriverSafelyAsync(1, null, mockLogger.Object);
                
                // Verify deletion was successful
                Assert.True(result);
                Assert.Null(await context.Drivers.FindAsync(1)); // Driver1 should be deleted
                
                // Verify RouteData references are nullified
                var updatedRouteData = await context.RouteData.FirstAsync();
                Assert.Null(updatedRouteData.AMDriverId); // Should be null
                Assert.Null(updatedRouteData.PMDriverId); // Should be null
            }
        }
    }
}
