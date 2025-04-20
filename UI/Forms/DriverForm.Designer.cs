using System.Windows.Forms;

namespace BusBuddy.UI.Forms
{
    partial class DriverForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.driversDataGridView = new System.Windows.Forms.DataGridView();
            this.driverNameLabel = new System.Windows.Forms.Label();
            this.driverNameTextBox = new System.Windows.Forms.TextBox();
            this.driverAddressLabel = new System.Windows.Forms.Label();
            this.driverAddressTextBox = new System.Windows.Forms.TextBox();
            this.driverCityLabel = new System.Windows.Forms.Label();
            this.driverCityTextBox = new System.Windows.Forms.TextBox();
            this.driverStateLabel = new System.Windows.Forms.Label();
            this.driverStateTextBox = new System.Windows.Forms.TextBox();
            this.driverZipLabel = new System.Windows.Forms.Label();
            this.driverZipTextBox = new System.Windows.Forms.TextBox();
            this.driverPhoneLabel = new System.Windows.Forms.Label();
            this.driverPhoneTextBox = new System.Windows.Forms.TextBox();
            this.driverEmailLabel = new System.Windows.Forms.Label();
            this.driverEmailTextBox = new System.Windows.Forms.TextBox();
            this.driverStipendLabel = new System.Windows.Forms.Label();
            this.driverStipendComboBox = new System.Windows.Forms.ComboBox();
            this.driverDLTypeLabel = new System.Windows.Forms.Label();
            this.driverDLTypeComboBox = new System.Windows.Forms.ComboBox();
            this.driverAddButton = new System.Windows.Forms.Button();
            this.driverClearButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();

            ((System.ComponentModel.ISupportInitialize)(this.driversDataGridView)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.inputGroupBox.SuspendLayout();
            this.SuspendLayout();

            // DataGridView
            this.driversDataGridView.Location = new System.Drawing.Point(10, 10);
            this.driversDataGridView.Size = new System.Drawing.Size(760, 200);
            this.driversDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.driversDataGridView.ReadOnly = true;
            this.driversDataGridView.AllowUserToAddRows = false;
            this.driversDataGridView.Name = "driversDataGridView";
            this.driversDataGridView.Columns.Add("DriverID", "Driver ID");
            this.driversDataGridView.Columns.Add("DriverName", "Driver Name");
            this.driversDataGridView.Columns.Add("Address", "Address");
            this.driversDataGridView.Columns.Add("City", "City");
            this.driversDataGridView.Columns.Add("State", "State");
            this.driversDataGridView.Columns.Add("ZipCode", "Zip Code");
            this.driversDataGridView.Columns.Add("PhoneNumber", "Phone Number");
            this.driversDataGridView.Columns.Add("EmailAddress", "Email Address");
            this.driversDataGridView.Columns.Add("IsStipendPaid", "Is Stipend Paid");
            this.driversDataGridView.Columns.Add("DLType", "DL Type");

            // Input GroupBox
            this.inputGroupBox.Text = "Add New Driver";
            this.inputGroupBox.Location = new System.Drawing.Point(10, 220);
            this.inputGroupBox.Size = new System.Drawing.Size(760, 300);
            this.inputGroupBox.Font = AppSettings.Theme.LabelFont;
            this.inputGroupBox.Controls.Add(this.driverNameLabel);
            this.inputGroupBox.Controls.Add(this.driverNameTextBox);
            this.inputGroupBox.Controls.Add(this.driverAddressLabel);
            this.inputGroupBox.Controls.Add(this.driverAddressTextBox);
            this.inputGroupBox.Controls.Add(this.driverCityLabel);
            this.inputGroupBox.Controls.Add(this.driverCityTextBox);
            this.inputGroupBox.Controls.Add(this.driverStateLabel);
            this.inputGroupBox.Controls.Add(this.driverStateTextBox);
            this.inputGroupBox.Controls.Add(this.driverZipLabel);
            this.inputGroupBox.Controls.Add(this.driverZipTextBox);
            this.inputGroupBox.Controls.Add(this.driverPhoneLabel);
            this.inputGroupBox.Controls.Add(this.driverPhoneTextBox);
            this.inputGroupBox.Controls.Add(this.driverEmailLabel);
            this.inputGroupBox.Controls.Add(this.driverEmailTextBox);
            this.inputGroupBox.Controls.Add(this.driverStipendLabel);
            this.inputGroupBox.Controls.Add(this.driverStipendComboBox);
            this.inputGroupBox.Controls.Add(this.driverDLTypeLabel);
            this.inputGroupBox.Controls.Add(this.driverDLTypeComboBox);
            this.inputGroupBox.Controls.Add(this.driverAddButton);
            this.inputGroupBox.Controls.Add(this.driverClearButton);

            this.driverNameLabel.Text = "Driver Name:";
            this.driverNameLabel.Location = new System.Drawing.Point(10, 30);
            this.driverNameLabel.AutoSize = true;
            this.driverNameLabel.Font = AppSettings.Theme.LabelFont;
            this.driverNameTextBox.Location = new System.Drawing.Point(120, 27);
            this.driverNameTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverNameTextBox.Name = "driverNameTextBox";

            this.driverAddressLabel.Text = "Address:";
            this.driverAddressLabel.Location = new System.Drawing.Point(10, 60);
            this.driverAddressLabel.AutoSize = true;
            this.driverAddressLabel.Font = AppSettings.Theme.LabelFont;
            this.driverAddressTextBox.Location = new System.Drawing.Point(120, 57);
            this.driverAddressTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverAddressTextBox.Name = "driverAddressTextBox";

            this.driverCityLabel.Text = "City:";
            this.driverCityLabel.Location = new System.Drawing.Point(10, 90);
            this.driverCityLabel.AutoSize = true;
            this.driverCityLabel.Font = AppSettings.Theme.LabelFont;
            this.driverCityTextBox.Location = new System.Drawing.Point(120, 87);
            this.driverCityTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverCityTextBox.Name = "driverCityTextBox";

            this.driverStateLabel.Text = "State:";
            this.driverStateLabel.Location = new System.Drawing.Point(10, 120);
            this.driverStateLabel.AutoSize = true;
            this.driverStateLabel.Font = AppSettings.Theme.LabelFont;
            this.driverStateTextBox.Location = new System.Drawing.Point(120, 117);
            this.driverStateTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverStateTextBox.Name = "driverStateTextBox";

            this.driverZipLabel.Text = "Zip Code:";
            this.driverZipLabel.Location = new System.Drawing.Point(10, 150);
            this.driverZipLabel.AutoSize = true;
            this.driverZipLabel.Font = AppSettings.Theme.LabelFont;
            this.driverZipTextBox.Location = new System.Drawing.Point(120, 147);
            this.driverZipTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverZipTextBox.Name = "driverZipTextBox";

            this.driverPhoneLabel.Text = "Phone Number:";
            this.driverPhoneLabel.Location = new System.Drawing.Point(10, 180);
            this.driverPhoneLabel.AutoSize = true;
            this.driverPhoneLabel.Font = AppSettings.Theme.LabelFont;
            this.driverPhoneTextBox.Location = new System.Drawing.Point(120, 177);
            this.driverPhoneTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverPhoneTextBox.Name = "driverPhoneTextBox";

            this.driverEmailLabel.Text = "Email Address:";
            this.driverEmailLabel.Location = new System.Drawing.Point(10, 210);
            this.driverEmailLabel.AutoSize = true;
            this.driverEmailLabel.Font = AppSettings.Theme.LabelFont;
            this.driverEmailTextBox.Location = new System.Drawing.Point(120, 207);
            this.driverEmailTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverEmailTextBox.Name = "driverEmailTextBox";

            this.driverStipendLabel.Text = "Is Stipend Paid:";
            this.driverStipendLabel.Location = new System.Drawing.Point(10, 240);
            this.driverStipendLabel.AutoSize = true;
            this.driverStipendLabel.Font = AppSettings.Theme.LabelFont;
            this.driverStipendComboBox.Location = new System.Drawing.Point(120, 237);
            this.driverStipendComboBox.Size = new System.Drawing.Size(150, 28);
            this.driverStipendComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverStipendComboBox.Items.AddRange(new object[] { "Yes", "No" });
            this.driverStipendComboBox.Name = "driverStipendComboBox";

            this.driverDLTypeLabel.Text = "DL Type:";
            this.driverDLTypeLabel.Location = new System.Drawing.Point(10, 270);
            this.driverDLTypeLabel.AutoSize = true;
            this.driverDLTypeLabel.Font = AppSettings.Theme.LabelFont;
            this.driverDLTypeComboBox.Location = new System.Drawing.Point(120, 267);
            this.driverDLTypeComboBox.Size = new System.Drawing.Size(150, 28);
            this.driverDLTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverDLTypeComboBox.Items.AddRange(new object[] { "CDL", "Regular" });
            this.driverDLTypeComboBox.Name = "driverDLTypeComboBox";

            this.driverAddButton.Text = "Add";
            this.driverAddButton.Location = new System.Drawing.Point(120, 297);
            this.driverAddButton.Size = new System.Drawing.Size(100, 35);
            this.driverAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.driverAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.driverAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverAddButton.FlatAppearance.BorderSize = 2;
            this.driverAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.driverAddButton.Font = AppSettings.Theme.ButtonFont;
            this.driverAddButton.Name = "driverAddButton";
            this.driverAddButton.Click += new System.EventHandler(this.DriverAddButton_Click);

            this.driverClearButton.Text = "Clear";
            this.driverClearButton.Location = new System.Drawing.Point(230, 297);
            this.driverClearButton.Size = new System.Drawing.Size(100, 35);
            this.driverClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.driverClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.driverClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverClearButton.FlatAppearance.BorderSize = 2;
            this.driverClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.driverClearButton.Font = AppSettings.Theme.ButtonFont;
            this.driverClearButton.Name = "driverClearButton";
            this.driverClearButton.Click += new System.EventHandler(this.DriverClearButton_Click);

            // Status Strip
            this.statusStrip.Location = new System.Drawing.Point(0, 568);
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });

            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Text = "Ready.";

            // Form Properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 590);
            this.Controls.Add(this.driversDataGridView);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.statusStrip);
            this.Name = "DriverForm";
            this.Text = "Driver Management - BusBuddy";

            ((System.ComponentModel.ISupportInitialize)(this.driversDataGridView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView driversDataGridView;
        private System.Windows.Forms.Label driverNameLabel;
        private System.Windows.Forms.TextBox driverNameTextBox;
        private System.Windows.Forms.Label driverAddressLabel;
        private System.Windows.Forms.TextBox driverAddressTextBox;
        private System.Windows.Forms.Label driverCityLabel;
        private System.Windows.Forms.TextBox driverCityTextBox;
        private System.Windows.Forms.Label driverStateLabel;
        private System.Windows.Forms.TextBox driverStateTextBox;
        private System.Windows.Forms.Label driverZipLabel;
        private System.Windows.Forms.TextBox driverZipTextBox;
        private System.Windows.Forms.Label driverPhoneLabel;
        private System.Windows.Forms.TextBox driverPhoneTextBox;
        private System.Windows.Forms.Label driverEmailLabel;
        private System.Windows.Forms.TextBox driverEmailTextBox;
        private System.Windows.Forms.Label driverStipendLabel;
        private System.Windows.Forms.ComboBox driverStipendComboBox;
        private System.Windows.Forms.Label driverDLTypeLabel;
        private System.Windows.Forms.ComboBox driverDLTypeComboBox;
        private System.Windows.Forms.Button driverAddButton;
        private System.Windows.Forms.Button driverClearButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox inputGroupBox;
    }
}