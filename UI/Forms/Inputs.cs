using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.Utilities;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public partial class Inputs : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;
        private List<string> _drivers;
        private List<int> _busNumbers;

        public Inputs() : base(new MainFormNavigator())
        {
            _logger = Log.Logger;
            _dbManager = new DatabaseManager(_logger);
            InitializeComponent();
            
            LoadComboBoxData();
            LoadAllDataGrids();

            // Subscribe to Load event to ensure UI updates occur after form is loaded
            this.Load += Inputs_Load;
        }

        private void Inputs_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void LoadComboBoxData()
        {
            try
            {
                UpdateStatus("Loading combo box data...", AppSettings.Theme.InfoColor);
                _drivers = _dbManager.GetDriverNames();
                _busNumbers = _dbManager.GetBusNumbers();

                // Trips tab
                tripDriverNameComboBox.Items.AddRange(_drivers.ToArray());
                tripBusNumberComboBox.Items.AddRange(_busNumbers.Select(n => n.ToString()).ToArray());

                // Fuel tab
                fuelBusNumberComboBox.Items.AddRange(_busNumbers.Select(n => n.ToString()).ToArray());

                // Activities tab
                activityBusNumberComboBox.Items.AddRange(_busNumbers.Select(n => n.ToString()).ToArray());
                activityDriverComboBox.Items.AddRange(_drivers.ToArray());

                UpdateStatus("Combo box data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading combo box data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading combo box data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading combo box data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllDataGrids()
        {
            LoadTripsDataGrid();
            LoadVehiclesDataGrid();
            LoadFuelDataGrid();
            LoadDriversDataGrid();
            LoadActivitiesDataGrid();
        }

        #region Trips Tab
        private void LoadTripsDataGrid()
        {
            try
            {
                UpdateStatus("Loading trips data...", AppSettings.Theme.InfoColor);
                var trips = _dbManager.GetTrips();
                tripsDataGridView.Rows.Clear();
                foreach (var trip in trips)
                {
                    tripsDataGridView.Rows.Add(trip.TripID, trip.TripType, trip.Date, trip.BusNumber.ToString(), trip.DriverName, trip.StartTime, trip.EndTime, trip.Destination);
                }
                UpdateStatus("Trips data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading trips data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading trips data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading trips data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void TripAddButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Trip Add button clicked.");

            var trip = new Trip
            {
                TripType = tripTypeComboBox.Text,
                Date = DateOnly.FromDateTime(tripDatePicker.Value),
                BusNumber = int.TryParse(tripBusNumberComboBox.Text, out int busNumber) ? busNumber : 0,
                DriverName = tripDriverNameComboBox.Text,
                StartTime = TimeOnly.FromDateTime(tripStartTimePicker.Value),
                EndTime = TimeOnly.FromDateTime(tripEndTimePicker.Value),
                Destination = tripDestinationTextBox.Text
            };

            var (isValid, errors) = DataValidator.ValidateTrip(trip, _drivers, _busNumbers);
            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate total hours driven
            TimeSpan totalHours = trip.EndTime.ToTimeSpan() - trip.StartTime.ToTimeSpan();
            if (totalHours.TotalHours < 0)
            {
                totalHours = totalHours.Add(TimeSpan.FromDays(1));
            }
            trip.TotalHoursDriven = totalHours.TotalHours;
            trip.Total_Hours_Driven = totalHours.ToString(@"hh\:mm");

            try
            {
                UpdateStatus("Adding trip...", AppSettings.Theme.InfoColor);
                await DataManager.AddRecordAsync(trip, _dbManager.AddTrip, _logger, UpdateStatus);
                LoadTripsDataGrid();
                ClearTripInputs();
                MessageBox.Show("Trip added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding trip: {ErrorMessage}", ex.Message);
                UpdateStatus("Error adding trip.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error adding trip: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TripClearButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Trip Clear button clicked.");
            ClearTripInputs();
        }

        private void ClearTripInputs()
        {
            tripTypeComboBox.SelectedIndex = -1;
            tripDatePicker.Value = DateTime.Now;
            tripBusNumberComboBox.SelectedIndex = -1;
            tripDriverNameComboBox.SelectedIndex = -1;
            tripStartTimePicker.Value = DateTime.Now;
            tripEndTimePicker.Value = DateTime.Now;
            tripDestinationTextBox.Clear();
            UpdateStatus("Trip inputs cleared.", AppSettings.Theme.InfoColor);
        }
        #endregion

        #region Vehicles Tab
        private void LoadVehiclesDataGrid()
        {
            try
            {
                UpdateStatus("Loading vehicles data...", AppSettings.Theme.InfoColor);
                var busNumbers = _dbManager.GetBusNumbers();
                vehiclesDataGridView.Rows.Clear();
                foreach (var busNumber in busNumbers)
                {
                    vehiclesDataGridView.Rows.Add(busNumber);
                }
                UpdateStatus("Vehicles data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading vehicles data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading vehicles data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading vehicles data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void VehicleAddButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Vehicle Add button clicked.");

            if (!int.TryParse(vehicleBusNumberTextBox.Text, out int busNumber) || busNumber <= 0)
            {
                MessageBox.Show("Please enter a valid Bus Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_busNumbers.Contains(busNumber))
            {
                MessageBox.Show("This Bus Number already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UpdateStatus("Adding vehicle...", AppSettings.Theme.InfoColor);
                using (var connection = new System.Data.SQLite.SQLiteConnection($"Data Source=WileySchool.db;Version=3;"))
                {
                    connection.Open();
                    using (var command = new System.Data.SQLite.SQLiteCommand("INSERT INTO Vehicles (\"Bus Number\") VALUES (@BusNumber)", connection))
                    {
                        command.Parameters.AddWithValue("@BusNumber", busNumber);
                        command.ExecuteNonQuery();
                    }
                }
                _logger.Information("Vehicle added: Bus Number {BusNumber}", busNumber);
                LoadVehiclesDataGrid();
                LoadComboBoxData(); // Refresh combo boxes
                ClearVehicleInputs();
                MessageBox.Show("Vehicle added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding vehicle: {ErrorMessage}", ex.Message);
                UpdateStatus("Error adding vehicle.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error adding vehicle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VehicleClearButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Vehicle Clear button clicked.");
            ClearVehicleInputs();
        }

        private void ClearVehicleInputs()
        {
            vehicleBusNumberTextBox.Clear();
            UpdateStatus("Vehicle inputs cleared.", AppSettings.Theme.InfoColor);
        }
        #endregion

        #region Fuel Tab
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
        #endregion

        #region Drivers Tab
        private void LoadDriversDataGrid()
        {
            try
            {
                UpdateStatus("Loading drivers data...", AppSettings.Theme.InfoColor);
                var drivers = _dbManager.GetDrivers();
                driversDataGridView.Rows.Clear();
                foreach (var driver in drivers)
                {
                    driversDataGridView.Rows.Add(driver.DriverID, driver.Driver_Name, driver.Address, driver.City, driver.State, driver.Zip_Code, driver.Phone_Number, driver.Email_Address, driver.Is_Stipend_Paid, driver.DL_Type);
                }
                UpdateStatus("Drivers data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading drivers data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading drivers data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading drivers data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                LoadComboBoxData(); // Refresh combo boxes
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
        #endregion

        #region Activities Tab
        private void LoadActivitiesDataGrid()
        {
            try
            {
                UpdateStatus("Loading activities data...", AppSettings.Theme.InfoColor);
                var activities = _dbManager.GetActivities();
                maintenanceDataGridView.Rows.Clear();
                foreach (var activity in activities)
                {
                    maintenanceDataGridView.Rows.Add(activity.ActivityID, activity.Date, activity.BusNumber, activity.Destination, activity.LeaveTime, activity.Driver, activity.HoursDriven, activity.StudentsDriven);
                }
                UpdateStatus("Activities data loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading activities data: {ErrorMessage}", ex.Message);
                UpdateStatus("Error loading activities data.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading activities data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MaintenanceAddButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Activity Add button clicked.");

            // Calculate hours driven from time picker
            TimeSpan hoursDriven = TimeSpan.FromHours((double)activityHoursNumericUpDown.Value) + 
                                  TimeSpan.FromMinutes((double)activityMinutesNumericUpDown.Value);
            
            var activity = new ActivityTrip
            {
                Date = activityDatePicker.Value.ToString("yyyy-MM-dd"),
                BusNumber = int.TryParse(activityBusNumberComboBox.Text, out int busNumber) ? busNumber : 0,
                Destination = activityDestinationTextBox.Text,
                LeaveTime = activityLeaveTimePicker.Value.ToString("HH:mm"),
                Driver = activityDriverComboBox.Text,
                HoursDriven = $"{(int)hoursDriven.TotalHours}:{hoursDriven.Minutes:D2}",
                StudentsDriven = (int)activityStudentsNumericUpDown.Value
            };

            var (isValid, errors) = DataValidator.ValidateActivity(activity, _drivers, _busNumbers);
            if (!isValid)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UpdateStatus("Adding activity...", AppSettings.Theme.InfoColor);
                await DataManager.AddActivityRecordAsync(activity, _dbManager, _logger, UpdateStatus);
                LoadActivitiesDataGrid();
                ClearActivityInputs();
                MessageBox.Show("Activity added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding activity: {ErrorMessage}", ex.Message);
                UpdateStatus("Error adding activity.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error adding activity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MaintenanceClearButton_Click(object sender, EventArgs e)
        {
            _logger.Information("Activity Clear button clicked.");
            ClearActivityInputs();
        }

        private void ClearActivityInputs()
        {
            activityDatePicker.Value = DateTime.Now;
            activityBusNumberComboBox.SelectedIndex = -1;
            activityDestinationTextBox.Clear();
            activityLeaveTimePicker.Value = DateTime.Now;
            activityDriverComboBox.SelectedIndex = -1;
            activityHoursNumericUpDown.Value = 0;
            activityMinutesNumericUpDown.Value = 0;
            activityStudentsNumericUpDown.Value = 0;
            UpdateStatus("Activity inputs cleared.", AppSettings.Theme.InfoColor);
        }
        #endregion
    }
}