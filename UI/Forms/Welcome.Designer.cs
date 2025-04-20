namespace BusBuddy.UI.Forms
{
    partial class Welcome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            // Form Properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimumSize = new System.Drawing.Size(1200, 800);
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BusBuddy - School Bus Management System";

            // Initialize components
            this.components = new System.ComponentModel.Container();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.headerLogoLabel = new System.Windows.Forms.Label();
            this.schoolNameLabel = new System.Windows.Forms.Label();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            // Daily Activities Panel
            this.dailyActivitiesPanel = new System.Windows.Forms.Panel();
            this.todaysActivitiesLabel = new System.Windows.Forms.Label();
            this.refreshTodayButton = new System.Windows.Forms.Button();
            this.todaysActivitiesGrid = new System.Windows.Forms.DataGridView();

            // System Stats Panel
            this.systemStatsPanel = new System.Windows.Forms.Panel();
            this.systemStatsLabel = new System.Windows.Forms.Label();
            this.statsLabel = new System.Windows.Forms.Label();

            // Main Buttons Panel and GroupBoxes
            this.mainButtonsPanel = new System.Windows.Forms.Panel();
            this.buttonGroupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonGroupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonGroupBox3 = new System.Windows.Forms.GroupBox();

            // Button GroupBox 1 - Trip Management
            this.routesButton = new System.Windows.Forms.Button();
            this.activitiesButton = new System.Windows.Forms.Button();
            this.schedulesButton = new System.Windows.Forms.Button();

            // Button GroupBox 2 - Fleet & Staff
            this.dataEntryButton = new System.Windows.Forms.Button();
            this.vehiclesButton = new System.Windows.Forms.Button();
            this.fuelButton = new System.Windows.Forms.Button();
            this.maintenanceButton = new System.Windows.Forms.Button();
            this.driversButton = new System.Windows.Forms.Button();

            // Button GroupBox 3 - System & Reports
            this.reportsButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.testDbButton = new System.Windows.Forms.Button();

            this.headerPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.dailyActivitiesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.todaysActivitiesGrid)).BeginInit();
            this.systemStatsPanel.SuspendLayout();
            this.mainButtonsPanel.SuspendLayout();
            this.buttonGroupBox1.SuspendLayout();
            this.buttonGroupBox2.SuspendLayout();
            this.buttonGroupBox3.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // 1. Header Panel - exactly 120 pixels high
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(0, 99, 177); // Deep blue
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Size = new System.Drawing.Size(1280, 120);
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Controls.Add(this.welcomeLabel);
            this.headerPanel.Controls.Add(this.headerLogoLabel);
            this.headerPanel.Controls.Add(this.schoolNameLabel);
            this.headerPanel.Controls.Add(this.dateTimeLabel);

            // Header Controls
            this.welcomeLabel.Text = "Welcome to BusBuddy Bus Management System";
            this.welcomeLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.ForeColor = System.Drawing.Color.White;
            this.welcomeLabel.Dock = System.Windows.Forms.DockStyle.None;
            this.welcomeLabel.Size = new System.Drawing.Size(1280, 50);
            this.welcomeLabel.Location = new System.Drawing.Point(0, 12);
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.welcomeLabel.Name = "welcomeLabel";

            this.headerLogoLabel.Text = "🚌 BusBuddy";
            this.headerLogoLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.headerLogoLabel.ForeColor = System.Drawing.Color.Yellow;
            this.headerLogoLabel.Location = new System.Drawing.Point(12, 9);
            this.headerLogoLabel.AutoSize = true;
            this.headerLogoLabel.Name = "headerLogoLabel";

            this.schoolNameLabel.Text = "Wiley School District";
            this.schoolNameLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.schoolNameLabel.ForeColor = System.Drawing.Color.White;
            this.schoolNameLabel.Size = new System.Drawing.Size(250, 23);
            this.schoolNameLabel.Location = new System.Drawing.Point(515, 62);
            this.schoolNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.schoolNameLabel.Name = "schoolNameLabel";

            this.dateTimeLabel.Text = "April 19, 2025";
            this.dateTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dateTimeLabel.ForeColor = System.Drawing.Color.White;
            this.dateTimeLabel.Location = new System.Drawing.Point(1076, 9);
            this.dateTimeLabel.Size = new System.Drawing.Size(192, 20);
            this.dateTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.dateTimeLabel.Name = "dateTimeLabel";

            // 2. Footer Panel - exactly 50 pixels high
            this.footerPanel.BackColor = System.Drawing.Color.FromArgb(0, 99, 177); // Deep blue
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Size = new System.Drawing.Size(1280, 50);
            this.footerPanel.Location = new System.Drawing.Point(0, 750);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Controls.Add(this.versionLabel);
            this.footerPanel.Controls.Add(this.exitButton);

            // Footer Controls
            this.versionLabel.Text = "Version 1.0.0";
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.versionLabel.ForeColor = System.Drawing.Color.White;
            this.versionLabel.Location = new System.Drawing.Point(12, 18);
            this.versionLabel.AutoSize = true;
            this.versionLabel.Name = "versionLabel";

            this.exitButton.Text = "Exit";
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(192, 0, 0); // Red
            this.exitButton.ForeColor = System.Drawing.Color.White;
            this.exitButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.exitButton.Location = new System.Drawing.Point(1180, 9);
            this.exitButton.Size = new System.Drawing.Size(90, 35);
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.Name = "exitButton";
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);

            // 3. Main Panel - exactly 630 pixels high with 30px padding on all sides
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240); // Light gray
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Size = new System.Drawing.Size(1280, 630);
            this.mainPanel.Location = new System.Drawing.Point(0, 120);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(30);
            this.mainPanel.Controls.Add(this.dailyActivitiesPanel);
            this.mainPanel.Controls.Add(this.systemStatsPanel);
            this.mainPanel.Controls.Add(this.mainButtonsPanel);

            // Daily Activities Panel - exactly 920x180 pixels
            this.dailyActivitiesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dailyActivitiesPanel.BackColor = System.Drawing.Color.White;
            this.dailyActivitiesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dailyActivitiesPanel.Location = new System.Drawing.Point(30, 30);
            this.dailyActivitiesPanel.Size = new System.Drawing.Size(920, 180);
            this.dailyActivitiesPanel.Name = "dailyActivitiesPanel";
            this.dailyActivitiesPanel.Controls.Add(this.todaysActivitiesLabel);
            this.dailyActivitiesPanel.Controls.Add(this.refreshTodayButton);
            this.dailyActivitiesPanel.Controls.Add(this.todaysActivitiesGrid);

            this.todaysActivitiesLabel.Text = "Today's Activities:";
            this.todaysActivitiesLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.todaysActivitiesLabel.ForeColor = System.Drawing.Color.FromArgb(0, 99, 177);
            this.todaysActivitiesLabel.Location = new System.Drawing.Point(10, 10);
            this.todaysActivitiesLabel.AutoSize = true;
            this.todaysActivitiesLabel.Name = "todaysActivitiesLabel";

            this.refreshTodayButton.Text = "Refresh";
            this.refreshTodayButton.BackColor = System.Drawing.Color.FromArgb(0, 122, 204); // Blue
            this.refreshTodayButton.ForeColor = System.Drawing.Color.White;
            this.refreshTodayButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.refreshTodayButton.Location = new System.Drawing.Point(820, 5);
            this.refreshTodayButton.Size = new System.Drawing.Size(90, 25);
            this.refreshTodayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshTodayButton.FlatAppearance.BorderSize = 0;
            this.refreshTodayButton.Name = "refreshTodayButton";
            this.refreshTodayButton.Click += new System.EventHandler(this.refreshTodayButton_Click);

            this.todaysActivitiesGrid.Location = new System.Drawing.Point(10, 35);
            this.todaysActivitiesGrid.Size = new System.Drawing.Size(898, 135);
            this.todaysActivitiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.todaysActivitiesGrid.BackgroundColor = System.Drawing.Color.White;
            this.todaysActivitiesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.todaysActivitiesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.todaysActivitiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.todaysActivitiesGrid.RowHeadersVisible = false;
            this.todaysActivitiesGrid.RowTemplate.Height = 29;
            this.todaysActivitiesGrid.AllowUserToAddRows = false;
            this.todaysActivitiesGrid.AllowUserToDeleteRows = false;
            this.todaysActivitiesGrid.AllowUserToResizeRows = false;
            this.todaysActivitiesGrid.ReadOnly = true;
            this.todaysActivitiesGrid.Name = "todaysActivitiesGrid";

            // System Stats Panel - exactly 280x180 pixels
            this.systemStatsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.systemStatsPanel.BackColor = System.Drawing.Color.White;
            this.systemStatsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.systemStatsPanel.Location = new System.Drawing.Point(970, 30);
            this.systemStatsPanel.Size = new System.Drawing.Size(280, 180);
            this.systemStatsPanel.Name = "systemStatsPanel";
            this.systemStatsPanel.Controls.Add(this.systemStatsLabel);
            this.systemStatsPanel.Controls.Add(this.statsLabel);

            this.systemStatsLabel.Text = "System Statistics:";
            this.systemStatsLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.systemStatsLabel.ForeColor = System.Drawing.Color.FromArgb(0, 99, 177);
            this.systemStatsLabel.Location = new System.Drawing.Point(10, 10);
            this.systemStatsLabel.AutoSize = true;
            this.systemStatsLabel.Name = "systemStatsLabel";

            this.statsLabel.Text = "Loading statistics...";
            this.statsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statsLabel.Location = new System.Drawing.Point(10, 35);
            this.statsLabel.Size = new System.Drawing.Size(260, 105);
            this.statsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statsLabel.Padding = new System.Windows.Forms.Padding(5);
            this.statsLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.statsLabel.Name = "statsLabel";

            // Main Buttons Panel - 920x370 pixels
            this.mainButtonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.mainButtonsPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.mainButtonsPanel.Location = new System.Drawing.Point(30, 230);
            this.mainButtonsPanel.Size = new System.Drawing.Size(1220, 370);
            this.mainButtonsPanel.Name = "mainButtonsPanel";
            this.mainButtonsPanel.Controls.Add(this.buttonGroupBox1);
            this.mainButtonsPanel.Controls.Add(this.buttonGroupBox2);
            this.mainButtonsPanel.Controls.Add(this.buttonGroupBox3);

            // Button GroupBox 1 - Trip Management - 260x200 pixels
            this.buttonGroupBox1.Text = "Trip Management";
            this.buttonGroupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonGroupBox1.Location = new System.Drawing.Point(20, 20);
            this.buttonGroupBox1.Size = new System.Drawing.Size(260, 200);
            this.buttonGroupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.buttonGroupBox1.Name = "buttonGroupBox1";
            this.buttonGroupBox1.Controls.Add(this.routesButton);
            this.buttonGroupBox1.Controls.Add(this.activitiesButton);
            this.buttonGroupBox1.Controls.Add(this.schedulesButton);

            this.routesButton.Text = "Routes";
            this.routesButton.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.routesButton.ForeColor = System.Drawing.Color.White;
            this.routesButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.routesButton.Location = new System.Drawing.Point(21, 32);
            this.routesButton.Size = new System.Drawing.Size(180, 45);
            this.routesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.routesButton.FlatAppearance.BorderSize = 0;
            this.routesButton.Name = "routesButton";
            this.routesButton.Click += new System.EventHandler(this.RoutesButton_Click);

            this.activitiesButton.Text = "Activities";
            this.activitiesButton.BackColor = System.Drawing.Color.FromArgb(80, 170, 220);
            this.activitiesButton.ForeColor = System.Drawing.Color.White;
            this.activitiesButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.activitiesButton.Location = new System.Drawing.Point(21, 83);
            this.activitiesButton.Size = new System.Drawing.Size(180, 40);
            this.activitiesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.activitiesButton.FlatAppearance.BorderSize = 0;
            this.activitiesButton.Name = "activitiesButton";
            this.activitiesButton.Click += new System.EventHandler(this.ActivitiesButton_Click);

            this.schedulesButton.Text = "School Calendar";
            this.schedulesButton.BackColor = System.Drawing.Color.FromArgb(80, 170, 220);
            this.schedulesButton.ForeColor = System.Drawing.Color.White;
            this.schedulesButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.schedulesButton.Location = new System.Drawing.Point(21, 129);
            this.schedulesButton.Size = new System.Drawing.Size(180, 40);
            this.schedulesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schedulesButton.FlatAppearance.BorderSize = 0;
            this.schedulesButton.Name = "schedulesButton";
            this.schedulesButton.Click += new System.EventHandler(this.SchedulesButton_Click);

            // Button GroupBox 2 - Fleet & Staff - 260x250 pixels
            this.buttonGroupBox2.Text = "Vehicle Management";
            this.buttonGroupBox2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonGroupBox2.Location = new System.Drawing.Point(320, 20);
            this.buttonGroupBox2.Size = new System.Drawing.Size(260, 250);
            this.buttonGroupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.buttonGroupBox2.Name = "buttonGroupBox2";
            this.buttonGroupBox2.Controls.Add(this.dataEntryButton);
            this.buttonGroupBox2.Controls.Add(this.vehiclesButton);
            this.buttonGroupBox2.Controls.Add(this.fuelButton);
            this.buttonGroupBox2.Controls.Add(this.maintenanceButton);
            this.buttonGroupBox2.Controls.Add(this.driversButton);

            this.dataEntryButton.Text = "Data Entry";
            this.dataEntryButton.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.dataEntryButton.ForeColor = System.Drawing.Color.White;
            this.dataEntryButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.dataEntryButton.Location = new System.Drawing.Point(21, 32);
            this.dataEntryButton.Size = new System.Drawing.Size(180, 40);
            this.dataEntryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dataEntryButton.FlatAppearance.BorderSize = 0;
            this.dataEntryButton.Name = "dataEntryButton";
            this.dataEntryButton.Click += new System.EventHandler(this.InputsButton_Click);

            this.vehiclesButton.Text = "Vehicles";
            this.vehiclesButton.BackColor = System.Drawing.Color.FromArgb(80, 170, 220);
            this.vehiclesButton.ForeColor = System.Drawing.Color.White;
            this.vehiclesButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.vehiclesButton.Location = new System.Drawing.Point(21, 78);
            this.vehiclesButton.Size = new System.Drawing.Size(180, 40);
            this.vehiclesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vehiclesButton.FlatAppearance.BorderSize = 0;
            this.vehiclesButton.Name = "vehiclesButton";

            this.fuelButton.Text = "Fuel";
            this.fuelButton.BackColor = System.Drawing.Color.FromArgb(80, 170, 220);
            this.fuelButton.ForeColor = System.Drawing.Color.White;
            this.fuelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.fuelButton.Location = new System.Drawing.Point(21, 124);
            this.fuelButton.Size = new System.Drawing.Size(180, 40);
            this.fuelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelButton.FlatAppearance.BorderSize = 0;
            this.fuelButton.Name = "fuelButton";
            this.fuelButton.Click += new System.EventHandler(this.FuelButton_Click);

            this.maintenanceButton.Text = "Maintenance";
            this.maintenanceButton.BackColor = System.Drawing.Color.FromArgb(80, 170, 220);
            this.maintenanceButton.ForeColor = System.Drawing.Color.White;
            this.maintenanceButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.maintenanceButton.Location = new System.Drawing.Point(21, 170);
            this.maintenanceButton.Size = new System.Drawing.Size(180, 40);
            this.maintenanceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maintenanceButton.FlatAppearance.BorderSize = 0;
            this.maintenanceButton.Name = "maintenanceButton";

            this.driversButton.Text = "Drivers";
            this.driversButton.BackColor = System.Drawing.Color.FromArgb(80, 170, 220);
            this.driversButton.ForeColor = System.Drawing.Color.White;
            this.driversButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.driversButton.Location = new System.Drawing.Point(21, 216);
            this.driversButton.Size = new System.Drawing.Size(180, 40);
            this.driversButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driversButton.FlatAppearance.BorderSize = 0;
            this.driversButton.Name = "driversButton";
            this.driversButton.Click += new System.EventHandler(this.DriverButton_Click);

            // Button GroupBox 3 - System & Reports - 260x250 pixels
            this.buttonGroupBox3.Text = "System & Reports";
            this.buttonGroupBox3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonGroupBox3.Location = new System.Drawing.Point(620, 20);
            this.buttonGroupBox3.Size = new System.Drawing.Size(260, 250);
            this.buttonGroupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.buttonGroupBox3.Name = "buttonGroupBox3";
            this.buttonGroupBox3.Controls.Add(this.reportsButton);
            this.buttonGroupBox3.Controls.Add(this.settingsButton);
            this.buttonGroupBox3.Controls.Add(this.testDbButton);

            this.reportsButton.Text = "Reports";
            this.reportsButton.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.reportsButton.ForeColor = System.Drawing.Color.White;
            this.reportsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.reportsButton.Location = new System.Drawing.Point(21, 32);
            this.reportsButton.Size = new System.Drawing.Size(180, 45);
            this.reportsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportsButton.FlatAppearance.BorderSize = 0;
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Click += new System.EventHandler(this.ReportsButton_Click);

            this.settingsButton.Text = "Settings";
            this.settingsButton.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.settingsButton.ForeColor = System.Drawing.Color.White;
            this.settingsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.settingsButton.Location = new System.Drawing.Point(21, 83);
            this.settingsButton.Size = new System.Drawing.Size(180, 40);
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);

            this.testDbButton.Text = "Test Database";
            this.testDbButton.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            this.testDbButton.ForeColor = System.Drawing.Color.White;
            this.testDbButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.testDbButton.Location = new System.Drawing.Point(21, 175);
            this.testDbButton.Size = new System.Drawing.Size(180, 30);
            this.testDbButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testDbButton.FlatAppearance.BorderSize = 0;
            this.testDbButton.Name = "testDbButton";
            this.testDbButton.Click += new System.EventHandler(this.TestDbButton_Click);

            // Status Strip
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusStrip.Location = new System.Drawing.Point(0, 728);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1280, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 11;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.statusLabel
            });

            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Text = "Ready";

            // Add controls to form
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.statusStrip);

            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.footerPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.dailyActivitiesPanel.ResumeLayout(false);
            this.dailyActivitiesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.todaysActivitiesGrid)).EndInit();
            this.systemStatsPanel.ResumeLayout(false);
            this.systemStatsPanel.PerformLayout();
            this.mainButtonsPanel.ResumeLayout(false);
            this.buttonGroupBox1.ResumeLayout(false);
            this.buttonGroupBox2.ResumeLayout(false);
            this.buttonGroupBox3.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Label headerLogoLabel;
        private System.Windows.Forms.Label schoolNameLabel;
        private System.Windows.Forms.Label dateTimeLabel;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel dailyActivitiesPanel;
        private System.Windows.Forms.Label todaysActivitiesLabel;
        private System.Windows.Forms.Button refreshTodayButton;
        private System.Windows.Forms.DataGridView todaysActivitiesGrid;
        private System.Windows.Forms.Panel systemStatsPanel;
        private System.Windows.Forms.Label systemStatsLabel;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.Panel mainButtonsPanel;
        private System.Windows.Forms.GroupBox buttonGroupBox1;
        private System.Windows.Forms.Button routesButton;
        private System.Windows.Forms.Button activitiesButton;
        private System.Windows.Forms.Button schedulesButton;
        private System.Windows.Forms.GroupBox buttonGroupBox2;
        private System.Windows.Forms.Button dataEntryButton;
        private System.Windows.Forms.Button vehiclesButton;
        private System.Windows.Forms.Button fuelButton;
        private System.Windows.Forms.Button maintenanceButton;
        private System.Windows.Forms.Button driversButton;
        private System.Windows.Forms.GroupBox buttonGroupBox3;
        private System.Windows.Forms.Button reportsButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button testDbButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private new System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}