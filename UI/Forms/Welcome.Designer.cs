#nullable enable
namespace BusBuddy.UI.Forms
{
    partial class Welcome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.schoolNameLabel = new System.Windows.Forms.Label();
            this.headerLogoLabel = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.systemStatsPanel = new System.Windows.Forms.Panel();
            this.statsLabel = new System.Windows.Forms.Label();
            this.systemStatsLabel = new System.Windows.Forms.Label();
            this.dailyActivitiesPanel = new System.Windows.Forms.Panel();
            this.refreshTodayButton = new System.Windows.Forms.Button();
            this.todaysActivitiesGrid = new System.Windows.Forms.DataGridView();
            this.todaysActivitiesLabel = new System.Windows.Forms.Label();
            this.mainButtonsPanel = new System.Windows.Forms.Panel();
            this.buttonGroupBox3 = new System.Windows.Forms.GroupBox();
            this.testDbButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.schedulesButton = new System.Windows.Forms.Button();
            this.reportsButton = new System.Windows.Forms.Button();
            this.buttonGroupBox2 = new System.Windows.Forms.GroupBox();
            this.driversButton = new System.Windows.Forms.Button();
            this.maintenanceButton = new System.Windows.Forms.Button();
            this.fuelButton = new System.Windows.Forms.Button();
            this.vehiclesButton = new System.Windows.Forms.Button();
            this.buttonGroupBox1 = new System.Windows.Forms.GroupBox();
            this.tripsButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.exitButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.headerPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.systemStatsPanel.SuspendLayout();
            this.dailyActivitiesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.todaysActivitiesGrid)).BeginInit();
            this.mainButtonsPanel.SuspendLayout();
            this.buttonGroupBox3.SuspendLayout();
            this.buttonGroupBox2.SuspendLayout();
            this.buttonGroupBox1.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(99)))), ((int)(((byte)(177)))));
            this.headerPanel.Controls.Add(this.dateTimeLabel);
            this.headerPanel.Controls.Add(this.welcomeLabel);
            this.headerPanel.Controls.Add(this.schoolNameLabel);
            this.headerPanel.Controls.Add(this.headerLogoLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1264, 120);
            this.headerPanel.TabIndex = 0;
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dateTimeLabel.ForeColor = System.Drawing.Color.White;
            this.dateTimeLabel.Location = new System.Drawing.Point(776, 9);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(476, 23);
            this.dateTimeLabel.TabIndex = 3;
            this.dateTimeLabel.Text = "April 15, 2025";
            this.dateTimeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.welcomeLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.ForeColor = System.Drawing.Color.White;
            this.welcomeLabel.Location = new System.Drawing.Point(0, 0);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(1264, 120);
            this.welcomeLabel.TabIndex = 2;
            this.welcomeLabel.Text = "Welcome to BusBuddy";
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.headerLogoLabel.BringToFront();
            this.schoolNameLabel.BringToFront();
            this.dateTimeLabel.BringToFront();
            // 
            // schoolNameLabel
            // 
            this.schoolNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.schoolNameLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.schoolNameLabel.ForeColor = System.Drawing.Color.White;
            this.schoolNameLabel.AutoSize = true;
            this.schoolNameLabel.Location = new System.Drawing.Point((1264 - this.schoolNameLabel.Width) / 2, 50);
            this.schoolNameLabel.Name = "schoolNameLabel";
            this.schoolNameLabel.TabIndex = 1;
            this.schoolNameLabel.Text = "Wiley School District";
            this.schoolNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // headerLogoLabel
            // 
            this.headerLogoLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.headerLogoLabel.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Bold);
            this.headerLogoLabel.ForeColor = System.Drawing.Color.Yellow;
            this.headerLogoLabel.Location = new System.Drawing.Point(0, 10);
            this.headerLogoLabel.Name = "headerLogoLabel";
            this.headerLogoLabel.Size = new System.Drawing.Size(1264, 80);
            this.headerLogoLabel.TabIndex = 0;
            this.headerLogoLabel.Text = "🚌 BusBuddy";
            this.headerLogoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.mainPanel.Controls.Add(this.systemStatsPanel);
            this.mainPanel.Controls.Add(this.dailyActivitiesPanel);
            this.mainPanel.Controls.Add(this.mainButtonsPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 120);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(1264, 591);
            this.mainPanel.TabIndex = 1;
            // 
            // systemStatsPanel
            // 
            this.systemStatsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.systemStatsPanel.BackColor = System.Drawing.Color.White;
            this.systemStatsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.systemStatsPanel.Controls.Add(this.statsLabel);
            this.systemStatsPanel.Controls.Add(this.systemStatsLabel);
            this.systemStatsPanel.Location = new System.Drawing.Point(974, 391);
            this.systemStatsPanel.Name = "systemStatsPanel";
            this.systemStatsPanel.Size = new System.Drawing.Size(280, 190);
            this.systemStatsPanel.TabIndex = 2;
            // 
            // statsLabel
            // 
            this.statsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statsLabel.Location = new System.Drawing.Point(10, 35);
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(260, 105);
            this.statsLabel.TabIndex = 1;
            this.statsLabel.Text = "Loading statistics...";
            // 
            // systemStatsLabel
            // 
            this.systemStatsLabel.AutoSize = true;
            this.systemStatsLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.systemStatsLabel.Location = new System.Drawing.Point(10, 10);
            this.systemStatsLabel.Name = "systemStatsLabel";
            this.systemStatsLabel.Size = new System.Drawing.Size(154, 23);
            this.systemStatsLabel.TabIndex = 0;
            this.systemStatsLabel.Text = "System Statistics:";
            // 
            // dailyActivitiesPanel
            // 
            this.dailyActivitiesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dailyActivitiesPanel.BackColor = System.Drawing.Color.White;
            this.dailyActivitiesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dailyActivitiesPanel.Controls.Add(this.refreshTodayButton);
            this.dailyActivitiesPanel.Controls.Add(this.todaysActivitiesGrid);
            this.dailyActivitiesPanel.Controls.Add(this.todaysActivitiesLabel);
            this.dailyActivitiesPanel.Location = new System.Drawing.Point(10, 10);
            this.dailyActivitiesPanel.Name = "dailyActivitiesPanel";
            this.dailyActivitiesPanel.Size = new System.Drawing.Size(1244, 180);
            this.dailyActivitiesPanel.TabIndex = 1;
            // 
            // refreshTodayButton
            // 
            this.refreshTodayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshTodayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.refreshTodayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshTodayButton.FlatAppearance.BorderSize = 0;
            this.refreshTodayButton.ForeColor = System.Drawing.Color.White;
            this.refreshTodayButton.Location = new System.Drawing.Point(1144, 5);
            this.refreshTodayButton.Name = "refreshTodayButton";
            this.refreshTodayButton.Size = new System.Drawing.Size(90, 30);
            this.refreshTodayButton.TabIndex = 2;
            this.refreshTodayButton.Text = "Refresh";
            this.refreshTodayButton.UseVisualStyleBackColor = false;
            this.refreshTodayButton.Click += new System.EventHandler(this.refreshTodayButton_Click);
            // 
            // todaysActivitiesGrid
            // 
            this.todaysActivitiesGrid.AllowUserToAddRows = false;
            this.todaysActivitiesGrid.AllowUserToDeleteRows = false;
            this.todaysActivitiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.todaysActivitiesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.todaysActivitiesGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.todaysActivitiesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.todaysActivitiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.todaysActivitiesGrid.Location = new System.Drawing.Point(10, 40);
            this.todaysActivitiesGrid.Name = "todaysActivitiesGrid";
            this.todaysActivitiesGrid.ReadOnly = true;
            this.todaysActivitiesGrid.RowHeadersVisible = false;
            this.todaysActivitiesGrid.RowHeadersWidth = 51;
            this.todaysActivitiesGrid.RowTemplate.Height = 29;
            this.todaysActivitiesGrid.Size = new System.Drawing.Size(1224, 130);
            this.todaysActivitiesGrid.TabIndex = 1;
            // 
            // todaysActivitiesLabel
            // 
            this.todaysActivitiesLabel.AutoSize = true;
            this.todaysActivitiesLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.todaysActivitiesLabel.Location = new System.Drawing.Point(10, 10);
            this.todaysActivitiesLabel.Name = "todaysActivitiesLabel";
            this.todaysActivitiesLabel.Size = new System.Drawing.Size(153, 23);
            this.todaysActivitiesLabel.TabIndex = 0;
            this.todaysActivitiesLabel.Text = "Today's Activities:";
            // 
            // mainButtonsPanel
            // 
            this.mainButtonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainButtonsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.mainButtonsPanel.Controls.Add(this.buttonGroupBox3);
            this.mainButtonsPanel.Controls.Add(this.buttonGroupBox2);
            this.mainButtonsPanel.Controls.Add(this.buttonGroupBox1);
            this.mainButtonsPanel.Location = new System.Drawing.Point(10, 200);
            this.mainButtonsPanel.Name = "mainButtonsPanel";
            this.mainButtonsPanel.Size = new System.Drawing.Size(954, 381);
            this.mainButtonsPanel.TabIndex = 0;
            // 
            // buttonGroupBox3
            // 
            this.buttonGroupBox3.Controls.Add(this.testDbButton);
            this.buttonGroupBox3.Controls.Add(this.settingsButton);
            this.buttonGroupBox3.Controls.Add(this.schedulesButton);
            this.buttonGroupBox3.Controls.Add(this.reportsButton);
            this.buttonGroupBox3.Location = new System.Drawing.Point(570, 10);
            this.buttonGroupBox3.Name = "buttonGroupBox3";
            this.buttonGroupBox3.Size = new System.Drawing.Size(270, 250);
            this.buttonGroupBox3.TabIndex = 2;
            this.buttonGroupBox3.TabStop = false;
            this.buttonGroupBox3.Text = "System && Reports";
            // 
            // testDbButton
            // 
            this.testDbButton.Location = new System.Drawing.Point(20, 195);
            this.testDbButton.Name = "testDbButton";
            this.testDbButton.Size = new System.Drawing.Size(230, 40);
            this.testDbButton.TabIndex = 3;
            this.testDbButton.Text = "Test DB Connection";
            this.testDbButton.UseVisualStyleBackColor = true;
            this.testDbButton.Click += new System.EventHandler(this.TestDbButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(20, 135);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(230, 40);
            this.settingsButton.TabIndex = 2;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            // 
            // schedulesButton
            // 
            this.schedulesButton.Location = new System.Drawing.Point(20, 75);
            this.schedulesButton.Name = "schedulesButton";
            this.schedulesButton.Size = new System.Drawing.Size(230, 40);
            this.schedulesButton.TabIndex = 1;
            this.schedulesButton.Text = "Schedules / Calendar";
            this.schedulesButton.UseVisualStyleBackColor = true;
            this.schedulesButton.Click += new System.EventHandler(this.SchedulesButton_Click);
            // 
            // reportsButton
            // 
            this.reportsButton.Location = new System.Drawing.Point(20, 25);
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Size = new System.Drawing.Size(230, 40);
            this.reportsButton.TabIndex = 0;
            this.reportsButton.Text = "Reports";
            this.reportsButton.UseVisualStyleBackColor = true;
            // 
            // buttonGroupBox2
            // 
            this.buttonGroupBox2.Controls.Add(this.driversButton);
            this.buttonGroupBox2.Controls.Add(this.maintenanceButton);
            this.buttonGroupBox2.Controls.Add(this.fuelButton);
            this.buttonGroupBox2.Controls.Add(this.vehiclesButton);
            this.buttonGroupBox2.Location = new System.Drawing.Point(290, 10);
            this.buttonGroupBox2.Name = "buttonGroupBox2";
            this.buttonGroupBox2.Size = new System.Drawing.Size(270, 250);
            this.buttonGroupBox2.TabIndex = 1;
            this.buttonGroupBox2.TabStop = false;
            this.buttonGroupBox2.Text = "Fleet && Staff";
            // 
            // driversButton
            // 
            this.driversButton.Location = new System.Drawing.Point(20, 195);
            this.driversButton.Name = "driversButton";
            this.driversButton.Size = new System.Drawing.Size(230, 40);
            this.driversButton.TabIndex = 3;
            this.driversButton.Text = "Drivers";
            this.driversButton.UseVisualStyleBackColor = true;
            // 
            // maintenanceButton
            // 
            this.maintenanceButton.Location = new System.Drawing.Point(20, 135);
            this.maintenanceButton.Name = "maintenanceButton";
            this.maintenanceButton.Size = new System.Drawing.Size(230, 40);
            this.maintenanceButton.TabIndex = 2;
            this.maintenanceButton.Text = "Maintenance";
            this.maintenanceButton.UseVisualStyleBackColor = true;
            // 
            // fuelButton
            // 
            this.fuelButton.Location = new System.Drawing.Point(20, 75);
            this.fuelButton.Name = "fuelButton";
            this.fuelButton.Size = new System.Drawing.Size(230, 40);
            this.fuelButton.TabIndex = 1;
            this.fuelButton.Text = "Fuel";
            this.fuelButton.UseVisualStyleBackColor = true;
            // 
            // vehiclesButton
            // 
            this.vehiclesButton.Location = new System.Drawing.Point(20, 25);
            this.vehiclesButton.Name = "vehiclesButton";
            this.vehiclesButton.Size = new System.Drawing.Size(230, 40);
            this.vehiclesButton.TabIndex = 0;
            this.vehiclesButton.Text = "Vehicles";
            this.vehiclesButton.UseVisualStyleBackColor = true;
            // 
            // buttonGroupBox1
            // 
            this.buttonGroupBox1.Controls.Add(this.tripsButton);
            this.buttonGroupBox1.Controls.Add(this.startButton);
            this.buttonGroupBox1.Location = new System.Drawing.Point(10, 10);
            this.buttonGroupBox1.Name = "buttonGroupBox1";
            this.buttonGroupBox1.Size = new System.Drawing.Size(270, 150);
            this.buttonGroupBox1.TabIndex = 0;
            this.buttonGroupBox1.TabStop = false;
            this.buttonGroupBox1.Text = "Trip Management";
            // 
            // tripsButton
            // 
            this.tripsButton.Location = new System.Drawing.Point(20, 85);
            this.tripsButton.Name = "tripsButton";
            this.tripsButton.Size = new System.Drawing.Size(230, 40);
            this.tripsButton.TabIndex = 1;
            this.tripsButton.Text = "Manage Trips";
            this.tripsButton.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(20, 25);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(230, 40);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "View Scheduled Routes";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // footerPanel
            // 
            this.footerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(99)))), ((int)(((byte)(177)))));
            this.footerPanel.Controls.Add(this.exitButton);
            this.footerPanel.Controls.Add(this.versionLabel);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 711);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new System.Drawing.Size(1264, 50);
            this.footerPanel.TabIndex = 2;
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.exitButton.ForeColor = System.Drawing.Color.White;
            this.exitButton.Location = new System.Drawing.Point(1180, 9);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 32);
            this.exitButton.TabIndex = 1;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.versionLabel.ForeColor = System.Drawing.Color.White;
            this.versionLabel.Location = new System.Drawing.Point(12, 18);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(101, 20);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "Version 1.0.0";
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 800);
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BusBuddy - School Bus Management System";
            this.Load += new System.EventHandler(this.Welcome_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.systemStatsPanel.ResumeLayout(false);
            this.systemStatsPanel.PerformLayout();
            this.dailyActivitiesPanel.ResumeLayout(false);
            this.dailyActivitiesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.todaysActivitiesGrid)).EndInit();
            this.mainButtonsPanel.ResumeLayout(false);
            this.buttonGroupBox3.ResumeLayout(false);
            this.buttonGroupBox2.ResumeLayout(false);
            this.buttonGroupBox1.ResumeLayout(false);
            this.footerPanel.ResumeLayout(false);
            this.footerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label headerLogoLabel;
        private System.Windows.Forms.Label schoolNameLabel;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Label dateTimeLabel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel mainButtonsPanel;
        private System.Windows.Forms.GroupBox buttonGroupBox1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button tripsButton;
        private System.Windows.Forms.GroupBox buttonGroupBox2;
        private System.Windows.Forms.Button vehiclesButton;
        private System.Windows.Forms.Button fuelButton;
        private System.Windows.Forms.Button maintenanceButton;
        private System.Windows.Forms.Button driversButton;
        private System.Windows.Forms.GroupBox buttonGroupBox3;
        private System.Windows.Forms.Button reportsButton;
        private System.Windows.Forms.Button schedulesButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button testDbButton;
        private System.Windows.Forms.Panel dailyActivitiesPanel;
        private System.Windows.Forms.Label todaysActivitiesLabel;
        private System.Windows.Forms.DataGridView todaysActivitiesGrid;
        private System.Windows.Forms.Button refreshTodayButton;
        private System.Windows.Forms.Panel systemStatsPanel;
        private System.Windows.Forms.Label systemStatsLabel;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button exitButton;
    }
}
#nullable restore
