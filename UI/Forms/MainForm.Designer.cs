// BusBuddy/UI/Forms/MainForm.Designer.cs
namespace BusBuddy.UI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.navPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
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
            this.navPanel.Controls.Add(this.inputsButton);
            this.navPanel.Controls.Add(this.settingsButton);
            this.navPanel.Controls.Add(this.exitButton);
            this.navPanel.Controls.Add(this.reportsButton);
            this.navPanel.Controls.Add(this.driverButton);
            this.navPanel.Controls.Add(this.fuelButton);
            this.navPanel.Controls.Add(this.schedulerButton);
            this.navPanel.Location = new System.Drawing.Point(12, 60);
            this.navPanel.Name = "navPanel";
            this.navPanel.Size = new System.Drawing.Size(200, 265);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 333);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(224, 31);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 364);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.navPanel);
            this.Controls.Add(this.welcomeLabel);
            this.Name = "MainForm";
            this.Text = "BusBuddy - Main";
            this.navPanel.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

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
    }
}