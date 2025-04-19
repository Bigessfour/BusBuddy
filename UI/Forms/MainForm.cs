#nullable enable
using System;
using System.Windows.Forms;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public partial class MainForm : BaseForm
    {
        private readonly StatusStrip _statusStrip;
        private readonly ToolStripStatusLabel _statusLabel;

        // Constants
        private const string ErrorCaption = "Error";
        private const string InfoCaption = "Information";
        private const string StatusReadyText = "Ready";
        private const string UnknownErrorMessage = "Unknown error occurred";

        public MainForm() : base(new MainFormNavigator())
        {
            InitializeComponent();
            
            // Initialize readonly fields in the constructor
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel { Text = StatusReadyText };
            
            // Setup status bar
            InitializeStatusBar();
            
            Log.Information("MainForm initialized.");
            UpdateStatus(StatusReadyText);
        }

        private void InitializeStatusBar()
        {
            // Use the already initialized fields
            _statusStrip.Items.Add(_statusLabel);
            Controls.Add(_statusStrip);
            _statusStrip.Dock = DockStyle.Bottom;
        }

        private void UpdateStatus(string message)
        {
            _statusLabel.Text = message;
            Log.Information("Status updated: {Message}", message);
        }

        private void SchedulerButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            NavigateToScheduler();
        }

        private void FuelButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            NavigateToFuelRecords();
        }

        private void DriverButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            NavigateToDriverManagement();
        }

        private void ReportsButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            NavigateToReports();
        }

        private void SettingsButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            NavigateToSettings();
        }

        private void InputsButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            NavigateToInputs();
        }

        private void ExitButton_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;
            ExitApplication();
        }

        public void NavigateToScheduler()
        {
            try
            {
                Log.Information("Navigating to Trip Scheduler.");
                UpdateStatus("Opening Trip Scheduler...");
                FormNavigator.NavigateTo("Trip Scheduler");
                UpdateStatus(StatusReadyText);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error navigating to scheduler: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error opening scheduler: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus(StatusReadyText);
            }
        }

        public void NavigateToFuelRecords()
        {
            try
            {
                Log.Information("Navigating to Fuel Records.");
                UpdateStatus("Opening Fuel Records...");
                FormNavigator.NavigateTo("Fuel Records");
                UpdateStatus(StatusReadyText);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error navigating to fuel records: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error opening fuel records: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus(StatusReadyText);
            }
        }

        public void NavigateToDriverManagement()
        {
            try
            {
                Log.Information("Navigating to Driver Management.");
                UpdateStatus("Opening Driver Management...");
                FormNavigator.NavigateTo("Driver Management");
                UpdateStatus(StatusReadyText);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error navigating to driver management: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error opening driver management: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus(StatusReadyText);
            }
        }

        public void NavigateToReports()
        {
            try
            {
                Log.Information("Navigating to Reports.");
                UpdateStatus("Opening Reports...");
                MessageBox.Show("Reports form not implemented yet.", InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatus(StatusReadyText);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error navigating to reports: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error opening reports: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus(StatusReadyText);
            }
        }

        public void NavigateToSettings()
        {
            try
            {
                Log.Information("Navigating to Settings.");
                UpdateStatus("Opening Settings...");
                using (var settingsForm = new Settings())
                {
                    settingsForm.ShowDialog();
                }
                UpdateStatus(StatusReadyText);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error navigating to settings: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error opening settings: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus(StatusReadyText);
            }
        }

        public void NavigateToInputs()
        {
            try
            {
                Log.Information("Navigating to Inputs.");
                UpdateStatus("Opening Inputs...");
                using (var inputsForm = new Inputs())
                {
                    inputsForm.ShowDialog();
                }
                UpdateStatus(StatusReadyText);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error navigating to inputs: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error opening inputs: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus(StatusReadyText);
            }
        }

        public void ExitApplication()
        {
            try
            {
                Log.Information("Exit button clicked.");
                var result = MessageBox.Show("Are you sure you want to exit BusBuddy?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Log.Information("Exiting application.");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Log.Error(ex, "Error exiting application: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error exiting application: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _statusStrip?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}