using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Controllers;
using BusBuddy.DTOs;
using BusBuddy.Services.Dashboard;
using Microsoft.AspNetCore.SignalR;
using BusBuddy.Hubs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit.Abstractions;
using BusBuddy.Data;
using Microsoft.EntityFrameworkCore;
using BusBuddy.Tests.Utilities;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests for the Dashboard functionality including service and controller components
    /// </summary>
    public class DashboardTests
    {
        private readonly ITestOutputHelper _output;
        private readonly Mock<ILogger<DashboardService>> _loggerMock;
        private readonly Mock<ILogger<DashboardController>> _controllerLoggerMock;
        private readonly Mock<BusBuddyContext> _dbContextMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IHubContext<DashboardHub>> _hubContextMock;
        private object _cachedValue;

        public DashboardTests(ITestOutputHelper output)
        {
            _output = output;
            _loggerMock = new Mock<ILogger<DashboardService>>();
            _controllerLoggerMock = new Mock<ILogger<DashboardController>>();
            _dbContextMock = new Mock<BusBuddyContext>();
            _hubContextMock = new Mock<IHubContext<DashboardHub>>();

            // Setup memory cache mock
            _cacheMock = new Mock<IMemoryCache>();
            _cacheMock
                .Setup(m => m.TryGetValue(It.IsAny<object>(), out _cachedValue))
                .Returns(false);
            _cacheMock
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);
        }

        [Fact]
        public async Task GetDashboardMetricsAsync_ShouldReturnMetricsDTO()
        {
            // Arrange
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetDashboardMetricsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<DashboardDto>(result);
        }

        [Fact]
        public async Task Controller_GetRoutes_ShouldReturnOkResult()
        {
            // Arrange
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(new List<RouteDto> 
                {
                    new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                    new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
                });
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetRoutes();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var routes = Assert.IsAssignableFrom<List<RouteDto>>(okResult.Value);
            Assert.Equal(2, routes.Count);
            Assert.Equal("Test Route 1", routes[0].RouteName);
        }

        [Fact]
        public async Task Controller_GetMetrics_ShouldReturnOkResult()
        {
            // Arrange
            var testMetrics = new DashboardDto
            {
                TotalRoutes = 5,
                TotalDrivers = 10,
                TotalVehicles = 8,
                ActiveAlertCount = 2,
                OnTimePerformance = 95.5m
            };
            
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetDashboardMetricsAsync())
                .ReturnsAsync(testMetrics);
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
            
            // Act
            var actionResult = await controller.GetMetrics();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var metrics = Assert.IsType<DashboardDto>(okResult.Value);
            Assert.Equal(5, metrics.TotalRoutes);
            Assert.Equal(10, metrics.TotalDrivers);
            Assert.Equal(8, metrics.TotalVehicles);
            Assert.Equal(2, metrics.ActiveAlertCount);
            Assert.Equal(95.5m, metrics.OnTimePerformance);
        }

        [Fact]
        public async Task Controller_GetAlerts_ShouldReturnOkResult()
        {
            // Arrange
            var testAlerts = new List<AlertDto>
            {
                new AlertDto { AlertId = 1, Message = "Test Alert 1", Severity = "Warning" },
                new AlertDto { AlertId = 2, Message = "Test Alert 2", Severity = "Critical" }
            };
            
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetActiveAlertsAsync())
                .ReturnsAsync(testAlerts);
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
            
            // Act
            var actionResult = await controller.GetAlerts();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var alerts = Assert.IsAssignableFrom<List<AlertDto>>(okResult.Value);
            Assert.Equal(2, alerts.Count);
            Assert.Equal("Test Alert 1", alerts[0].Message);
            Assert.Equal("Warning", alerts[0].Severity);
            Assert.Equal("Critical", alerts[1].Severity);
        }

        [Fact]
        public async Task Controller_GetOverview_ShouldReturnOkResult()
        {
            // Arrange
            var testMetrics = new DashboardDto
            {
                TotalRoutes = 5,
                TotalDrivers = 10,
                OnTimePerformance = 95.5m
            };
            
            var testTrips = new List<TripDto>
            {
                new TripDto { TripId = 1, Status = "OnTime" },
                new TripDto { TripId = 2, Status = "OnTime" },
                new TripDto { TripId = 3, Status = "Delayed" }
            };
            
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetDashboardMetricsAsync())
                .ReturnsAsync(testMetrics);
            mockDashboardService
                .Setup(s => s.GetActiveTripsAsync())
                .ReturnsAsync(testTrips);
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
            
            // Act
            var actionResult = await controller.GetOverview();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            dynamic result = okResult.Value;
            
            // Verify metrics data exists
            Assert.NotNull(result.metrics);
            Assert.NotNull(result.routeStatusCounts);
        }

        [Fact]
        public async Task Service_GetActiveAlertsAsync_ShouldHandleExceptions()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<BusBuddy.Entities.Alert>>();
            _dbContextMock
                .Setup(db => db.Alerts)
                .Throws(new Exception("Database error"));
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetActiveAlertsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            // Verify error was logged
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Controller_GetRoutes_ShouldHandleExceptions()
        {
            // Arrange
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetAllRoutesAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetRoutes();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            // Verify error was logged
            _controllerLoggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception>(e => e.Message == "Test exception"),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Controller_GetMetrics_ShouldHandleExceptions()
        {
            // Arrange
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetDashboardMetricsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetMetrics();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            // Verify error was logged
            _controllerLoggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception>(e => e.Message == "Test exception"),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Controller_GetAlerts_ShouldHandleExceptions()
        {
            // Arrange
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetActiveAlertsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetAlerts();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            // Verify error was logged
            _controllerLoggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception>(e => e.Message == "Test exception"),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Controller_GetOverview_ShouldHandleExceptions()
        {
            // Arrange
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetDashboardMetricsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetOverview();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            // Verify error was logged
            _controllerLoggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Controller_GetBusRoutes_ShouldReturnOkResult()
        {
            // Arrange
            var testRoutes = new List<RouteDto> 
            {
                new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
            };
            
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(testRoutes);
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutes();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var routes = Assert.IsAssignableFrom<List<RouteDto>>(okResult.Value);
            Assert.Equal(2, routes.Count);
            Assert.Equal("Test Route 1", routes[0].RouteName);
        }

        [Fact]
        public async Task Controller_GetBusRoutes_ShouldHandleExceptions()
        {
            // Arrange
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetAllRoutesAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutes();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
            // Verify error was logged
            _controllerLoggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception>(e => e.Message == "Test exception"),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task Controller_GetBusRoutesLegacy_ShouldReturnOkResult()
        {
            // Arrange
            var testRoutes = new List<RouteDto> 
            {
                new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
            };
            
            var mockDashboardService = new Mock<DashboardService>(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            mockDashboardService
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(testRoutes);
            
            var controller = new DashboardController(
                mockDashboardService.Object, 
                _hubContextMock.Object, 
                _controllerLoggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutesLegacy();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var routes = Assert.IsAssignableFrom<List<RouteDto>>(okResult.Value);
            Assert.Equal(2, routes.Count);
            Assert.Equal("Test Route 1", routes[0].RouteName);
        }

        [Fact]
        public async Task Service_GetAllRoutesAsync_ShouldReturnRoutes()
        {
            // Arrange
            var mockRouteList = new List<BusBuddy.Entities.Route>
            {
                new BusBuddy.Entities.Route 
                { 
                    Id = 1, 
                    RouteName = "Downtown Express", 
                    StartLocation = "Main Station", 
                    EndLocation = "Downtown",
                    Distance = 5.5m,
                    LastModified = DateTime.Now.AddDays(-1)
                },
                new BusBuddy.Entities.Route 
                { 
                    Id = 2, 
                    RouteName = "Airport Shuttle", 
                    StartLocation = "Main Station", 
                    EndLocation = "Airport",
                    Distance = 10.2m,
                    LastModified = DateTime.Now.AddDays(-2)
                }
            };
            
            var mockRoutesDbSet = MockDbSet.CreateMockDbSet(mockRouteList);
            _dbContextMock.Setup(db => db.Routes).Returns(mockRoutesDbSet.Object);
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetAllRoutesAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Downtown Express", result[0].RouteName);
            Assert.Equal("Airport Shuttle", result[1].RouteName);
            Assert.Equal(5.5m, result[0].Distance);
            Assert.Equal(10.2m, result[1].Distance);
        }

        [Fact]
        public async Task Service_GetAllRoutesAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Routes).Throws(new Exception("Database error"));
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetAllRoutesAsync());
            
            // Verify error was logged
            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
