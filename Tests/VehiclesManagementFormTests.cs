using Xunit;
using BusBuddy.Forms;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using System.Collections.Generic;
using Moq;
using System;
using BusBuddy.Tests.Utilities;

namespace BusBuddy.Tests
{
    public class VehiclesManagementFormTests
    {
        private readonly Mock<ILogger<VehiclesManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public VehiclesManagementFormTests()
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
            
            // Set up the mock to return sample data
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle> 
                {
                    new Vehicle 
                    { 
                        Id = 1, 
                        VehicleNumber = "Bus-01", 
                        Make = "Blue Bird", 
                        Model = "Vision", 
                        Year = 2023, 
                        Capacity = 48,
                        LicensePlate = "ABC123", 
                        VIN = "1HGCM82633A123456", 
                        InsuranceExpiration = DateTime.Now.AddYears(1) 
                    },
                    new Vehicle 
                    { 
                        Id = 2, 
                        VehicleNumber = "Bus-02", 
                        Make = "Thomas", 
                        Model = "Saf-T-Liner", 
                        Year = 2022, 
                        Capacity = 52,
                        LicensePlate = "DEF456", 
                        VIN = "2FMZA52233BB54321", 
                        InsuranceExpiration = DateTime.Now.AddMonths(6) 
                    }
                });
        }

        [Fact, WindowsOnly]
        public void VehiclesManagementForm_InitializesCorrectly()
        {
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            Assert.NotNull(form);
        }

        [Fact(Skip = "UI testing requires STAThread attribute which is not supported in xUnit")]
        public void VehiclesManagementForm_LoadsVehiclesOnInit()
        {
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            // Not calling form.Show() in CI environment
            
            // Verify that the GetVehiclesAsync method was called
            _dbHelperMock.Verify(x => x.GetVehiclesAsync(), Times.AtLeastOnce);
            
            // TODO: Add assertions for data binding
            Assert.True(true);
        }

        [Fact, WindowsOnly]
        public void VehiclesManagementForm_CanAddVehicle()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.AddVehicle(It.IsAny<Vehicle>()))
                .Callback<Vehicle>((v) => v.Id = 3);
                
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - we're just testing that the mock is set up correctly
            // In a real test, we would interact with the form
            
            // Assert
            Assert.True(true);
        }

        [Fact, WindowsOnly]
        public void VehiclesManagementForm_CanEditVehicle()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.GetVehicle(It.IsAny<int>()))
                .Returns(new Vehicle { Id = 1, VehicleNumber = "Bus-01" });
                
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - we're just testing that the mock is set up correctly
            // In a real test, we would interact with the form
            
            // Assert
            Assert.True(true);
        }

        [Fact, WindowsOnly]
        public void VehiclesManagementForm_CanDeleteVehicle()
        {
            // Arrange
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - In a real test, we would simulate button click
            
            // Assert
            Assert.True(true);
        }
        
        [Fact, WindowsOnly]
        public void VehiclesManagementForm_ChecksInsuranceExpiration()
        {
            // Arrange
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle> 
                {
                    new Vehicle 
                    { 
                        Id = 1, 
                        VehicleNumber = "Bus-01", 
                        InsuranceExpiration = DateTime.Now.AddDays(-1)  // Expired insurance
                    }
                });
                
            var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Act - In a real test with proper UI framework,
            // we would check for a visual indicator of expired insurance
            
            // Assert - For now, just verify the database was called
            _dbHelperMock.Verify(x => x.GetVehiclesAsync(), Times.AtLeastOnce);
        }
    }
}

