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
using System.Runtime.Versioning;
using BusBuddy.Tests.Utilities;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests for the VehiclesManagementForm focusing on data display functionality
    /// Using MessageBoxHandler to automatically handle MessageBox dialogs during tests
    /// </summary>
    [SupportedOSPlatform("windows6.1")]  // Indicate this class requires Windows 6.1 (Windows 7) or later
    public class VehiclesManagementForm_DataDisplayTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<VehiclesManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public VehiclesManagementForm_DataDisplayTests() : base(1) // Use 1 (IDOK) as the default button to click
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
        }
        
        [Fact, WindowsOnly]        
        public async Task LoadVehicles_ShouldPopulateDataGridWithVehicles()
        {
            // Arrange
            var testVehicles = new List<Vehicle> {
                new Vehicle { 
                    Id = 1, 
                    VehicleNumber = "Bus-101", 
                    Make = "Blue Bird", 
                    Model = "All American", 
                    Year = 2020, 
                    Capacity = 72,
                    LicensePlate = "SCH-001",
                    VIN = "1HVBBABN11H123456",
                    InsuranceExpiration = DateTime.Now.AddYears(1)
                },
                new Vehicle { 
                    Id = 2, 
                    VehicleNumber = "Bus-202", 
                    Make = "Thomas", 
                    Model = "Saf-T-Liner", 
                    Year = 2019, 
                    Capacity = 65,
                    LicensePlate = "SCH-002",
                    VIN = "2XVBBABN11H789012",
                    InsuranceExpiration = DateTime.Now.AddYears(1)
                }
            };
            
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(testVehicles);

            // Create a task completion source that will be completed when the log happens
            var tcs = new TaskCompletionSource<bool>();
            
            // Setup logger mock to complete the task when the expected log message is seen
            _loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("vehicles")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)))
                .Callback(() => tcs.TrySetResult(true));

            // Creating the form with task completion using a lambda to access the form
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Clear any previously captured message boxes
            ClearCapturedDialogs();
            
            // Act - call the LoadVehicles method directly 
            form.LoadVehicles();
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 10 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetVehiclesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report successful loading
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("vehicles")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        
        [Fact, WindowsOnly]
        public async Task LoadVehicles_ShouldHandleEmptyVehicleList()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle>());

            // Create a task completion source that will be completed when the log happens
            var tcs = new TaskCompletionSource<bool>();
            
            // Setup logger mock to complete the task when the expected log message is seen
            _loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Loaded 0 vehicles")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)))
                .Callback(() => tcs.TrySetResult(true));

            // Creating the form
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Clear any previously captured message boxes
            ClearCapturedDialogs();
            
            // Act - call the LoadVehicles method directly
            form.LoadVehicles();
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 10 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetVehiclesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report empty vehicle list
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Loaded 0 vehicles")),
                    It.IsAny<Exception?>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }
        
        [Fact, WindowsOnly]
        public async Task LoadVehicles_ShouldHandleDatabaseException()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ThrowsAsync(new Exception("Database connection failed"));

            // Creating the form
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
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
                
            // Start the LoadVehicles process
            form.LoadVehicles();
            
            // Use a timeout to prevent hanging
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                // Test is taking too long, possibly hanging
                throw new TimeoutException("Test execution timed out after 10 seconds");
            }

            // Assert
            // Verify the database was called
            _dbHelperMock.Verify(x => x.GetVehiclesAsync(), Times.AtLeastOnce);
            
            // Verify logger was called to report the exception
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception?>(e => e != null && e.Message.Contains("Database connection failed")),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
            
            // Also verify that a message box was shown with an error
            Assert.Contains(CapturedDialogs, dialog => dialog.Title == "Error");
        }
    }
}

