using System;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.Utilities;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public partial class DriverForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;

        public DriverForm() : base(new MainFormNavigator())
        {
            _logger = Log.Logger;
            _dbManager = new DatabaseManager(_logger);
            InitializeComponent();
            LoadDriversDataGrid();

            // Subscribe to Load event to ensure UI updates occur after form is loaded
            this.Load += DriverForm_Load;
        }

        private void DriverForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void LoadDriversDataGrid()
        {
            try
            {
                UpdateStatus("Loading drivers...", AppSettings.Theme.InfoColor);
                var drivers = _dbManager.GetDrivers();
                driversDataGridView.Rows.Clear();
                foreach (var driver in drivers)
                {
                    driversDataGridView.Rows.Add(driver.DriverID, driver.Driver_Name, driver.Address, driver.City, driver.State, driver.Zip_Code, driver.Phone_Number, driver.Email_Address, driver.Is_Stipend_Paid, driver.DL_Type);
                }
                UpdateStatus("Drivers loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading drivers: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading drivers.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading drivers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DriverAddButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Driver Add button clicked.");
            var driver = new BusBuddy.Models.Driver
            {
                Driver_Name = driverNameTextBox.Text,
                Address = driverAddressTextBox.Text,
                City = driverCityTextBox.Text,
                State = driverStateTextBox.Text,
                Zip_Code = driverZipTextBox.Text,
                Phone_Number = driverPhoneTextBox.Text,
                Email_Address = driverEmailTextBox.Text,
                Is_Stipend_Paid = driverStipendComboBox.Text,
                DL_Type = driverDLTypeComboBox.Text
            };

            var (isValid, errors) = DataValidator.ValidateDriver(driver);
            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UpdateStatus("Adding driver...", AppSettings.Theme.InfoColor);
                await DataManager.AddDriverAsync(driver, _dbManager, _logger, UpdateStatus);
                LoadDriversDataGrid();
                ClearDriverInputs();
                MessageBox.Show("Driver added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding driver: {ErrorMessage}", ex.Message);
                UpdateStatus("Error adding driver.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error adding driver: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DriverClearButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Driver Clear button clicked.");
            ClearDriverInputs();
        }

        private void ClearDriverInputs()
        {
            driverNameTextBox.Clear();
            driverAddressTextBox.Clear();
            driverCityTextBox.Clear();
            driverStateTextBox.Clear();
            driverZipTextBox.Clear();
            driverPhoneTextBox.Clear();
            driverEmailTextBox.Clear();
            driverStipendComboBox.SelectedIndex = -1;
            driverDLTypeComboBox.SelectedIndex = -1;
            UpdateStatus("Driver inputs cleared.", AppSettings.Theme.InfoColor);
        }
    }
}