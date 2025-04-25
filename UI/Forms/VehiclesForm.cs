using System;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Utilities;
using System.ComponentModel;
using BusBuddy.Models;
using BusBuddy.Data;

namespace BusBuddy.UI.Forms
{
    /// <summary>
    /// Form for managing the fleet of school buses and their details.
    /// </summary>
    public partial class VehiclesForm : BaseForm
    {
        private new readonly ILogger _logger;
        private readonly IDatabaseManager? _dbManager;
        private BindingList<Vehicle>? _bindingList;
        private Vehicle? _selectedVehicle;
        private bool _isEditMode = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesForm"/> class.
        /// </summary>
        public VehiclesForm()
        {
            _logger = FormManager.GetLogger(GetType().Name);
            InitializeComponent();
            
            // Apply styling to ensure consistency with other forms
            ApplyCustomStyling();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VehiclesForm"/> class with dependencies.
        /// </summary>
        /// <param name="dbManager">The database manager to use.</param>
        /// <param name="logger">The logger to use.</param>
        public VehiclesForm(IDatabaseManager dbManager, Serilog.ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializeComponent();
            
            // Apply styling to ensure consistency with other forms
            ApplyCustomStyling();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Apply styling to data grid
            FormStyler.StyleDataGridView(vehiclesDataGridView);
            
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
                }
                else if (control is Label label && label != statusLabel)
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

        /// <summary>
        /// Handles saving a vehicle record.
        /// </summary>
        protected override void SaveRecord()
        {
            _logger.Information("VehiclesForm: SaveRecord called.");
            statusLabel.Text = "Save clicked.";
            
            // Add implementation for saving vehicle records
        }

        /// <summary>
        /// Handles editing a vehicle record.
        /// </summary>
        protected override void EditRecord()
        {
            _logger.Information("VehiclesForm: EditRecord called.");
            statusLabel.Text = "Edit clicked.";
            
            // Add implementation for editing vehicle records
        }

        /// <summary>
        /// Refreshes the vehicle data.
        /// </summary>
        protected override void RefreshData()
        {
            _logger.Information("VehiclesForm: RefreshData called.");
            statusLabel.Text = "Refresh clicked.";
            
            // Add implementation for refreshing vehicle data
        }

        /// <summary>
        /// Handles deleting a vehicle record.
        /// </summary>
        protected override void DeleteRecord()
        {
            _logger.Information("VehiclesForm: DeleteRecord called.");
            statusLabel.Text = "Delete clicked.";
            
            // Add implementation for deleting vehicle records
        }

        /// <summary>
        /// Handles selection changes in the vehicles data grid view.
        /// </summary>
        private void VehiclesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            _logger.Information("VehiclesForm: Selection changed in DataGridView.");
            editButton.Enabled = vehiclesDataGridView.SelectedRows.Count > 0;
            deleteButton.Enabled = vehiclesDataGridView.SelectedRows.Count > 0;
        }
        
        /// <summary>
        /// Handles the form load event.
        /// </summary>
        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);
            
            // Load vehicle data
            RefreshData();
        }
    }
}