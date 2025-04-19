// BusBuddy/UI/Forms/Welcome.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusBuddy.Models;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public partial class Welcome : BaseForm, IWelcomeView
    {
        private readonly WelcomePresenter _presenter;
        private Timer? _autoRefreshTimer;
        private const string NoTripsMessage = "No scheduled trips for today";
        private const string TimeFormatPlaceholder = "--:--";
        private readonly ILogger _logger = Log.Logger;

        public Welcome() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _presenter = new WelcomePresenter(this);

            // Set window title
            this.Text = "BusBuddy - School Bus Management System";
            
            // Set current date
            dateTimeLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            
            _logger.Information("Welcome form initialized");
            UpdateStatus("Ready", Color.Black);
            
            // Setup refresh timer (5 minutes)
            SetupRefreshTimer();

            // Load initial trips data
            LoadTripsData();
        }
        
        private void SetupRefreshTimer()
        {
            try
            {
                if (_autoRefreshTimer == null)
                {
                    _autoRefreshTimer = new Timer
                    {
                        Interval = 5 * 60 * 1000 // 5 minutes
                    };
                    _autoRefreshTimer.Tick += AutoRefreshTimer_Tick;
                    _autoRefreshTimer.Start();
                    _logger.Information("Auto refresh timer started with interval {Interval}ms", _autoRefreshTimer.Interval);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error setting up refresh timer: {ErrorMessage}", ex.Message);
            }
        }

        private void AutoRefreshTimer_Tick(object? sender, EventArgs e)
        {
            LoadTripsData();
        }

        private void LoadTripsData()
        {
            try
            {
                _logger.Information("Loading today's trips data");
                UpdateStatus("Loading trips data...", Color.FromArgb(0, 99, 177));
                
                var todaysTrips = _presenter.GetTodaysTripsData();
                DisplayTodaysTripsInGrid(todaysTrips);
                
                // Display stats
                string stats = _presenter.GetDashboardStats();
                statsLabel.Text = stats;
                
                UpdateStatus("Ready", Color.Black);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading trips data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading data", Color.Red);
            }
        }

        private void DisplayTodaysTripsInGrid(List<Trip> trips)
        {
            if (trips == null)
            {
                _logger.Warning("Trips list is null");
                return;
            }

            try
            {
                if (trips.Count == 0)
                {
                    ShowNoTripsMessage();
                    return;
                }

                var tripTable = new DataTable();
                tripTable.Columns.Add("Type", typeof(string));
                tripTable.Columns.Add("Route", typeof(string));
                tripTable.Columns.Add("Start", typeof(string));
                tripTable.Columns.Add("End", typeof(string));
                tripTable.Columns.Add("Driver", typeof(string));
                tripTable.Columns.Add("Bus", typeof(string));

                foreach (var trip in trips)
                {
                    tripTable.Rows.Add(
                        trip.TripType ?? "Unknown",
                        trip.Destination ?? "Unspecified",
                        trip.StartTime ?? TimeFormatPlaceholder,
                        trip.EndTime ?? TimeFormatPlaceholder,
                        trip.DriverName ?? "Unassigned",
                        trip.BusNumber.ToString() ?? "N/A"
                    );
                }

                todaysActivitiesGrid.DataSource = tripTable;
                FormatGridColumns();

                _logger.Information("Displayed {Count} trips in the grid", trips.Count);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error displaying trips in grid: {ErrorMessage}", ex.Message);
            }
        }

        private void ShowNoTripsMessage()
        {
            var noTrips = new DataTable();
            noTrips.Columns.Add("Info", typeof(string));
            noTrips.Rows.Add(NoTripsMessage);
            todaysActivitiesGrid.DataSource = noTrips;
            _logger.Information("No scheduled trips found for today");
        }

        private void FormatGridColumns()
        {
            if (todaysActivitiesGrid.Columns == null || todaysActivitiesGrid.Columns.Count == 0)
                return;

            todaysActivitiesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            todaysActivitiesGrid.EnableHeadersVisualStyles = false;
            todaysActivitiesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 99, 177);
            todaysActivitiesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            todaysActivitiesGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            todaysActivitiesGrid.ColumnHeadersHeight = 30;
            todaysActivitiesGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);

            if (todaysActivitiesGrid.Columns.Count <= 1)
                return;

            // Set column widths if we have the expected columns
            if (todaysActivitiesGrid.Columns.Count >= 6)
            {
                SetColumnWidth("Type", 60);
                SetColumnWidth("Route", 150);
                SetColumnWidth("Start", 70);
                SetColumnWidth("End", 70);
                SetColumnWidth("Driver", 120);
                SetColumnWidth("Bus", 50);
            }
        }

        private void SetColumnWidth(string columnName, int width)
        {
            if (todaysActivitiesGrid?.Columns != null && 
                todaysActivitiesGrid.Columns.Contains(columnName))
            {
                todaysActivitiesGrid.Columns[columnName]!.Width = width;
            }
        }

        public void UpdateStatus(string message, Color color)
        {
            if (statusLabel != null && !this.IsDisposed)
            {
                // Use BeginInvoke to ensure thread safety for UI updates
                this.BeginInvoke(new Action(() =>
                {
                    statusLabel.ForeColor = color;
                    statusLabel.Text = message;
                }));
            }
        }

        // Navigation methods
        public void NavigateToInputs()
        {
            _logger.Information("Navigating to Inputs");
            UpdateStatus("Opening Inputs...", Color.FromArgb(0, 99, 177));
            using (var inputsForm = new Inputs())
            {
                inputsForm.ShowDialog();
            }
            LoadTripsData(); // Refresh data when returning from Inputs
            UpdateStatus("Ready", Color.Black);
        }

        public void NavigateToScheduler()
        {
            _logger.Information("Navigating to Trip Scheduler");
            UpdateStatus("Opening Trip Scheduler...", Color.FromArgb(0, 99, 177));
            FormNavigator.NavigateTo("Trip Scheduler");
            LoadTripsData(); // Refresh data when returning from Scheduler
            UpdateStatus("Ready", Color.Black);
        }

        public void NavigateToFuelRecords()
        {
            _logger.Information("Navigating to Fuel Records");
            UpdateStatus("Opening Fuel Records...", Color.FromArgb(0, 99, 177));
            FormNavigator.NavigateTo("Fuel Records");
            UpdateStatus("Ready", Color.Black);
        }

        public void NavigateToDriverManagement()
        {
            _logger.Information("Navigating to Driver Management");
            UpdateStatus("Opening Driver Management...", Color.FromArgb(0, 99, 177));
            FormNavigator.NavigateTo("Driver Management");
            UpdateStatus("Ready", Color.Black);
        }

        public void NavigateToReports()
        {
            _logger.Information("Navigating to Reports");
            UpdateStatus("Opening Reports...", Color.FromArgb(0, 99, 177));
            MessageBox.Show("Reports form not implemented yet.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateStatus("Ready", Color.Black);
        }

        public void NavigateToSettings()
        {
            _logger.Information("Navigating to Settings");
            UpdateStatus("Opening Settings...", Color.FromArgb(0, 99, 177));
            using (var settingsForm = new Settings())
            {
                settingsForm.ShowDialog();
            }
            UpdateStatus("Ready", Color.Black);
        }

        // Event handlers for the UI buttons
        private void refreshTodayButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Refresh button clicked");
            LoadTripsData();
        }

        private void InputsButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Data Entry button clicked");
            NavigateToInputs();
        }

        private void SchedulerButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Scheduler button clicked");
            NavigateToScheduler();
        }

        private void FuelButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Fuel button clicked");
            NavigateToFuelRecords();
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Driver button clicked");
            NavigateToDriverManagement();
        }

        private void ReportsButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Reports button clicked");
            NavigateToReports();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Settings button clicked");
            NavigateToSettings();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Exit button clicked");
            var result = MessageBox.Show("Are you sure you want to exit BusBuddy?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _logger.Information("Exiting application");
                Application.Exit();
            }
        }

        private void TestDbButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Test Database button clicked");
            try
            {
                UpdateStatus("Testing database connection...", Color.FromArgb(0, 99, 177));
                var stats = _presenter.GetDashboardStats();
                MessageBox.Show($"Database connection successful!\n\n{stats}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatus("Database test completed", Color.Green);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Database connection failed: {ErrorMessage}", ex.Message);
                MessageBox.Show($"Database connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Database test failed", Color.Red);
            }
        }
    }
}