namespace BusBuddy.UI.Forms
{
    partial class Welcome : BaseForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button vehiclesButton;
        private System.Windows.Forms.Button fuelButton;
        private System.Windows.Forms.Button driverButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button maintenanceButton;
        private System.Windows.Forms.Label dashboardStatsLabel;
        private System.Windows.Forms.DataGridView todaysTripsGrid;
        private System.Windows.Forms.GroupBox dataInputGroupBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.vehiclesButton = new System.Windows.Forms.Button();
            this.fuelButton = new System.Windows.Forms.Button();
            this.driverButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.maintenanceButton = new System.Windows.Forms.Button();
            this.dashboardStatsLabel = new System.Windows.Forms.Label();
            this.todaysTripsGrid = new System.Windows.Forms.DataGridView();
            this.dataInputGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.todaysTripsGrid)).BeginInit();
            this.dataInputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dashboardStatsLabel
            // 
            this.dashboardStatsLabel.Location = new System.Drawing.Point(40, 20);
            this.dashboardStatsLabel.Name = "dashboardStatsLabel";
            this.dashboardStatsLabel.Size = new System.Drawing.Size(720, 30);
            this.dashboardStatsLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dashboardStatsLabel.Text = "Loading stats...";
            this.dashboardStatsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // todaysTripsGrid
            // 
            this.todaysTripsGrid.Location = new System.Drawing.Point(40, 60);
            this.todaysTripsGrid.Name = "todaysTripsGrid";
            this.todaysTripsGrid.Size = new System.Drawing.Size(720, 380);
            this.todaysTripsGrid.ReadOnly = true;
            this.todaysTripsGrid.AllowUserToAddRows = false;
            this.todaysTripsGrid.AllowUserToDeleteRows = false;
            this.todaysTripsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.todaysTripsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.todaysTripsGrid.Dock = System.Windows.Forms.DockStyle.None;
            this.todaysTripsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right) | System.Windows.Forms.AnchorStyles.Bottom)));
            // 
            // dataInputGroupBox
            // 
            this.dataInputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataInputGroupBox.Controls.Add(this.vehiclesButton);
            this.dataInputGroupBox.Controls.Add(this.fuelButton);
            this.dataInputGroupBox.Controls.Add(this.driverButton);
            this.dataInputGroupBox.Controls.Add(this.settingsButton);
            this.dataInputGroupBox.Controls.Add(this.maintenanceButton);
            this.dataInputGroupBox.Location = new System.Drawing.Point(40, 460);
            this.dataInputGroupBox.Name = "dataInputGroupBox";
            this.dataInputGroupBox.Size = new System.Drawing.Size(720, 310);
            this.dataInputGroupBox.TabIndex = 5;
            this.dataInputGroupBox.TabStop = false;
            this.dataInputGroupBox.Text = "Data Input and Driving Staff";
            this.dataInputGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            // 
            // vehiclesButton
            // 
            this.vehiclesButton.Location = new System.Drawing.Point(260, 40);
            this.vehiclesButton.Name = "vehiclesButton";
            this.vehiclesButton.Size = new System.Drawing.Size(200, 40);
            this.vehiclesButton.TabIndex = 0;
            this.vehiclesButton.Text = "Vehicles";
            this.vehiclesButton.UseVisualStyleBackColor = true;
            this.vehiclesButton.Click += new System.EventHandler(this.OpenVehiclesForm_Click);
            this.vehiclesButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            // 
            // fuelButton
            // 
            this.fuelButton.Location = new System.Drawing.Point(260, 90);
            this.fuelButton.Name = "fuelButton";
            this.fuelButton.Size = new System.Drawing.Size(200, 40);
            this.fuelButton.TabIndex = 1;
            this.fuelButton.Text = "Fuel Records";
            this.fuelButton.UseVisualStyleBackColor = true;
            this.fuelButton.Click += new System.EventHandler(this.OpenFuelForm_Click);
            this.fuelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            // 
            // driverButton
            // 
            this.driverButton.Location = new System.Drawing.Point(260, 140);
            this.driverButton.Name = "driverButton";
            this.driverButton.Size = new System.Drawing.Size(200, 40);
            this.driverButton.TabIndex = 2;
            this.driverButton.Text = "Drivers";
            this.driverButton.UseVisualStyleBackColor = true;
            this.driverButton.Click += new System.EventHandler(this.OpenDriverForm_Click);
            this.driverButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(260, 190);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(200, 40);
            this.settingsButton.TabIndex = 3;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.OpenSettingsForm_Click);
            this.settingsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            // 
            // maintenanceButton
            // 
            this.maintenanceButton.Location = new System.Drawing.Point(260, 240);
            this.maintenanceButton.Name = "maintenanceButton";
            this.maintenanceButton.Size = new System.Drawing.Size(200, 40);
            this.maintenanceButton.TabIndex = 4;
            this.maintenanceButton.Text = "Maintenance";
            this.maintenanceButton.UseVisualStyleBackColor = true;
            this.maintenanceButton.Click += new System.EventHandler(this.OpenMaintenanceForm_Click);
            this.maintenanceButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Controls.Add(this.dashboardStatsLabel);
            this.Controls.Add(this.todaysTripsGrid);
            this.Controls.Add(this.dataInputGroupBox);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "Welcome";
            this.Text = "Welcome - BusBuddy";
            ((System.ComponentModel.ISupportInitialize)(this.todaysTripsGrid)).EndInit();
            this.dataInputGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void OpenMaintenanceForm_Click(object sender, EventArgs e)
        {
            _logger.Information("Welcome: Opening MaintenanceForm.");
            FormManager.DisplayForm("maintenance", this);
        }
    }
}