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
            this.ClientSize = new System.Drawing.Size(450, 380);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimumSize = new System.Drawing.Size(450, 380);
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BusBuddy - Main";

            // Initialize components
            this.components = new System.ComponentModel.Container();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.schedulerButton = new System.Windows.Forms.Button();
            this.fuelButton = new System.Windows.Forms.Button();
            this.driverButton = new System.Windows.Forms.Button();
            this.reportsButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // Welcome Label
            this.welcomeLabel.Text = "Welcome to BusBuddy";
            this.welcomeLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.ForeColor = System.Drawing.Color.FromArgb(255, 140, 0); // Orange color
            this.welcomeLabel.Location = new System.Drawing.Point(0, 20);
            this.welcomeLabel.Size = new System.Drawing.Size(450, 40);
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.welcomeLabel.Name = "welcomeLabel";

            // Trip Scheduler Button
            this.schedulerButton.Text = "Trip Scheduler";
            this.schedulerButton.BackColor = System.Drawing.Color.White;
            this.schedulerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schedulerButton.Location = new System.Drawing.Point(105, 80);
            this.schedulerButton.Size = new System.Drawing.Size(240, 40);
            this.schedulerButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.schedulerButton.Image = null;
            this.schedulerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.schedulerButton.Name = "schedulerButton";
            this.schedulerButton.Click += new System.EventHandler(this.SchedulerButton_Click);

            // Fuel Records Button
            this.fuelButton.Text = "Fuel Records";
            this.fuelButton.BackColor = System.Drawing.Color.White;
            this.fuelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelButton.Location = new System.Drawing.Point(105, 130);
            this.fuelButton.Size = new System.Drawing.Size(240, 40);
            this.fuelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.fuelButton.Image = null;
            this.fuelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fuelButton.Name = "fuelButton";
            this.fuelButton.Click += new System.EventHandler(this.FuelButton_Click);

            // Driver Button
            this.driverButton.Text = "Driver";
            this.driverButton.BackColor = System.Drawing.Color.White;
            this.driverButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverButton.Location = new System.Drawing.Point(105, 180);
            this.driverButton.Size = new System.Drawing.Size(240, 40);
            this.driverButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.driverButton.Image = null;
            this.driverButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.driverButton.Name = "driverButton";
            this.driverButton.Click += new System.EventHandler(this.DriverButton_Click);

            // Reports Button
            this.reportsButton.Text = "Reports";
            this.reportsButton.BackColor = System.Drawing.Color.White;
            this.reportsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportsButton.Location = new System.Drawing.Point(105, 230);
            this.reportsButton.Size = new System.Drawing.Size(240, 40);
            this.reportsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.reportsButton.Image = null;
            this.reportsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Click += new System.EventHandler(this.ReportsButton_Click);

            // Settings Button
            this.settingsButton.Text = "Settings";
            this.settingsButton.BackColor = System.Drawing.Color.White;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Location = new System.Drawing.Point(105, 280);
            this.settingsButton.Size = new System.Drawing.Size(240, 40);
            this.settingsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.settingsButton.Image = null;
            this.settingsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);

            // Status Strip
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusStrip.Location = new System.Drawing.Point(0, 358);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(450, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 11;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.statusLabel
            });

            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Text = "Ready";

            // Add controls to form
            this.Controls.Add(this.welcomeLabel);
            this.Controls.Add(this.schedulerButton);
            this.Controls.Add(this.fuelButton);
            this.Controls.Add(this.driverButton);
            this.Controls.Add(this.reportsButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.statusStrip);

            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Button schedulerButton;
        private System.Windows.Forms.Button fuelButton;
        private System.Windows.Forms.Button driverButton;
        private System.Windows.Forms.Button reportsButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}