using System;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Utilities;
using System.Collections.Generic;
using BusBuddy.Models;

namespace BusBuddy.UI.Forms
{
    public partial class ScheduledRoutesForm : BaseForm
    {
        private new readonly ILogger _logger;
        private List<ScheduledRoute> _scheduledRoutes = new List<ScheduledRoute>();
        private ScheduledRoute? _selectedRoute;

        public ScheduledRoutesForm()
        {
            _logger = FormManager.GetLogger(GetType().Name);
            InitializeComponent();
            
            // Apply styling to ensure consistency with other forms
            ApplyCustomStyling();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Apply styling to data grid if present
            if (routesDataGridView != null)
            {
                FormStyler.StyleDataGridView(routesDataGridView);
            }
            
            // Style buttons
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    FormStyler.StyleButton(button, !button.Name.Contains("Delete") && !button.Name.Contains("Exit"));
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
                        else if (innerControl is ComboBox comboBox)
                        {
                            comboBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                            comboBox.BackColor = System.Drawing.Color.White;
                        }
                    }
                }
                else if (control is TabControl tabControl)
                {
                    tabControl.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                }
            }
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }
        
        /// <summary>
        /// Load scheduled routes data
        /// </summary>
        private void LoadRoutesData()
        {
            try
            {
                // Implementation will depend on the actual data source
                _logger.Information("Loading scheduled routes");
                statusLabel.Text = "Scheduled routes loaded.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading scheduled routes");
                statusLabel.Text = "Error loading scheduled routes.";
            }
        }
        
        #region BaseForm Overrides
        
        /// <summary>
        /// Implementation of the BaseForm's SaveRecord method
        /// </summary>
        protected override void SaveRecord()
        {
            _logger.Information("ScheduledRoutesForm: SaveRecord called");
            statusLabel.Text = "Scheduled route saved.";
            // TODO: Implement actual save logic
        }
        
        /// <summary>
        /// Implementation of the BaseForm's EditRecord method
        /// </summary>
        protected override void EditRecord()
        {
            _logger.Information("ScheduledRoutesForm: EditRecord called");
            statusLabel.Text = "Scheduled route updated.";
            // TODO: Implement actual edit logic
        }
        
        /// <summary>
        /// Implementation of the BaseForm's RefreshData method
        /// </summary>
        protected override void RefreshData()
        {
            _logger.Information("ScheduledRoutesForm: RefreshData called");
            LoadRoutesData();
        }
        
        /// <summary>
        /// Implementation of the BaseForm's DeleteRecord method
        /// </summary>
        protected override void DeleteRecord()
        {
            _logger.Information("ScheduledRoutesForm: DeleteRecord called");
            statusLabel.Text = "Scheduled route deleted.";
            // TODO: Implement actual delete logic
        }
        
        #endregion
        
        /// <summary>
        /// Handle form load event
        /// </summary>
        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);
            LoadRoutesData();
        }
    }
}