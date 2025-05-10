using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using BusBuddy.Data;
using BusBuddy.Data.Interfaces;

namespace BusBuddy.Forms
{    public partial class Dashboard : Form
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Dashboard> _logger;
        private BindingSource bindingSource = new BindingSource();

        // NOTE: All UI controls are automatically defined in Dashboard.Designer.cs

        public Dashboard(
            IDatabaseHelper databaseHelper,
            IServiceProvider serviceProvider,
            ILogger<Dashboard> logger)
        {
            try
            {
                _databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
                _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));

                // Initialize the basic form components first
                InitializeComponent();

                // Register events BEFORE setting up controls
                this.Load += Dashboard_Load;
                this.Resize += Dashboard_Resize;

                _logger!.LogInformation("Dashboard constructor completed successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "Error constructing Dashboard");
                MessageBox.Show($"Error initializing Dashboard: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                _logger!.LogInformation("Dashboard Load event starting");

                // Verify that required services are available
                try
                {
                    VerifyRequiredServices();
                }
                catch (Exception serviceEx)
                {
                    // If verification fails but user wants to continue, we'll still proceed
                    if (!HandleInitializationError(serviceEx, "service initialization"))
                    {
                        Close();
                        return;
                    }
                }

                // Setup UI with exception handling
                try
                {
                    // Setup custom controls during the Load event, not in constructor
                    SetupCustomControls();
                    InitializeStandardMenus();

                    // Set form properties after controls are added
                    this.WindowState = FormWindowState.Maximized;

                    // Register Shown event after controls are set up
                    this.Shown += Dashboard_Shown;
                }
                catch (Exception uiEx)
                {
                    // If UI setup fails but user wants to continue, we'll create a minimal UI
                    if (!HandleInitializationError(uiEx, "UI initialization"))
                    {
                        Close();
                        return;
                    }

                    // Create minimal UI to allow some functionality
                    CreateMinimalUI();
                }

                _logger!.LogInformation("Dashboard Load event completed");
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "Critical error in Dashboard_Load");
                MessageBox.Show($"A critical error occurred: {ex.Message}",
                    "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            try
            {
                // Basic resize logic - just adjust content panel size
                if (contentPanel != null)
                {
                    _logger.LogDebug("Resizing dashboard content panel");
                    // Add actual resize logic here in the future
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Dashboard_Resize");
            }
        }

        private void Dashboard_Shown(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("Dashboard shown to user");
                // Add any initialization that should happen after the form is visible
                statusLabel.Text = "Ready";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Dashboard_Shown");
            }
        }

        private void VerifyRequiredServices()
        {
            _logger.LogInformation("Verifying required services");
            
            // Check database connection
            if (_databaseHelper == null)
            {
                throw new InvalidOperationException("Database helper is not available");
            }
            
            // In the future, add more service verifications here
            
            _logger.LogInformation("All required services verified");
        }

        private bool HandleInitializationError(Exception ex, string context)
        {
            _logger.LogError(ex, "Error during {Context}", context);
            
            var result = MessageBox.Show(
                $"An error occurred during {context}: {ex.Message}\n\nDo you want to continue anyway?",
                "Initialization Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            
            return result == DialogResult.Yes;
        }

        private void SetupCustomControls()
        {
            _logger.LogInformation("Setting up custom controls");
            
            // Set form title
            this.Text = "BusBuddy - School Bus Management System";
            
            // Configure side panel
            sidePanel.Dock = DockStyle.Left;
            sidePanel.Width = 200;
            sidePanel.BackColor = Color.FromArgb(50, 50, 80);
            
            // Configure content panel
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.White;
            
            _logger.LogInformation("Custom controls setup complete");
        }

        private void InitializeStandardMenus()
        {
            _logger.LogInformation("Initializing standard menus");
            
            // Wire up menu click events
            exitToolStripMenuItem.Click += (s, e) => Close();
            aboutToolStripMenuItem.Click += (s, e) => ShowAboutDialog();
            
            // Add event handlers for other menu items
            routesToolStripMenuItem.Click += (s, e) => ShowRoutesManagement();
            
            _logger.LogInformation("Standard menus initialized");
        }

        private void CreateMinimalUI()
        {
            _logger.LogWarning("Creating minimal UI due to initialization errors");
            
            // Clear existing controls
            this.Controls.Clear();
            
            // Create a simple label
            var label = new Label
            {
                Text = "BusBuddy is running in minimal mode due to initialization errors.\nPlease check the logs for details.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.FontFamily, 12),
                ForeColor = Color.Red
            };
            
            this.Controls.Add(label);
            
            _logger.LogInformation("Minimal UI created");
        }
        
        private void ShowAboutDialog()
        {
            MessageBox.Show(
                "BusBuddy - School Bus Management System\nVersion 1.0\n© 2025 BusBuddy Team",
                "About BusBuddy",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        
        private void ShowRoutesManagement()
        {
            _logger.LogInformation("Routes management requested - functionality not yet implemented");
            MessageBox.Show(
                "Routes management will be implemented in a future version.",
                "Not Implemented",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
