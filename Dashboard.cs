using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using BusBuddyMVP.Data;

namespace BusBuddyMVP.Forms.MainForms
{    public partial class Dashboard : Form, BusBuddyMVP.Forms.Interfaces.IForm
    {        private readonly DatabaseHelper _databaseHelper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Dashboard> _logger;
        private BindingSource bindingSource = new BindingSource();

        // NOTE: All UI controls are automatically defined in Dashboard.Designer.cs

        public Dashboard(
            DatabaseHelper databaseHelper,
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
        }        private void Dashboard_Load(object sender, EventArgs e)
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
          private void VerifyRequiredServices()
        {
            // Check that all required services are available before proceeding
            try
            {
                _logger?.LogInformation("Verifying required services...");

                // Pre-fetch forms that will be needed to ensure they can be resolved
                var routeManagementForm = _serviceProvider.GetService<RouteManagementForm>();
                var driversManagementForm = _serviceProvider.GetService<DriversManagementForm>();
                var vehiclesManagementForm = _serviceProvider.GetService<VehiclesManagementForm>();
                var activityTripsForm = _serviceProvider.GetService<ActivityTripsForm>();

                // Log services that couldn't be resolved
                if (routeManagementForm == null) _logger?.LogWarning("RouteManagementForm service could not be resolved");
                if (driversManagementForm == null) _logger?.LogWarning("DriversManagementForm service could not be resolved");
                if (vehiclesManagementForm == null) _logger?.LogWarning("VehiclesManagementForm service could not be resolved");
                if (activityTripsForm == null) _logger?.LogWarning("ActivityTripsForm service could not be resolved");

                // Verify database connection works
                var connected = _databaseHelper.TestConnection();
                if (!connected)
                {
                    _logger?.LogWarning("Database connection test failed. Some features may not work properly.");
                    // Show a non-blocking warning to the user
                    Task.Run(() => {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() => {
                                MessageBox.Show(
                                    "Database connection failed. Some features may not work properly.",
                                    "Database Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }));
                        }
                    });
                }
                else
                {
                    _logger?.LogInformation("Database connection verified successfully");
                }
            }
            catch (Exception ex)
            {
                if (!HandleInitializationError(ex, "service verification"))
                {
                    throw new InvalidOperationException("Required services are not available. Check the service registration.", ex);
                }
            }
        }
          private void Dashboard_Resize(object sender, EventArgs e)
        {
            try
            {
                // With proper docking, we don't need to manually adjust panel sizes
                // This is just for additional processing when resize occurs

                // Re-center any welcome labels if needed
                foreach (Control panel in contentPanel.Controls)
                {
                    if (panel is Panel welcomePanel)
                    {
                        foreach (Control ctrl in welcomePanel.Controls)
                        {
                            if (ctrl is Label label && label.Tag?.ToString() == "NeedsCentering")
                            {
                                // Center the label in its parent
                                label.Location = new Point(
                                    (welcomePanel.Width - label.Width) / 2,
                                    (welcomePanel.Height - label.Height) / 3
                                );
                            }
                        }
                    }
                }

                _logger?.LogDebug("Dashboard resized: {0}x{1}", this.Width, this.Height);
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "Error in Dashboard_Resize");
            }
        }private void SetupCustomControls()
        {
            _logger?.LogInformation("Starting SetupCustomControls");

            // Use SuspendLayout for better performance when adding multiple controls
            this.SuspendLayout();
            sidePanel.SuspendLayout();
            contentPanel.SuspendLayout();

            // Basic form setup - do not modify properties set by the Designer
            Text = "BusBuddy MVP - Dashboard";

            // Configure designer-defined panels - use colors only, preserve layout settings
            sidePanel.BackColor = Color.FromArgb(30, 40, 50);
            contentPanel.BackColor = Color.White;

            // Create a FlowLayoutPanel for better organization of sidebar buttons
            var sidebarFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(10, 20, 10, 10)
            };
            sidePanel.Controls.Add(sidebarFlow);

            // Create sections and add them to the FlowLayoutPanel
            AddSectionHeader(sidebarFlow, "Trips");
            AddNavigationButton(sidebarFlow, "Routes", OpenRouteManagement);
            AddNavigationButton(sidebarFlow, "Activity Trips", () => {
                try {
                    var activityTripsForm = _serviceProvider.GetRequiredService<ActivityTripsForm>();
                    activityTripsForm.ShowDialog();
                } catch (Exception ex) {
                    _logger!.LogError(ex, "Error opening ActivityTripsForm");
                    MessageBox.Show($"Error opening ActivityTripsForm: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            AddSectionHeader(sidebarFlow, "Operations");
            AddNavigationButton(sidebarFlow, "Drivers", () => {
                try {
                    var driversManagementForm = _serviceProvider.GetRequiredService<DriversManagementForm>();
                    driversManagementForm.ShowDialog();
                } catch (Exception ex) {
                    _logger!.LogError(ex, "Error opening DriversManagementForm");
                    MessageBox.Show($"Error opening DriversManagementForm: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            AddNavigationButton(sidebarFlow, "Vehicles", () => {
                try {
                    var vehiclesManagementForm = _serviceProvider.GetRequiredService<VehiclesManagementForm>();
                    vehiclesManagementForm.ShowDialog();
                } catch (Exception ex) {
                    _logger!.LogError(ex, "Error opening VehiclesManagementForm");
                    MessageBox.Show($"Error opening VehiclesManagementForm: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            // Add welcome panel to the content area
            AddWelcomeContent(contentPanel);

            // Resume layouts after adding all controls
            sidePanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

            // Log successful completion of control setup
            _logger?.LogInformation("Custom controls setup completed successfully");
        }

        private void AddSectionHeader(FlowLayoutPanel parent, string text)
        {
            var label = new Label
            {
                Text = text,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 5),
                Width = parent.Width - 20
            };
            parent.Controls.Add(label);
        }

        private void AddNavigationButton(FlowLayoutPanel parent, string text, Action clickHandler)
        {
            var button = new Button
            {
                Text = text,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Width = parent.Width - 20,
                Height = 40,
                Margin = new Padding(0, 5, 0, 10),
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false
            };

            button.Click += (s, e) =>
            {
                try
                {
                    clickHandler();
                }
                catch (Exception ex)
                {
                    _logger!.LogError(ex, $"Error in button click handler for {text}");
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            parent.Controls.Add(button);
        }

        private void AddWelcomeContent(Panel contentPanel)
        {
            var welcomePanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            contentPanel.Controls.Add(welcomePanel);

            var welcomeLabel = new Label
            {
                Text = "Welcome to BusBuddy MVP",
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.None,
                TextAlign = ContentAlignment.MiddleCenter
            };

            welcomePanel.Controls.Add(welcomeLabel);

            // Position will be finalized in the Shown event
            welcomeLabel.Tag = "NeedsCentering";
        }

        private void OpenRouteManagement()
        {
            var routeForm = _serviceProvider.GetRequiredService<RouteManagementForm>();
            routeForm.ShowDialog();
        }        private void Dashboard_Shown(object sender, EventArgs e)
        {
            try
            {
                _logger!.LogInformation("Dashboard is now visible");

                // Force a layout pass to ensure all controls are positioned correctly
                this.PerformLayout();

                // Final adjustments to ensure proper display
                Dashboard_Resize(this, EventArgs.Empty);

                // Center any labels that need centering (when we know the final size)
                foreach (Control panel in contentPanel.Controls)
                {
                    if (panel is Panel welcomePanel)
                    {
                        foreach (Control ctrl in welcomePanel.Controls)
                        {
                            if (ctrl is Label label && label.Tag?.ToString() == "NeedsCentering")
                            {
                                // Center the label in its parent
                                label.Location = new Point(
                                    (welcomePanel.Width - label.Width) / 2,
                                    (welcomePanel.Height - label.Height) / 3 // Position at 1/3 from top
                                );
                            }
                        }
                    }
                }

                // Make sure we're the foreground window
                this.Activate();
                this.BringToFront();
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "Error in Dashboard_Shown");
            }
        }

        /// <summary>
        /// Handles initialization errors with graceful recovery options
        /// </summary>
        private bool HandleInitializationError(Exception ex, string context)
        {
            try
            {
                _logger?.LogError(ex, $"Error during {context}");

                var result = MessageBox.Show(
                    $"An error occurred during {context}: {ex.Message}\n\nWould you like to continue anyway?",
                    "Initialization Error",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                return result == DialogResult.Yes;
            }
            catch
            {
                // Last resort if logging fails
                MessageBox.Show(
                    $"A critical error occurred. The application may not function correctly.\n\n{ex.Message}",
                    "Critical Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Creates a minimal fallback UI when the standard UI initialization fails
        /// </summary>
        private void CreateMinimalUI()
        {
            // Clear any existing controls to start fresh
            this.Controls.Clear();
            this.SuspendLayout();

            _logger?.LogInformation("Creating minimal fallback UI");

            // Create simple panel layout
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };

            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));

            // Add header with warning
            var headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.LightYellow
            };

            var warningLabel = new Label
            {
                Text = "⚠️ Application is running in limited functionality mode due to initialization errors",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true
            };

            warningLabel.Location = new Point(
                (headerPanel.ClientSize.Width - warningLabel.Width) / 2,
                (headerPanel.ClientSize.Height - warningLabel.Height) / 2);

            headerPanel.Controls.Add(warningLabel);

            // Add content with basic functionality
            var contentPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            var infoLabel = new Label
            {
                Text = "Some features may not be available. Please restart the application if you encounter issues.",
                AutoSize = true,
                Margin = new Padding(5, 10, 5, 20)
            };

            contentPanel.Controls.Add(infoLabel);

            // Add some basic functionality buttons
            var retryButton = new Button
            {
                Text = "Retry Initialization",
                Width = 200,
                Height = 40,
                Margin = new Padding(5, 5, 5, 15)
            };
            retryButton.Click += (s, e) => { Application.Restart(); };
            contentPanel.Controls.Add(retryButton);

            var exitButton = new Button
            {
                Text = "Exit Application",
                Width = 200,
                Height = 40,
                Margin = new Padding(5)
            };
            exitButton.Click += (s, e) => { Application.Exit(); };
            contentPanel.Controls.Add(exitButton);

            // Add panels to main layout
            mainPanel.Controls.Add(headerPanel, 0, 0);
            mainPanel.Controls.Add(contentPanel, 0, 1);

            this.Controls.Add(mainPanel);
            this.ResumeLayout(false);

            _logger?.LogInformation("Minimal UI created successfully");
        }        private void InitializeStandardMenus()
        {
            try
            {
                _logger?.LogInformation("Initializing standard Windows Forms menu event handlers");

                // Ensure the menu is visible and properly docked
                if (mainMenuStrip != null)
                {
                    mainMenuStrip.Visible = true;
                    mainMenuStrip.Dock = DockStyle.Top;
                }

                // Ensure the status strip is properly configured
                if (statusStrip != null)
                {
                    statusStrip.Visible = true;
                    statusStrip.Dock = DockStyle.Bottom;
                    this.Controls.Add(statusStrip);
                }

                // Configure handlers for the designer-created menu items with proper error handling
                if (reloadDatabaseToolStripMenuItem != null)
                    reloadDatabaseToolStripMenuItem.Click += (s, e) => RefreshDatabaseConnection();

                if (exitToolStripMenuItem != null)
                    exitToolStripMenuItem.Click += (s, e) => this.Close();

                if (routesToolStripMenuItem != null)
                    routesToolStripMenuItem.Click += (s, e) => OpenRouteManagement();

                // Set up navigation menu items
                ConfigureNavigationMenuItem(
                    activityTripsToolStripMenuItem,
                    "ActivityTripsForm",
                    () => _serviceProvider.GetRequiredService<ActivityTripsForm>().ShowDialog()
                );

                ConfigureNavigationMenuItem(
                    driversToolStripMenuItem,
                    "DriversManagementForm",
                    () => _serviceProvider.GetRequiredService<DriversManagementForm>().ShowDialog()
                );

                ConfigureNavigationMenuItem(
                    vehiclesToolStripMenuItem,
                    "VehiclesManagementForm",
                    () => _serviceProvider.GetRequiredService<VehiclesManagementForm>().ShowDialog()
                );

                if (aboutToolStripMenuItem != null)
                    aboutToolStripMenuItem.Click += (s, e) => {
                        MessageBox.Show("BusBuddy MVP Application\nVersion 1.0\n© 2023 BusBuddy Team",
                            "About BusBuddy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    };

                // Add Reports menu functionality if applicable
                if (reportsToolStripMenuItem != null)
                {
                    // Create a submenu for report types
                    reportsToolStripMenuItem.DropDownItems.Clear();

                    var dailyReportItem = new ToolStripMenuItem("&Daily Report");
                    dailyReportItem.Click += (s, e) => MessageBox.Show("Daily Report feature will be implemented in the next release.",
                        "Feature Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var weeklyReportItem = new ToolStripMenuItem("&Weekly Report");
                    weeklyReportItem.Click += (s, e) => MessageBox.Show("Weekly Report feature will be implemented in the next release.",
                        "Feature Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    reportsToolStripMenuItem.DropDownItems.Add(dailyReportItem);
                    reportsToolStripMenuItem.DropDownItems.Add(weeklyReportItem);
                }

                // Set initial status text
                if (statusLabel != null)
                    statusLabel.Text = "Ready";

                _logger?.LogInformation("Standard Windows Forms menu event handlers initialized successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error initializing standard menu handlers");
                // Continue without menu functionality if there's an error
            }
        }

        /// <summary>
        /// Helper method to configure navigation menu items with proper error handling
        /// </summary>
        private void ConfigureNavigationMenuItem(ToolStripMenuItem menuItem, string formName, Action openAction)
        {
            if (menuItem == null) return;

            menuItem.Click += (s, e) => {
                try
                {
                    if (statusLabel != null)
                        statusLabel.Text = $"Opening {formName}...";

                    openAction();

                    if (statusLabel != null)
                        statusLabel.Text = "Ready";
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"Error opening {formName}");
                    MessageBox.Show($"Error opening {formName}: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (statusLabel != null)
                        statusLabel.Text = $"Error opening {formName}";
                }
            };
        }

        private void RefreshDatabaseConnection()
        {
            try
            {
                var controls = this.Tag as object[];
                var statusLabel = controls?[2] as ToolStripStatusLabel;

                if (statusLabel != null)
                    statusLabel.Text = "Refreshing database connection...";

                // Test connection
                bool connected = _databaseHelper.TestConnection();

                if (connected)
                {
                    _logger?.LogInformation("Database connection refreshed successfully");
                    if (statusLabel != null)
                        statusLabel.Text = "Database connection refreshed successfully";
                    MessageBox.Show("Database connection refreshed successfully", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _logger?.LogWarning("Database connection refresh failed");
                    if (statusLabel != null)
                        statusLabel.Text = "Database connection failed";
                    MessageBox.Show("Failed to connect to database. Some features may not work properly.", "Database Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error refreshing database connection");
                MessageBox.Show($"Error refreshing database connection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
