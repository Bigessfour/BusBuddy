// BusBuddy/UI/Forms/MaintenanceForm.cs
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.UI.Interfaces;
using Serilog;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class MaintenanceForm : BaseForm
    {
        private readonly IDatabaseManager _dbManager;
        private new readonly ILogger _logger;
        private readonly IFormNavigator _formNavigator;
        private Maintenance? _selectedRecord;
        private bool _isEditMode = false;
        private List<Maintenance> _maintenanceRecords = new List<Maintenance>();

        public MaintenanceForm(IDatabaseManager dbManager, ILogger logger, IFormNavigator formNavigator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _formNavigator = formNavigator ?? throw new ArgumentNullException(nameof(formNavigator));
            
            InitializeComponent();
            
            // Apply custom styling
            ApplyCustomStyling();
            
            LoadData();
            ResetForm();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Apply styling to data grid
            FormStyler.StyleDataGridView(maintenanceDataGridView);
            
            // Style buttons
            FormStyler.StyleButton(saveButton);
            FormStyler.StyleButton(editButton);
            FormStyler.StyleButton(deleteButton);
            FormStyler.StyleButton(refreshButton);
            FormStyler.StyleButton(exitButton);
            
            // Apply professional light blue styling to all input controls
            this.SuspendLayout();
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                    textBox.BackColor = System.Drawing.Color.White;
                }
                else if (control is Label label && label != statusLabel)
                {
                    FormStyler.StyleLabel(label, label.Name.Contains("Header") || label.Name.Contains("Title"));
                }
                else if (control is GroupBox groupBox)
                {
                    FormStyler.StyleGroupBox(groupBox);
                    
                    // Apply styling to controls within the group box
                    foreach (Control innerControl in groupBox.Controls)
                    {
                        if (innerControl is TextBox innerTextBox)
                        {
                            innerTextBox.BorderStyle = BorderStyle.FixedSingle;
                            innerTextBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                            innerTextBox.BackColor = System.Drawing.Color.White;
                        }
                        else if (innerControl is Label innerLabel)
                        {
                            FormStyler.StyleLabel(innerLabel, false);
                        }
                    }
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                    numericUpDown.BackColor = System.Drawing.Color.White;
                }
            }
            this.ResumeLayout();
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }

        private void LoadData()
        {
            try
            {
                statusLabel.Text = "Loading maintenance records...";
                Cursor = Cursors.WaitCursor;
                
                _maintenanceRecords = _dbManager.GetMaintenanceRecords();
                maintenanceDataGridView.DataSource = _maintenanceRecords;
                
                statusLabel.Text = $"Loaded {_maintenanceRecords.Count} maintenance records.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading maintenance records");
                statusLabel.Text = "Error loading maintenance records.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ResetForm()
        {
            busNumberTextBox.Text = string.Empty;
            datePerformedPicker.Value = DateTime.Today;
            descriptionTextBox.Text = string.Empty;
            costNumericUpDown.Value = 0;
            odometerNumericUpDown.Value = 0;
            
            _selectedRecord = null;
            _isEditMode = false;
            saveButton.Text = "Save";
        }

        protected override void SaveRecord()
        {
            try
            {
                statusLabel.Text = "Saving maintenance record...";
                Cursor = Cursors.WaitCursor;

                if (!ValidateInput())
                {
                    statusLabel.Text = "Please correct validation errors.";
                    Cursor = Cursors.Default;
                    return;
                }

                var record = new Maintenance
                {
                    BusNumber = int.Parse(busNumberTextBox.Text),
                    DatePerformed = datePerformedPicker.Value.ToString("yyyy-MM-dd"),
                    Description = descriptionTextBox.Text,
                    Cost = (double)costNumericUpDown.Value,
                    OdometerReading = (int)odometerNumericUpDown.Value
                };

                if (_isEditMode && _selectedRecord != null)
                {
                    record.MaintenanceID = _selectedRecord.MaintenanceID;
                    _dbManager.UpdateMaintenanceRecord(record);
                    statusLabel.Text = $"Updated maintenance record for bus #{record.BusNumber}.";
                    _logger.Information("Updated maintenance record ID {ID} for bus #{BusNumber}", 
                        record.MaintenanceID, record.BusNumber);
                }
                else
                {
                    _dbManager.AddMaintenanceRecord(record);
                    statusLabel.Text = $"Added new maintenance record for bus #{record.BusNumber}.";
                    _logger.Information("Added new maintenance record for bus #{BusNumber}", record.BusNumber);
                }

                ResetForm();
                LoadData();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving maintenance record");
                MessageBox.Show($"Error saving maintenance record: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error saving maintenance record.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void EditRecord()
        {
            if (_selectedRecord != null)
            {
                _isEditMode = true;
                saveButton.Text = "Update";
                
                busNumberTextBox.Text = _selectedRecord.BusNumber.ToString();
                datePerformedPicker.Value = DateTime.Parse(_selectedRecord.DatePerformed);
                descriptionTextBox.Text = _selectedRecord.Description;
                costNumericUpDown.Value = _selectedRecord.Cost.HasValue ? Convert.ToDecimal(_selectedRecord.Cost.Value) : 0;
                odometerNumericUpDown.Value = _selectedRecord.OdometerReading ?? 0;
            }
        }

        protected override void RefreshData()
        {
            LoadData();
            ResetForm();
        }

        protected override void DeleteRecord()
        {
            if (_selectedRecord != null)
            {
                if (MessageBox.Show($"Are you sure you want to delete the maintenance record for Bus #{_selectedRecord.BusNumber} from {_selectedRecord.DatePerformed}?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        statusLabel.Text = "Deleting maintenance record...";
                        Cursor = Cursors.WaitCursor;

                        _dbManager.DeleteMaintenanceRecord(_selectedRecord.MaintenanceID);
                        statusLabel.Text = "Maintenance record deleted successfully.";
                        _logger.Information("Deleted maintenance record ID {ID}", _selectedRecord.MaintenanceID);
                        LoadData();
                        ResetForm();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error deleting maintenance record ID {ID}", _selectedRecord.MaintenanceID);
                        MessageBox.Show($"Error deleting maintenance record: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        statusLabel.Text = "Error deleting maintenance record.";
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(busNumberTextBox.Text) || !int.TryParse(busNumberTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid bus number.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description of the maintenance.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void MaintenanceDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (maintenanceDataGridView.SelectedRows.Count > 0 && maintenanceDataGridView.SelectedRows[0].Index < _maintenanceRecords.Count)
            {
                _selectedRecord = _maintenanceRecords[maintenanceDataGridView.SelectedRows[0].Index];
            }
        }

        private void ReturnToMainForm()
        {
            _formNavigator.NavigateToMainForm(this);
        }

        // Override the default button event handlers in Designer.cs
        private void SaveButton_Click(object sender, EventArgs e) => SaveRecord();
        private void EditButton_Click(object sender, EventArgs e) => EditRecord();
        private void RefreshButton_Click(object sender, EventArgs e) => RefreshData();
        private void DeleteButton_Click(object sender, EventArgs e) => DeleteRecord();
        private void ExitButton_Click(object sender, EventArgs e) => ReturnToMainForm();
    }
}