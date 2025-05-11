using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using BusBuddy;
using BusBuddy.Data;
using BusBuddy.Services.Dashboard;
using BusBuddy.DTOs;
using BusBuddy.Hubs;
using BusBuddy.Models.Entities;
using BusBuddy.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Text.RegularExpressions;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Integration tests for the BusBuddy application that test from startup through
    /// database initialization and Dashboard.razor rendering
    /// </summary>
    public class DashboardStartupTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly ITestOutputHelper _output;

        public DashboardStartupTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task CompleteStartupTest_ApplicationLoadsWithDatabase_DashboardRendersSuccessfully()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act - Request the Dashboard page
            var response = await client.GetAsync("/dashboard");
            var content = await response.Content.ReadAsStringAsync();
            
            // Debug output
            _output.WriteLine($"Response Status: {response.StatusCode}");
            _output.WriteLine($"Content Length: {content.Length} characters");
            
            // Assert
            
            // 1. Verify successful response
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // 2. Verify the page contains expected dashboard elements
            Assert.Contains("<div class=\"dashboard-container\">", content);
            Assert.Contains("<h1 class=\"dashboard-title\">BusBuddy Dashboard</h1>", content);
            
            // 3. Verify the metrics sections are present
            Assert.Contains("<div class=\"metrics\">", content);
            Assert.Contains("<div class=\"metric-card\">", content);
            
            // 4. Verify chart container element is present
            Assert.Contains("routeStatusChart", content);
            
            // 5. Verify alerts section is present
            Assert.Contains("Current Alerts", content);
        }

        [Fact]
        public async Task DashboardService_DataIsLoadedCorrectly_MetricsArePopulated()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var dashboardService = scope.ServiceProvider.GetRequiredService<DashboardService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BusBuddyContext>();
            
            // Make sure we have at least one route in the database
            await EnsureDatabaseHasTestData(dbContext);

            // Act
            var metrics = await dashboardService.GetDashboardMetricsAsync();
            var alerts = await dashboardService.GetActiveAlertsAsync();
            var routes = await dashboardService.GetRoutesAsync();

            // Debug output
            _output.WriteLine($"Total Routes: {metrics.TotalRoutes}");
            _output.WriteLine($"Total Vehicles: {metrics.TotalVehicles}");
            _output.WriteLine($"Total Drivers: {metrics.TotalDrivers}");
            _output.WriteLine($"Active Alerts: {alerts?.Count ?? 0}");
            
            // Assert
            Assert.NotNull(metrics);
            Assert.True(metrics.TotalRoutes > 0, "Total routes should be greater than zero");
            Assert.NotNull(routes);
        }

        [Fact]
        public void DatabaseInitialization_ConfiguredCorrectly_CanConnectToDatabase()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BusBuddyContext>();
            
            // Act
            bool canConnect = dbContext.Database.CanConnect();
            
            // Debug output
            _output.WriteLine($"Can connect to database: {canConnect}");
            
            // Assert
            Assert.True(canConnect, "Should be able to connect to the database");
        }

        private async Task EnsureDatabaseHasTestData(BusBuddyContext dbContext)
        {
            // Check if we need to add data
            if (!dbContext.Routes.Any())
            {
                _output.WriteLine("Adding test route data to database");
                
                // Add a test route
                var route = new Route
                {
                    RouteName = "Test Route",
                    StartLocation = "Test Start",
                    EndLocation = "Test End",
                    Distance = 10.5m,
                    CreatedDate = DateTime.Now,
                    LastModified = DateTime.Now,
                    Description = "Test route for integration tests"
                };
                
                dbContext.Routes.Add(route);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                _output.WriteLine("Database already contains route data");
            }
            
            // Check if we need to add alert data
            if (!dbContext.Alerts.Any())
            {
                _output.WriteLine("Adding test alert data to database");
                
                // Add a test alert
                var alert = new Alert
                {
                    Message = "Test Alert",
                    Severity = "Info",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    RouteId = dbContext.Routes.First().Id
                };
                
                dbContext.Alerts.Add(alert);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    /// <summary>
    /// Custom WebApplicationFactory to configure services for testing
    /// </summary>
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Find the DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BusBuddyContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                services.AddDbContext<BusBuddyContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
                
                // Build the service provider
                var sp = services.BuildServiceProvider();
                
                // Initialize the database
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BusBuddyContext>();
                
                // Ensure the database is created
                db.Database.EnsureCreated();
            });
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<WebStartup>();
                });
        }
    }
}
