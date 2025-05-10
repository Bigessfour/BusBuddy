using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using System.Linq;
using BusBuddy.Services;

namespace BusBuddy.Forms
{
    /// <summary>
    /// Form for managing drivers, including license expiration alerts
    /// </summary>
    public class DriverManagementForm : Form
    {
        private readonly DriverService _driverService;
        private readonly IDatabaseHelper _dbHelper;
        private readonly ILogger<DriverManagementForm> _logger;
        private BindingSource _bindingSource;
        private DataGridView _dgvDrivers;
        private Button _btnAdd;
        private Button _btnEdit;
        private Button _btnDelete;
        private Button _btnRefresh;
        private Panel _expirationAlertPanel;
        private System.Windows.Forms.Timer _refreshTimer;
        
        /// <summary>
        /// Initializes a new instance of the DriverManagementForm class.
        /// </summary>
        /// <param name="driverService">The driver service for managing driver data</param>
        /// <param name="dbHelper">The database helper for direct database operations</param>
        /// <param name="logger">The logger for tracking operations</param>
        public DriverManagementForm(DriverService driverService, IDatabaseHelper dbHelper, ILogger<DriverManagementForm> logger)
        {
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            InitializeComponent();
            LoadDriversAsync().ConfigureAwait(false);
            SetupRefreshTimer();
        }
        
        private void InitializeComponent()
        {
            Text = "Driver Management";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            
            // Create binding source for data grid
            _bindingSource = new BindingSource();
            
            // Set up the data grid view
            _dgvDrivers = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = _bindingSource
            };
            
            // Set up the buttons panel
            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            
            _btnAdd = new Button
            {
                Text = "Add Driver",
                Width = 100,
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White
            };
            _btnAdd.Click += BtnAdd_Click;
            
            _btnEdit = new Button
            {
                Text = "Edit Driver",
                Width = 100,
                Location = new Point(120, 10),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White
            };
            _btnEdit.Click += BtnEdit_Click;
            
            _btnDelete = new Button
            {
                Text = "Delete Driver",
                Width = 100,
                Location = new Point(230, 10),
                BackColor = Color.FromArgb(192, 0, 0),
                ForeColor = Color.White
            };
            _btnDelete.Click += BtnDelete_Click;
            
            _btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 100,
                Location = new Point(340, 10),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White
            };
            _btnRefresh.Click += BtnRefresh_Click;
            
            buttonsPanel.Controls.Add(_btnAdd);
            buttonsPanel.Controls.Add(_btnEdit);
            buttonsPanel.Controls.Add(_btnDelete);
            buttonsPanel.Controls.Add(_btnRefresh);
            
            // Set up license expiration alert panel
            _expirationAlertPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 0, // Initially hidden, will expand when there are alerts
                BackColor = Color.FromArgb(255, 235, 238),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };
            
            // Add controls to form
            Controls.Add(_dgvDrivers);
            Controls.Add(buttonsPanel);
            Controls.Add(_expirationAlertPanel);
            
            // Configure DataGridView columns after creation
            ConfigureDataGridColumns();
        }
        
        private void ConfigureDataGridColumns()
        {
            _dgvDrivers.AutoGenerateColumns = false;

            // Add columns to the DataGridView
            _dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "Id",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });

            _dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FullName",
                Name = "FullName",
                HeaderText = "Driver Name",
                Width = 200
            });

            _dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LicenseNumber",
                Name = "LicenseNumber",
                HeaderText = "License Number",
                Width = 120
            });

            _dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LicenseExpiration",
                Name = "LicenseExpiration",
                HeaderText = "License Expiration",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }
            });

            _dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PhoneNumber",
                Name = "PhoneNumber",
                HeaderText = "Phone",
                Width = 120
            });

            _dgvDrivers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                Name = "Email",
                HeaderText = "Email",
                Width = 200
            });
        }
        
        private void SetupRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 60000 // Check every minute
            };
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }
        
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadDriversAsync().ConfigureAwait(false);
        }
        
        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            ShowDriverForm(null); // null for new driver
        }

        private async void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_dgvDrivers.CurrentRow != null)
            {
                var selectedDriver = _dgvDrivers.CurrentRow.DataBoundItem as Driver;
                if (selectedDriver != null)
                {
                    ShowDriverForm(selectedDriver.Id);
                }
            }
        }
        
        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_dgvDrivers.CurrentRow != null)
            {
                var selectedDriver = _dgvDrivers.CurrentRow.DataBoundItem as Driver;
                if (selectedDriver != null)
                {
                    await DeleteDriverAsync(selectedDriver);
                }
            }
        }
        
        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDriversAsync();
        }
        
        /// <summary>
        /// Loads all drivers from the database and displays them in the grid
        /// </summary>
        public async Task LoadDriversAsync()
        {
            try
            {
                _logger.LogInformation("Loading drivers");
                var drivers = await _driverService.GetAllDriversAsync();
                
                // Apply the data to the binding source
                _bindingSource.DataSource = drivers;
                
                // Check for expiring licenses
                await DisplayLicenseExpirationAlertsAsync();
                
                _logger.LogInformation("Loaded {Count} drivers", drivers.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading drivers");
                MessageBox.Show($"Error loading drivers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Displays alerts for drivers with licenses expiring within 30 days
        /// </summary>
        public async Task DisplayLicenseExpirationAlertsAsync()
        {
            try
            {
                // Clear previous alerts
                _expirationAlertPanel.Controls.Clear();
                  // Get all drivers
                var allDrivers = await _driverService.GetAllDriversAsync();
                
                // Find drivers with licenses expiring in the next 30 days
                // Handle null or default dates by checking if LicenseExpiration is not default
                var expiringDrivers = allDrivers.Where(d => 
                    d.LicenseExpiration != default &&
                    d.LicenseExpiration > DateTime.Now && 
                    d.LicenseExpiration < DateTime.Now.AddDays(30)
                ).OrderBy(d => d.LicenseExpiration).ToList();
                
                // Update panel based on results
                if (expiringDrivers.Any())
                {
                    _logger.LogInformation("Found {Count} drivers with licenses expiring in the next 30 days", expiringDrivers.Count);
                    
                    // Set up the panel height based on number of alerts
                    _expirationAlertPanel.Height = Math.Min(150, expiringDrivers.Count * 50);
                    
                    // Add a label for each expiring license
                    int yPos = 5;
                    foreach (var driver in expiringDrivers)
                    {
                        int daysRemaining = (int)(driver.LicenseExpiration - DateTime.Now).TotalDays;
                        string message = $"ALERT: {driver.FullName}'s license expires in {daysRemaining} days (on {driver.LicenseExpiration:d})";
                        
                        // Create the alert label
                        var alertLabel = new Label
                        {
                            Text = message,
                            AutoSize = false,
                            Width = _expirationAlertPanel.Width - 20,
                            Height = 40,
                            Location = new Point(10, yPos),
                            BackColor = daysRemaining <= 7 ? Color.FromArgb(255, 200, 200) : Color.Transparent,
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            TextAlign = ContentAlignment.MiddleLeft
                        };
                        
                        _expirationAlertPanel.Controls.Add(alertLabel);
                        yPos += 45;
                    }
                }
                else
                {
                    // No expirations, hide the panel
                    _expirationAlertPanel.Height = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking license expirations");
            }
        }
        
        /// <summary>
        /// Shows the form for adding or editing a driver
        /// </summary>
        /// <param name="driverId">The ID of the driver to edit, or null for a new driver</param>
        private void ShowDriverForm(int? driverId)
        {
            // Create a driver editor form (to be implemented)
            using (var driverEditor = new DriverEditorDialog(_dbHelper, _logger, driverId))
            {
                if (driverEditor.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the data grid
                    LoadDriversAsync().ConfigureAwait(false);
                }
            }
        }
        
        /// <summary>
        /// Safely deletes a driver with confirming dialog
        /// </summary>
        /// <param name="driver">The driver to delete</param>
        private async Task<bool> DeleteDriverAsync(Driver driver)
        {
            if (driver == null)
                return false;

            // Show a message box to confirm deletion
            var result = MessageBox.Show(
                $"Are you sure you want to delete driver {driver.FullName}?" +
                "\n\nThis may affect routes and trips assigned to this driver.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Show the delete driver dialog to handle reassignment options
                    using (var deleteDialog = new DeleteDriverDialog(_driverService, _logger, driver))
                    {
                        // If the user confirmed the deletion and it was successful
                        if (deleteDialog.ShowDialog() == DialogResult.OK)
                        {
                            _logger.LogInformation("Driver {DriverName} (ID: {DriverId}) deleted successfully", 
                                driver.FullName, driver.Id);
                            
                            // Refresh the grid after successful deletion
                            await LoadDriversAsync();
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting driver {DriverName} (ID: {DriverId})", 
                        driver.FullName, driver.Id);
                    
                    MessageBox.Show(
                        $"Error deleting driver: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            
            return false;
        }

        /// <summary>
        /// Gets a list of drivers with licenses expiring within the specified days
        /// </summary>
        /// <param name="daysThreshold">Number of days to check for expiration</param>
        /// <returns>List of drivers with expiring licenses</returns>
        private async Task<IEnumerable<Driver>> GetDriversWithExpiringLicensesAsync(int daysThreshold)
        {
            try
            {
                var allDrivers = await _driverService.GetAllDriversAsync();
                var expiringDate = DateTime.Now.AddDays(daysThreshold);
                
                return allDrivers.Where(d => 
                    d.LicenseExpiration > DateTime.Now && 
                    d.LicenseExpiration <= expiringDate
                ).OrderBy(d => d.LicenseExpiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drivers with expiring licenses");
                return Enumerable.Empty<Driver>();
            }
        }
    }
}
