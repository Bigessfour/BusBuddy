// BusBuddy/UI/Forms/SchedulerForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.API;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class SchedulerForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private List<string> _drivers = new List<string>();
        private List<int> _busNumbers = new List<int>();

        public SchedulerForm() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _dbManager = new DatabaseManager(Logger);
            LoadDriversAndBuses();
            LoadTrips();
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
                Logger.Error(ex, "Unexpected error loading drivers or bus numbers.");
                UpdateStatus("Error loading data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Unexpected error loading drivers or bus numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrips()
        {
            try
            {
                UpdateStatus("Loading trips...", AppSettings.Theme.InfoColor);
                var trips = _dbManager.GetTrips();
                dataGridView.DataSource = trips;
                UpdateStatus("Trips loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unexpected error loading trips.");
                UpdateStatus("Error loading trips.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Unexpected error loading trips.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddTripButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Add Trip button clicked.");

            var trip = new Trip
            {
                TripType = tripTypeComboBox.Text,
                Date = datePicker.Value.ToString("yyyy-MM-dd"),
                BusNumber = int.TryParse(busNumberComboBox.Text, out int busNumber) ? busNumber : 0,
                DriverName = driverNameComboBox.Text,
                StartTime = startTimeTextBox.Text,
                EndTime = endTimeTextBox.Text
            };

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
                Logger.Error(ex, "Error checking for scheduling conflicts: {ErrorMessage}", ex.Message);
                UpdateStatus("Error checking conflicts.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error checking for scheduling conflicts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TimeSpan startTime = TimeSpan.Parse(trip.StartTime);
            TimeSpan endTime = TimeSpan.Parse(trip.EndTime);
            TimeSpan totalHours = endTime - startTime;
            if (totalHours.TotalHours < 0)
            {
                totalHours = totalHours.Add(TimeSpan.FromDays(1));
            }
            trip.Total_Hours_Driven = totalHours.ToString(@"hh\:mm");

            try
            {
                UpdateStatus("Adding trip...", AppSettings.Theme.InfoColor);
                _dbManager.AddTrip(trip);
                LoadTrips();
                ClearInputs();
                MessageBox.Show("Trip added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unexpected error adding trip.");
                UpdateStatus("Error adding trip.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Unexpected error adding trip.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Refresh button clicked.");
            LoadTrips();
        }

        private void ClearInputs()
        {
            tripTypeComboBox.SelectedIndex = -1;
            datePicker.Value = DateTime.Today;
            busNumberComboBox.SelectedIndex = -1;
            driverNameComboBox.SelectedIndex = -1;
            startTimeTextBox.Clear();
            endTimeTextBox.Clear();
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }
    }
}