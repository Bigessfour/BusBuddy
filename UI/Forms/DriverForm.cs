// BusBuddy/UI/Forms/DriverForm.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class DriverForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;

        public DriverForm() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _dbManager = new DatabaseManager(Logger);
            LoadDrivers();
        }

        private void LoadDrivers()
        {
            try
            {
                UpdateStatus("Loading driver records...", AppSettings.Theme.InfoColor);
                var drivers = _dbManager.GetDrivers();
                dataGridView.DataSource = drivers;
                UpdateStatus("Driver records loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to load drivers: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading drivers.", AppSettings.Theme.ErrorColor);
                MessageBox.Show("Failed to load drivers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddDriverButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Add Driver button clicked.");

            // Map UI inputs to Driver model
            var driver = MapInputsToDriver();
            if (driver == null)
            {
                UpdateStatus("Invalid input data.", AppSettings.Theme.ErrorColor);
                return;
            }

            // Validate the driver data
            var (isValid, errors) = DataValidator.ValidateDriver(driver);
            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateStatus("Validation failed.", AppSettings.Theme.ErrorColor);
                return;
            }

            // Add the driver using DataManager
            bool success = await DataManager.AddRecordAsync(
                driver,
                _dbManager.AddDriver,
                Logger,
                UpdateStatus
            );

            if (success)
            {
                LoadDrivers();
                ClearInputs();
                MessageBox.Show("Driver added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to add driver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Driver MapInputsToDriver()
        {
            try
            {
                return new Driver
                {
                    Driver_Name = driverNameTextBox.Text?.Trim() ?? string.Empty,
                    Address = addressTextBox.Text?.Trim() ?? string.Empty,
                    City = cityTextBox.Text?.Trim() ?? string.Empty,
                    State = stateTextBox.Text?.Trim() ?? string.Empty,
                    Zip_Code = zipCodeTextBox.Text?.Trim() ?? string.Empty,
                    Phone_Number = phoneNumberTextBox.Text?.Trim() ?? string.Empty,
                    Email_Address = emailTextBox.Text?.Trim() ?? string.Empty,
                    Is_Stipend_Paid = isStipendPaidCheckBox.Checked ? "TRUE" : "FALSE",
                    DL_Type = dlTypeTextBox.Text?.Trim() ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to map inputs to Driver: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Refresh button clicked.");
            LoadDrivers();
        }

        private void ClearInputs()
        {
            driverNameTextBox.Clear();
            addressTextBox.Clear();
            cityTextBox.Clear();
            stateTextBox.Clear();
            zipCodeTextBox.Clear();
            phoneNumberTextBox.Clear();
            emailTextBox.Clear();
            isStipendPaidCheckBox.Checked = false;
            dlTypeTextBox.Clear();
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }
    }
}