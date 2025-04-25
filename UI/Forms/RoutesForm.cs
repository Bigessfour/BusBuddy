// BusBuddy/UI/Forms/RoutesForm.cs
using System;
using System.Linq;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using Serilog;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class RoutesForm : BaseForm
    {
        private readonly IDatabaseManager _dbManager;
        private readonly new ILogger _logger;
        private Route? _selectedRoute;

        public RoutesForm(IDatabaseManager dbManager, ILogger logger)
        {
            _logger = logger;
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            InitializeComponent();
            
            // Apply consistent styling
            ApplyCustomStyling();
            
            LoadRoutes();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Apply styling to data grid
            FormStyler.StyleDataGridView(routesDataGridView);
            
            // Style buttons
            FormStyler.StyleButton(saveButton);
            FormStyler.StyleButton(editButton);
            FormStyler.StyleButton(deleteButton);
            FormStyler.StyleButton(refreshButton);
            FormStyler.StyleButton(backButton);
            
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
                        }
                        else if (innerControl is Label innerLabel)
                        {
                            FormStyler.StyleLabel(innerLabel, false);
                        }
                    }
                }
            }
            this.ResumeLayout();
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }

        private void LoadRoutes()
        {
            try
            {
                var routes = _dbManager.GetRoutes();
                routesDataGridView.DataSource = routes;
                _logger.Information("Loaded {Count} routes", routes.Count);
                statusLabel.Text = $"{routes.Count} routes loaded.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading routes");
                statusLabel.Text = "Error loading routes.";
            }
        }

        private void RoutesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (routesDataGridView.SelectedRows.Count > 0)
                {
                    _selectedRoute = (Route)routesDataGridView.SelectedRows[0].DataBoundItem;
                    nameTextBox.Text = _selectedRoute.RouteName;
                    descriptionTextBox.Text = _selectedRoute.Description;
                    defaultBusNumberTextBox.Text = _selectedRoute.DefaultBusNumber.ToString();
                    
                    // Get driver name from driver ID if available
                    if (_selectedRoute.DefaultDriverID.HasValue)
                    {
                        var drivers = _dbManager.GetDrivers();
                        var driver = drivers.FirstOrDefault(d => d.DriverID == _selectedRoute.DefaultDriverID.Value);
                        defaultDriverNameTextBox.Text = driver?.DriverName ?? string.Empty;
                    }
                    else
                    {
                        defaultDriverNameTextBox.Text = string.Empty;
                    }
                    
                    editButton.Enabled = true;
                    deleteButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error selecting route");
                statusLabel.Text = "Error selecting route.";
            }
        }

        protected override void SaveRecord()
        {
            try
            {
                var route = new Route
                {
                    RouteName = nameTextBox.Text,
                    Description = descriptionTextBox.Text,
                    DefaultBusNumber = int.TryParse(defaultBusNumberTextBox.Text, out int busNumber) ? busNumber : 0
                    // Note: DefaultDriverID would need to be looked up from the name
                };

                _dbManager.AddRoute(route);
                _logger.Information("Route added: {RouteName}", route.RouteName);
                statusLabel.Text = "Route added successfully.";
                LoadRoutes();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding route");
                statusLabel.Text = "Error adding route.";
            }
        }

        protected override void EditRecord()
        {
            if (_selectedRoute == null)
            {
                statusLabel.Text = "No route selected.";
                return;
            }

            try
            {
                _selectedRoute.RouteName = nameTextBox.Text;
                _selectedRoute.Description = descriptionTextBox.Text;
                _selectedRoute.DefaultBusNumber = int.TryParse(defaultBusNumberTextBox.Text, out int busNumber) ? busNumber : 0;
                // Note: DefaultDriverID would need to be looked up from the name

                _dbManager.UpdateRoute(_selectedRoute);
                _logger.Information("Route updated: {RouteName}", _selectedRoute.RouteName);
                statusLabel.Text = "Route updated successfully.";
                LoadRoutes();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating route");
                statusLabel.Text = "Error updating route.";
            }
        }

        protected override void DeleteRecord()
        {
            if (_selectedRoute == null)
            {
                statusLabel.Text = "No route selected.";
                return;
            }

            try
            {
                if (MessageBox.Show(
                    $"Are you sure you want to delete route '{_selectedRoute.RouteName}'?", 
                    "Confirm Delete", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _dbManager.DeleteRoute(_selectedRoute.RouteID);
                    _logger.Information("Route deleted: {RouteName}", _selectedRoute.RouteName);
                    statusLabel.Text = "Route deleted successfully.";
                    LoadRoutes();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting route");
                statusLabel.Text = "Error deleting route.";
            }
        }

        protected override void RefreshData()
        {
            LoadRoutes();
        }
    }
}