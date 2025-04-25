// BusBuddy/UI/Forms/TripSchedulerForm.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using Serilog;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class TripSchedulerForm : BaseForm
    {
        private readonly IDatabaseManager _dbManager;
        private readonly new ILogger _logger;
        private List<Trip> _trips = new List<Trip>();
        private Trip? _selectedTrip; // Mark as nullable

        public TripSchedulerForm(IDatabaseManager dbManager, ILogger logger)
        {
            _logger = logger;
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            InitializeComponent();
            
            // Apply consistent styling
            ApplyCustomStyling();
            
            LoadTrips();
            SetupEventHandlers();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Apply styling to data grid
            FormStyler.StyleDataGridView(tripsDataGridView);
            
            // Style buttons
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    FormStyler.StyleButton(button, !button.Name.Contains("Delete") && !button.Name.Contains("Cancel"));
                }
                else if (control is Label label && label != statusLabel)
                {
                    FormStyler.StyleLabel(label, label.Name.Contains("Header") || label.Name.Contains("Title"));
                }
                else if (control is GroupBox groupBox)
                {
                    FormStyler.StyleGroupBox(groupBox);
                    
                    // Apply styling to controls within the group box
                    foreach (Control innerControl in groupBox.Controls)
                    {
                        if (innerControl is TextBox innerTextBox)
                        {
                            innerTextBox.BorderStyle = BorderStyle.FixedSingle;
                            innerTextBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                            innerTextBox.BackColor = System.Drawing.Color.White;
                        }
                        else if (innerControl is Label innerLabel)
                        {
                            FormStyler.StyleLabel(innerLabel, false);
                        }
                        else if (innerControl is DateTimePicker dateTimePicker)
                        {
                            dateTimePicker.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                        }
                        else if (innerControl is ComboBox comboBox)
                        {
                            comboBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                            comboBox.BackColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }

        private void SetupEventHandlers()
        {
            // Add event handler for trip selection
            tripsDataGridView.SelectionChanged += TripsDataGridView_SelectionChanged;
        }

        private void TripsDataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            // Update selected trip when selection changes
            if (tripsDataGridView.SelectedRows.Count > 0 && tripsDataGridView.SelectedRows[0].Index < _trips.Count)
            {
                int selectedIndex = tripsDataGridView.SelectedRows[0].Index;
                _selectedTrip = _trips[selectedIndex];
                _logger.Information("Selected trip ID: {TripID}", _selectedTrip.TripID);
            }
            else
            {
                _selectedTrip = null;
            }
        }

        private void LoadTrips()
        {
            try
            {
                _trips = _dbManager.GetTrips();
                tripsDataGridView.DataSource = _trips;
                _logger.Information("Loaded {Count} trips", _trips.Count);
                statusLabel.Text = $"{_trips.Count} trips loaded.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading trips");
                statusLabel.Text = "Error loading trips.";
            }
        }

        protected override void SaveRecord()
        {
            try
            {
                // This would normally use data from form controls
                var trip = new Trip
                {
                    TripType = "Regular", // Replace with actual input
                    Date = DateOnly.FromDateTime(DateTime.Now), // Use DateOnly instead of string
                    BusNumber = 101,
                    DriverName = "Driver Name", // Use DriverName instead of DriverID
                    StartTime = TimeOnly.FromTimeSpan(new TimeSpan(7, 0, 0)), // Use TimeOnly instead of string
                    EndTime = TimeOnly.FromTimeSpan(new TimeSpan(8, 0, 0)), // Use TimeOnly instead of string
                    TotalHoursDriven = 1.0,
                    Destination = "School"
                };

                _dbManager.AddTrip(trip);
                _logger.Information("Trip added for {TripDate}", trip.Date.ToString("yyyy-MM-dd"));
                statusLabel.Text = "Trip added successfully.";
                LoadTrips();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding trip");
                statusLabel.Text = "Error adding trip: " + ex.Message;
            }
        }

        protected override void RefreshData()
        {
            LoadTrips();
        }

        protected override void EditRecord()
        {
            if (_selectedTrip == null)
            {
                statusLabel.Text = "No trip selected.";
                return;
            }

            try
            {
                // Here you would update the selected trip with values from form controls
                // For now we'll just reload the trips
                LoadTrips();
                statusLabel.Text = "Trip updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating trip");
                statusLabel.Text = "Error updating trip: " + ex.Message;
            }
        }

        protected override void DeleteRecord()
        {
            if (_selectedTrip == null)
            {
                statusLabel.Text = "No trip selected.";
                return;
            }

            try
            {
                // Here we would delete the selected trip
                // _dbManager.DeleteTrip(_selectedTrip.TripID);
                LoadTrips();
                statusLabel.Text = "Trip deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting trip");
                statusLabel.Text = "Error deleting trip: " + ex.Message;
            }
        }
    }
}