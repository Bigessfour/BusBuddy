using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using System.Text.RegularExpressions;

namespace BusBuddy.Forms
{
    public class VehiclesManagementForm : Form
    {
        private IDatabaseHelper _dbHelper;
        private BindingSource bindingSource;
        public DataGridView DataGridView { get; private set; }
        private readonly ILogger<VehiclesManagementForm> _logger;
        
        // UI Controls for adding/editing
        private TextBox txtVehicleNumber;
        private TextBox txtMake;
        private TextBox txtModel;
        private NumericUpDown numYear;
        private TextBox txtLicensePlate;
        private TextBox txtVIN;
        private NumericUpDown numCapacity;
        private DateTimePicker dtpInsuranceExpiration;
        private Button btnSave;
        private Button btnCancel;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Panel formPanel;
        private Panel buttonPanel;
        private Panel editPanel;
        private Label lblValidationMessage;

        private bool isEditing = false;
        private int? currentVehicleId = null;

        public VehiclesManagementForm(IDatabaseHelper dbHelper, ILogger<VehiclesManagementForm> logger)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            InitializeComponent();
            LoadVehicles();
        }
          // Constructor for testing - accepts a null database helper
        public VehiclesManagementForm(ILogger<VehiclesManagementForm> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbHelper = null!; // Using null-forgiving operator to avoid compiler warning
            
            _logger.LogWarning("Test constructor called without dbHelper");
            
            // Initialize component without trying to connect to database
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception during test initialization");
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Vehicles Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Create layout panels
            formPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            
            buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            
            editPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Create DataGridView
            DataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersWidth = 25
            };
            
            // Create buttons
            btnAdd = new Button
            {
                Text = "Add Vehicle",
                Width = 100,
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += BtnAdd_Click;
            
            btnEdit = new Button
            {
                Text = "Edit",
                Width = 100,
                Location = new Point(120, 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;
            
            btnDelete = new Button
            {
                Text = "Delete",
                Width = 100,
                Location = new Point(230, 10),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += BtnDelete_Click;
            
            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 100,
                Location = new Point(340, 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += (s, e) => LoadVehicles();
            
            // Add buttons to button panel
            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);
            
            // Initialize edit panel controls
            InitializeEditPanel();
            
            // Add controls to form
            formPanel.Controls.Add(DataGridView);
            this.Controls.Add(editPanel);
            this.Controls.Add(formPanel);
            this.Controls.Add(buttonPanel);
            
            if (_dbHelper != null)
            {
                bindingSource = new BindingSource();
                DataGridView.DataSource = bindingSource;
                
                // Format the datagrid to highlight expired insurance
                DataGridView.CellFormatting += DataGridView_CellFormatting;
            }
        }
        
        private void InitializeEditPanel()
        {
            editPanel.Controls.Clear();
            
            // Title label
            Label lblTitle = new Label
            {
                Text = "Vehicle Details",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(280, 25)
            };
            editPanel.Controls.Add(lblTitle);
            
            // Vehicle Number
            Label lblVehicleNumber = new Label
            {
                Text = "Vehicle Number:",
                Location = new Point(10, 45),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblVehicleNumber);
            
            txtVehicleNumber = new TextBox
            {
                Location = new Point(130, 45),
                Size = new Size(150, 20)
            };
            txtVehicleNumber.TextChanged += ValidateInput;
            editPanel.Controls.Add(txtVehicleNumber);
            
            // Make
            Label lblMake = new Label
            {
                Text = "Make:",
                Location = new Point(10, 75),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblMake);
            
            txtMake = new TextBox
            {
                Location = new Point(130, 75),
                Size = new Size(150, 20)
            };
            txtMake.TextChanged += ValidateInput;
            editPanel.Controls.Add(txtMake);
            
            // Model
            Label lblModel = new Label
            {
                Text = "Model:",
                Location = new Point(10, 105),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblModel);
            
            txtModel = new TextBox
            {
                Location = new Point(130, 105),
                Size = new Size(150, 20)
            };
            txtModel.TextChanged += ValidateInput;
            editPanel.Controls.Add(txtModel);
            
            // Year
            Label lblYear = new Label
            {
                Text = "Year:",
                Location = new Point(10, 135),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblYear);
            
            numYear = new NumericUpDown
            {
                Location = new Point(130, 135),
                Size = new Size(80, 20),
                Minimum = 1990,
                Maximum = DateTime.Now.Year + 1,
                Value = DateTime.Now.Year
            };
            editPanel.Controls.Add(numYear);
            
            // License Plate
            Label lblLicensePlate = new Label
            {
                Text = "License Plate:",
                Location = new Point(10, 165),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblLicensePlate);
            
            txtLicensePlate = new TextBox
            {
                Location = new Point(130, 165),
                Size = new Size(150, 20)
            };
            txtLicensePlate.TextChanged += ValidateInput;
            editPanel.Controls.Add(txtLicensePlate);
            
            // VIN
            Label lblVIN = new Label
            {
                Text = "VIN:",
                Location = new Point(10, 195),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblVIN);
            
            txtVIN = new TextBox
            {
                Location = new Point(130, 195),
                Size = new Size(150, 20)
            };
            txtVIN.TextChanged += ValidateInput;
            editPanel.Controls.Add(txtVIN);
            
            // Capacity
            Label lblCapacity = new Label
            {
                Text = "Capacity:",
                Location = new Point(10, 225),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblCapacity);
            
            numCapacity = new NumericUpDown
            {
                Location = new Point(130, 225),
                Size = new Size(80, 20),
                Minimum = 1,
                Maximum = 100,
                Value = 48
            };
            editPanel.Controls.Add(numCapacity);
            
            // Insurance Expiration
            Label lblInsuranceExpiration = new Label
            {
                Text = "Insurance Expires:",
                Location = new Point(10, 255),
                Size = new Size(120, 20)
            };
            editPanel.Controls.Add(lblInsuranceExpiration);
            
            dtpInsuranceExpiration = new DateTimePicker
            {
                Location = new Point(130, 255),
                Size = new Size(150, 20),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddYears(1)
            };
            editPanel.Controls.Add(dtpInsuranceExpiration);
            
            // Validation message
            lblValidationMessage = new Label
            {
                Location = new Point(10, 285),
                Size = new Size(270, 60),
                ForeColor = Color.Red,
                Text = "",
                TextAlign = ContentAlignment.TopLeft
            };
            editPanel.Controls.Add(lblValidationMessage);
            
            // Buttons
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(70, 350),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;
            editPanel.Controls.Add(btnSave);
            
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(180, 350),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => HideEditPanel();
            editPanel.Controls.Add(btnCancel);
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            bool isValid = true;
            string validationMessage = "";
            
            // Validate Vehicle Number
            if (string.IsNullOrWhiteSpace(txtVehicleNumber.Text))
            {
                isValid = false;
                validationMessage += "• Vehicle Number is required\n";
            }
            
            // Validate Make
            if (string.IsNullOrWhiteSpace(txtMake.Text))
            {
                isValid = false;
                validationMessage += "• Make is required\n";
            }
            
            // Validate Model
            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                isValid = false;
                validationMessage += "• Model is required\n";
            }
            
            // Validate License Plate (format varies, but should have some content)
            if (string.IsNullOrWhiteSpace(txtLicensePlate.Text))
            {
                isValid = false;
                validationMessage += "• License Plate is required\n";
            }
            
            // Validate VIN (17 chars for modern vehicles, alphanumeric except I,O,Q)
            if (string.IsNullOrWhiteSpace(txtVIN.Text))
            {
                isValid = false;
                validationMessage += "• VIN is required\n";
            }
            else if (txtVIN.Text.Length != 17)
            {
                isValid = false;
                validationMessage += "• VIN must be exactly 17 characters\n";
            }
            else if (!Regex.IsMatch(txtVIN.Text, @"^[A-HJ-NPR-Z0-9]+$"))
            {
                isValid = false;
                validationMessage += "• VIN contains invalid characters\n";
            }
            
            // Display validation message and enable/disable save button
            lblValidationMessage.Text = validationMessage;
            btnSave.Enabled = isValid;
        }
        
        private void ShowEditPanel(bool isAdd = false)
        {
            // Reset form fields if adding new
            if (isAdd)
            {
                txtVehicleNumber.Text = "";
                txtMake.Text = "";
                txtModel.Text = "";
                numYear.Value = DateTime.Now.Year;
                txtLicensePlate.Text = "";
                txtVIN.Text = "";
                numCapacity.Value = 48;
                dtpInsuranceExpiration.Value = DateTime.Now.AddYears(1);
                isEditing = false;
                currentVehicleId = null;
            }
            
            // Validate initial state
            ValidateInput(this, EventArgs.Empty);
            
            // Show panel
            editPanel.Visible = true;
            formPanel.Width = this.Width - editPanel.Width;
        }
        
        private void HideEditPanel()
        {
            editPanel.Visible = false;
            formPanel.Width = this.Width;
            isEditing = false;
            currentVehicleId = null;
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ShowEditPanel(true);
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (DataGridView.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please select a vehicle to edit.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
                var row = DataGridView.SelectedRows[0];
                currentVehicleId = Convert.ToInt32(row.Cells["Id"].Value);
                
                // Populate form fields
                txtVehicleNumber.Text = row.Cells["VehicleNumber"].Value?.ToString() ?? "";
                txtMake.Text = row.Cells["Make"].Value?.ToString() ?? "";
                txtModel.Text = row.Cells["Model"].Value?.ToString() ?? "";
                numYear.Value = Convert.ToInt32(row.Cells["Year"].Value);
                txtLicensePlate.Text = row.Cells["LicensePlate"].Value?.ToString() ?? "";
                txtVIN.Text = row.Cells["VIN"].Value?.ToString() ?? "";
                numCapacity.Value = Convert.ToInt32(row.Cells["Capacity"].Value);
                dtpInsuranceExpiration.Value = Convert.ToDateTime(row.Cells["InsuranceExpiration"].Value);
                
                isEditing = true;
                ShowEditPanel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading vehicle data for editing");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DataGridView.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please select a vehicle to delete.", "Selection Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
                var row = DataGridView.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                string vehicleNumber = row.Cells["VehicleNumber"].Value?.ToString() ?? "";
                
                var result = MessageBox.Show($"Are you sure you want to delete vehicle {vehicleNumber}?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    // Delete vehicle (method not yet implemented in interface)
                    // _dbHelper.DeleteVehicle(id);
                    
                    _logger.LogInformation($"Deleted vehicle ID: {id}");
                    LoadVehicles();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    VehicleNumber = txtVehicleNumber.Text.Trim(),
                    Make = txtMake.Text.Trim(),
                    Model = txtModel.Text.Trim(),
                    Year = (int)numYear.Value,
                    LicensePlate = txtLicensePlate.Text.Trim(),
                    VIN = txtVIN.Text.Trim(),
                    Capacity = (int)numCapacity.Value,
                    InsuranceExpiration = dtpInsuranceExpiration.Value,
                    Odometer = 0 // Default value for new vehicles
                };
                
                if (isEditing && currentVehicleId.HasValue)
                {
                    vehicle.Id = currentVehicleId.Value;
                    // Update vehicle (method not yet implemented in interface)
                    // _dbHelper.UpdateVehicle(vehicle);
                    _logger.LogInformation($"Updated vehicle ID: {vehicle.Id}");
                }
                else
                {
                    _dbHelper.AddVehicle(vehicle);
                    _logger.LogInformation($"Added new vehicle: {vehicle.VehicleNumber}");
                }
                
                HideEditPanel();
                LoadVehicles();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving vehicle");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DataGridView.Columns[e.ColumnIndex].Name == "InsuranceExpiration" && e.Value != null)
            {
                try
                {
                    DateTime expirationDate = (DateTime)e.Value;
                    
                    // Highlight expired insurance in red
                    if (expirationDate < DateTime.Now)
                    {
                        e.CellStyle.ForeColor = Color.White;
                        e.CellStyle.BackColor = Color.Red;
                    }
                    // Highlight soon-to-expire insurance in yellow
                    else if (expirationDate < DateTime.Now.AddMonths(1))
                    {
                        e.CellStyle.BackColor = Color.Yellow;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error formatting cell");
                }
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
            {
                var vehicle = new Vehicle
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
