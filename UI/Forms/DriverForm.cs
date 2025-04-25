// Copyright (c) YourCompanyName. All rights reserved.
// BusBuddy/Data/Repositories/DriverForm.cs
using Serilog;

namespace BusBuddy.UI.Forms
{
    /// <summary>
    /// Form for managing driver records.
    /// </summary>
    public partial class DriverForm : BaseForm
    {
        private new readonly ILogger _logger;
        private readonly Services.IDriverService? _driverService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverForm"/> class.
        /// </summary>
        public DriverForm()
        {
            _logger = Log.Logger ?? new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/busbuddy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverForm"/> class with dependencies.
        /// </summary>
        /// <param name="driverService">The driver service to use.</param>
        /// <param name="logger">The logger to use.</param>
        public DriverForm(Services.IDriverService driverService, Serilog.ILogger logger)
        {
            _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializeComponent();
        }

        /// <summary>
        /// Handles saving a driver record.
        /// </summary>
        protected override void SaveRecord()
        {
            try
            {
                // Implement save logic for DriverForm
                statusLabel.Text = "Driver saved.";
                _logger.Information("DriverForm: Driver saved.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "DriverForm: Error saving driver.");
                statusLabel.Text = "Error saving driver.";
            }
        }

        /// <summary>
        /// Handles editing a driver record.
        /// </summary>
        protected override void EditRecord()
        {
            try
            {
                // Implement edit logic for DriverForm
                statusLabel.Text = "Driver updated.";
                _logger.Information("DriverForm: Driver updated.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "DriverForm: Error updating driver.");
                statusLabel.Text = "Error updating driver.";
            }
        }

        /// <summary>
        /// Refreshes the driver data.
        /// </summary>
        protected override void RefreshData()
        {
            try
            {
                // Implement refresh logic for DriverForm
                statusLabel.Text = "Data refreshed.";
                _logger.Information("DriverForm: Data refreshed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "DriverForm: Error refreshing data.");
                statusLabel.Text = "Error refreshing data.";
            }
        }
    }
}