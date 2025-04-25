// BusBuddy/UI/Forms/FuelForm.cs
using System;
using System.ComponentModel;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.Services;
using BusBuddy.API;
using Serilog;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class FuelForm : BaseForm
    {
        private new readonly Serilog.ILogger _logger;
        private readonly IDatabaseManager _dbManager;
        private readonly IFuelService _fuelService;
        private readonly IFormNavigator _formNavigator;
        private readonly GrokApiClient _grokApiClient;
        
        private List<FuelRecord> _fuelRecords = new List<FuelRecord>();
        private FuelRecord? _selectedRecord;
        private BindingList<FuelRecord>? _bindingList;
        private bool _isEditMode = false;

        /// <summary>
        /// Simple parameterless constructor for use with the Windows Forms Designer and FormManager.
        /// This should not be used in production code - use the dependency injection constructor instead.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public FuelForm()
        {
            // Set up a basic logger
            _logger = Log.Logger ?? new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/busbuddy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                // Create the database manager directly with Serilog
                _dbManager = new DatabaseManager(_logger);
                
                // Create form navigator using the new implementation
                _formNavigator = new FormNavigatorImpl(form => form.Close());
                
                // Initialize the form UI
                InitializeComponent();
                
                // Hide the test Grok button which requires additional dependencies
                testGrokButton.Visible = false;
                
                // Initialize the form data
                InitializeFormData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger?.Error(ex, "Error initializing FuelForm");
            }
        }

        public FuelForm(
            IDatabaseManager dbManager, 
            Serilog.ILogger logger, 
            IFuelService fuelService,
            IFormNavigator formNavigator,
            GrokApiClient? grokApiClient = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _fuelService = fuelService ?? throw new ArgumentNullException(nameof(fuelService));
            _formNavigator = formNavigator ?? throw new ArgumentNullException(nameof(formNavigator));
            _grokApiClient = grokApiClient; // Optional, can be null
            
            InitializeComponent();
            
            try
            {
                // Set Grok button visibility based on API client availability
                SetGrokButtonVisibility();
                
                // Initialize the form data
                InitializeFormData();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error initializing FuelForm");
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sets the visibility of the Test Grok button based on the availability of the Grok API client
        /// </summary>
        private void SetGrokButtonVisibility()
        {
            // Hide Test Grok button if GrokApiClient is not available
            testGrokButton.Visible = _grokApiClient != null;
        }

        /// <summary>
        /// Initializes the form data including loading bus numbers, configuring the data grid view, and loading data
        /// </summary>
        private void InitializeFormData()
        {
            LoadBusNumbers();
            ConfigureDataGridView();
            LoadData();
            ResetForm();
            
            // Apply custom styling to ensure the form looks consistent
            ApplyCustomStyling();
        }

        /// <summary>
        /// Apply custom styling to all controls in this form
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Apply styling to data grid
            FormStyler.StyleDataGridView(fuelDataGridView);
            
            // Style buttons
            FormStyler.StyleButton(saveButton);
            FormStyler.StyleButton(refreshButton);
            FormStyler.StyleButton(exitButton);
            FormStyler.StyleButton(testGrokButton);
            
            // Style labels - don't try to style statusLabel if it's a ToolStripStatusLabel
            // since it's not compatible with the Label styling method
            
            // Apply professional light blue styling to all input controls
            this.SuspendLayout();
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                }
                else if (control is Label label)
                {
                    FormStyler.StyleLabel(label, false);
                }
                else if (control is GroupBox groupBox)
                {
                    FormStyler.StyleGroupBox(groupBox);
                }
            }
            this.ResumeLayout();
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }

        private void ConfigureDataGridView()
        {
            fuelDataGridView.AutoGenerateColumns = false;
            
            fuelDataGridView.Columns.Clear();
            fuelDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FuelID",
                HeaderText = "ID",
                Width = 50,
                Visible = false
            });
            
            fuelDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FuelDate",
                HeaderText = "Date",
                Width = 100
            });
            
            fuelDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "BusNumber",
                HeaderText = "Bus #",
                Width = 70
            });
            
            fuelDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Gallons",
                HeaderText = "Gallons",
                Width = 80
            });
            
            fuelDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Odometer",
                HeaderText = "Odometer",
                Width = 100
            });
            
            fuelDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Notes",
                HeaderText = "Notes",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            fuelDataGridView.CellDoubleClick += FuelDataGridView_CellDoubleClick;
        }

        private void FuelDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _fuelRecords.Count)
            {
                _selectedRecord = _fuelRecords[e.RowIndex];
                PopulateFormFields(_selectedRecord);
                _isEditMode = true;
                saveButton.Text = "Update";
            }
        }

        private void PopulateFormFields(FuelRecord record)
        {
            if (record == null) return;

            busNumberTextBox.Text = record.BusNumber.ToString();
            datePicker.Value = DateTime.Parse(record.FuelDate);
            gallonsNumericUpDown.Value = Convert.ToDecimal(record.Gallons);
            odometerNumericUpDown.Value = record.Odometer;
            fuelTypeTextBox.Text = record.FuelType;
            if (record.Cost.HasValue)
            {
                costNumericUpDown.Value = Convert.ToDecimal(record.Cost.Value);
            }
            else
            {
                costNumericUpDown.Value = 0;
            }
        }

        private void LoadBusNumbers()
        {
            try
            {
                var busNumbers = _fuelService.GetBusNumbers();
                busNumberTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                busNumberTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                
                var autoCompleteCollection = new AutoCompleteStringCollection();
                foreach (var busNumber in busNumbers)
                {
                    autoCompleteCollection.Add(busNumber.ToString());
                }
                
                busNumberTextBox.AutoCompleteCustomSource = autoCompleteCollection;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading bus numbers");
                statusLabel.Text = "Error loading bus numbers.";
            }
        }

        private void LoadData()
        {
            try
            {
                statusLabel.Text = "Loading fuel records...";
                Cursor = Cursors.WaitCursor;
                
                _fuelRecords = _fuelService.GetFuelRecords();
                _bindingList = new BindingList<FuelRecord>(_fuelRecords);
                fuelDataGridView.DataSource = _bindingList;
                
                statusLabel.Text = $"Loaded {_fuelRecords.Count} fuel records.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading fuel records");
                statusLabel.Text = "Error loading fuel records.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ResetForm()
        {
            busNumberTextBox.Text = string.Empty;
            datePicker.Value = DateTime.Today;
            gallonsNumericUpDown.Value = 0;
            odometerNumericUpDown.Value = 0;
            costNumericUpDown.Value = 0;
            fuelTypeTextBox.Text = "Diesel";  // Default value
            
            _selectedRecord = null;  // Removed null-forgiving operator
            _isEditMode = false;
            saveButton.Text = "Save";
        }

        protected override void RefreshData()
        {
            LoadData();
            ResetForm();
        }

        protected override void SaveRecord()
        {
            try
            {
                statusLabel.Text = "Saving fuel record...";
                Cursor = Cursors.WaitCursor;

                if (!ValidateInput())
                {
                    statusLabel.Text = "Please correct validation errors.";
                    Cursor = Cursors.Default;
                    return;
                }

                var record = new FuelRecord
                {
                    BusNumber = int.Parse(busNumberTextBox.Text),
                    FuelDate = datePicker.Value.ToString("yyyy-MM-dd"),
                    Gallons = (double)gallonsNumericUpDown.Value,
                    Odometer = (int)odometerNumericUpDown.Value,
                    FuelType = fuelTypeTextBox.Text,
                    Cost = (double)costNumericUpDown.Value,
                    Notes = ""  // Add a notes field if needed
                };

                bool success;
                if (_isEditMode && _selectedRecord != null)
                {
                    record.FuelID = _selectedRecord.FuelID;
                    success = _fuelService.UpdateFuelRecord(record);
                    if (success)
                    {
                        statusLabel.Text = $"Updated fuel record for bus #{record.BusNumber}.";
                        _logger.Information("Updated fuel record ID {ID} for bus #{BusNumber}", 
                            record.FuelID, record.BusNumber);
                    }
                }
                else
                {
                    success = _fuelService.AddFuelRecord(record);
                    if (success)
                    {
                        statusLabel.Text = $"Added new fuel record for bus #{record.BusNumber}.";
                        _logger.Information("Added new fuel record for bus #{BusNumber}", record.BusNumber);
                    }
                }

                if (!success)
                {
                    MessageBox.Show("Failed to save fuel record.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Failed to save fuel record.";
                    _logger.Warning("Failed to save fuel record for bus #{BusNumber}", record.BusNumber);
                }
                else
                {
                    ResetForm();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving fuel record");
                MessageBox.Show($"Error saving fuel record: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error saving fuel record.";
            }
            finally
            {
                Cursor = Cursors.Default;
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

            if (gallonsNumericUpDown.Value <= 0)
            {
                MessageBox.Show("Please enter a positive number of gallons.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void AddEditDeleteButtons()
        {
            // Check if the columns already exist to prevent adding duplicates
            if (fuelDataGridView.Columns["editColumn"] != null || fuelDataGridView.Columns["deleteColumn"] != null)
            {
                return; // Buttons are already added
            }

            // Add buttons to the DataGridView for editing and deleting records
            var editColumn = new DataGridViewButtonColumn
            {
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                HeaderText = "Edit",
                Name = "editColumn",
                Width = 50
            };

            var deleteColumn = new DataGridViewButtonColumn
            {
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                HeaderText = "Delete",
                Name = "deleteColumn",
                Width = 60
            };

            fuelDataGridView.Columns.Add(editColumn);
            fuelDataGridView.Columns.Add(deleteColumn);

            fuelDataGridView.CellClick += FuelDataGridView_CellClick;
        }

        private void FuelDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle the edit button click
            if (e.RowIndex >= 0 && e.ColumnIndex == fuelDataGridView.Columns["editColumn"].Index)
            {
                if (e.RowIndex < _fuelRecords.Count)
                {
                    _selectedRecord = _fuelRecords[e.RowIndex];
                    PopulateFormFields(_selectedRecord);
                    _isEditMode = true;
                    saveButton.Text = "Update";
                }
            }
            // Handle the delete button click
            else if (e.RowIndex >= 0 && e.ColumnIndex == fuelDataGridView.Columns["deleteColumn"].Index)
            {
                if (e.RowIndex < _fuelRecords.Count)
                {
                    _selectedRecord = _fuelRecords[e.RowIndex];
                    if (MessageBox.Show($"Are you sure you want to delete the fuel record for Bus #{_selectedRecord.BusNumber} from {_selectedRecord.FuelDate}?", 
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DeleteFuelRecord(_selectedRecord.FuelID);
                    }
                }
            }
        }

        private void DeleteFuelRecord(int recordId)
        {
            try
            {
                statusLabel.Text = "Deleting fuel record...";
                Cursor = Cursors.WaitCursor;

                bool success = _fuelService.DeleteFuelRecord(recordId);
                if (success)
                {
                    statusLabel.Text = "Fuel record deleted successfully.";
                    _logger.Information("Deleted fuel record ID {ID}", recordId);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to delete fuel record.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Failed to delete fuel record.";
                    _logger.Warning("Failed to delete fuel record ID {ID}", recordId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting fuel record ID {ID}", recordId);
                MessageBox.Show($"Error deleting fuel record: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error deleting fuel record.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AddEditDeleteButtons();
            
            // Reapply styling after adding buttons to ensure consistent appearance
            ApplyCustomStyling();
        }

        private void ReturnToMainForm()
        {
            _formNavigator.NavigateToMainForm(this);
        }

        private void SaveButton_Click(object sender, EventArgs e) => SaveRecord();
        private void RefreshButton_Click(object sender, EventArgs e) => RefreshData();
        private void ExitButton_Click(object sender, EventArgs e) => ReturnToMainForm();

        private void TestGrokButton_Click(object sender, EventArgs e)
        {
            if (_grokApiClient != null)
            {
                try
                {
                    statusLabel.Text = "Testing Grok API connection...";
                    Cursor = Cursors.WaitCursor;
                    
                    var result = _grokApiClient.TestConnection();
                    MessageBox.Show($"Grok API connection test result: {result}", 
                        "Grok API Test", MessageBoxButtons.OK, 
                        result ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                    
                    statusLabel.Text = result ? "Grok API connection test successful." : "Grok API connection test failed.";
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error testing Grok API connection");
                    MessageBox.Show($"Error testing Grok API connection: {ex.Message}", 
                        "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Error testing Grok API connection.";
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("Grok API client is not configured.", 
                    "Feature Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}