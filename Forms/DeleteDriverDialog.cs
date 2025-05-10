using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using BusBuddy.Services;
using BusBuddy.Models.Entities;

namespace BusBuddy.Forms
{
    /// <summary>
    /// Dialog for safely deleting a driver
    /// </summary>
    public class DeleteDriverDialog : Form
    {
        private readonly DriverService _driverService;
        private readonly ILogger<DeleteDriverDialog> _logger;
        private readonly int _driverId;
        private ComboBox cmbReassignDriver;
        private RadioButton radDelete, radReassign;
        private Button btnConfirm, btnCancel;
        private Label lblMessage, lblReassignTo;
        private Driver[] _availableDrivers;

        /// <summary>
        /// Initializes a new instance of the DeleteDriverDialog class
        /// </summary>
        /// <param name="driverService">The driver service</param>
        /// <param name="logger">The logger</param>
        /// <param name="driverToDelete">The driver to delete</param>
        public DeleteDriverDialog(DriverService driverService, ILogger<DeleteDriverDialog> logger, Driver driverToDelete)
        {
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _driverId = driverToDelete?.Id ?? throw new ArgumentNullException(nameof(driverToDelete));

            InitializeComponent();
            Text = $"Delete Driver: {driverToDelete.FullName}";
            lblMessage.Text = $"You are about to delete driver {driverToDelete.FullName}. " +
                "This may affect routes and trips assigned to this driver.";

            LoadAvailableDrivers();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            // Form properties
            ClientSize = new System.Drawing.Size(450, 300);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Main message label
            lblMessage = new Label
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(410, 40),
                AutoSize = false
            };

            // Option radio buttons
            radDelete = new RadioButton
            {
                Text = "Delete driver and set route references to null",
                Location = new System.Drawing.Point(30, 80),
                Size = new System.Drawing.Size(400, 20),
                Checked = true
            };
            radDelete.CheckedChanged += RadioOption_CheckedChanged;

            radReassign = new RadioButton
            {
                Text = "Delete driver and reassign routes to another driver",
                Location = new System.Drawing.Point(30, 110),
                Size = new System.Drawing.Size(400, 20)
            };
            radReassign.CheckedChanged += RadioOption_CheckedChanged;

            // Reassign components
            lblReassignTo = new Label
            {
                Text = "Reassign to:",
                Location = new System.Drawing.Point(50, 140),
                AutoSize = true,
                Enabled = false
            };

            cmbReassignDriver = new ComboBox
            {
                Location = new System.Drawing.Point(130, 140),
                Size = new System.Drawing.Size(270, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };

            // Buttons
            btnConfirm = new Button
            {
                Text = "Confirm Delete",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(220, 250),
                Size = new System.Drawing.Size(100, 30)
            };
            btnConfirm.Click += BtnConfirm_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(330, 250),
                Size = new System.Drawing.Size(100, 30)
            };

            // Add controls
            Controls.AddRange(new Control[] {
                lblMessage, radDelete, radReassign, lblReassignTo, cmbReassignDriver, btnConfirm, btnCancel
            });

            // Set cancel button
            CancelButton = btnCancel;

            ResumeLayout(false);
        }

        private async void LoadAvailableDrivers()
        {
            try
            {
                // Get all drivers except the one to delete
                var allDrivers = await _driverService.GetAllDriversAsync();
                _availableDrivers = allDrivers.Where(d => d.Id != _driverId).ToArray();
                
                // Populate ComboBox
                cmbReassignDriver.DisplayMember = "FullName";
                cmbReassignDriver.ValueMember = "Id";
                cmbReassignDriver.DataSource = _availableDrivers;
                
                // Enable confirm button only if we have other drivers for reassignment
                if (radReassign.Checked && _availableDrivers.Length == 0)
                {
                    btnConfirm.Enabled = false;
                    MessageBox.Show("No other drivers available for reassignment.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading available drivers");
                MessageBox.Show($"Error loading drivers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void RadioOption_CheckedChanged(object sender, EventArgs e)
        {
            var enableReassign = radReassign.Checked;
            lblReassignTo.Enabled = enableReassign;
            cmbReassignDriver.Enabled = enableReassign;
            
            // Disable confirm button if no drivers available for reassignment
            if (enableReassign && _availableDrivers.Length == 0)
            {
                btnConfirm.Enabled = false;
            }
            else
            {
                btnConfirm.Enabled = true;
            }
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                bool success;
                Cursor = Cursors.WaitCursor;
                btnConfirm.Enabled = false;
                btnCancel.Enabled = false;
                
                if (radReassign.Checked && cmbReassignDriver.SelectedItem is Driver selectedDriver)
                {
                    // Delete and reassign to selected driver
                    int reassignToId = selectedDriver.Id;
                    success = await _driverService.DeleteDriverAndReassignAsync(_driverId, reassignToId);
                    _logger.LogInformation("Deleted driver {DriverId} and reassigned routes to driver {ReassignDriverId}",
                        _driverId, reassignToId);
                }
                else
                {
                    // Just delete the driver
                    success = await _driverService.DeleteDriverAsync(_driverId);
                    _logger.LogInformation("Deleted driver {DriverId} with no reassignment", _driverId);
                }

                if (success)
                {
                    MessageBox.Show("Driver deleted successfully.", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Failed to delete driver.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Abort;
                }
                
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting driver {DriverId}", _driverId);
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                btnConfirm.Enabled = true;
                btnCancel.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}
