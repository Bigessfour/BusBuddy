using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.Utilities;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public partial class FuelForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;
        private List<int> _busNumbers;

        public FuelForm() : base(new MainFormNavigator())
        {
            _logger = Log.Logger;
            _dbManager = new DatabaseManager(_logger);
            InitializeComponent();
            LoadComboBoxData();
            LoadFuelDataGrid();

            // Subscribe to Load event to ensure UI updates occur after form is loaded
            this.Load += FuelForm_Load;
        }

        private void FuelForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void LoadComboBoxData()
        {
            try
            {
                UpdateStatus("Loading combo box data...", AppSettings.Theme.InfoColor);
                _busNumbers = _dbManager.GetBusNumbers();
                fuelBusNumberComboBox.Items.AddRange(_busNumbers.Select(n => n.ToString()).ToArray());
                UpdateStatus("Combo box data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading combo box data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading combo box data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading combo box data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFuelDataGrid()
        {
            try
            {
                UpdateStatus("Loading fuel data...", AppSettings.Theme.InfoColor);
                var fuelRecords = _dbManager.GetFuelRecords();
                fuelDataGridView.Rows.Clear();
                foreach (var fuel in fuelRecords)
                {
                    fuelDataGridView.Rows.Add(fuel.Fuel_ID, fuel.Bus_Number, fuel.Fuel_Gallons, fuel.Fuel_Date, fuel.Fuel_Type, fuel.Odometer_Reading);
                }
                UpdateStatus("Fuel data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading fuel data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading fuel data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading fuel data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FuelAddButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Fuel Add button clicked.");

            var fuel = new Fuel
            {
                Bus_Number = int.TryParse(fuelBusNumberComboBox.Text, out int busNumber) ? busNumber : 0,
                Fuel_Gallons = (int)fuelGallonsNumericUpDown.Value,
                Fuel_Date = fuelDatePicker.Value.ToString("yyyy-MM-dd"),
                Fuel_Type = fuelTypeComboBox.Text,
                Odometer_Reading = (int)fuelOdometerNumericUpDown.Value
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
                // Use the new helper method instead
                await DataManager.AddFuelRecordAsync(fuel, _dbManager, _logger, UpdateStatus);
                LoadFuelDataGrid();
                ClearFuelInputs();
                MessageBox.Show("Fuel record added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record: {ErrorMessage}", ex.Message);
                UpdateStatus("Error adding fuel record.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error adding fuel record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FuelClearButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Fuel Clear button clicked.");
            ClearFuelInputs();
        }

        private void ClearFuelInputs()
        {
            fuelBusNumberComboBox.SelectedIndex = -1;
            fuelGallonsNumericUpDown.Value = 0;
            fuelDatePicker.Value = DateTime.Now;
            fuelTypeComboBox.SelectedIndex = -1;
            fuelOdometerNumericUpDown.Value = 0;
            UpdateStatus("Fuel inputs cleared.", AppSettings.Theme.InfoColor);
        }
    }
}