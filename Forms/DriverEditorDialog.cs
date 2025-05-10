using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusBuddy.Forms
{
    /// <summary>
    /// Dialog for adding or editing driver information
    /// </summary>
    public class DriverEditorDialog : Form
    {
        private readonly IDatabaseHelper _dbHelper;
        private readonly ILogger<DriverEditorDialog> _logger;
        private readonly int? _driverId;
        
        // UI controls
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtLicenseNumber;
        private DateTimePicker dtpLicenseExpiration;
        private TextBox txtPhoneNumber;
        private TextBox txtEmail;
        private Button btnSave;
        private Button btnCancel;
        private Label lblValidationMessage;
        
        // Track whether this is an edit operation
        private bool _isEditing;
        
        /// <summary>
        /// Initializes a new instance of the DriverEditorDialog
        /// </summary>
        /// <param name="dbHelper">The database helper for database operations</param>
        /// <param name="logger">The logger for tracking operations</param>
        /// <param name="driverId">The ID of the driver to edit, or null for a new driver</param>
        public DriverEditorDialog(IDatabaseHelper dbHelper, ILogger<DriverEditorDialog> logger, int? driverId)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _driverId = driverId;
            _isEditing = driverId.HasValue;
            
            InitializeComponent();
            Text = _isEditing ? "Edit Driver" : "Add New Driver";
            
            if (_isEditing)
            {
                LoadDriverDataAsync().ConfigureAwait(false);
            }
        }
        
        private void InitializeComponent()
        {
            // Form properties
            Size = new Size(500, 400);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            
            // Form title
            var lblTitle = new Label
            {
                Text = "Driver Information",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(300, 30)
            };
            Controls.Add(lblTitle);
            
            // First Name
            var lblFirstName = new Label
            {
                Text = "First Name:",
                Location = new Point(20, 60),
                Size = new Size(100, 20)
            };
            Controls.Add(lblFirstName);
            
            txtFirstName = new TextBox
            {
                Location = new Point(130, 60),
                Size = new Size(330, 20)
            };
            txtFirstName.TextChanged += ValidateInput;
            Controls.Add(txtFirstName);
            
            // Last Name
            var lblLastName = new Label
            {
                Text = "Last Name:",
                Location = new Point(20, 90),
                Size = new Size(100, 20)
            };
            Controls.Add(lblLastName);
            
            txtLastName = new TextBox
            {
                Location = new Point(130, 90),
                Size = new Size(330, 20)
            };
            txtLastName.TextChanged += ValidateInput;
            Controls.Add(txtLastName);
            
            // License Number
            var lblLicenseNumber = new Label
            {
                Text = "License Number:",
                Location = new Point(20, 120),
                Size = new Size(100, 20)
            };
            Controls.Add(lblLicenseNumber);
            
            txtLicenseNumber = new TextBox
            {
                Location = new Point(130, 120),
                Size = new Size(330, 20)
            };
            txtLicenseNumber.TextChanged += ValidateInput;
            Controls.Add(txtLicenseNumber);
            
            // License Expiration
            var lblLicenseExpiration = new Label
            {
                Text = "License Expires:",
                Location = new Point(20, 150),
                Size = new Size(100, 20)
            };
            Controls.Add(lblLicenseExpiration);
            
            dtpLicenseExpiration = new DateTimePicker
            {
                Location = new Point(130, 150),
                Size = new Size(200, 20),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddYears(1)
            };
            dtpLicenseExpiration.ValueChanged += ValidateInput;
            Controls.Add(dtpLicenseExpiration);
            
            // Phone Number
            var lblPhoneNumber = new Label
            {
                Text = "Phone Number:",
                Location = new Point(20, 180),
                Size = new Size(100, 20)
            };
            Controls.Add(lblPhoneNumber);
            
            txtPhoneNumber = new TextBox
            {
                Location = new Point(130, 180),
                Size = new Size(200, 20)
            };
            txtPhoneNumber.TextChanged += ValidateInput;
            Controls.Add(txtPhoneNumber);
            
            // Email
            var lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(20, 210),
                Size = new Size(100, 20)
            };
            Controls.Add(lblEmail);
            
            txtEmail = new TextBox
            {
                Location = new Point(130, 210),
                Size = new Size(330, 20)
            };
            txtEmail.TextChanged += ValidateInput;
            Controls.Add(txtEmail);
            
            // Validation message
            lblValidationMessage = new Label
            {
                Location = new Point(20, 240),
                Size = new Size(440, 60),
                ForeColor = Color.Red
            };
            Controls.Add(lblValidationMessage);
            
            // Save Button
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(260, 310),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White
            };
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);
            
            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(370, 310),
                Size = new Size(100, 30)
            };
            Controls.Add(btnCancel);
            
            // Set the cancel button
            CancelButton = btnCancel;
            
            // Do initial validation
            ValidateInput(this, EventArgs.Empty);
        }
        
        private async Task LoadDriverDataAsync()
        {
            try
            {                if (!_driverId.HasValue)
                    return;
                
                var driver = await _dbHelper.GetDriverByIdAsync(_driverId.Value);
                if (driver != null)
                {
                    txtFirstName.Text = driver.FirstName;
                    txtLastName.Text = driver.LastName;
                    txtLicenseNumber.Text = driver.LicenseNumber;
                    dtpLicenseExpiration.Value = driver.LicenseExpiration;
                    txtPhoneNumber.Text = driver.PhoneNumber;
                    txtEmail.Text = driver.Email;
                }
                else
                {
                    MessageBox.Show($"Could not find driver with ID {_driverId.Value}.", 
                        "Driver Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading driver with ID {DriverId}", _driverId);
                MessageBox.Show($"Error loading driver: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        
        private void ValidateInput(object sender, EventArgs e)
        {
            bool isValid = true;
            string errorMessage = "";
            
            // Check required fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                isValid = false;
                errorMessage += "• First name is required\n";
            }
            
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                isValid = false;
                errorMessage += "• Last name is required\n";
            }
            
            if (string.IsNullOrWhiteSpace(txtLicenseNumber.Text))
            {
                isValid = false;
                errorMessage += "• License number is required\n";
            }
            
            // Check license expiration date
            if (dtpLicenseExpiration.Value < DateTime.Today)
            {
                isValid = false;
                errorMessage += "• License has already expired\n";
            }
            
            if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                isValid = false;
                errorMessage += "• Phone number is required\n";
            }
            else if (!IsValidPhoneNumber(txtPhoneNumber.Text))
            {
                isValid = false;
                errorMessage += "• Invalid phone number format\n";
            }
            
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                isValid = false;
                errorMessage += "• Email is required\n";
            }
            else if (!IsValidEmail(txtEmail.Text))
            {
                isValid = false;
                errorMessage += "• Invalid email address format\n";
            }
            
            // Update UI
            lblValidationMessage.Text = errorMessage;
            btnSave.Enabled = isValid;
        }
        
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Allow a variety of phone formats
            string cleaned = new string(phoneNumber.Where(c => char.IsDigit(c)).ToArray());
            return cleaned.Length >= 10;
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                // Use .NET's built-in email validation
                return new EmailAddressAttribute().IsValid(email);
            }
            catch
            {
                return false;
            }
        }
        
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable the button to prevent double-clicks
                btnSave.Enabled = false;
                Cursor = Cursors.WaitCursor;
                
                // Create a driver object from form data
                var driver = new Driver
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    LicenseNumber = txtLicenseNumber.Text.Trim(),
                    LicenseExpiration = dtpLicenseExpiration.Value,
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    Email = txtEmail.Text.Trim()
                };
                
                bool success;
                if (_isEditing)
                {
                    // Update existing driver
                    driver.Id = _driverId.Value;
                    success = await _dbHelper.UpdateDriverAsync(driver);
                    _logger.LogInformation("Updated driver with ID {DriverId}", driver.Id);
                }
                else
                {
                    // Add new driver
                    driver.CreatedDate = DateTime.Now;
                    var addedDriver = await _dbHelper.AddDriverAsync(driver);
                    success = addedDriver != null;
                    _logger.LogInformation("Added new driver with ID {DriverId}", addedDriver?.Id);
                }
                
                if (success)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to save driver information.", 
                        "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving driver");
                MessageBox.Show($"Error saving driver: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = true;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
