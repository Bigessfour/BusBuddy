using Xunit;
using BusBuddy.Forms;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using BusBuddy.Tests.Utilities;
using Moq;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Tests specifically focused on the UI design aspects of the VehiclesManagementForm
    /// </summary>
    [SupportedOSPlatform("windows6.1")]
    public class VehiclesManagementForm_UIDesignTests : MessageBoxHandlerTestBase
    {
        private readonly Mock<ILogger<VehiclesManagementForm>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;

        public VehiclesManagementForm_UIDesignTests() : base(1)
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<VehiclesManagementForm>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_InitialState_HasCorrectSizeAndTitle()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            Assert.Equal("Vehicles Management", form.Text);
            Assert.Equal(new Size(1000, 600), form.Size);
            Assert.Equal(FormStartPosition.CenterParent, form.StartPosition);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_InitialState_HasRequiredPanelsAndControls()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Verify main panels exist
            var formPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Fill);
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            
            Assert.NotNull(formPanel);
            Assert.NotNull(buttonPanel);
            Assert.NotNull(editPanel);
            
            // Verify DataGridView exists
            Assert.NotNull(form.DataGridView);
            Assert.Equal(DockStyle.Fill, form.DataGridView.Dock);
            
            // Verify buttons in button panel
            var buttons = buttonPanel.Controls.Cast<Control>().Where(c => c is Button).Cast<Button>().ToList();
            Assert.Equal(4, buttons.Count);
            
            // Verify button texts
            Assert.Contains(buttons, b => b.Text == "Add Vehicle");
            Assert.Contains(buttons, b => b.Text == "Edit");
            Assert.Contains(buttons, b => b.Text == "Delete");
            Assert.Contains(buttons, b => b.Text == "Refresh");
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void EditPanel_InitialState_IsHidden()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Get the edit panel
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            
            Assert.NotNull(editPanel);
            Assert.False(editPanel.Visible);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void EditPanel_ContainsRequiredInputFields()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Get the edit panel
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            Assert.NotNull(editPanel);

            // Verify control presence
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is Label && ((Label)c).Text.Contains("Vehicle Details"));
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is Label && ((Label)c).Text.Contains("Vehicle Number"));
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is TextBox);
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is Label && ((Label)c).Text.Contains("Make"));
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is Label && ((Label)c).Text.Contains("Model"));
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is Label && ((Label)c).Text.Contains("Year"));
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is NumericUpDown);
            Assert.Contains(editPanel.Controls.Cast<Control>(), c => c is DateTimePicker);
            
            // Verify save and cancel buttons
            var buttons = editPanel.Controls.Cast<Control>().Where(c => c is Button).Cast<Button>().ToList();
            Assert.Contains(buttons, b => b.Text == "Save");
            Assert.Contains(buttons, b => b.Text == "Cancel");
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Buttons_HaveCorrectStyling()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var buttons = buttonPanel.Controls.Cast<Control>().Where(c => c is Button).Cast<Button>().ToList();
            
            // Verify Add button styling
            var addButton = buttons.FirstOrDefault(b => b.Text == "Add Vehicle");
            Assert.NotNull(addButton);
            Assert.Equal(Color.FromArgb(0, 120, 215), addButton.BackColor);
            Assert.Equal(Color.White, addButton.ForeColor);
            Assert.Equal(FlatStyle.Flat, addButton.FlatStyle);
            
            // Verify Delete button styling
            var deleteButton = buttons.FirstOrDefault(b => b.Text == "Delete");
            Assert.NotNull(deleteButton);
            Assert.Equal(Color.FromArgb(220, 53, 69), deleteButton.BackColor);
            Assert.Equal(Color.White, deleteButton.ForeColor);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void DataGridView_HasCorrectConfiguration()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            var dataGridView = form.DataGridView;
            
            Assert.NotNull(dataGridView);
            Assert.False(dataGridView.AllowUserToAddRows);
            Assert.False(dataGridView.AllowUserToDeleteRows);
            Assert.True(dataGridView.ReadOnly);
            Assert.False(dataGridView.MultiSelect);
            Assert.Equal(DataGridViewSelectionMode.FullRowSelect, dataGridView.SelectionMode);
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void Form_RespondsCorrectlyToResize()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            // Get references to panels
            var formPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Fill);
            var buttonPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Bottom);
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            
            // Initial size
            var initialSize = form.Size;
            
            // Resize the form
            form.Size = new Size(initialSize.Width + 200, initialSize.Height + 100);
            
            // Verify panels responded correctly
            Assert.Equal(DockStyle.Fill, formPanel.Dock);
            Assert.Equal(DockStyle.Bottom, buttonPanel.Dock);
            Assert.Equal(DockStyle.Right, editPanel.Dock);
            
            // Form panel should expand, button panel should maintain height and expand width
            Assert.Equal(50, buttonPanel.Height); // Button panel height should stay constant
        }

        [Fact(Skip = "UI testing may require STAThread attribute which is not supported in xUnit")]
        public void EditPanel_ShowsHidesCorrectly()
        {
            using var form = new VehiclesManagementForm(_dbHelperMock.Object, _loggerMock.Object);
            
            var formPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Fill);
            var editPanel = form.Controls.Cast<Control>().FirstOrDefault(c => c is Panel && c.Dock == DockStyle.Right);
            
            // Initial state - edit panel should be hidden
            Assert.False(editPanel.Visible);
            Assert.Equal(form.Width, formPanel.Width);
            
            // Call method to show edit panel using reflection (since it's private)
            var showEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("ShowEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            showEditPanelMethod.Invoke(form, new object[] { true });
            
            // Verify edit panel is now visible
            Assert.True(editPanel.Visible);
            Assert.Equal(form.Width - editPanel.Width, formPanel.Width);
            
            // Call method to hide edit panel
            var hideEditPanelMethod = typeof(VehiclesManagementForm).GetMethod("HideEditPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            hideEditPanelMethod.Invoke(form, null);
            
            // Verify edit panel is hidden again
            Assert.False(editPanel.Visible);
            Assert.Equal(form.Width, formPanel.Width);
        }
    }
}
