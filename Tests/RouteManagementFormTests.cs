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
    [SupportedOSPlatform("windows6.1")]  // Indicate this class requires Windows 6.1 (Windows 7) or later
    public class RouteManagementFormTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<RouteManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public RouteManagementFormTests() : base(1) // Use 1 (IDOK) as the default button to click
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<RouteManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
            
            // Set up the mock to return sample data
            _dbHelperMock.Setup(x => x.GetRoutesAsync())
                .ReturnsAsync(new List<Route> 
                {
                    new Route { Id = 1, RouteName = "Test Route 1", StartLocation = "School", EndLocation = "Downtown", Distance = 5.5m },
                    new Route { Id = 2, RouteName = "Test Route 2", StartLocation = "Library", EndLocation = "Park", Distance = 3.2m }
                });
        }

        // Helper method to stop the refresh timer to prevent infinite loops in tests
        [SupportedOSPlatform("windows6.1")]        private void StopRefreshTimer(RouteManagementForm form)
        {
            // Get the private refreshTimer field using reflection
            var timerField = typeof(RouteManagementForm).GetField("refreshTimer",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var timer = timerField?.GetValue(form) as System.Windows.Forms.Timer;
            
            // Stop the timer
            timer?.Stop();
        }

        [Fact]
        public void RouteManagementForm_InitializesCorrectly()
        {
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            Assert.NotNull(form);
        }

        [Fact]
        public async Task SaveRoute_ValidRoute_SavesSuccessfully()
        {
            // Arrange
            var testRoute = new Route { 
                Id = 0, // New route
                RouteName = "Test Route", 
                StartLocation = "School", 
                EndLocation = "Downtown", 
                Distance = 5.5m,
                CreatedDate = DateTime.Now,
                Description = "Test route description"
            };

            _dbHelperMock.Setup(x => x.AddRouteAsync(It.IsAny<Route>()))
                .ReturnsAsync((Route r) => {
                    r.Id = 1; // Simulate database assigning ID
                    return r;
                });

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Act - use reflection to call the private SaveRouteAsync method
            var method = form.GetType().GetMethod("SaveRouteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { testRoute });
            var result = await task;
            
            // Assert
            Assert.True(result);
            _dbHelperMock.Verify(x => x.AddRouteAsync(It.IsAny<Route>()), Times.Once);
        }

        [Fact]
        public async Task SaveRoute_InvalidRoute_ReturnsFalse()
        {
            // Arrange
            var testRoute = new Route { 
                Id = 0,
                RouteName = "", // Invalid empty name
                StartLocation = "School",
                EndLocation = "Downtown",
                Distance = 5.5m
            };

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Act - use reflection to call the private SaveRouteAsync method
            var method = form.GetType().GetMethod("SaveRouteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { testRoute });
            var result = await task;
            
            // Assert
            Assert.False(result);
            _dbHelperMock.Verify(x => x.AddRouteAsync(It.IsAny<Route>()), Times.Never);
        }

        [Fact]
        public async Task SaveRoute_UpdateExisting_UpdatesSuccessfully()
        {
            // Arrange
            var testRoute = new Route { 
                Id = 1, // Existing route
                RouteName = "Updated Route",
                StartLocation = "School",
                EndLocation = "Downtown",
                Distance = 7.5m,
                LastModified = DateTime.Now
            };

            _dbHelperMock.Setup(x => x.UpdateRouteAsync(It.IsAny<Route>()))
                .ReturnsAsync(true);

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Act - use reflection to call the private SaveRouteAsync method
            var method = form.GetType().GetMethod("SaveRouteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { testRoute });
            var result = await task;
            
            // Assert
            Assert.True(result);
            _dbHelperMock.Verify(x => x.UpdateRouteAsync(It.IsAny<Route>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRoute_ConfirmedDeletion_DeletesSuccessfully()
        {
            // Arrange
            int routeId = 1;
            _dbHelperMock.Setup(x => x.DeleteRouteAsync(routeId))
                .ReturnsAsync(true);

            // Set up message box handler to simulate clicking "Yes"
            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Change the auto close button to "Yes" (6)
            var messageBoxHandlerField = typeof(MessageBoxHandlerTestBase).GetField("_messageBoxHandler", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var handler = messageBoxHandlerField.GetValue(this);
            var startMonitoringMethod = handler.GetType().GetMethod("StartMonitoring");
            startMonitoringMethod.Invoke(handler, new object[] { 6 }); // 6 = Yes button
            
            ClearCapturedDialogs();
            
            // Act - use reflection to call the private DeleteRouteAsync method
            var method = form.GetType().GetMethod("DeleteRouteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { routeId });
            var result = await task;
            
            // Assert
            Assert.True(result);
            _dbHelperMock.Verify(x => x.DeleteRouteAsync(routeId), Times.Once);
            Assert.NotEmpty(CapturedDialogs); // Verify a dialog was shown
        }

        [Fact]
        public async Task DeleteRoute_CanceledDeletion_DoesNotDelete()
        {
            // Arrange
            int routeId = 1;

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Change the auto close button to "No" (7)
            var messageBoxHandlerField = typeof(MessageBoxHandlerTestBase).GetField("_messageBoxHandler", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var handler = messageBoxHandlerField.GetValue(this);
            var startMonitoringMethod = handler.GetType().GetMethod("StartMonitoring");
            startMonitoringMethod.Invoke(handler, new object[] { 7 }); // 7 = No button
            
            ClearCapturedDialogs();
            
            // Act - use reflection to call the private DeleteRouteAsync method
            var method = form.GetType().GetMethod("DeleteRouteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { routeId });
            var result = await task;
            
            // Assert
            Assert.False(result);
            _dbHelperMock.Verify(x => x.DeleteRouteAsync(It.IsAny<int>()), Times.Never);
            Assert.NotEmpty(CapturedDialogs); // Verify a dialog was shown
        }

        [Fact]
        public void ValidateRoute_ValidInput_ReturnsTrue()
        {
            // Arrange
            var testRoute = new Route { 
                RouteName = "Test Route",
                StartLocation = "School",
                EndLocation = "Downtown",
                Distance = 5.5m
            };

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Act - use reflection to call the private ValidateRoute method
            var method = form.GetType().GetMethod("ValidateRoute", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testRoute });
            
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateRoute_EmptyRouteName_ReturnsFalse()
        {
            // Arrange
            var testRoute = new Route { 
                RouteName = "",  // Invalid empty name
                StartLocation = "School",
                EndLocation = "Downtown",
                Distance = 5.5m
            };

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Act - use reflection to call the private ValidateRoute method
            var method = form.GetType().GetMethod("ValidateRoute", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testRoute });
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateRoute_NegativeDistance_ReturnsFalse()
        {
            // Arrange
            var testRoute = new Route { 
                RouteName = "Test Route",
                StartLocation = "School",
                EndLocation = "Downtown",
                Distance = -1.0m // Invalid negative distance
            };

            var form = new RouteManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            StopRefreshTimer(form);
            
            // Act - use reflection to call the private ValidateRoute method
            var method = form.GetType().GetMethod("ValidateRoute", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testRoute });
            
            // Assert
            Assert.False(result);
        }
    }
}
