using Xunit;
using BusBuddy.Forms;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using BusBuddy.Tests.Utilities;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests for the VehiclesManagementForm's business logic
    /// </summary>
    [SupportedOSPlatform("windows6.1")]
    public class VehiclesManagementFormBusinessTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<VehiclesManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public VehiclesManagementFormBusinessTests() : base(1) // Use 1 (IDOK) as the default button to click
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();

            // Set up the mock to return sample data
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle> 
                {
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
                        InsuranceExpiration = DateTime.Now.AddMonths(2)
                    }
                });
        }

        [Fact]
        public void ValidateVehicle_ValidInput_ReturnsTrue()
        {
            // Arrange
            var testVehicle = new Vehicle
            {
                VehicleNumber = "Bus-303",
                Make = "IC Bus",
                Model = "CE Series",
                Year = 2022,
                Capacity = 60,
                LicensePlate = "SCH-003",
                VIN = "3ABCDEFG12H345678",
                InsuranceExpiration = DateTime.Now.AddMonths(6)
            };

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ValidateVehicle method
            var method = form.GetType().GetMethod("ValidateVehicle", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testVehicle });

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateVehicle_MissingRequiredField_ReturnsFalse()
        {
            // Arrange
            var testVehicle = new Vehicle
            {
                VehicleNumber = "", // Missing required field
                Make = "IC Bus",
                Model = "CE Series",
                Year = 2022,
                Capacity = 60,
                LicensePlate = "SCH-003",
                VIN = "3ABCDEFG12H345678",
                InsuranceExpiration = DateTime.Now.AddMonths(6)
            };

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ValidateVehicle method
            var method = form.GetType().GetMethod("ValidateVehicle", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testVehicle });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateVehicle_InvalidYear_ReturnsFalse()
        {
            // Arrange
            var testVehicle = new Vehicle
            {
                VehicleNumber = "Bus-303",
                Make = "IC Bus",
                Model = "CE Series",
                Year = 1900, // Invalid year (too old)
                Capacity = 60,
                LicensePlate = "SCH-003",
                VIN = "3ABCDEFG12H345678",
                InsuranceExpiration = DateTime.Now.AddMonths(6)
            };

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ValidateVehicle method
            var method = form.GetType().GetMethod("ValidateVehicle", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testVehicle });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateVehicle_InvalidCapacity_ReturnsFalse()
        {
            // Arrange
            var testVehicle = new Vehicle
            {
                VehicleNumber = "Bus-303",
                Make = "IC Bus",
                Model = "CE Series",
                Year = 2022,
                Capacity = -5, // Invalid negative capacity
                LicensePlate = "SCH-003",
                VIN = "3ABCDEFG12H345678",
                InsuranceExpiration = DateTime.Now.AddMonths(6)
            };

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ValidateVehicle method
            var method = form.GetType().GetMethod("ValidateVehicle", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testVehicle });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateVehicle_ExpiredInsurance_ReturnsFalse()
        {
            // Arrange
            var testVehicle = new Vehicle
            {
                VehicleNumber = "Bus-303",
                Make = "IC Bus",
                Model = "CE Series",
                Year = 2022,
                Capacity = 60,
                LicensePlate = "SCH-003",
                VIN = "3ABCDEFG12H345678",
                InsuranceExpiration = DateTime.Now.AddDays(-1) // Expired insurance
            };

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ValidateVehicle method
            var method = form.GetType().GetMethod("ValidateVehicle", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method.Invoke(form, new object[] { testVehicle });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShowVehicleForm_EditMode_PopulatesFormWithVehicleData()
        {
            // Arrange
            var vehicle = new Vehicle 
            { 
                Id = 1, 
                VehicleNumber = "Bus-101", 
                Make = "Blue Bird", 
                Model = "All American", 
                Year = 2020, 
                Capacity = 72,
                LicensePlate = "SCH-001",
                VIN = "1HVBBABN11H123456",
                InsuranceExpiration = DateTime.Now.AddYears(1)
            };

            _dbHelperMock.Setup(x => x.GetVehicle(1))
                .Returns(vehicle);

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ShowVehicleForm method for edit mode
            var method = form.GetType().GetMethod("ShowVehicleForm", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(form, new object[] { 1 }); // ID 1 = edit mode

            // The actual UI verification isn't feasible in a unit test, but we can verify
            // the database was queried correctly
            _dbHelperMock.Verify(x => x.GetVehicle(1), Times.Once);
        }

        [Fact]
        public void ShowVehicleForm_AddMode_CreatesNewVehicleForm()
        {
            // Arrange
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private ShowVehicleForm method for add mode
            var method = form.GetType().GetMethod("ShowVehicleForm", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(form, new object[] { null }); // null ID = add mode

            // Verify that no vehicle was looked up
            _dbHelperMock.Verify(x => x.GetVehicle(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteVehicle_ConfirmsBeforeDeleting()
        {
            // Arrange
            int vehicleId = 1;
            _dbHelperMock.Setup(x => x.DeleteVehicleAsync(vehicleId))
                .ReturnsAsync(true);

            // Change the auto close button to "Yes" (6)
            var messageBoxHandlerField = typeof(MessageBoxHandlerTestBase).GetField("_messageBoxHandler", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var handler = messageBoxHandlerField.GetValue(this);
            var startMonitoringMethod = handler.GetType().GetMethod("StartMonitoring");
            startMonitoringMethod.Invoke(handler, new object[] { 6 }); // 6 = Yes button
            
            ClearCapturedDialogs();
            
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private DeleteVehicle method
            var method = form.GetType().GetMethod("DeleteVehicleAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { vehicleId });
            var result = await task;

            // Assert
            Assert.True(result);
            _dbHelperMock.Verify(x => x.DeleteVehicleAsync(vehicleId), Times.Once);
            Assert.NotEmpty(CapturedDialogs); // Verify a confirmation dialog was shown
        }

        [Fact]
        public async Task DeleteVehicle_CancelsWhenUserClicksNo()
        {
            // Arrange
            int vehicleId = 1;

            // Change the auto close button to "No" (7)
            var messageBoxHandlerField = typeof(MessageBoxHandlerTestBase).GetField("_messageBoxHandler", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var handler = messageBoxHandlerField.GetValue(this);
            var startMonitoringMethod = handler.GetType().GetMethod("StartMonitoring");
            startMonitoringMethod.Invoke(handler, new object[] { 7 }); // 7 = No button
            
            ClearCapturedDialogs();
            
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);

            // Act - use reflection to call the private DeleteVehicle method
            var method = form.GetType().GetMethod("DeleteVehicleAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var task = (Task<bool>)method.Invoke(form, new object[] { vehicleId });
            var result = await task;

            // Assert
            Assert.False(result);
            _dbHelperMock.Verify(x => x.DeleteVehicleAsync(It.IsAny<int>()), Times.Never);
            Assert.NotEmpty(CapturedDialogs); // Verify a confirmation dialog was shown
        }

        [Fact]
        public void CheckInsuranceExpirations_IdentifiesExpiringInsurance()
        {
            // Arrange - one vehicle with insurance expiring soon
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle> 
                {
                    new Vehicle { 
                        Id = 1, 
                        VehicleNumber = "Bus-101", 
                        Make = "Blue Bird", 
                        Model = "All American", 
                        Year = 2020, 
                        InsuranceExpiration = DateTime.Now.AddDays(20) // Expires within 30 days
                    },
                    new Vehicle { 
                        Id = 2, 
                        VehicleNumber = "Bus-202", 
                        Make = "Thomas", 
                        Model = "Saf-T-Liner", 
                        Year = 2019,
                        InsuranceExpiration = DateTime.Now.AddYears(1) // Not expiring soon
                    }
                });

            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            ClearCapturedDialogs();

            // Act - use reflection to call the private CheckInsuranceExpirations method
            var method = form.GetType().GetMethod("CheckInsuranceExpirations", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(form, null);            // Assert - should have shown at least one message box about expiring insurance
            Assert.Contains(CapturedDialogs, dialog => 
                dialog.Title.Contains("insurance") && dialog.Title.Contains("expiring"));
        }
    }
}
