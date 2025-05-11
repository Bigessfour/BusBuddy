using Xunit;
using BusBuddy.Forms;
using BusBuddy.Data.Interfaces;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using BusBuddy.Tests.Utilities;
using Moq;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests focusing on the accessibility aspects of the VehiclesManagementForm
    /// </summary>
    [SupportedOSPlatform("windows6.1")]
    public class VehiclesManagementForm_AccessibilityTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<VehiclesManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public VehiclesManagementForm_AccessibilityTests() : base(1)
        {
            _loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
            _dbHelperMock = new Mock<IDatabaseHelper>();
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_ButtonsHaveAccessibleLabels()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var buttons = buttonPanel.Controls.Cast<Control>().Where(c => c is Button).Cast<Button>().ToList();
            
            foreach (var button in buttons)
            {
                Assert.False(string.IsNullOrEmpty(button.Text));
                Assert.Equal(button.Text, button.AccessibleName);
            }
        }
        
        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_InputControlsHaveAccessibleLabels()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // First, use reflection to show the edit panel
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Get edit panel
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            Assert.NotNull(editPanel);
            
            // Get all input controls (TextBox, NumericUpDown, DateTimePicker)
            var textboxes = editPanel.Controls.Cast<Control>().Where(c => c is TextBox).ToList();
            var numericUpdowns = editPanel.Controls.Cast<Control>().Where(c => c is NumericUpDown).ToList();
            var dateTimePickers = editPanel.Controls.Cast<Control>().Where(c => c is DateTimePicker).ToList();
            
            // Verify each input control has a corresponding label
            foreach (var textbox in textboxes)
            {
                var matchingLabel = FindMatchingLabel(editPanel, textbox);
                Assert.NotNull(matchingLabel);
                Assert.False(string.IsNullOrEmpty(matchingLabel.Text));
            }
            
            foreach (var numericUpDown in numericUpdowns)
            {
                var matchingLabel = FindMatchingLabel(editPanel, numericUpDown);
                Assert.NotNull(matchingLabel);
                Assert.False(string.IsNullOrEmpty(matchingLabel.Text));
            }
            
            foreach (var dateTimePicker in dateTimePickers)
            {
                var matchingLabel = FindMatchingLabel(editPanel, dateTimePicker);
                Assert.NotNull(matchingLabel);
                Assert.False(string.IsNullOrEmpty(matchingLabel.Text));
            }
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_ContrastColorsAreAccessible()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Get buttons 
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var buttons = buttonPanel.Controls.Cast<Control>().Where(c => c is Button).Cast<Button>().ToList();
            
            foreach (var button in buttons)
            {
                // Verify contrast between background and foreground
                var contrastRatio = CalculateContrastRatio(button.BackColor, button.ForeColor);
                
                // WCAG 2.0 Level AA requires a contrast ratio of at least 4.5:1 for normal text
                Assert.True(contrastRatio >= 4.5);
            }
            
            // First, use reflection to show the edit panel
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Get edit panel
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            
            // Get save and cancel buttons from edit panel
            var editPanelButtons = editPanel.Controls.Cast<Control>().Where(c => c is Button).Cast<Button>().ToList();
            
            foreach (var button in editPanelButtons)
            {
                // Verify contrast between background and foreground
                var contrastRatio = CalculateContrastRatio(button.BackColor, button.ForeColor);
                Assert.True(contrastRatio >= 4.5);
            }
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_ValidationMessageIsClearlyVisible()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // First, use reflection to show the edit panel
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Get edit panel
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            
            // Find validation message label
            var validationLabel = editPanel.Controls.Cast<Control>()
                .FirstOrDefault(c => c is Label && ((Label)c).ForeColor == Color.Red);
            
            Assert.NotNull(validationLabel);
            
            // Trigger validation using reflection (to access private fields and methods)
            // This should update the validation label text
            // Access txtVehicleNumber field
            var txtVehicleNumberField = typeof(VehiclesManagementForm).GetField("txtVehicleNumber", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var txtVehicleNumber = (TextBox)txtVehicleNumberField.GetValue(form);
            
            // Clear the field to trigger validation
            txtVehicleNumber.Text = "";
            
            // Call ValidateInput method
            var validateInputMethod = typeof(VehiclesManagementForm).GetMethod("ValidateInput", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            validateInputMethod.Invoke(form, new object[] { form, EventArgs.Empty });
            
            // Verify validation message is now visible
            Assert.NotEqual("", validationLabel.Text);
            Assert.Equal(Color.Red, validationLabel.ForeColor);
            
            // Verify contrast between validation text and background
            var contrastRatio = CalculateContrastRatio(validationLabel.BackColor, validationLabel.ForeColor);
            Assert.True(contrastRatio >= 4.5);
        }
        
        // Helper method to find a matching label for a given input control
        private Label FindMatchingLabel(Control container, Control inputControl)
        {
            return container.Controls.Cast<Control>()
                .Where(c => c is Label)
                .Cast<Label>()
                .FirstOrDefault(l => 
                    // Label should be to the left of the input and vertically aligned
                    l.Left < inputControl.Left && 
                    Math.Abs(l.Top - inputControl.Top) < 30);
        }
        
        // Helper method to calculate contrast ratio between two colors
        // Based on WCAG 2.0 formula: https://www.w3.org/TR/WCAG20-TECHS/G17.html
        private double CalculateContrastRatio(Color backgroundColor, Color foregroundColor)
        {
            // Calculate relative luminance for both colors
            double bgLuminance = GetRelativeLuminance(backgroundColor);
            double fgLuminance = GetRelativeLuminance(foregroundColor);
            
            // Calculate contrast ratio
            double contrastRatio;
            if (bgLuminance > fgLuminance)
                contrastRatio = (bgLuminance + 0.05) / (fgLuminance + 0.05);
            else
                contrastRatio = (fgLuminance + 0.05) / (bgLuminance + 0.05);
            
            return contrastRatio;
        }
        
        // Calculate relative luminance of a color
        private double GetRelativeLuminance(Color color)
        {
            double r = GetColorComponent(color.R / 255.0);
            double g = GetColorComponent(color.G / 255.0);
            double b = GetColorComponent(color.B / 255.0);
            
            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }
        
        // Helper for calculating color component for luminance
        private double GetColorComponent(double component)
        {
            return component <= 0.03928
                ? component / 12.92
                : Math.Pow((component + 0.055) / 1.055, 2.4);
        }
    }
}
