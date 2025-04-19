// BusBuddy/UI/Forms/DriverForm.Designer.cs
namespace BusBuddy.UI.Forms
{
    partial class DriverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip toolTip;

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
            this.components = new System.ComponentModel.Container();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.driverNameTextBox = new System.Windows.Forms.TextBox();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.zipCodeTextBox = new System.Windows.Forms.TextBox();
            this.phoneNumberTextBox = new System.Windows.Forms.TextBox();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.isStipendPaidCheckBox = new System.Windows.Forms.CheckBox();
            this.dlTypeTextBox = new System.Windows.Forms.TextBox();
            this.addDriverButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.driverNameLabel = new System.Windows.Forms.Label();
            this.addressLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.stateLabel = new System.Windows.Forms.Label();
            this.zipCodeLabel = new System.Windows.Forms.Label();
            this.phoneNumberLabel = new System.Windows.Forms.Label();
            this.emailLabel = new System.Windows.Forms.Label();
            this.isStipendPaidLabel = new System.Windows.Forms.Label();
            this.dlTypeLabel = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(800, 200);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.driverNameLabel);
            this.inputGroupBox.Controls.Add(this.driverNameTextBox);
            this.inputGroupBox.Controls.Add(this.addressLabel);
            this.inputGroupBox.Controls.Add(this.addressTextBox);
            this.inputGroupBox.Controls.Add(this.cityLabel);
            this.inputGroupBox.Controls.Add(this.cityTextBox);
            this.inputGroupBox.Controls.Add(this.stateLabel);
            this.inputGroupBox.Controls.Add(this.stateTextBox);
            this.inputGroupBox.Controls.Add(this.zipCodeLabel);
            this.inputGroupBox.Controls.Add(this.zipCodeTextBox);
            this.inputGroupBox.Controls.Add(this.phoneNumberLabel);
            this.inputGroupBox.Controls.Add(this.phoneNumberTextBox);
            this.inputGroupBox.Controls.Add(this.emailLabel);
            this.inputGroupBox.Controls.Add(this.emailTextBox);
            this.inputGroupBox.Controls.Add(this.isStipendPaidLabel);
            this.inputGroupBox.Controls.Add(this.isStipendPaidCheckBox);
            this.inputGroupBox.Controls.Add(this.dlTypeLabel);
            this.inputGroupBox.Controls.Add(this.dlTypeTextBox);
            this.inputGroupBox.Controls.Add(this.addDriverButton);
            this.inputGroupBox.Controls.Add(this.refreshButton);
            this.inputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputGroupBox.Location = new System.Drawing.Point(12, 218);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(800, 340);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Add New Driver";
            this.inputGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            // 
            // driverNameTextBox
            // 
            this.driverNameTextBox.Location = new System.Drawing.Point(120, 30);
            this.driverNameTextBox.Name = "driverNameTextBox";
            this.driverNameTextBox.Size = new System.Drawing.Size(150, 26);
            this.driverNameTextBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.driverNameTextBox, "Enter the driver's full name.");
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(120, 60);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(150, 26);
            this.addressTextBox.TabIndex = 2;
            this.toolTip.SetToolTip(this.addressTextBox, "Enter the driver's address (optional).");
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(120, 90);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(150, 26);
            this.cityTextBox.TabIndex = 3;
            this.toolTip.SetToolTip(this.cityTextBox, "Enter the driver's city (optional).");
            // 
            // stateTextBox
            // 
            this.stateTextBox.Location = new System.Drawing.Point(120, 120);
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new System.Drawing.Size(150, 26);
            this.stateTextBox.TabIndex = 4;
            this.toolTip.SetToolTip(this.stateTextBox, "Enter the driver's state (optional).");
            // 
            // zipCodeTextBox
            // 
            this.zipCodeTextBox.Location = new System.Drawing.Point(120, 150);
            this.zipCodeTextBox.Name = "zipCodeTextBox";
            this.zipCodeTextBox.Size = new System.Drawing.Size(150, 26);
            this.zipCodeTextBox.TabIndex = 5;
            this.toolTip.SetToolTip(this.zipCodeTextBox, "Enter the driver's zip code (optional).");
            // 
            // phoneNumberTextBox
            // 
            this.phoneNumberTextBox.Location = new System.Drawing.Point(120, 180);
            this.phoneNumberTextBox.Name = "phoneNumberTextBox";
            this.phoneNumberTextBox.Size = new System.Drawing.Size(150, 26);
            this.phoneNumberTextBox.TabIndex = 6;
            this.toolTip.SetToolTip(this.phoneNumberTextBox, "Enter the driver's phone number (optional).");
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(120, 210);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(150, 26);
            this.emailTextBox.TabIndex = 7;
            this.toolTip.SetToolTip(this.emailTextBox, "Enter the driver's email address (optional).");
            // 
            // isStipendPaidCheckBox
            // 
            this.isStipendPaidCheckBox.AutoSize = true;
            this.isStipendPaidCheckBox.Location = new System.Drawing.Point(120, 240);
            this.isStipendPaidCheckBox.Name = "isStipendPaidCheckBox";
            this.isStipendPaidCheckBox.Size = new System.Drawing.Size(22, 21);
            this.isStipendPaidCheckBox.TabIndex = 8;
            this.isStipendPaidCheckBox.UseVisualStyleBackColor = true;
            this.toolTip.SetToolTip(this.isStipendPaidCheckBox, "Check if the driver has been paid a stipend.");
            // 
            // dlTypeTextBox
            // 
            this.dlTypeTextBox.Location = new System.Drawing.Point(120, 270);
            this.dlTypeTextBox.Name = "dlTypeTextBox";
            this.dlTypeTextBox.Size = new System.Drawing.Size(150, 26);
            this.dlTypeTextBox.TabIndex = 9;
            this.toolTip.SetToolTip(this.dlTypeTextBox, "Enter the driver's license type (e.g., CDL, Passenger).");
            // 
            // addDriverButton
            // 
            this.addDriverButton.FlatAppearance.BorderSize = 0;
            this.addDriverButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addDriverButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addDriverButton.Location = new System.Drawing.Point(120, 300);
            this.addDriverButton.Name = "addDriverButton";
            this.addDriverButton.Size = new System.Drawing.Size(120, 35);
            this.addDriverButton.TabIndex = 10;
            this.addDriverButton.Text = "Add Driver";
            this.addDriverButton.UseVisualStyleBackColor = true;
            this.addDriverButton.Click += new System.EventHandler(this.AddDriverButton_Click);
            this.toolTip.SetToolTip(this.addDriverButton, "Add the driver to the database.");
            // 
            // refreshButton
            // 
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.Location = new System.Drawing.Point(250, 300);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 35);
            this.refreshButton.TabIndex = 11;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            this.toolTip.SetToolTip(this.refreshButton, "Reload the driver list from the database.");
            // 
            // driverNameLabel
            // 
            this.driverNameLabel.AutoSize = true;
            this.driverNameLabel.Location = new System.Drawing.Point(12, 33);
            this.driverNameLabel.Name = "driverNameLabel";
            this.driverNameLabel.Size = new System.Drawing.Size(98, 20);
            this.driverNameLabel.TabIndex = 12;
            this.driverNameLabel.Text = "Driver Name:";
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(12, 63);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(67, 20);
            this.addressLabel.TabIndex = 13;
            this.addressLabel.Text = "Address:";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(12, 93);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(40, 20);
            this.cityLabel.TabIndex = 14;
            this.cityLabel.Text = "City:";
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(12, 123);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(49, 20);
            this.stateLabel.TabIndex = 15;
            this.stateLabel.Text = "State:";
            // 
            // zipCodeLabel
            // 
            this.zipCodeLabel.AutoSize = true;
            this.zipCodeLabel.Location = new System.Drawing.Point(12, 153);
            this.zipCodeLabel.Name = "zipCodeLabel";
            this.zipCodeLabel.Size = new System.Drawing.Size(76, 20);
            this.zipCodeLabel.TabIndex = 16;
            this.zipCodeLabel.Text = "Zip Code:";
            // 
            // phoneNumberLabel
            // 
            this.phoneNumberLabel.AutoSize = true;
            this.phoneNumberLabel.Location = new System.Drawing.Point(12, 183);
            this.phoneNumberLabel.Name = "phoneNumberLabel";
            this.phoneNumberLabel.Size = new System.Drawing.Size(107, 20);
            this.phoneNumberLabel.TabIndex = 17;
            this.phoneNumberLabel.Text = "Phone Number:";
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(12, 213);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(93, 20);
            this.emailLabel.TabIndex = 18;
            this.emailLabel.Text = "Email Address:";
            // 
            // isStipendPaidLabel
            // 
            this.isStipendPaidLabel.AutoSize = true;
            this.isStipendPaidLabel.Location = new System.Drawing.Point(12, 243);
            this.isStipendPaidLabel.Name = "isStipendPaidLabel";
            this.isStipendPaidLabel.Size = new System.Drawing.Size(102, 20);
            this.isStipendPaidLabel.TabIndex = 19;
            this.isStipendPaidLabel.Text = "Is Stipend Paid:";
            // 
            // dlTypeLabel
            // 
            this.dlTypeLabel.AutoSize = true;
            this.dlTypeLabel.Location = new System.Drawing.Point(12, 273);
            this.dlTypeLabel.Name = "dlTypeLabel";
            this.dlTypeLabel.Size = new System.Drawing.Size(68, 20);
            this.dlTypeLabel.TabIndex = 20;
            this.dlTypeLabel.Text = "DL Type:";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 571);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(824, 31);
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
            // DriverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(824, 602);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.statusStrip);
            this.Name = "DriverForm";
            this.Text = "Driver Management";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.TextBox driverNameTextBox;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.TextBox stateTextBox;
        private System.Windows.Forms.TextBox zipCodeTextBox;
        private System.Windows.Forms.TextBox phoneNumberTextBox;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.CheckBox isStipendPaidCheckBox;
        private System.Windows.Forms.TextBox dlTypeTextBox;
        private System.Windows.Forms.Button addDriverButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label driverNameLabel;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Label zipCodeLabel;
        private System.Windows.Forms.Label phoneNumberLabel;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Label isStipendPaidLabel;
        private System.Windows.Forms.Label dlTypeLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}