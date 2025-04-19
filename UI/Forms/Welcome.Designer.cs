// BusBuddy/UI/Forms/Welcome.Designer.cs
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
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.navPanel = new System.Windows.Forms.Panel();
            this.schedulerButton = new System.Windows.Forms.Button();
            this.fuelButton = new System.Windows.Forms.Button();
            this.driverButton = new System.Windows.Forms.Button();
            this.reportsButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.inputsButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.todaysActivitiesGrid = new System.Windows.Forms.DataGridView();
            this.tripsPanel = new System.Windows.Forms.Panel();
            this.todayTripsLabel = new System.Windows.Forms.Label();
            this.refreshTodayButton = new System.Windows.Forms.Button();
            this.statsLabel = new System.Windows.Forms.Label();
            this.testDbButton = new System.Windows.Forms.Button();
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.navPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.todaysActivitiesGrid)).BeginInit();
            this.tripsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = AppSettings.Theme.HeaderFont;
            this.welcomeLabel.ForeColor = AppSettings.Theme.AccentColor;
            this.welcomeLabel.Location = new System.Drawing.Point(12, 20);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(204, 32);
            this.welcomeLabel.TabIndex = 0;
            this.welcomeLabel.Text = "Welcome to BusBuddy";
            // 
            // navPanel
            // 
            this.navPanel.BackColor = AppSettings.Theme.BackgroundColor;
            this.navPanel.Controls.Add(this.testDbButton);
            this.navPanel.Controls.Add(this.inputsButton);
            this.navPanel.Controls.Add(this.settingsButton);
            this.navPanel.Controls.Add(this.exitButton);
            this.navPanel.Controls.Add(this.reportsButton);
            this.navPanel.Controls.Add(this.driverButton);
            this.navPanel.Controls.Add(this.fuelButton);
            this.navPanel.Controls.Add(this.schedulerButton);
            this.navPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navPanel.Location = new System.Drawing.Point(0, 0);
            this.navPanel.Name = "navPanel";
            this.navPanel.Size = new System.Drawing.Size(200, 548);
            this.navPanel.TabIndex = 1;
            // 
            // schedulerButton
            // 
            this.schedulerButton.FlatAppearance.BorderSize = 0;
            this.schedulerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schedulerButton.Font = AppSettings.Theme.ButtonFont;
            this.schedulerButton.Location = new System.Drawing.Point(10, 10);
            this.schedulerButton.Name = "schedulerButton";
            this.schedulerButton.Size = new System.Drawing.Size(180, 35);
            this.schedulerButton.TabIndex = 0;
            this.schedulerButton.Text = "📅 Trip Scheduler";
            this.schedulerButton.UseVisualStyleBackColor = true;
            this.schedulerButton.Click += new System.EventHandler(this.SchedulerButton_Click);
            // 
            // fuelButton
            // 
            this.fuelButton.FlatAppearance.BorderSize = 0;
            this.fuelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelButton.Font = AppSettings.Theme.ButtonFont;
            this.fuelButton.Location = new System.Drawing.Point(10, 55);
            this.fuelButton.Name = "fuelButton";
            this.fuelButton.Size = new System.Drawing.Size(180, 35);
            this.fuelButton.TabIndex = 1;
            this.fuelButton.Text = "⛽ Fuel Records";
            this.fuelButton.UseVisualStyleBackColor = true;
            this.fuelButton.Click += new System.EventHandler(this.FuelButton_Click);
            // 
            // driverButton
            // 
            this.driverButton.FlatAppearance.BorderSize = 0;
            this.driverButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverButton.Font = AppSettings.Theme.ButtonFont;
            this.driverButton.Location = new System.Drawing.Point(10, 100);
            this.driverButton.Name = "driverButton";
            this.driverButton.Size = new System.Drawing.Size(180, 35);
            this.driverButton.TabIndex = 2;
            this.driverButton.Text = "👤 Driver Management";
            this.driverButton.UseVisualStyleBackColor = true;
            this.driverButton.Click += new System.EventHandler(this.DriverButton_Click);
            // 
            // reportsButton
            // 
            this.reportsButton.FlatAppearance.BorderSize = 0;
            this.reportsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportsButton.Font = AppSettings.Theme.ButtonFont;
            this.reportsButton.Location = new System.Drawing.Point(10, 145);
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Size = new System.Drawing.Size(180, 35);
            this.reportsButton.TabIndex = 3;
            this.reportsButton.Text = "📊 Reports";
            this.reportsButton.UseVisualStyleBackColor = true;
            this.reportsButton.Click += new System.EventHandler(this.ReportsButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Font = AppSettings.Theme.ButtonFont;
            this.settingsButton.Location = new System.Drawing.Point(10, 190);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(180, 35);
            this.settingsButton.TabIndex = 4;
            this.settingsButton.Text = "⚙️ Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // inputsButton
            // 
            this.inputsButton.FlatAppearance.BorderSize = 0;
            this.inputsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputsButton.Font = AppSettings.Theme.ButtonFont;
            this.inputsButton.Location = new System.Drawing.Point(10, 235);
            this.inputsButton.Name = "inputsButton";
            this.inputsButton.Size = new System.Drawing.Size(180, 35);
            this.inputsButton.TabIndex = 5;
            this.inputsButton.Text = "📋 Inputs";
            this.inputsButton.UseVisualStyleBackColor = true;
            this.inputsButton.Click += new System.EventHandler(this.InputsButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = AppSettings.Theme.ButtonFont;
            this.exitButton.Location = new System.Drawing.Point(10, 280);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(180, 35);
            this.exitButton.TabIndex = 6;
            this.exitButton.Text = "🚪 Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 579);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(804, 31);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(53, 26);
            this.statusLabel.Text = "Ready.";
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.AutoSize = true;
            this.dateTimeLabel.Font = AppSettings.Theme.NormalFont;
            this.dateTimeLabel.Location = new System.Drawing.Point(15, 60);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(123, 20);
            this.dateTimeLabel.TabIndex = 3;
            this.dateTimeLabel.Text = "April 19, 2025";
            // 
            // todaysActivitiesGrid
            // 
            this.todaysActivitiesGrid.AllowUserToAddRows = false;
            this.todaysActivitiesGrid.AllowUserToDeleteRows = false;
            this.todaysActivitiesGrid.AllowUserToResizeRows = false;
            this.todaysActivitiesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.todaysActivitiesGrid.BackgroundColor = AppSettings.Theme.PanelColor;
            this.todaysActivitiesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.todaysActivitiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.todaysActivitiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.todaysActivitiesGrid.Location = new System.Drawing.Point(0, 40);
            this.todaysActivitiesGrid.Name = "todaysActivitiesGrid";
            this.todaysActivitiesGrid.ReadOnly = true;
            this.todaysActivitiesGrid.RowHeadersWidth = 62;
            this.todaysActivitiesGrid.RowTemplate.Height = 25;
            this.todaysActivitiesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.todaysActivitiesGrid.Size = new System.Drawing.Size(600, 508);
            this.todaysActivitiesGrid.TabIndex = 4;
            // 
            // tripsPanel
            // 
            this.tripsPanel.Controls.Add(this.todaysActivitiesGrid);
            this.tripsPanel.Controls.Add(this.todayTripsLabel);
            this.tripsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tripsPanel.Location = new System.Drawing.Point(0, 0);
            this.tripsPanel.Name = "tripsPanel";
            this.tripsPanel.Size = new System.Drawing.Size(600, 548);
            this.tripsPanel.TabIndex = 5;
            // 
            // todayTripsLabel
            // 
            this.todayTripsLabel.BackColor = AppSettings.Theme.PanelHeaderColor;
            this.todayTripsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.todayTripsLabel.Font = AppSettings.Theme.SubheaderFont;
            this.todayTripsLabel.ForeColor = AppSettings.Theme.TextColor;
            this.todayTripsLabel.Location = new System.Drawing.Point(0, 0);
            this.todayTripsLabel.Name = "todayTripsLabel";
            this.todayTripsLabel.Size = new System.Drawing.Size(600, 40);
            this.todayTripsLabel.TabIndex = 0;
            this.todayTripsLabel.Text = "Today\'s Trips";
            this.todayTripsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // refreshTodayButton
            // 
            this.refreshTodayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshTodayButton.Font = AppSettings.Theme.ButtonFont;
            this.refreshTodayButton.Location = new System.Drawing.Point(589, 57);
            this.refreshTodayButton.Name = "refreshTodayButton";
            this.refreshTodayButton.Size = new System.Drawing.Size(100, 30);
            this.refreshTodayButton.TabIndex = 6;
            this.refreshTodayButton.Text = "↻ Refresh";
            this.refreshTodayButton.UseVisualStyleBackColor = true;
            this.refreshTodayButton.Click += new System.EventHandler(this.refreshTodayButton_Click);
            // 
            // statsLabel
            // 
            this.statsLabel.AutoSize = true;
            this.statsLabel.Font = AppSettings.Theme.NormalFont;
            this.statsLabel.Location = new System.Drawing.Point(245, 60);
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(196, 20);
            this.statsLabel.TabIndex = 7;
            this.statsLabel.Text = "Loading statistics...";
            // 
            // testDbButton
            // 
            this.testDbButton.FlatAppearance.BorderSize = 0;
            this.testDbButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testDbButton.Font = AppSettings.Theme.ButtonFont;
            this.testDbButton.Location = new System.Drawing.Point(10, 325);
            this.testDbButton.Name = "testDbButton";
            this.testDbButton.Size = new System.Drawing.Size(180, 35);
            this.testDbButton.TabIndex = 7;
            this.testDbButton.Text = "🔍 Test Database";
            this.testDbButton.UseVisualStyleBackColor = true;
            this.testDbButton.Click += new System.EventHandler(this.TestDbButton_Click);
            // 
            // mainContainer
            // 
            this.mainContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainContainer.Location = new System.Drawing.Point(0, 100);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.navPanel);
            this.mainContainer.Panel1MinSize = 200;
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.tripsPanel);
            this.mainContainer.Panel2MinSize = 400;
            this.mainContainer.Size = new System.Drawing.Size(804, 548);
            this.mainContainer.SplitterDistance = 200;
            this.mainContainer.TabIndex = 9;
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 610);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.statsLabel);
            this.Controls.Add(this.refreshTodayButton);
            this.Controls.Add(this.dateTimeLabel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.welcomeLabel);
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BusBuddy - Welcome";
            this.navPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.todaysActivitiesGrid)).EndInit();
            this.tripsPanel.ResumeLayout(false);
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Panel navPanel;
        private System.Windows.Forms.Button schedulerButton;
        private System.Windows.Forms.Button fuelButton;
        private System.Windows.Forms.Button driverButton;
        private System.Windows.Forms.Button reportsButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button inputsButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button testDbButton;
        private System.Windows.Forms.Label dateTimeLabel;
        private System.Windows.Forms.DataGridView todaysActivitiesGrid;
        private System.Windows.Forms.Panel tripsPanel;
        private System.Windows.Forms.Label todayTripsLabel;
        private System.Windows.Forms.Button refreshTodayButton;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.SplitContainer mainContainer;
    }
}