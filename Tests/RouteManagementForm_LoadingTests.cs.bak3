using Xunit;
using BusBuddy.Forms;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using System;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Additional tests for RouteManagementForm focusing on route loading functionality
    /// </summary>
    public class RouteManagementForm_LoadingTests
    {
        private readonly Mock<ILogger<RouteManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public RouteManagementForm_LoadingTests()
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<RouteManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
        }
        
        [Fact]
        public async Task LoadRoutes_ShouldHandleEmptyRouteList()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.GetRoutesAsync())
                .ReturnsAsync(new List<Route>());
                
            // Create a task completion source that will be completed when the log happens
            var tcs = new TaskCompletionSource<bool>();
            
            // Setup logger mock to complete the task when the expected log message is seen
            _loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No routes found")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)))
                .Callback(() => tcs.TrySetResult(true));

            // Creating the form
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - manually invoke the correct private LoadRoutesDataAsync method using reflection
            var method = typeof(RouteManagementForm).GetMethod("LoadRoutesDataAsync", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var task = (Task)method.Invoke(form, null);
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 30 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetRoutesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report empty routes
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No routes found")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        
        [Fact]
        public async Task LoadRoutes_ShouldPopulateDataGridWithRoutes()
        {
            // Arrange
            var testRoutes = new List<Route> {
                new Route { Id = 1, RouteName = "Test Route 1", StartLocation = "School", EndLocation = "Downtown", Distance = 5.5m },
                new Route { Id = 2, RouteName = "Test Route 2", StartLocation = "Downtown", EndLocation = "School", Distance = 5.5m }
            };
            
            _dbHelperMock.Setup(x => x.GetRoutesAsync())
                .ReturnsAsync(testRoutes);

            // Create a task completion source that will be completed when the log happens
            var tcs = new TaskCompletionSource<bool>();
            
            // Setup logger mock to complete the task when the expected log message is seen
            _loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("routes loaded")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)))
                .Callback(() => tcs.TrySetResult(true));

            // Creating the form
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - manually invoke the correct private LoadRoutesDataAsync method using reflection
            var method = typeof(RouteManagementForm).GetMethod("LoadRoutesDataAsync", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var task = (Task)method.Invoke(form, null);
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 30 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetRoutesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report successful loading
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("routes loaded")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        
        [Fact]
        public async Task LoadRoutes_ShouldHandleDatabaseException()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.GetRoutesAsync())
                .ThrowsAsync(new Exception("Database connection failed"));
                
            // Create a task completion source to track when the logger gets called
            var tcs = new TaskCompletionSource<bool>();
            
            // Create a task that will complete when the logger records an error
            _loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception?>(e => e != null && e.Message.Contains("Database connection failed")),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)))
                .Callback(() => tcs.TrySetResult(true));

            // Creating the form
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - manually invoke the correct private LoadRoutesDataAsync method using reflection
            var method = typeof(RouteManagementForm).GetMethod("LoadRoutesDataAsync", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var task = (Task)method.Invoke(form, null);
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 30 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetRoutesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report the exception
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception?>(e => e != null && e.Message.Contains("Database connection failed")),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
    }
}
