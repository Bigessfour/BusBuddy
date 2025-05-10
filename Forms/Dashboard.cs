using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using BusBuddy.Data;
using BusBuddy.Data.Interfaces;
using MaterialSkin;
using MaterialSkin.Controls;

namespace BusBuddy.Forms
{    
    public partial class Dashboard : MaterialForm
    {
        private readonly IDatabaseHelper _databaseHelper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Dashboard> _logger;
        private BindingSource bindingSource = new BindingSource();

        // MaterialSkin.2 UI enhancement: Add MaterialTabControl for Tracking and Analytics
        private MaterialTabControl materialTabControl;
        private MaterialTabSelector materialTabSelector;
        private TabPage tabTracking;
        private TabPage tabAnalytics;
        
        // Analytics placeholders
        private Panel fuelTrendChartPanel;
        private Label fuelTrendChartLabel;
        private Button btnViewDetailedReports;
        
        // GPS tracking placeholders
        private Panel mapPanel;
        private Label mapPlaceholderLabel;
        private Button btnRefreshMap;
        private Button btnTrackVehicle;
        private ComboBox cboVehicles;

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
                ApplyMaterialSkinTheme();
                InitializeMaterialTabs();

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

        private void InitializeMaterialTabs()
        {
            materialTabControl = new MaterialTabControl();
            materialTabSelector = new MaterialTabSelector();
            tabTracking = new TabPage { Text = "Tracking" };
            tabAnalytics = new TabPage { Text = "Analytics" };
            materialTabControl.Controls.Add(tabTracking);
            materialTabControl.Controls.Add(tabAnalytics);
            materialTabControl.Depth = 0;
            materialTabControl.Location = new System.Drawing.Point(200, 60);
            materialTabControl.Size = new System.Drawing.Size(600, 400);
            materialTabControl.TabIndex = 0;
            materialTabSelector.BaseTabControl = materialTabControl;
            materialTabSelector.Location = new System.Drawing.Point(200, 30);
            materialTabSelector.Size = new System.Drawing.Size(600, 30);
            this.Controls.Add(materialTabSelector);
            this.Controls.Add(materialTabControl);
            _logger.LogInformation("Added MaterialTabControl to Dashboard");

            // Add map placeholder to Tracking tab
            InitializeTrackingTab();
        }
        
        /// <summary>
        /// Initializes the tracking tab with GPS map placeholder
        /// </summary>
        private void InitializeTrackingTab()
        {
            try
            {
                // Create map panel as a placeholder for future GMap.NET integration
                mapPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.AliceBlue,
                    BorderStyle = BorderStyle.FixedSingle
                };
                
                // Add a placeholder label
                mapPlaceholderLabel = new Label
                {
                    Text = "GPS Tracking Map Placeholder\n\nThis area will display real-time vehicle locations\nusing GMap.NET or similar library in future implementation.",
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    ForeColor = Color.Navy,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                
                // Create controls panel for tracking options
                Panel trackingControlsPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 60,
                    BackColor = Color.WhiteSmoke
                };
                
                // Vehicle selector dropdown
                Label vehicleLabel = new Label
                {
                    Text = "Select Vehicle:",
                    Location = new Point(10, 20),
                    AutoSize = true
                };
                
                cboVehicles = new ComboBox
                {
                    Location = new Point(110, 16),
                    Width = 150,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                
                // Add sample vehicles (would be populated from database in real implementation)
                cboVehicles.Items.AddRange(new string[] { "Bus 001", "Bus 002", "Bus 003", "All Vehicles" });
                cboVehicles.SelectedIndex = 3;  // Default to All Vehicles
                
                // Add tracking buttons
                btnTrackVehicle = new Button
                {
                    Text = "Track Vehicle",
                    Location = new Point(280, 15),
                    Size = new Size(120, 30)
                };
                
                btnRefreshMap = new Button
                {
                    Text = "Refresh Map",
                    Location = new Point(420, 15),
                    Size = new Size(120, 30)
                };
                
                // Wire up event handlers
                btnTrackVehicle.Click += (sender, e) => {
                    string selectedVehicle = cboVehicles.SelectedItem.ToString();
                    _logger.LogInformation($"Track vehicle selected: {selectedVehicle}");
                    
                    // Log to error file as requested
                    string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Track vehicle requested: {selectedVehicle}";
                    System.IO.File.AppendAllText("busbuddy_errors.log", logMessage + Environment.NewLine);
                    
                    // Update the map placeholder with vehicle info
                    DrawVehicleLocationOnMap(selectedVehicle);
                };
                
                btnRefreshMap.Click += (sender, e) => {
                    _logger.LogInformation("Refresh map requested");
                    
                    // Log to error file as requested
                    string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Map refresh requested";
                    System.IO.File.AppendAllText("busbuddy_errors.log", logMessage + Environment.NewLine);
                    
                    // Simulate map refresh
                    mapPlaceholderLabel.Text = "Map refreshed at " + DateTime.Now.ToString("HH:mm:ss") + 
                        "\n\nGPS Tracking Map Placeholder\nThis is a simulated map refresh.";
                };
                
                // Add controls to tracking controls panel
                trackingControlsPanel.Controls.Add(vehicleLabel);
                trackingControlsPanel.Controls.Add(cboVehicles);
                trackingControlsPanel.Controls.Add(btnTrackVehicle);
                trackingControlsPanel.Controls.Add(btnRefreshMap);
                
                // Add map placeholder and controls to tracking tab
                mapPanel.Controls.Add(mapPlaceholderLabel);
                tabTracking.Controls.Add(mapPanel);
                tabTracking.Controls.Add(trackingControlsPanel);
                
                _logger.LogInformation("Tracking tab initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing tracking tab");
                
                // Log to error file as requested
                string errorMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error initializing tracking tab: {ex.Message}";
                System.IO.File.AppendAllText("busbuddy_errors.log", errorMessage + Environment.NewLine);
                
                MessageBox.Show($"Error initializing tracking features: {ex.Message}", 
                    "Tracking Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Simulates drawing a vehicle location on the map
        /// </summary>
        private void DrawVehicleLocationOnMap(string vehicleId)
        {
            try
            {
                // In a real implementation, this would use GMap.NET to plot vehicle location
                // For now, we'll just update the placeholder text
                
                mapPanel.Paint += (sender, e) => {
                    Graphics g = e.Graphics;
                    
                    // Draw a simple map background
                    g.FillRectangle(Brushes.LightBlue, 50, 50, mapPanel.Width - 100, mapPanel.Height - 100);
                    g.DrawRectangle(Pens.Blue, 50, 50, mapPanel.Width - 100, mapPanel.Height - 100);
                    
                    // Add some "roads"
                    g.DrawLine(Pens.Gray, 100, 100, mapPanel.Width - 100, 100);
                    g.DrawLine(Pens.Gray, 100, 150, mapPanel.Width - 100, 150);
                    g.DrawLine(Pens.Gray, 100, 200, mapPanel.Width - 100, 200);
                    g.DrawLine(Pens.Gray, 100, 100, 100, mapPanel.Height - 100);
                    g.DrawLine(Pens.Gray, 200, 100, 200, mapPanel.Height - 100);
                    
                    // Draw a "vehicle" at a random position
                    Random rand = new Random();
                    int x = rand.Next(100, mapPanel.Width - 120);
                    int y = rand.Next(100, mapPanel.Height - 120);
                    
                    g.FillEllipse(Brushes.Red, x, y, 20, 20);
                    g.DrawEllipse(Pens.Black, x, y, 20, 20);
                    g.DrawString(vehicleId, new Font("Arial", 8), Brushes.Black, x - 10, y + 25);
                    
                    // Add a legend
                    g.DrawString("Map Legend:", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 60, 60);
                    g.DrawString("● Vehicle Location", new Font("Arial", 8), Brushes.Black, 70, 80);
                    g.DrawString("— Roads", new Font("Arial", 8), Brushes.Black, 70, 100);
                };
                
                // Force repaint to show the "vehicle"
                mapPanel.Invalidate();
                
                _logger.LogInformation($"Drew vehicle {vehicleId} on map");
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Vehicle {vehicleId} drawn on map";
                System.IO.File.AppendAllText("busbuddy_errors.log", logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error drawing vehicle on map");
                string errorMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error drawing vehicle on map: {ex.Message}";
                System.IO.File.AppendAllText("busbuddy_errors.log", errorMessage + Environment.NewLine);
            }
        }

        private void ApplyMaterialSkinTheme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Grey800, Primary.Grey900,
                Primary.Grey500, Accent.Orange700,
                TextShade.WHITE);
        }

        /// <summary>
        /// TODO: Integrate GMap.NET when validated.
        /// </summary>
        public void InitializeMapPanel()
        {
            // Placeholder for future map panel integration
            _logger.LogInformation("InitializeMapPanel placeholder called");
        }
    }
}
