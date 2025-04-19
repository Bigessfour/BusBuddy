// BusBuddy/UI/Forms/Welcome.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusBuddy.Models;
using BusBuddy.UI.Interfaces;

namespace BusBuddy.UI.Forms
{
    public partial class Welcome : BaseForm, IWelcomeView
    {
        private readonly WelcomePresenter _presenter;
        private Timer? _autoRefreshTimer;
        private const string NoTripsMessage = "No scheduled trips for today";
        private const string TimeFormatPlaceholder = "--:--";

        public Welcome() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _presenter = new WelcomePresenter(this);
            
            // Apply modern styling
            ApplyModernStyling();
            
            Logger.Information("Welcome form initialized.");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);

            // Set current date
            dateTimeLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            
            // Setup refresh timer
            SetupRefreshTimer();

            // Load initial trips data
            LoadTripsData();
        }

        private void ApplyModernStyling()
        {
            // Style the navigation buttons
            foreach (Control control in navPanel.Controls)
            {
                if (control is Button button)
                {
                    button.MouseEnter += (sender, e) => button.BackColor = AppSettings.Theme.NavHoverColor;
                    button.MouseLeave += (sender, e) => button.BackColor = AppSettings.Theme.NavBackgroundColor;
                }
            }

            // Style the DataGridView for modern look
            if (todaysActivitiesGrid != null)
            {
                todaysActivitiesGrid.BorderStyle = BorderStyle.None;
                todaysActivitiesGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
                todaysActivitiesGrid.EnableHeadersVisualStyles = false;
            }
            
            // Style the refresh button
            refreshTodayButton.FlatAppearance.MouseOverBackColor = AppSettings.Theme.SecondaryColor;
            
            // Set form title
            this.Text = "BusBuddy - Dashboard";
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
                    Logger.Information("Auto refresh timer started with interval {Interval}ms", _autoRefreshTimer.Interval);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error setting up refresh timer: {ErrorMessage}", ex.Message);
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
                Logger.Information("Loading today's trips data");
                UpdateStatus("Loading trips data...", AppSettings.Theme.InfoColor);
                
                var todaysTrips = _presenter.GetTodaysTripsData();
                DisplayTodaysTripsInGrid(todaysTrips);
                
                // Display stats
                string stats = _presenter.GetDashboardStats();
                statsLabel.Text = stats;
                
                UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error loading trips data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading data.", AppSettings.Theme.ErrorColor);
            }
        }

        private void DisplayTodaysTripsInGrid(List<Trip> trips)
        {
            if (trips == null)
            {
                Logger.Warning("Trips list is null");
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

                Logger.Information("Displayed {Count} trips in the grid", trips.Count);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error displaying trips in grid: {ErrorMessage}", ex.Message);
            }
        }

        private void ShowNoTripsMessage()
        {
            var noTrips = new DataTable();
            noTrips.Columns.Add("Info", typeof(string));
            noTrips.Rows.Add(NoTripsMessage);
            todaysActivitiesGrid.DataSource = noTrips;
            Logger.Information("No scheduled trips found for today");
        }

        private void FormatGridColumns()
        {
            if (todaysActivitiesGrid.Columns == null || todaysActivitiesGrid.Columns.Count == 0)
                return;

            todaysActivitiesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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
                // Use null-forgiving operator to tell compiler we've checked for null
                todaysActivitiesGrid.Columns[columnName]!.Width = width;
            }
        }

        private void refreshTodayButton_Click(object sender, EventArgs e)
        {
            LoadTripsData();
        }

        // Navigation methods from your original code
        public void NavigateToInputs()
        {
            Logger.Information("Navigating to Inputs.");
            UpdateStatus("Opening Inputs...", AppSettings.Theme.InfoColor);
            using (var inputsForm = new Inputs())
            {
                inputsForm.ShowDialog();
            }
            LoadTripsData(); // Refresh data when returning from Inputs
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToScheduler()
        {
            Logger.Information("Navigating to Trip Scheduler.");
            UpdateStatus("Opening Trip Scheduler...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Trip Scheduler");
            LoadTripsData(); // Refresh data when returning from Scheduler
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToFuelRecords()
        {
            Logger.Information("Navigating to Fuel Records.");
            UpdateStatus("Opening Fuel Records...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Fuel Records");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToDriverManagement()
        {
            Logger.Information("Navigating to Driver Management.");
            UpdateStatus("Opening Driver Management...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Driver Management");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToReports()
        {
            Logger.Information("Navigating to Reports.");
            UpdateStatus("Opening Reports...", AppSettings.Theme.InfoColor);
            MessageBox.Show("Reports form not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToSettings()
        {
            Logger.Information("Navigating to Settings.");
            UpdateStatus("Opening Settings...", AppSettings.Theme.InfoColor);
            using (var settingsForm = new Settings())
            {
                settingsForm.ShowDialog();
            }
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void SchedulerButton_Click(object sender, EventArgs e)
        {
            NavigateToScheduler();
        }

        private void FuelButton_Click(object sender, EventArgs e)
        {
            NavigateToFuelRecords();
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            NavigateToDriverManagement();
        }

        private void ReportsButton_Click(object sender, EventArgs e)
        {
            NavigateToReports();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigateToSettings();
        }

        private void InputsButton_Click(object sender, EventArgs e)
        {
            NavigateToInputs();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Exit button clicked.");
            var result = MessageBox.Show("Are you sure you want to exit BusBuddy?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Logger.Information("Exiting application.");
                Application.Exit();
            }
        }

        private void TestDbButton_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Testing database connection...", AppSettings.Theme.InfoColor);
                var stats = _presenter.GetDashboardStats();
                MessageBox.Show($"Database connection successful!\n\n{stats}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStatus("Database test completed.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Database connection failed: {ErrorMessage}", ex.Message);
                MessageBox.Show($"Database connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Database test failed.", AppSettings.Theme.ErrorColor);
            }
        }
    }
}