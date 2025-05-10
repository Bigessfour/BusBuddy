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

            // Add chart placeholder to Analytics tab
            InitializeAnalyticsTab();
        }

        private void InitializeAnalyticsTab()
        {
            try {
                // Create a panel for the fuel trend chart
                fuelTrendChartPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 250,
                    BackColor = Color.White,
                    Margin = new Padding(10)
                };
                
                // Add a label as a placeholder for the chart
                fuelTrendChartLabel = new Label
                {
                    Text = "Fuel Consumption Trends (Chart Placeholder)",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.DarkBlue,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Height = 30
                };
                
                // Create a sample representation of a chart with basic shapes
                Panel chartPlaceholder = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.WhiteSmoke,
                    Margin = new Padding(10)
                };
                
                chartPlaceholder.Paint += (sender, e) => {
                    // Draw a simple bar chart as a placeholder
                    Graphics g = e.Graphics;
                    int barWidth = 40;
                    int spacing = 20;
                    int startX = 50;
                    int baseY = chartPlaceholder.Height - 50;
                    
                    // Draw X and Y axes
                    g.DrawLine(Pens.Black, 40, 30, 40, baseY);
                    g.DrawLine(Pens.Black, 40, baseY, chartPlaceholder.Width - 30, baseY);
                    
                    // Draw bars representing fuel consumption
                    Random rand = new Random(42); // Fixed seed for consistent results
                    for (int i = 0; i < 6; i++)
                    {
                        int barHeight = rand.Next(50, 150);
                        g.FillRectangle(Brushes.SkyBlue, startX + i * (barWidth + spacing), 
                            baseY - barHeight, barWidth, barHeight);
                        g.DrawRectangle(Pens.Blue, startX + i * (barWidth + spacing), 
                            baseY - barHeight, barWidth, barHeight);
                        
                        // Month labels
                        string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
                        g.DrawString(months[i], new Font("Arial", 8), Brushes.Black, 
                            startX + i * (barWidth + spacing) + barWidth/2 - 10, baseY + 5);
                    }
                    
                    // Add Y-axis labels
                    g.DrawString("Gallons", new Font("Arial", 8), Brushes.Black, 5, baseY/2);
                    g.DrawString("Months", new Font("Arial", 8), Brushes.Black, chartPlaceholder.Width/2, baseY + 20);
                    
                    // Add chart title
                    g.DrawString("Monthly Fuel Consumption Trends", new Font("Arial", 12, FontStyle.Bold), 
                        Brushes.DarkBlue, chartPlaceholder.Width/2 - 100, 5);
                };
                
                // Add "View Detailed Reports" button
                btnViewDetailedReports = new Button
                {
                    Text = "View Detailed Reports",
                    Size = new Size(200, 40),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(tabAnalytics.Width - 220, fuelTrendChartPanel.Bottom + 20),
                    BackColor = Color.LightBlue,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                
                btnViewDetailedReports.Click += (sender, e) => {
                    try {
                        _logger.LogInformation("Opening detailed reports form");
                        
                        // Create and show the reports form
                        using (var reportsForm = new ReportsForm(_databaseHelper, _serviceProvider.GetService<ILogger<ReportsForm>>()))
                        {
                            reportsForm.ShowDialog();
                        }
                    }
                    catch (Exception ex) {
                        _logger.LogError(ex, "Error opening reports form");
                        
                        // Log to the error file directly
                        string errorMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error opening reports form: {ex.Message}";
                        System.IO.File.AppendAllText("busbuddy_errors.log", errorMessage + Environment.NewLine);
                        
                        MessageBox.Show("Reports functionality will be implemented in a future update.", 
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };
                
                // Add the components to the panel and tab
                fuelTrendChartPanel.Controls.Add(chartPlaceholder);
                fuelTrendChartPanel.Controls.Add(fuelTrendChartLabel);
                tabAnalytics.Controls.Add(btnViewDetailedReports);
                tabAnalytics.Controls.Add(fuelTrendChartPanel);
                
                _logger.LogInformation("Analytics tab components initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing analytics tab");
                
                // Write to the error log file directly
                string errorMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error initializing analytics tab: {ex.Message}";
                System.IO.File.AppendAllText("busbuddy_errors.log", errorMessage + Environment.NewLine);
                
                MessageBox.Show($"Error initializing analytics features: {ex.Message}", 
                    "Analytics Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
