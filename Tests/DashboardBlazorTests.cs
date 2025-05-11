using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using BusBuddy.Services.Dashboard;
using BusBuddy.DTOs;
using Microsoft.JSInterop;
using FluentAssertions;
using AngleSharp.Dom;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests for the Dashboard.razor Blazor component
    /// </summary>
    public class DashboardBlazorTests : TestContext
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<IDashboardService> _mockDashboardService;
        private readonly Mock<IJSRuntime> _mockJsRuntime;

        public DashboardBlazorTests(ITestOutputHelper output)
        {
            _output = output;
            _mockDashboardService = new Mock<IDashboardService>();
            _mockJsRuntime = new Mock<IJSRuntime>();

            // Register services
            Services.AddSingleton(_mockDashboardService.Object);
            Services.AddSingleton(_mockJsRuntime.Object);
        }

        [Fact]
        public void Dashboard_WhenInitialized_ShouldRenderTitleCorrectly()
        {
            // Arrange
            SetupMockDashboardService();
            
            // Act
            var cut = RenderComponent<Dashboard>();
            
            // Log component markup for debugging
            _output.WriteLine("Component Markup:");
            _output.WriteLine(cut.Markup);
            
            // Assert
            cut.Find("h1").MarkupMatches("<h1>BusBuddy Dashboard</h1>");
        }

        [Fact]
        public void Dashboard_WhenInitialized_ShouldRenderMetricsSection()
        {
            // Arrange
            SetupMockDashboardService();
            
            // Act
            var cut = RenderComponent<Dashboard>();
              // Assert
            cut.FindAll(".metric-card").Count.Should().BeGreaterThan(0);
            cut.Find(".metrics").Should().NotBeNull();
        }

        [Fact]
        public void Dashboard_WithAlerts_ShouldDisplayAlertsSection()
        {
            // Arrange
            SetupMockDashboardService(alertCount: 2);
            
            // Act
            var cut = RenderComponent<Dashboard>();
            
            // Assert
            cut.FindAll("table").Should().NotBeEmpty();
            cut.Find("h2").TextContent.Should().Contain("Current Alerts");
        }

        [Fact]
        public void Dashboard_WithNoAlerts_ShouldDisplayNoAlertsMessage()
        {
            // Arrange
            SetupMockDashboardService(alertCount: 0);
            
            // Act
            var cut = RenderComponent<Dashboard>();
            
            // Assert
            cut.Find(".col-md-6:nth-child(2)").TextContent.Should().Contain("No active alerts");
        }

        [Fact]
        public void Dashboard_WhenRefreshButtonClicked_ShouldCallRefreshDashboard()
        {
            // Arrange
            SetupMockDashboardService();
            var cut = RenderComponent<Dashboard>();
            
            // Act
            cut.Find("button.btn-primary").Click();
            
            // Assert
            // Verify service calls were made again
            _mockDashboardService.Verify(s => s.GetDashboardMetricsAsync(), Times.AtLeast(2));
            _mockDashboardService.Verify(s => s.GetActiveAlertsAsync(), Times.AtLeast(2));
        }

        private void SetupMockDashboardService(int alertCount = 1)
        {
            // Mock dashboard metrics response
            var mockMetrics = new DashboardDto
            {
                TotalRoutes = 5,
                TotalDrivers = 3,
                TotalVehicles = 7,
                TotalMileage = 543.2m,
                TripsToday = 12,
                RecentActivity = new List<ActivityLogDto>
                {
                    new ActivityLogDto
                    {
                        ActivityId = 1,
                        Description = "Test activity",
                        ActivityType = "Route",
                        Timestamp = DateTime.Now.AddHours(-1)
                    }
                }
            };

            // Mock alerts
            var mockAlerts = Enumerable.Range(1, alertCount).Select(i => new AlertDto
            {
                AlertId = i,
                Message = $"Test Alert {i}",
                RouteId = 1,
                Severity = i % 2 == 0 ? "Warning" : "Critical",
                IsActive = true,
                CreatedAt = DateTime.Now.AddMinutes(-i * 30)
            }).ToList();

            // Configure mock services
            _mockDashboardService
                .Setup(x => x.GetDashboardMetricsAsync())
                .ReturnsAsync(mockMetrics);
            
            _mockDashboardService
                .Setup(x => x.GetActiveAlertsAsync())
                .ReturnsAsync(mockAlerts);

            _mockDashboardService
                .Setup(x => x.GetActiveTripsAsync())
                .ReturnsAsync(new List<TripDto>
                {
                    new TripDto
                    {
                        TripId = 1,
                        Status = "Active",
                        StartTime = DateTime.Now.AddHours(-2),
                        EndTime = DateTime.Now.AddHours(1)
                    }
                });

            // Mock JS calls for chart rendering
            _mockJsRuntime
                .Setup(x => x.InvokeVoidAsync("renderPieChart", It.IsAny<object[]>()))
                .Returns(ValueTask.CompletedTask);
        }
    }
}
