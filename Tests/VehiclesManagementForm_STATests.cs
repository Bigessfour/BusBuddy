using Xunit;
using Xunit.Abstractions;
using BusBuddy.Forms;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using BusBuddy.Tests.Utilities;
using Moq;

namespace BusBuddy.Tests
{
    /// <summary>
    /// UI tests for VehiclesManagementForm using the STA test runner to enable proper Windows Forms testing
    /// </summary>
    [SupportedOSPlatform("windows6.1")]
    public class VehiclesManagementForm_STATests
    {
        private readonly ITestOutputHelper _output;
        private readonly STATestRunner _testRunner;
        
        public VehiclesManagementForm_STATests(ITestOutputHelper output)
        {
            _output = output;
            _testRunner = new STATestRunner(_output as IMessageSink);
        }
        
        [Fact, WindowsOnly]
        public void Form_InitializesAndDisplaysCorrectly()
        {
            _testRunner.RunTest(() =>
            {
                // Arrange
                var loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
                var dbHelperMock = new Mock<IDatabaseHelper>();
                
                // Set up mock data
                dbHelperMock.Setup(x => x.GetVehiclesAsync())
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
                
                // Act
                using var form = new VehiclesManagementForm(dbHelperMock.Object, loggerMock.Object);
                
                // Assert - basic checks that won't throw exceptions
                Assert.Equal("Vehicles Management", form.Text);
                Assert.Equal(new System.Drawing.Size(1000, 600), form.Size);
                
                // Verify DataGridView is present
                Assert.NotNull(form.DataGridView);
            });
        }
        
        [Fact, WindowsOnly]
        public void AddButton_ShowsAndHidesEditPanel()
        {
            _testRunner.RunTest(() =>
            {
                // Arrange
                var loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
                var dbHelperMock = new Mock<IDatabaseHelper>();
                
                using var form = new VehiclesManagementForm(dbHelperMock.Object, loggerMock.Object);
                
                // Get panels and buttons
                var formPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Fill);
                var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
                var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
                
                Assert.NotNull(formPanel);
                Assert.NotNull(editPanel);
                Assert.NotNull(buttonPanel);
                
                // Initially edit panel should be hidden
                Assert.False(editPanel.Visible);
                
                // Find and click Add button
                var addButton = buttonPanel.Controls.Cast<Control>()
                    .Where(c => c is Button)
                    .Cast<Button>()
                    .FirstOrDefault(b => b.Text == "Add Vehicle");
                
                Assert.NotNull(addButton);
                
                // Act - click Add button
                addButton.PerformClick();
                
                // Assert - edit panel should be visible
                Assert.True(editPanel.Visible);
                
                // Find and click Cancel button
                var cancelButton = editPanel.Controls.Cast<Control>()
                    .Where(c => c is Button)
                    .Cast<Button>()
                    .FirstOrDefault(b => b.Text == "Cancel");
                
                Assert.NotNull(cancelButton);
                
                // Act - click Cancel button
                cancelButton.PerformClick();
                
                // Assert - edit panel should be hidden again
                Assert.False(editPanel.Visible);
            });
        }
        
        [Fact, WindowsOnly]
        public void VehicleFormValidation_WorksCorrectly()
        {
            _testRunner.RunTest(() =>
            {
                // Arrange
                var loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
                var dbHelperMock = new Mock<IDatabaseHelper>();
                
                using var form = new VehiclesManagementForm(dbHelperMock.Object, loggerMock.Object);
                
                // Get panels and buttons
                var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
                var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
                
                // Find and click Add button to show edit panel
                var addButton = buttonPanel.Controls.Cast<Control>()
                    .Where(c => c is Button)
                    .Cast<Button>()
                    .FirstOrDefault(b => b.Text == "Add Vehicle");
                
                addButton.PerformClick();
                
                // Get form controls using reflection
                var txtVehicleNumberField = typeof(VehiclesManagementForm).GetField("txtVehicleNumber", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var txtVehicleNumber = (TextBox)txtVehicleNumberField.GetValue(form);
                
                var txtMakeField = typeof(VehiclesManagementForm).GetField("txtMake", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var txtMake = (TextBox)txtMakeField.GetValue(form);
                
                var txtModelField = typeof(VehiclesManagementForm).GetField("txtModel", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var txtModel = (TextBox)txtModelField.GetValue(form);
                
                var lblValidationMessageField = typeof(VehiclesManagementForm).GetField("lblValidationMessage", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var lblValidationMessage = (Label)lblValidationMessageField.GetValue(form);
                
                var btnSaveField = typeof(VehiclesManagementForm).GetField("btnSave", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var btnSave = (Button)btnSaveField.GetValue(form);
                
                // Act - enter invalid data (clear fields)
                txtVehicleNumber.Text = "";
                txtMake.Text = "";
                txtModel.Text = "";
                
                // Need to explicitly trigger validation since we're not interacting with the UI normally
                var validateInputMethod = typeof(VehiclesManagementForm).GetMethod("ValidateInput", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                validateInputMethod.Invoke(form, new object[] { txtVehicleNumber, EventArgs.Empty });
                
                // Assert - validation message should show errors and save button should be disabled
                Assert.NotEmpty(lblValidationMessage.Text);
                Assert.Contains("Vehicle Number is required", lblValidationMessage.Text);
                Assert.False(btnSave.Enabled);
                
                // Act - enter valid data
                txtVehicleNumber.Text = "Bus-999";
                txtMake.Text = "Test Make";
                txtModel.Text = "Test Model";
                
                // Trigger validation again
                validateInputMethod.Invoke(form, new object[] { txtVehicleNumber, EventArgs.Empty });
                
                // Assert - validation should pass and save button should be enabled
                Assert.DoesNotContain("Vehicle Number is required", lblValidationMessage.Text);
                Assert.True(btnSave.Enabled);
            });
        }
        
        [Fact, WindowsOnly]
        public void ResizeForm_ControlsResizeCorrectly()
        {
            _testRunner.RunTest(() =>
            {
                // Arrange
                var loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
                var dbHelperMock = new Mock<IDatabaseHelper>();
                
                using var form = new VehiclesManagementForm(dbHelperMock.Object, loggerMock.Object);
                
                // Get panels
                var formPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Fill);
                var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
                
                Assert.NotNull(formPanel);
                Assert.NotNull(buttonPanel);
                
                // Remember initial sizes
                var initialFormPanelWidth = formPanel.Width;
                var initialButtonPanelWidth = buttonPanel.Width;
                var initialButtonPanelHeight = buttonPanel.Height;
                
                // Act - resize the form
                form.Size = new System.Drawing.Size(form.Width + 200, form.Height + 100);
                
                // Assert - panels should resize accordingly
                Assert.Equal(initialFormPanelWidth + 200, formPanel.Width);
                Assert.Equal(initialButtonPanelWidth + 200, buttonPanel.Width);
                Assert.Equal(initialButtonPanelHeight, buttonPanel.Height); // Height should remain constant
                
                // DataGridView should fill its parent
                Assert.Equal(formPanel.Width, form.DataGridView.Width);
                Assert.Equal(formPanel.Height, form.DataGridView.Height);
            });
        }
    }
}

