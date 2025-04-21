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
        private System.Windows.Forms.Timer? _autoRefreshTimer; // Nullable to comply with SonarCloud
        private const string NoTripsMessage = "No scheduled trips for today";
        private const string TimeFormatPlaceholder = "--:--";
        private readonly ILogger _logger = Log.Logger;

        // Fields for dragging controls
        private bool _isDragging = false;
        private Point _dragStartPoint;
        private Control? _draggedControl = null; // Keep track of the control being dragged

        public Welcome() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _presenter = new WelcomePresenter(this);

            // Set window title
            this.Text = "BusBuddy - School Bus Management System";
            
            // Set current date
            dateTimeLabel.Text = DateTime.Now.ToString("MMMM d, yyyy");
            
            // Assign event handlers (assuming they might not be assigned in the designer)
            AssignEventHandlers();

            _logger.Information("Welcome form initialized");
            
            // Setup refresh timer (5 minutes)
            SetupRefreshTimer();

            // Subscribe to Load event for UI updates and drag setup
            Load += Welcome_Load;
        }

        private void AssignEventHandlers()
        {
            // Assign handlers from designer if not already done
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            this.tripsButton.Click += new System.EventHandler(this.tripsButton_Click);
            this.fuelButton.Click += new System.EventHandler(this.fuelButton_Click);
            this.driversButton.Click += new System.EventHandler(this.driversButton_Click);
            this.reportsButton.Click += new System.EventHandler(this.reportsButton_Click);
            this.schedulesButton.Click += new System.EventHandler(this.SchedulesButton_Click); // Keep original casing if designer uses it
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            this.testDbButton.Click += new System.EventHandler(this.TestDbButton_Click); // Keep original casing
            this.refreshTodayButton.Click += new System.EventHandler(this.refreshTodayButton_Click); // Keep original casing
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click); // Keep original casing
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready", Color.Black);
            LoadTripsData();
            SetupDraggableControls(); // Call setup for drag handlers
        }

        private void SetupRefreshTimer()
        {
            try
            {
                // SonarCloud compliant null check
                if (_autoRefreshTimer is null)
                {
                    _autoRefreshTimer = new System.Windows.Forms.Timer
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

        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
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
                    // SonarCloud compliant null handling
                    string tripType = trip.TripType ?? "Unknown";
                    string destination = trip.Destination ?? "Unspecified";
                    string startTime = trip.StartTime.ToString() != "00:00" ? trip.StartTime.ToString() : TimeFormatPlaceholder;
                    string endTime = trip.EndTime.ToString() != "00:00" ? trip.EndTime.ToString() : TimeFormatPlaceholder;
                    string driverName = trip.DriverName ?? "Unassigned";
                    string busNumber = trip.BusNumber.ToString();
                    
                    tripTable.Rows.Add(
                        tripType,
                        destination,
                        startTime,
                        endTime,
                        driverName,
                        busNumber
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
            // SonarCloud compliant null check with pattern matching
            if (todaysActivitiesGrid.Columns is null || todaysActivitiesGrid.Columns.Count == 0)
                return;

            todaysActivitiesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            todaysActivitiesGrid.EnableHeadersVisualStyles = false;
            
            // Ensure cell style objects are not null before accessing
            if (todaysActivitiesGrid.ColumnHeadersDefaultCellStyle is not null)
            {
                todaysActivitiesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 99, 177);
                todaysActivitiesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                todaysActivitiesGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
            
            todaysActivitiesGrid.ColumnHeadersHeight = 30;
            
            if (todaysActivitiesGrid.DefaultCellStyle is not null)
            {
                todaysActivitiesGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
            }

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
            // SonarCloud compliant null check with pattern matching
            if (todaysActivitiesGrid?.Columns is not null && 
                !string.IsNullOrEmpty(columnName) &&
                todaysActivitiesGrid.Columns.Contains(columnName))
            {
                todaysActivitiesGrid.Columns[columnName].Width = width;
            }
        }

        public new void UpdateStatus(string message, Color color)
        {
            // SonarCloud compliant null check and thread safety
            if (statusLabel is not null && !this.IsDisposed)
            {
                // Use BeginInvoke to ensure thread safety for UI updates
                this.BeginInvoke(new Action(() =>
                {
                    statusLabel.ForeColor = color;
                    statusLabel.Text = message ?? string.Empty; // Null-safe assignment
                }));
            }
        }

        private void SetupDraggableControls()
        {
            // Make group boxes draggable
            AttachDragEvents(buttonGroupBox1);
            AttachDragEvents(buttonGroupBox2);
            AttachDragEvents(buttonGroupBox3);
        }

        private void AttachDragEvents(Control control)
        {
            control.MouseDown += Control_MouseDown;
            control.MouseMove += Control_MouseMove;
            control.MouseUp += Control_MouseUp;
            // Also attach to children if you want dragging by clicking inside the groupbox
            foreach (Control child in control.Controls)
            {
                 // Be careful not to attach to controls that need their own mouse events (like buttons)
                 if (!(child is Button)) // Example: Don't attach to buttons
                 {
                    child.MouseDown += Control_MouseDown;
                    child.MouseMove += Control_MouseMove;
                    child.MouseUp += Control_MouseUp;
                 }
            }
        }

        private void Control_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                // If the sender is a child control, get the parent groupbox
                _draggedControl = sender as Control;
                if (_draggedControl != null && !(_draggedControl is GroupBox))
                {
                    _draggedControl = _draggedControl.Parent;
                }

                if (_draggedControl != null)
                {
                    // Calculate offset from the control's top-left corner
                    _dragStartPoint = new Point(e.X, e.Y);
                    _draggedControl.Cursor = Cursors.SizeAll; // Change cursor to indicate dragging
                    _draggedControl.BringToFront(); // Bring the dragged control to the front
                }
            }
        }

        private void Control_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedControl != null)
            {
                // Calculate the new location of the control
                Point currentScreenPos = _draggedControl.PointToScreen(new Point(e.X, e.Y));
                // Need parent's coordinate system
                Point parentClientPos = _draggedControl.Parent.PointToClient(currentScreenPos);

                // Calculate new top-left corner, considering the drag start offset
                int newX = parentClientPos.X - _dragStartPoint.X;
                int newY = parentClientPos.Y - _dragStartPoint.Y;

                // Optional: Add boundary checks if needed
                // newX = Math.Max(0, Math.Min(newX, _draggedControl.Parent.ClientSize.Width - _draggedControl.Width));
                // newY = Math.Max(0, Math.Min(newY, _draggedControl.Parent.ClientSize.Height - _draggedControl.Height));

                _draggedControl.Location = new Point(newX, newY);
            }
        }

        private void Control_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                if (_draggedControl != null)
                {
                    _draggedControl.Cursor = Cursors.Default; // Restore default cursor
                }
                _draggedControl = null;
            }
        }

        // Navigation methods
        public void NavigateToInputs() // Added to satisfy IWelcomeView
        {
            _logger.Warning("NavigateToInputs called, but the corresponding UI element does not exist.");
            // Or throw new NotImplementedException("Inputs UI element not found.");
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

        public void NavigateToRoutes()
        {
            _logger.Information("Navigating to Routes Management");
            using (var routesForm = new ScheduledRoutesForm())
            {
                routesForm.ShowDialog();
            }
            LoadTripsData(); // Refresh data when returning from Routes
            UpdateStatus("Ready", Color.Black);
        }

        // Event handlers for the UI buttons - Renamed to match designer controls

        private void refreshTodayButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Refresh button clicked");
            LoadTripsData();
        }

        // Renamed from SchedulerButton_Click, assigned to startButton
        private void startButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Manage Bus Routes button clicked");
            NavigateToRoutes(); // Changed from NavigateToScheduler() to NavigateToRoutes()
        }

        // Renamed from FuelButton_Click
        private void fuelButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Fuel button clicked");
            NavigateToFuelRecords();
        }

        // Renamed from DriverButton_Click
        private void driversButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Drivers button clicked");
            NavigateToDriverManagement();
        }

        // Renamed from ReportsButton_Click
        private void reportsButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Reports button clicked");
            NavigateToReports();
        }

        // Renamed from SettingsButton_Click
        private void settingsButton_Click(object sender, EventArgs e)
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

        // Renamed from RoutesButton_Click, assigned to tripsButton
        private void tripsButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Manage Trips button clicked");
            NavigateToRoutes(); // Assuming Manage Trips opens Routes Management
        }

        // Keep original name as it matches designer
        private void SchedulesButton_Click(object sender, EventArgs e)
        {
            _logger.Information("School Calendar button clicked");
            UpdateStatus("Opening School Calendar...", Color.FromArgb(0, 99, 177));
            using (var calendarForm = new SchoolCalendarForm())
            {
                calendarForm.ShowDialog();
            }
            UpdateStatus("Ready", Color.Black);
        }
    }
}