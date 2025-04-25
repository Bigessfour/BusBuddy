using System;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class Welcome : BaseForm
    {
        public Welcome()
        {
            InitializeComponent();
            
            // The logger and common form properties are now handled by BaseForm
            this.Text = "Welcome - BusBuddy";
            
            // Explicitly apply styling to controls that may have been overridden in the designer
            ApplyCustomStyling();
        }
        
        // Override the base form's load event if needed
        protected override void BaseForm_Load(object sender, EventArgs e)
        {
            base.BaseForm_Load(sender, e);
            
            // Load today's trips and stats
            LoadDashboardStats();
            LoadTodaysTrips();
        }
        
        private void ApplyCustomStyling()
        {
            // Apply styling to the data grid
            FormStyler.StyleDataGridView(todaysTripsGrid);
            
            // Style all buttons
            FormStyler.StyleButton(vehiclesButton);
            FormStyler.StyleButton(fuelButton);
            FormStyler.StyleButton(driverButton);
            FormStyler.StyleButton(settingsButton);
            FormStyler.StyleButton(maintenanceButton);
            
            // Style the group box
            FormStyler.StyleGroupBox(dataInputGroupBox);
            
            // Style the label with heading formatting
            FormStyler.StyleLabel(dashboardStatsLabel, true);
        }
        
        private void LoadDashboardStats()
        {
            // Implementation for loading dashboard stats
            dashboardStatsLabel.Text = "Welcome to BusBuddy - Your Fleet Management Solution";
            _logger.Information("Dashboard stats loaded");
        }
        
        private void LoadTodaysTrips()
        {
            // Implementation for loading today's trips into the grid
            _logger.Information("Today's trips loaded");
        }
        
        private void OpenVehiclesForm_Click(object sender, EventArgs e)
        {
            NavigateToForm("vehicles");
        }
        
        private void OpenFuelForm_Click(object sender, EventArgs e)
        {
            NavigateToForm("fuel");
        }
        
        private void OpenDriverForm_Click(object sender, EventArgs e)
        {
            NavigateToForm("drivers");
        }
        
        private void OpenSettingsForm_Click(object sender, EventArgs e)
        {
            NavigateToForm("settings");
        }
    }
}