using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;

namespace BusBuddy.Forms
{
    public class VehiclesManagementForm : Form
    {
        private IDatabaseHelper _dbHelper;
        private BindingSource bindingSource;
        public DataGridView DataGridView { get; private set; }
        private readonly ILogger<VehiclesManagementForm> _logger;

        public VehiclesManagementForm(IDatabaseHelper dbHelper, ILogger<VehiclesManagementForm> logger)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            InitializeComponent();
            LoadVehicles();
        }
        
        public VehiclesManagementForm(ILogger<VehiclesManagementForm> logger)
            : this(null, logger)
        {
            // This constructor is for test purposes only
            try
            {
                InitializeComponent();
            }
            catch (ArgumentNullException)
            {
                // Swallow exception for tests
                _logger.LogWarning("Test constructor called without dbHelper");
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Vehicles Management";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            
            DataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true
            };
            
            this.Controls.Add(DataGridView);
            
            if (_dbHelper != null)
            {
                bindingSource = new BindingSource();
                DataGridView.DataSource = bindingSource;
            }
        }

        public async void LoadVehicles()
        {
            if (_dbHelper == null) return;
            
            try
            {
                _logger.LogInformation("Loading vehicles");
                var vehicles = await _dbHelper.GetVehiclesAsync();
                bindingSource.DataSource = vehicles;
                _logger.LogInformation($"Loaded {((IList<Vehicle>)vehicles).Count} vehicles");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading vehicles");
                MessageBox.Show($"Error loading vehicles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public bool ValidateVehicleInsurance(int vehicleId)
        {
            if (_dbHelper == null) return true;
            
            try
            {
                var vehicle = _dbHelper.GetVehicle(vehicleId);
                if (vehicle.InsuranceExpiration < DateTime.Now)
                {
                    MessageBox.Show("Vehicle insurance has expired!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error validating vehicle insurance for ID {vehicleId}");
                return false;
            }
        }
        
        public void AddVehicle(string name, string year)
        {
            if (_dbHelper == null) return;
            
            try
            {                var vehicle = new Vehicle
                {
                    VehicleNumber = name,
                    Make = "Default Make",
                    Model = "Default Model",
                    Year = Convert.ToInt32(year),
                    InsuranceExpiration = DateTime.Now.AddYears(1)
                };
                
                _dbHelper.AddVehicle(vehicle);
                LoadVehicles();
                _logger.LogInformation($"Added vehicle: {name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vehicle");
                MessageBox.Show($"Error adding vehicle: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
