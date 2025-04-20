using System.Windows.Forms;

namespace BusBuddy.UI.Forms
{
    partial class FuelForm
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
            this.fuelDataGridView = new System.Windows.Forms.DataGridView();
            this.fuelBusNumberLabel = new System.Windows.Forms.Label();
            this.fuelBusNumberComboBox = new System.Windows.Forms.ComboBox();
            this.fuelGallonsLabel = new System.Windows.Forms.Label();
            this.fuelGallonsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fuelDateLabel = new System.Windows.Forms.Label();
            this.fuelDatePicker = new System.Windows.Forms.DateTimePicker();
            this.fuelTypeLabel = new System.Windows.Forms.Label();
            this.fuelTypeComboBox = new System.Windows.Forms.ComboBox();
            this.fuelOdometerLabel = new System.Windows.Forms.Label();
            this.fuelOdometerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fuelAddButton = new System.Windows.Forms.Button();
            this.fuelClearButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();

            ((System.ComponentModel.ISupportInitialize)(this.fuelDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelGallonsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelOdometerNumericUpDown)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.inputGroupBox.SuspendLayout();
            this.SuspendLayout();

            // DataGridView
            this.fuelDataGridView.Location = new System.Drawing.Point(10, 10);
            this.fuelDataGridView.Size = new System.Drawing.Size(760, 200);
            this.fuelDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.fuelDataGridView.ReadOnly = true;
            this.fuelDataGridView.AllowUserToAddRows = false;
            this.fuelDataGridView.Name = "fuelDataGridView";
            this.fuelDataGridView.Columns.Add("FuelID", "Fuel ID");
            this.fuelDataGridView.Columns.Add("BusNumber", "Bus Number");
            this.fuelDataGridView.Columns.Add("FuelGallons", "Fuel Gallons");
            this.fuelDataGridView.Columns.Add("FuelDate", "Fuel Date");
            this.fuelDataGridView.Columns.Add("FuelType", "Fuel Type");
            this.fuelDataGridView.Columns.Add("OdometerReading", "Odometer Reading");

            // Input GroupBox
            this.inputGroupBox.Text = "Add New Fuel Record";
            this.inputGroupBox.Location = new System.Drawing.Point(10, 220);
            this.inputGroupBox.Size = new System.Drawing.Size(760, 300);
            this.inputGroupBox.Font = AppSettings.Theme.LabelFont;
            this.inputGroupBox.Controls.Add(this.fuelBusNumberLabel);
            this.inputGroupBox.Controls.Add(this.fuelBusNumberComboBox);
            this.inputGroupBox.Controls.Add(this.fuelGallonsLabel);
            this.inputGroupBox.Controls.Add(this.fuelGallonsNumericUpDown);
            this.inputGroupBox.Controls.Add(this.fuelDateLabel);
            this.inputGroupBox.Controls.Add(this.fuelDatePicker);
            this.inputGroupBox.Controls.Add(this.fuelTypeLabel);
            this.inputGroupBox.Controls.Add(this.fuelTypeComboBox);
            this.inputGroupBox.Controls.Add(this.fuelOdometerLabel);
            this.inputGroupBox.Controls.Add(this.fuelOdometerNumericUpDown);
            this.inputGroupBox.Controls.Add(this.fuelAddButton);
            this.inputGroupBox.Controls.Add(this.fuelClearButton);

            this.fuelBusNumberLabel.Text = "Bus Number:";
            this.fuelBusNumberLabel.Location = new System.Drawing.Point(10, 30);
            this.fuelBusNumberLabel.AutoSize = true;
            this.fuelBusNumberLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelBusNumberComboBox.Location = new System.Drawing.Point(120, 27);
            this.fuelBusNumberComboBox.Size = new System.Drawing.Size(150, 28);
            this.fuelBusNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelBusNumberComboBox.Name = "fuelBusNumberComboBox";

            this.fuelGallonsLabel.Text = "Fuel Gallons:";
            this.fuelGallonsLabel.Location = new System.Drawing.Point(10, 60);
            this.fuelGallonsLabel.AutoSize = true;
            this.fuelGallonsLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelGallonsNumericUpDown.Location = new System.Drawing.Point(120, 57);
            this.fuelGallonsNumericUpDown.Size = new System.Drawing.Size(150, 28);
            this.fuelGallonsNumericUpDown.Minimum = 0;
            this.fuelGallonsNumericUpDown.Maximum = 1000;
            this.fuelGallonsNumericUpDown.Name = "fuelGallonsNumericUpDown";

            this.fuelDateLabel.Text = "Fuel Date:";
            this.fuelDateLabel.Location = new System.Drawing.Point(10, 90);
            this.fuelDateLabel.AutoSize = true;
            this.fuelDateLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelDatePicker.Location = new System.Drawing.Point(120, 87);
            this.fuelDatePicker.Size = new System.Drawing.Size(150, 28);
            this.fuelDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fuelDatePicker.Name = "fuelDatePicker";

            this.fuelTypeLabel.Text = "Fuel Type:";
            this.fuelTypeLabel.Location = new System.Drawing.Point(10, 120);
            this.fuelTypeLabel.AutoSize = true;
            this.fuelTypeLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelTypeComboBox.Location = new System.Drawing.Point(120, 117);
            this.fuelTypeComboBox.Size = new System.Drawing.Size(150, 28);
            this.fuelTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelTypeComboBox.Items.AddRange(new object[] { "Diesel", "Gasoline" });
            this.fuelTypeComboBox.Name = "fuelTypeComboBox";

            this.fuelOdometerLabel.Text = "Odometer Reading:";
            this.fuelOdometerLabel.Location = new System.Drawing.Point(10, 150);
            this.fuelOdometerLabel.AutoSize = true;
            this.fuelOdometerLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelOdometerNumericUpDown.Location = new System.Drawing.Point(120, 147);
            this.fuelOdometerNumericUpDown.Size = new System.Drawing.Size(150, 28);
            this.fuelOdometerNumericUpDown.Minimum = 0;
            this.fuelOdometerNumericUpDown.Maximum = 1000000;
            this.fuelOdometerNumericUpDown.Name = "fuelOdometerNumericUpDown";

            this.fuelAddButton.Text = "Add";
            this.fuelAddButton.Location = new System.Drawing.Point(120, 177);
            this.fuelAddButton.Size = new System.Drawing.Size(100, 35);
            this.fuelAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.fuelAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.fuelAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelAddButton.FlatAppearance.BorderSize = 2;
            this.fuelAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.fuelAddButton.Font = AppSettings.Theme.ButtonFont;
            this.fuelAddButton.Name = "fuelAddButton";
            this.fuelAddButton.Click += new System.EventHandler(this.FuelAddButton_Click);

            this.fuelClearButton.Text = "Clear";
            this.fuelClearButton.Location = new System.Drawing.Point(230, 177);
            this.fuelClearButton.Size = new System.Drawing.Size(100, 35);
            this.fuelClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.fuelClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.fuelClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelClearButton.FlatAppearance.BorderSize = 2;
            this.fuelClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.fuelClearButton.Font = AppSettings.Theme.ButtonFont;
            this.fuelClearButton.Name = "fuelClearButton";
            this.fuelClearButton.Click += new System.EventHandler(this.FuelClearButton_Click);

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
            this.Controls.Add(this.fuelDataGridView);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.statusStrip);
            this.Name = "FuelForm";
            this.Text = "Fuel Records - BusBuddy";

            ((System.ComponentModel.ISupportInitialize)(this.fuelDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelGallonsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelOdometerNumericUpDown)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView fuelDataGridView;
        private System.Windows.Forms.Label fuelBusNumberLabel;
        private System.Windows.Forms.ComboBox fuelBusNumberComboBox;
        private System.Windows.Forms.Label fuelGallonsLabel;
        private System.Windows.Forms.NumericUpDown fuelGallonsNumericUpDown;
        private System.Windows.Forms.Label fuelDateLabel;
        private System.Windows.Forms.DateTimePicker fuelDatePicker;
        private System.Windows.Forms.Label fuelTypeLabel;
        private System.Windows.Forms.ComboBox fuelTypeComboBox;
        private System.Windows.Forms.Label fuelOdometerLabel;
        private System.Windows.Forms.NumericUpDown fuelOdometerNumericUpDown;
        private System.Windows.Forms.Button fuelAddButton;
        private System.Windows.Forms.Button fuelClearButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox inputGroupBox;
    }
}