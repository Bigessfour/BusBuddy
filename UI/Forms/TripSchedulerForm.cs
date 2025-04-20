using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.API;
using BusBuddy.Utilities;
using Serilog;
using BusBuddy.UI.Forms;
using BusBuddy.UI.Interfaces; // Add reference to the interface namespace

namespace BusBuddy.UI.Forms // Changed from BusBuddy.Forms to BusBuddy.UI.Forms
{
    public partial class TripSchedulerForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;
        private List<string> _drivers = new List<string>();
        private List<int> _busNumbers = new List<int>();
        private DataGridView dataGridView;
        private ComboBox tripTypeComboBox;
        private DateTimePicker datePicker;
        private ComboBox busNumberComboBox;
        private ComboBox driverNameComboBox;
        private DateTimePicker startTimePicker;
        private DateTimePicker endTimePicker;
        private Button addButton;
        private Button refreshButton;
        private StatusStrip statusStrip;

        public TripSchedulerForm() : base(new MainFormNavigator())
        {
            _dbManager = new DatabaseManager();
            _logger = Log.Logger;
            InitializeComponent();

            // Defer loading until the form is fully loaded
            this.Load += TripSchedulerForm_Load;
        }

        private void TripSchedulerForm_Load(object sender, EventArgs e)
        {
            LoadDriversAndBuses();
            LoadTrips();
        }

        private void InitializeComponent()
        {
            this.Text = "Trip Scheduler";
            this.Size = new System.Drawing.Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = AppSettings.Theme.BackgroundColor;

            // DataGridView
            dataGridView = new DataGridView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(760, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackgroundColor = AppSettings.Theme.PanelColor,
                GridColor = AppSettings.Theme.BorderColor
            };
            dataGridView.Columns.Add("TripID", "TripID");
            dataGridView.Columns.Add("TripType", "TripType");
            dataGridView.Columns.Add("Date", "Date");
            dataGridView.Columns.Add("BusNumber", "BusNumber");
            dataGridView.Columns.Add("DriverName", "DriverName");
            dataGridView.Columns.Add("StartTime", "StartTime");
            this.Controls.Add(dataGridView);

            // Form Section
            var inputGroupBox = new GroupBox
            {
                Text = "Add New Trip",
                Location = new System.Drawing.Point(10, 220),
                Size = new System.Drawing.Size(760, 300),
                BackColor = AppSettings.Theme.GroupBoxColor,
                FlatStyle = FlatStyle.Flat,
                Font = AppSettings.Theme.LabelFont
            };

            var labelTripType = new Label { Text = "Trip Type:", Location = new System.Drawing.Point(10, 30), AutoSize = true, Font = AppSettings.Theme.LabelFont };
            tripTypeComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(120, 27),
                Size = new System.Drawing.Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = AppSettings.Theme.DataFont
            };
            tripTypeComboBox.Items.AddRange(new[] { "Regular", "Field Trip", "Charter" });

            var labelDate = new Label { Text = "Date:", Location = new System.Drawing.Point(10, 70), AutoSize = true, Font = AppSettings.Theme.LabelFont };
            datePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(120, 67),
                Size = new System.Drawing.Size(200, 30),
                Format = DateTimePickerFormat.Short,
                Value = new DateTime(2025, 4, 19),
                Font = AppSettings.Theme.DataFont
            };

            var labelBusNumber = new Label { Text = "Bus Number:", Location = new System.Drawing.Point(10, 110), AutoSize = true, Font = AppSettings.Theme.LabelFont };
            busNumberComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(120, 107),
                Size = new System.Drawing.Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = AppSettings.Theme.DataFont
            };

            var labelDriverName = new Label { Text = "Driver Name:", Location = new System.Drawing.Point(10, 150), AutoSize = true, Font = AppSettings.Theme.LabelFont };
            driverNameComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(120, 147),
                Size = new System.Drawing.Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = AppSettings.Theme.DataFont
            };

            var labelStartTime = new Label { Text = "Start Time:", Location = new System.Drawing.Point(10, 190), AutoSize = true, Font = AppSettings.Theme.LabelFont };
            startTimePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(120, 187),
                Size = new System.Drawing.Size(200, 30),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Font = AppSettings.Theme.DataFont
            };

            var labelEndTime = new Label { Text = "End Time:", Location = new System.Drawing.Point(10, 230), AutoSize = true, Font = AppSettings.Theme.LabelFont };
            endTimePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(120, 227),
                Size = new System.Drawing.Size(200, 30),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Font = AppSettings.Theme.DataFont
            };

            // Buttons with styles
            addButton = new Button
            {
                Text = "Add",
                Location = new System.Drawing.Point(120, 267),
                Size = new System.Drawing.Size(100, 35),
                BackColor = AppSettings.Theme.SuccessColor,
                ForeColor = AppSettings.Theme.TextLightColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 2, BorderColor = AppSettings.Theme.SuccessColor },
                Font = AppSettings.Theme.ButtonFont
            };
            addButton.Click += AddButton_Click;

            refreshButton = new Button
            {
                Text = "Refresh",
                Location = new System.Drawing.Point(230, 267),
                Size = new System.Drawing.Size(100, 35),
                BackColor = AppSettings.Theme.InfoColor,
                ForeColor = AppSettings.Theme.TextLightColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 2, BorderColor = AppSettings.Theme.InfoColor },
                Font = AppSettings.Theme.ButtonFont
            };
            refreshButton.Click += RefreshButton_Click;

            // Status Strip
            statusStrip = new StatusStrip
            {
                Location = new System.Drawing.Point(0, 568),
                Size = new System.Drawing.Size(784, 22),
                SizingGrip = false
            };
            statusLabel = new ToolStripStatusLabel
            {
                Text = "Ready.",
                ForeColor = AppSettings.Theme.TextColor
            };
            statusStrip.Items.Add(statusLabel);

            inputGroupBox.Controls.AddRange(new Control[] { labelTripType, tripTypeComboBox, labelDate, datePicker, labelBusNumber,
                busNumberComboBox, labelDriverName, driverNameComboBox, labelStartTime, startTimePicker,
                labelEndTime, endTimePicker, addButton, refreshButton });

            this.Controls.AddRange(new Control[] { dataGridView, inputGroupBox, statusStrip });

            // Form Closing Event
            this.FormClosing += TripSchedulerForm_FormClosing;
        }

        private void LoadDriversAndBuses()
        {
            try
            {
                UpdateStatus("Loading drivers and buses...", AppSettings.Theme.InfoColor);
                _drivers = _dbManager.GetDriverNames();
                _busNumbers = _dbManager.GetBusNumbers();

                driverNameComboBox.Items.Clear();
                driverNameComboBox.Items.AddRange(_drivers.ToArray());

                busNumberComboBox.Items.Clear();
                busNumberComboBox.Items.AddRange(_busNumbers.Select(n => n.ToString()).ToArray());

                UpdateStatus("Drivers and buses loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error loading drivers or bus numbers: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Unexpected error loading drivers or bus numbers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrips()
        {
            try
            {
                UpdateStatus("Loading trips...", AppSettings.Theme.InfoColor);
                var trips = _dbManager.GetTrips();
                dataGridView.Rows.Clear();
                foreach (var trip in trips)
                {
                    dataGridView.Rows.Add(trip.TripID, trip.TripType, trip.Date, trip.BusNumber.ToString(), trip.DriverName, trip.StartTime);
                }
                UpdateStatus("Trips loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error loading trips: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading trips.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Unexpected error loading trips: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Add Trip button clicked.");

            var trip = new Trip
            {
                TripType = tripTypeComboBox.Text,
                Date = datePicker.Value.ToString("yyyy-MM-dd"),
                BusNumber = int.TryParse(busNumberComboBox.Text, out int busNumber) ? busNumber : 0,
                DriverName = driverNameComboBox.Text,
                StartTime = startTimePicker.Value.ToString("HH:mm"),
                EndTime = endTimePicker.Value.ToString("HH:mm")
            };

            // Validation
            var (isValid, errors) = DataValidator.ValidateTrip(trip, _drivers, _busNumbers);
            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for scheduling conflicts using AI
            try
            {
                UpdateStatus("Checking for scheduling conflicts...", AppSettings.Theme.InfoColor);
                var (hasConflict, conflictDetails) = await ApiClient.DetectSchedulingConflictsAsync(trip, _dbManager.GetTrips());
                if (hasConflict)
                {
                    UpdateStatus("Scheduling conflict detected.", AppSettings.Theme.ErrorColor);
                    MessageBox.Show($"Scheduling conflict detected:\n{conflictDetails}", "Conflict Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error checking for scheduling conflicts: {ErrorMessage}", ex.Message);
                UpdateStatus("Error checking conflicts.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error checking for scheduling conflicts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculate total hours driven
            TimeSpan startTime = TimeSpan.Parse(trip.StartTime);
            TimeSpan endTime = TimeSpan.Parse(trip.EndTime);
            TimeSpan totalHours = endTime - startTime;
            if (totalHours.TotalHours < 0)
            {
                totalHours = totalHours.Add(TimeSpan.FromDays(1));
            }
            trip.Total_Hours_Driven = totalHours.ToString(@"hh\:mm");

            // Add the trip to the database
            try
            {
                UpdateStatus("Adding trip...", AppSettings.Theme.InfoColor);
                await DataManager.AddRecordAsync(trip, _dbManager.AddTrip, _logger, UpdateStatus);
                LoadTrips();
                RefreshForm();
                MessageBox.Show("Trip added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error adding trip: {ErrorMessage}", ex.Message);
                UpdateStatus("Error adding trip.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Unexpected error adding trip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Refresh button clicked.");
            LoadTrips();
            RefreshForm();
        }

        private void RefreshForm()
        {
            tripTypeComboBox.SelectedIndex = -1;
            datePicker.Value = new DateTime(2025, 4, 19);
            busNumberComboBox.SelectedIndex = -1;
            driverNameComboBox.SelectedIndex = -1;
            startTimePicker.Value = DateTime.Now;
            endTimePicker.Value = DateTime.Now;
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void TripSchedulerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close this form?", "Confirm Close",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                e.Cancel = false; // Allow form to close
            }
            else if (result == DialogResult.No)
            {
                e.Cancel = true; // Prevent form from closing
            }
        }
    }
}