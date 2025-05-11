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
using System.Reflection;
using System.Runtime.Versioning;
using BusBuddy.Tests.Utilities;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Additional tests for RouteManagementForm focusing on route loading functionality
    /// Using MessageBoxHandler to automatically handle MessageBox dialogs during tests
    /// </summary>
    [SupportedOSPlatform("windows6.1")]  // Indicate this class requires Windows 6.1 (Windows 7) or later
    public class RouteManagementForm_LoadingTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<RouteManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public RouteManagementForm_LoadingTests() : base(1) // Use 1 (IDOK) as the default button to click
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<RouteManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
        }
        
        // Helper method to stop the refresh timer to prevent infinite loops
        [SupportedOSPlatform("windows6.1")]
        private void StopRefreshTimer(RouteManagementForm form)
        {
            // Get the private refreshTimer field using reflection
            var timerField = typeof(RouteManagementForm).GetField("refreshTimer", 
                BindingFlags.NonPublic | BindingFlags.Instance);
                
            if (timerField != null)
            {
                var timer = timerField.GetValue(form) as System.Windows.Forms.Timer;
                timer?.Stop();
            }
        }
        
        [Fact, WindowsOnly]
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

            // Clear any previously captured message boxes
            ClearCapturedDialogs();

            // Creating the form
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Stop the refresh timer to prevent continuous calls
            StopRefreshTimer(form);
            
            // Act - manually invoke the correct private LoadRoutesDataAsync method using reflection
            var method = typeof(RouteManagementForm).GetMethod("LoadRoutesDataAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            var task = (Task)method.Invoke(form, null);
            await task; // Actually await the task now
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5)); // Reduced timeout since we're now awaiting the task
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 5 seconds");
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
        
        [Fact, WindowsOnly]
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
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("routes")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)))
                .Callback(() => tcs.TrySetResult(true));

            // Clear any previously captured message boxes
            ClearCapturedDialogs();

            // Creating the form
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Stop the refresh timer to prevent continuous calls
            StopRefreshTimer(form);
            
            // Act - manually invoke the correct private LoadRoutesDataAsync method using reflection
            var method = typeof(RouteManagementForm).GetMethod("LoadRoutesDataAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            var task = (Task)method.Invoke(form, null);
            await task; // Actually await the task
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5)); // Reduced timeout since we're now awaiting the task
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 5 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetRoutesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report successful loading
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("routes")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        
        [Fact, WindowsOnly]
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

            // Clear any previously captured message boxes
            ClearCapturedDialogs();

            // Creating the form
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Stop the refresh timer to prevent continuous calls
            StopRefreshTimer(form);
            
            // Act - manually invoke the correct private LoadRoutesDataAsync method using reflection
            var method = typeof(RouteManagementForm).GetMethod("LoadRoutesDataAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            var task = (Task)method.Invoke(form, null);
            await task; // Actually await the task
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5)); // Reduced timeout since we're now awaiting the task
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 5 seconds");
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
                
            // Also verify that a message box was shown with an error about loading routes
            Assert.Contains(CapturedDialogs, dialog => dialog.Title == "Error");
        }
    }
}

