using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BusBuddy.Controllers;
using BusBuddy.Data;
using BusBuddy.DTOs;
using BusBuddy.Entities;
using BusBuddy.Hubs;
using BusBuddy.Services.Dashboard;
using BusBuddy.Tests.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Unit tests for DashboardService class
    /// </summary>
    public class DashboardServiceTests
    {
        private readonly Mock<ILogger<DashboardService>> _loggerMock;
        private readonly Mock<BusBuddyContext> _dbContextMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private object _cachedValue;

        public DashboardServiceTests()
        {
            _loggerMock = new Mock<ILogger<DashboardService>>();
            _dbContextMock = new Mock<BusBuddyContext>();

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
            // Set up mock DbSets for entities that will be counted
            var mockRoutes = MockDbSet.CreateMockDbSet(new List<Route>
            {
                new Route { Id = 1, RouteName = "Route 1", Distance = 10.5m },
                new Route { Id = 2, RouteName = "Route 2", Distance = 5.2m }
            });
            
            var mockDrivers = MockDbSet.CreateMockDbSet(new List<Driver>
            {
                new Driver { Id = 1, FullName = "John Doe" },
                new Driver { Id = 2, FullName = "Jane Smith" },
                new Driver { Id = 3, FullName = "Bob Johnson" }
            });
            
            var mockVehicles = MockDbSet.CreateMockDbSet(new List<Vehicle>
            {
                new Vehicle { Id = 1, VehicleName = "Bus 101" },
                new Vehicle { Id = 2, VehicleName = "Bus 102" }
            });
            
            var mockAlerts = MockDbSet.CreateMockDbSet(new List<Alert>
            {
                new Alert { AlertId = 1, IsActive = true },
                new Alert { AlertId = 2, IsActive = true },
                new Alert { AlertId = 3, IsActive = false }
            });
            
            var mockTrips = MockDbSet.CreateMockDbSet(new List<Trip>
            {
                new Trip { TripId = 1, DelayMinutes = 3 },
                new Trip { TripId = 2, DelayMinutes = 12 },
                new Trip { TripId = 3, DelayMinutes = 0 },
                new Trip { TripId = 4, DelayMinutes = 2 }
            });

            var today = DateTime.Today;
            var mockActivityTrips = MockDbSet.CreateMockDbSet(new List<ActivityTrip>
            {
                new ActivityTrip { Id = 1, DepartureTime = today.AddHours(2), 
                    Route = new Route { RouteName = "Morning Route" },
                    Driver = new Driver { FullName = "John Doe" },
                    Vehicle = new Vehicle { VehicleName = "Bus 101" },
                    ActivityName = "Morning Trip"
                },
                new ActivityTrip { Id = 2, DepartureTime = today.AddHours(5),
                    Route = new Route { RouteName = "Afternoon Route" },
                    Driver = new Driver { FullName = "Jane Smith" },
                    Vehicle = new Vehicle { VehicleName = "Bus 102" },
                    ActivityName = "Afternoon Trip"
                },
                new ActivityTrip { Id = 3, DepartureTime = today.AddDays(-1).AddHours(2), 
                    ActivityName = "Yesterday Trip" }
            });

            _dbContextMock.Setup(db => db.Routes).Returns(mockRoutes.Object);
            _dbContextMock.Setup(db => db.Drivers).Returns(mockDrivers.Object);
            _dbContextMock.Setup(db => db.Vehicles).Returns(mockVehicles.Object);
            _dbContextMock.Setup(db => db.Alerts).Returns(mockAlerts.Object);
            _dbContextMock.Setup(db => db.Trips).Returns(mockTrips.Object);
            _dbContextMock.Setup(db => db.ActivityTrips).Returns(mockActivityTrips.Object);

            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetDashboardMetricsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRoutes);
            Assert.Equal(3, result.TotalDrivers);
            Assert.Equal(2, result.TotalVehicles);
            Assert.Equal(2, result.ActiveAlertCount);
            Assert.Equal(15.7m, result.TotalMileage);
            Assert.Equal(2, result.TripsToday);
            Assert.Equal(75.0m, result.OnTimePerformance); // 3 out of 4 trips are on time (delay <= 5 min)
        }

        [Fact]
        public async Task GetDashboardMetricsAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Routes).Throws(new Exception("Database error"));
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetDashboardMetricsAsync());
            
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
        public async Task GetAllRoutesAsync_ShouldReturnRoutes()
        {
            // Arrange
            var mockRoutes = new List<Route>
            {
                new Route 
                { 
                    Id = 1, 
                    RouteName = "Downtown Express", 
                    StartLocation = "Main Station", 
                    EndLocation = "Downtown",
                    Distance = 5.5m,
                    LastModified = DateTime.Now.AddDays(-1)
                },
                new Route 
                { 
                    Id = 2, 
                    RouteName = "Airport Shuttle", 
                    StartLocation = "Main Station", 
                    EndLocation = "Airport",
                    Distance = 10.2m,
                    LastModified = DateTime.Now.AddDays(-2)
                }
            };
            
            var mockRoutesDbSet = MockDbSet.CreateMockDbSet(mockRoutes);
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
        public async Task GetAllRoutesAsync_ShouldHandleExceptions()
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

        [Fact]
        public async Task GetActiveTripsAsync_ShouldReturnTrips()
        {
            // Arrange
            var mockRoutes = new List<Route>
            {
                new Route { Id = 1, RouteName = "Route 1" },
                new Route { Id = 2, RouteName = "Route 2" }
            };

            var mockTrips = new List<Trip>
            {
                new Trip 
                { 
                    TripId = 1, 
                    RouteId = 1, 
                    Route = mockRoutes[0],
                    PassengerCount = 25, 
                    DelayMinutes = 0, 
                    Status = "OnTime", 
                    IsActive = true,
                    LastUpdated = DateTime.Now.AddMinutes(-5)
                },
                new Trip 
                { 
                    TripId = 2, 
                    RouteId = 2, 
                    Route = mockRoutes[1],
                    PassengerCount = 15, 
                    DelayMinutes = 10, 
                    Status = "Delayed", 
                    IsActive = true,
                    LastUpdated = DateTime.Now.AddMinutes(-10)
                },
                new Trip 
                { 
                    TripId = 3, 
                    RouteId = 1, 
                    Route = mockRoutes[0],
                    PassengerCount = 0, 
                    DelayMinutes = 0, 
                    Status = "Cancelled", 
                    IsActive = false,
                    LastUpdated = DateTime.Now.AddMinutes(-15)
                }
            };
            
            var mockTripsDbSet = MockDbSet.CreateMockDbSet(mockTrips);
            _dbContextMock.Setup(db => db.Trips).Returns(mockTripsDbSet.Object);
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetActiveTripsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.TripId == 1 && t.Status == "OnTime");
            Assert.Contains(result, t => t.TripId == 2 && t.Status == "Delayed");
            Assert.DoesNotContain(result, t => t.TripId == 3); // Inactive trip should not be returned
        }
        
        [Fact]
        public async Task GetActiveTripsAsync_ShouldReturnFromCache()
        {
            // Arrange
            var cachedTrips = new List<TripDto> 
            {
                new TripDto { TripId = 1, RouteName = "Cached Route", Status = "OnTime" }
            };
            
            object cachedValue = cachedTrips;
            _cacheMock
                .Setup(m => m.TryGetValue("ActiveTrips", out cachedValue))
                .Returns(true);
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetActiveTripsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Cached Route", result[0].RouteName);
            
            // Verify we didn't hit the database
            _dbContextMock.Verify(db => db.Trips, Times.Never);
        }

        [Fact]
        public async Task GetActiveTripsAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Trips).Throws(new Exception("Database error"));
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetActiveTripsAsync();
            
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
        public async Task GetActiveAlertsAsync_ShouldReturnAlerts()
        {
            // Arrange
            var mockRoutes = new List<Route>
            {
                new Route { Id = 1, RouteName = "Route 1", StartLocation = "Start1", EndLocation = "End1", Distance = 10 },
                new Route { Id = 2, RouteName = "Route 2", StartLocation = "Start2", EndLocation = "End2", Distance = 15 }
            };

            var mockAlerts = new List<Alert>
            {
                new Alert 
                { 
                    AlertId = 1, 
                    RouteId = 1, 
                    Route = mockRoutes[0],
                    Message = "Traffic congestion", 
                    Severity = "Warning", 
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddHours(-2)
                },
                new Alert 
                { 
                    AlertId = 2, 
                    RouteId = 2, 
                    Route = mockRoutes[1],
                    Message = "Road closure", 
                    Severity = "Critical", 
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddHours(-1)
                },
                new Alert 
                { 
                    AlertId = 3, 
                    RouteId = 1, 
                    Route = mockRoutes[0],
                    Message = "Resolved issue", 
                    Severity = "Info", 
                    IsActive = false,
                    CreatedAt = DateTime.Now.AddHours(-3)
                }
            };
            
            var mockAlertsDbSet = MockDbSet.CreateMockDbSet(mockAlerts);
            _dbContextMock.Setup(db => db.Alerts).Returns(mockAlertsDbSet.Object);
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetActiveAlertsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, a => a.AlertId == 1 && a.Message == "Traffic congestion");
            Assert.Contains(result, a => a.AlertId == 2 && a.Message == "Road closure");
            Assert.DoesNotContain(result, a => a.AlertId == 3); // Inactive alert should not be returned
        }
        
        [Fact]
        public async Task GetActiveAlertsAsync_ShouldReturnFromCache()
        {
            // Arrange
            var cachedAlerts = new List<AlertDto> 
            {
                new AlertDto { AlertId = 1, Message = "Cached Alert", Severity = "Critical" }
            };
            
            object cachedValue = cachedAlerts;
            _cacheMock
                .Setup(m => m.TryGetValue("ActiveAlerts", out cachedValue))
                .Returns(true);
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var result = await service.GetActiveAlertsAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Cached Alert", result[0].Message);
            
            // Verify we didn't hit the database
            _dbContextMock.Verify(db => db.Alerts, Times.Never);
        }

        [Fact]
        public async Task GetActiveAlertsAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Alerts).Throws(new Exception("Database error"));
            
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
        public async Task GetDashboardOverviewAsync_ShouldReturnOverviewData()
        {
            // Arrange
            // Setup metrics and trips for the overview
            var mockTrips = new List<Trip>
            {
                new Trip { TripId = 1, Status = "OnTime", IsActive = true },
                new Trip { TripId = 2, Status = "OnTime", IsActive = true },
                new Trip { TripId = 3, Status = "Delayed", IsActive = true },
                new Trip { TripId = 4, Status = "Cancelled", IsActive = true }
            };
            
            var mockTripsDbSet = MockDbSet.CreateMockDbSet(mockTrips);
            _dbContextMock.Setup(db => db.Trips).Returns(mockTripsDbSet.Object);
            
            // We'll set up minimal necessary mocks for the metrics
            var mockRoutes = MockDbSet.CreateMockDbSet(new List<Route> { new Route(), new Route() });
            var mockDrivers = MockDbSet.CreateMockDbSet(new List<Driver> { new Driver(), new Driver() });
            var mockVehicles = MockDbSet.CreateMockDbSet(new List<Vehicle> { new Vehicle() });
            var mockAlerts = MockDbSet.CreateMockDbSet(new List<Alert> { new Alert { IsActive = true } });
            var mockActivityTrips = MockDbSet.CreateMockDbSet(new List<ActivityTrip>());
            
            _dbContextMock.Setup(db => db.Routes).Returns(mockRoutes.Object);
            _dbContextMock.Setup(db => db.Drivers).Returns(mockDrivers.Object);
            _dbContextMock.Setup(db => db.Vehicles).Returns(mockVehicles.Object);
            _dbContextMock.Setup(db => db.Alerts).Returns(mockAlerts.Object);
            _dbContextMock.Setup(db => db.ActivityTrips).Returns(mockActivityTrips.Object);
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var (metrics, routeStatusCounts) = await service.GetDashboardOverviewAsync();
            
            // Assert
            Assert.NotNull(metrics);
            Assert.NotNull(routeStatusCounts);
            Assert.Equal(3, routeStatusCounts.Count); // OnTime, Delayed, Cancelled
            Assert.Equal(2, routeStatusCounts["OnTime"]);
            Assert.Equal(1, routeStatusCounts["Delayed"]);
            Assert.Equal(1, routeStatusCounts["Cancelled"]);
        }

        [Fact]
        public async Task GetDashboardOverviewAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dbContextMock.Setup(db => db.Trips).Throws(new Exception("Database error"));
            
            var service = new DashboardService(_dbContextMock.Object, _loggerMock.Object, _cacheMock.Object);
            
            // Act
            var (metrics, routeStatusCounts) = await service.GetDashboardOverviewAsync();
            
            // Assert
            Assert.NotNull(metrics);
            Assert.NotNull(routeStatusCounts);
            Assert.Empty(routeStatusCounts);
            
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

    /// <summary>
    /// Unit tests for DashboardController class
    /// </summary>
    public class DashboardControllerTests
    {
        private readonly Mock<ILogger<DashboardController>> _loggerMock;
        private readonly Mock<DashboardService> _serviceMock;
        private readonly Mock<IHubContext<DashboardHub>> _hubContextMock;

        public DashboardControllerTests()
        {
            _loggerMock = new Mock<ILogger<DashboardController>>();
            _serviceMock = new Mock<DashboardService>(
                Mock.Of<BusBuddyContext>(), 
                Mock.Of<ILogger<DashboardService>>(), 
                Mock.Of<IMemoryCache>());
            _hubContextMock = new Mock<IHubContext<DashboardHub>>();
        }

        [Fact]
        public async Task GetRoutes_ShouldReturnOkResult()
        {
            // Arrange
            var testRoutes = new List<RouteDto> 
            {
                new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
            };
            
            _serviceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(testRoutes);
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetRoutes();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var routes = Assert.IsAssignableFrom<List<RouteDto>>(okResult.Value);
            Assert.Equal(2, routes.Count);
            Assert.Equal("Test Route 1", routes[0].RouteName);
        }

        [Fact]
        public async Task GetRoutes_ShouldHandleExceptions()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetRoutes();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
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
        public async Task GetMetrics_ShouldReturnOkResult()
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
            
            _serviceMock
                .Setup(s => s.GetDashboardMetricsAsync())
                .ReturnsAsync(testMetrics);
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
            
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
        public async Task GetMetrics_ShouldHandleExceptions()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetDashboardMetricsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetMetrics();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
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
        public async Task GetAlerts_ShouldReturnOkResult()
        {
            // Arrange
            var testAlerts = new List<AlertDto>
            {
                new AlertDto { AlertId = 1, Message = "Test Alert 1", Severity = "Warning" },
                new AlertDto { AlertId = 2, Message = "Test Alert 2", Severity = "Critical" }
            };
            
            _serviceMock
                .Setup(s => s.GetActiveAlertsAsync())
                .ReturnsAsync(testAlerts);
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
            
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
        public async Task GetAlerts_ShouldHandleExceptions()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetActiveAlertsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetAlerts();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
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
        public async Task GetOverview_ShouldReturnOkResult()
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
            
            _serviceMock
                .Setup(s => s.GetDashboardMetricsAsync())
                .ReturnsAsync(testMetrics);
            _serviceMock
                .Setup(s => s.GetActiveTripsAsync())
                .ReturnsAsync(testTrips);
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
            
            // Act
            var actionResult = await controller.GetOverview();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            dynamic result = okResult.Value;
            
            // Verify metrics and route status counts are present
            Assert.NotNull(result.metrics);
            Assert.NotNull(result.routeStatusCounts);
        }

        [Fact]
        public async Task GetOverview_ShouldHandleExceptions()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetDashboardMetricsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetOverview();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
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
        public async Task GetBusRoutes_ShouldReturnOkResult()
        {
            // Arrange
            var testRoutes = new List<RouteDto> 
            {
                new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
            };
            
            _serviceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(testRoutes);
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutes();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var routes = Assert.IsAssignableFrom<List<RouteDto>>(okResult.Value);
            Assert.Equal(2, routes.Count);
            Assert.Equal("Test Route 1", routes[0].RouteName);
        }

        [Fact]
        public async Task GetBusRoutes_ShouldHandleExceptions()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutes();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            
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
        public async Task GetBusRoutesLegacy_ShouldReturnOkResult()
        {
            // Arrange
            var testRoutes = new List<RouteDto> 
            {
                new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
            };
            
            _serviceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(testRoutes);
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutesLegacy();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var routes = Assert.IsAssignableFrom<List<RouteDto>>(okResult.Value);
            Assert.Equal(2, routes.Count);
            Assert.Equal("Test Route 1", routes[0].RouteName);
        }

        [Fact]
        public async Task GetBusRoutesLegacy_ShouldHandleExceptions()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var controller = new DashboardController(
                _serviceMock.Object, 
                _hubContextMock.Object, 
                _loggerMock.Object);
                
            // Act
            var actionResult = await controller.GetBusRoutesLegacy();
            
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }

    /// <summary>
    /// Unit tests for DashboardUpdatesService class
    /// </summary>
    public class DashboardUpdatesServiceTests
    {
        private readonly Mock<IHubContext<DashboardHub>> _hubContextMock;
        private readonly Mock<DashboardService> _dashboardServiceMock;
        private readonly Mock<ILogger<DashboardUpdatesService>> _loggerMock;
        
        public DashboardUpdatesServiceTests()
        {
            _hubContextMock = new Mock<IHubContext<DashboardHub>>();
            _dashboardServiceMock = new Mock<DashboardService>(
                Mock.Of<BusBuddyContext>(), 
                Mock.Of<ILogger<DashboardService>>(), 
                Mock.Of<IMemoryCache>());
            _loggerMock = new Mock<ILogger<DashboardUpdatesService>>();
        }

        [Fact]
        public async Task BroadcastMetricsUpdateAsync_ShouldSendMetricsToClients()
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
            
            _dashboardServiceMock
                .Setup(s => s.GetDashboardMetricsAsync())
                .ReturnsAsync(testMetrics);
            
            var clientsProxy = new Mock<IClientProxy>();
            _hubContextMock
                .Setup(h => h.Clients.All)
                .Returns(clientsProxy.Object);
            
            var service = new DashboardUpdatesService(
                _hubContextMock.Object,
                _dashboardServiceMock.Object,
                _loggerMock.Object);

            // Act
            await service.BroadcastMetricsUpdateAsync();
            
            // Assert
            _dashboardServiceMock.Verify(s => s.GetDashboardMetricsAsync(), Times.Once);
            clientsProxy.Verify(
                c => c.SendCoreAsync(
                    "ReceiveMetricsUpdate",
                    It.Is<object[]>(o => o.Length == 1 && o[0] == testMetrics),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task BroadcastMetricsUpdateAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dashboardServiceMock
                .Setup(s => s.GetDashboardMetricsAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var service = new DashboardUpdatesService(
                _hubContextMock.Object,
                _dashboardServiceMock.Object,
                _loggerMock.Object);

            // Act & Assert (should not throw)
            await service.BroadcastMetricsUpdateAsync();
            
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
        public async Task BroadcastRoutesUpdateAsync_ShouldSendRoutesToClients()
        {
            // Arrange
            var testRoutes = new List<RouteDto>
            {
                new RouteDto { RouteID = 1, RouteName = "Test Route 1" },
                new RouteDto { RouteID = 2, RouteName = "Test Route 2" }
            };
            
            _dashboardServiceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ReturnsAsync(testRoutes);
            
            var clientsProxy = new Mock<IClientProxy>();
            _hubContextMock
                .Setup(h => h.Clients.All)
                .Returns(clientsProxy.Object);
            
            var service = new DashboardUpdatesService(
                _hubContextMock.Object,
                _dashboardServiceMock.Object,
                _loggerMock.Object);

            // Act
            await service.BroadcastRoutesUpdateAsync();
            
            // Assert
            _dashboardServiceMock.Verify(s => s.GetAllRoutesAsync(), Times.Once);
            clientsProxy.Verify(
                c => c.SendCoreAsync(
                    "ReceiveRoutesUpdate",
                    It.Is<object[]>(o => o.Length == 1 && o[0] == testRoutes),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task BroadcastRoutesUpdateAsync_ShouldHandleExceptions()
        {
            // Arrange
            _dashboardServiceMock
                .Setup(s => s.GetAllRoutesAsync())
                .ThrowsAsync(new Exception("Test exception"));
            
            var service = new DashboardUpdatesService(
                _hubContextMock.Object,
                _dashboardServiceMock.Object,
                _loggerMock.Object);

            // Act & Assert (should not throw)
            await service.BroadcastRoutesUpdateAsync();
            
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
        public async Task SendActivityNotificationAsync_ShouldSendNotificationToClients()
        {
            // Arrange
            var activity = new ActivitySummaryDto
            {
                ActivityId = 1,
                ActivityType = "Trip",
                Description = "New trip started",
                Timestamp = DateTime.Now
            };
            
            var clientsProxy = new Mock<IClientProxy>();
            _hubContextMock
                .Setup(h => h.Clients.All)
                .Returns(clientsProxy.Object);
            
            var service = new DashboardUpdatesService(
                _hubContextMock.Object,
                _dashboardServiceMock.Object,
                _loggerMock.Object);

            // Act
            await service.SendActivityNotificationAsync(activity);
            
            // Assert
            clientsProxy.Verify(
                c => c.SendCoreAsync(
                    "ReceiveActivityNotification",
                    It.Is<object[]>(o => o.Length == 1 && o[0] == activity),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task SendActivityNotificationAsync_ShouldHandleExceptions()
        {
            // Arrange
            var activity = new ActivitySummaryDto
            {
                ActivityId = 1,
                ActivityType = "Trip",
                Description = "New trip started",
                Timestamp = DateTime.Now
            };
            
            var clientsProxy = new Mock<IClientProxy>();
            _hubContextMock
                .Setup(h => h.Clients.All)
                .Returns(clientsProxy.Object);
            
            clientsProxy
                .Setup(c => c.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    default))
                .ThrowsAsync(new Exception("Connection error"));
            
            var service = new DashboardUpdatesService(
                _hubContextMock.Object,
                _dashboardServiceMock.Object,
                _loggerMock.Object);

            // Act & Assert (should not throw)
            await service.SendActivityNotificationAsync(activity);
            
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
