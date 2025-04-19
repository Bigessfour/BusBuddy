// BusBuddy/UI/Forms/FuelForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class FuelForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private List<int> _busNumbers = new List<int>();

        public FuelForm() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _dbManager = new DatabaseManager(Logger);
            LoadBusNumbers();
            LoadFuelRecords();
        }

        private void LoadBusNumbers()
        {
            try
            {
                UpdateStatus("Loading bus numbers...", AppSettings.Theme.InfoColor);
                _busNumbers = _dbManager.GetBusNumbers();
                busNumberComboBox.Items.Clear();
                busNumberComboBox.Items.AddRange(_busNumbers.Select(n => n.ToString()).ToArray());
                UpdateStatus("Bus numbers loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to load bus numbers.");
                UpdateStatus("Error loading bus numbers.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Failed to load bus numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFuelRecords()
        {
            try
            {
                UpdateStatus("Loading fuel records...", AppSettings.Theme.InfoColor);
                var fuelRecords = _dbManager.GetFuelRecords();
                dataGridView.DataSource = fuelRecords;
                UpdateStatus("Fuel records loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to load fuel records.");
                UpdateStatus("Error loading fuel records.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Failed to load fuel records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddFuelButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Add Fuel button clicked.");

            var fuel = new Fuel
            {
                Bus_Number = int.TryParse(busNumberComboBox.Text, out int busNumber) ? busNumber : 0,
                Fuel_Gallons = int.TryParse(fuelGallonsTextBox.Text, out int gallons) ? gallons : 0,
                Fuel_Date = fuelDatePicker.Value.ToString("yyyy-MM-dd"),
                Fuel_Type = fuelTypeTextBox.Text,
                Odometer_Reading = int.TryParse(odometerTextBox.Text, out int odometer) ? odometer : -1
            };

            var (isValid, errors) = DataValidator.ValidateFuel(fuel, _busNumbers);
            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UpdateStatus("Adding fuel record...", AppSettings.Theme.InfoColor);
                _dbManager.AddFuelRecord(fuel);
                LoadFuelRecords();
                ClearInputs();
                MessageBox.Show("Fuel record added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to add fuel record.");
                UpdateStatus("Error adding fuel record.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Failed to add fuel record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Refresh button clicked.");
            LoadFuelRecords();
        }

        private void ClearInputs()
        {
            busNumberComboBox.SelectedIndex = -1;
            fuelGallonsTextBox.Clear();
            fuelDatePicker.Value = DateTime.Today;
            fuelTypeTextBox.Clear();
            odometerTextBox.Clear();
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }
    }
}