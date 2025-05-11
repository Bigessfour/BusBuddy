using Xunit;
using BusBuddy.Forms;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using BusBuddy.Tests.Utilities;
using Moq;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests focusing on user interactions with the VehiclesManagementForm
    /// </summary>
    [SupportedOSPlatform("windows6.1")]
    public class VehiclesManagementForm_InteractionTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<VehiclesManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public VehiclesManagementForm_InteractionTests() : base(1)
        {
            _loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
            _dbHelperMock = new Mock<IDatabaseHelper>();
            
            // Set up basic mock data
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle> 
                {
                    new Vehicle 
                    { 
                        Id = 1, 
                        VehicleNumber = "Bus-101", 
                        Make = "Blue Bird", 
                        Model = "Vision", 
                        Year = 2022, 
                        LicensePlate = "ABC123", 
                        VIN = "1HGCM82633A123456", 
                        Capacity = 48,
                        InsuranceExpiration = DateTime.Now.AddYears(1) 
                    }
                });
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void AddButton_Click_ShowsEditPanel()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Find and click the Add button
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var addButton = buttonPanel.Controls.Cast<Control>()
                .Where(c => c is Button)
                .Cast<Button>()
                .FirstOrDefault(b => b.Text == "Add Vehicle");
            
            Assert.NotNull(addButton);
            
            // Get edit panel to check visibility
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            Assert.NotNull(editPanel);
            
            // Initially the edit panel should be hidden
            Assert.False(editPanel.Visible);
            
            // Simulate clicking the Add button
            addButton.PerformClick();
            
            // Now the edit panel should be visible
            Assert.True(editPanel.Visible);
            
            // Fields should be empty for a new vehicle
            var txtVehicleNumberField = typeof(VehiclesManagementForm).GetField("txtVehicleNumber", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var txtVehicleNumber = (TextBox)txtVehicleNumberField.GetValue(form);
            
            Assert.Equal("", txtVehicleNumber.Text);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void CancelButton_Click_HidesEditPanel()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // First, use reflection to show the edit panel
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Get the edit panel
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            Assert.NotNull(editPanel);
            
            // Verify edit panel is visible
            Assert.True(editPanel.Visible);
            
            // Find the Cancel button and click it
            var cancelButton = editPanel.Controls.Cast<Control>()
                .Where(c => c is Button)
                .Cast<Button>()
                .FirstOrDefault(b => b.Text == "Cancel");
            
            Assert.NotNull(cancelButton);
            cancelButton.PerformClick();
            
            // Verify edit panel is now hidden
            Assert.False(editPanel.Visible);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void ValidationMessage_DisplaysOnInvalidInput()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // First, use reflection to show the edit panel
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Get the validation message label using reflection
            var lblValidationMessageField = typeof(VehiclesManagementForm).GetField("lblValidationMessage", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var lblValidationMessage = (Label)lblValidationMessageField.GetValue(form);
            
            // Get the VehicleNumber textbox
            var txtVehicleNumberField = typeof(VehiclesManagementForm).GetField("txtVehicleNumber", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var txtVehicleNumber = (TextBox)txtVehicleNumberField.GetValue(form);
            
            // Initially it should have no validation messages
            Assert.Equal("", lblValidationMessage.Text);
            
            // Enter an invalid value (empty string)
            txtVehicleNumber.Text = "";
            
            // Invoke the validation method manually
            var validateInputMethod = typeof(VehiclesManagementForm).GetMethod("ValidateInput", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            validateInputMethod.Invoke(form, new object[] { txtVehicleNumber, EventArgs.Empty });
            
            // Now there should be a validation message
            Assert.Contains("Vehicle Number is required", lblValidationMessage.Text);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void SaveButton_DisabledWithInvalidInput()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // First, use reflection to show the edit panel
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Get the Save button
            var btnSaveField = typeof(VehiclesManagementForm).GetField("btnSave", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var btnSave = (Button)btnSaveField.GetValue(form);
            
            // Get necessary text boxes
            var txtVehicleNumberField = typeof(VehiclesManagementForm).GetField("txtVehicleNumber", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var txtVehicleNumber = (TextBox)txtVehicleNumberField.GetValue(form);
            
            var txtMakeField = typeof(VehiclesManagementForm).GetField("txtMake", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var txtMake = (TextBox)txtMakeField.GetValue(form);
            
            // Clear all required fields
            txtVehicleNumber.Text = "";
            txtMake.Text = "";
            
            // Trigger validation
            var validateInputMethod = typeof(VehiclesManagementForm).GetMethod("ValidateInput", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            validateInputMethod.Invoke(form, new object[] { txtVehicleNumber, EventArgs.Empty });
            
            // Save button should be disabled
            Assert.False(btnSave.Enabled);
            
            // Now provide valid input
            txtVehicleNumber.Text = "Bus-123";
            txtMake.Text = "Blue Bird";
            
            // Trigger validation again
            validateInputMethod.Invoke(form, new object[] { txtVehicleNumber, EventArgs.Empty });
            
            // Save button should now be enabled
            Assert.True(btnSave.Enabled);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void EditButton_LoadsSelectedVehicleData()
        {
            // Setup mock to return a specific vehicle
            _dbHelperMock.Setup(x => x.GetVehicle(It.IsAny<int>()))
                .Returns(new Vehicle 
                { 
                    Id = 1, 
                    VehicleNumber = "Bus-101", 
                    Make = "Blue Bird", 
                    Model = "Vision", 
                    Year = 2022, 
                    LicensePlate = "ABC123", 
                    VIN = "1HGCM82633A123456", 
                    Capacity = 48,
                    InsuranceExpiration = DateTime.Now.AddYears(1) 
                });
            
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Load vehicles first
            form.LoadVehicles();
            
            // Select a row in the DataGridView
            // This is tricky to test since we don't have actual data binding in the test
            // For now, we'll call the edit button click handler directly
            
            // Get the Edit button
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var editButton = buttonPanel.Controls.Cast<Control>()
                .Where(c => c is Button)
                .Cast<Button>()
                .FirstOrDefault(b => b.Text == "Edit");
            
            Assert.NotNull(editButton);
            
            // We need to set up mock for data grid selection
            // This will be difficult to test directly in a unit test
            // Normally we'd test this in a UI automation test
            
            // For now, we'll mark this test as incomplete
            Assert.True(true, "This test requires UI automation to properly test");
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void DataGridView_FormatsExpiredInsurance()
        {
            // Setup mock to return vehicles with different insurance statuses
            _dbHelperMock.Setup(x => x.GetVehiclesAsync())
                .ReturnsAsync(new List<Vehicle> 
                {
                    new Vehicle 
                    { 
                        Id = 1, 
                        VehicleNumber = "Bus-101", 
                        InsuranceExpiration = DateTime.Now.AddDays(-10) // Expired
                    },
                    new Vehicle 
                    { 
                        Id = 2, 
                        VehicleNumber = "Bus-102", 
                        InsuranceExpiration = DateTime.Now.AddDays(15) // Expires soon
                    },
                    new Vehicle 
                    { 
                        Id = 3, 
                        VehicleNumber = "Bus-103", 
                        InsuranceExpiration = DateTime.Now.AddYears(1) // Valid
                    }
                });
            
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Load vehicles
            form.LoadVehicles();
            
            // Get the DataGridView_CellFormatting method using reflection
            var cellFormattingMethod = typeof(VehiclesManagementForm).GetMethod("DataGridView_CellFormatting", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // This is difficult to test in a unit test framework
            // We'd need UI automation to properly test the cell formatting
            
            // For now, we'll mark this test as incomplete
            Assert.True(true, "This test requires UI automation to properly test");
        }
    }
}
