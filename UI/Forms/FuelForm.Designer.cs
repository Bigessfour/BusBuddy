// BusBuddy/UI/Forms/FuelForm.Designer.cs
namespace BusBuddy.UI.Forms
{
    partial class FuelForm
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.busNumberComboBox = new System.Windows.Forms.ComboBox();
            this.fuelGallonsTextBox = new System.Windows.Forms.TextBox();
            this.fuelDatePicker = new System.Windows.Forms.DateTimePicker();
            this.fuelTypeTextBox = new System.Windows.Forms.TextBox();
            this.odometerTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.addFuelButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.busNumberLabel = new System.Windows.Forms.Label();
            this.fuelGallonsLabel = new System.Windows.Forms.Label();
            this.fuelDateLabel = new System.Windows.Forms.Label();
            this.fuelTypeLabel = new System.Windows.Forms.Label();
            this.odometerLabel = new System.Windows.Forms.Label();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // busNumberComboBox
            // 
            this.busNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.busNumberComboBox.Location = new System.Drawing.Point(120, 30);
            this.busNumberComboBox.Name = "busNumberComboBox";
            this.busNumberComboBox.Size = new System.Drawing.Size(150, 28);
            this.busNumberComboBox.TabIndex = 1;
            // 
            // fuelGallonsTextBox
            // 
            this.fuelGallonsTextBox.Location = new System.Drawing.Point(120, 60);
            this.fuelGallonsTextBox.Name = "fuelGallonsTextBox";
            this.fuelGallonsTextBox.Size = new System.Drawing.Size(150, 26);
            this.fuelGallonsTextBox.TabIndex = 2;
            // 
            // fuelDatePicker
            // 
            this.fuelDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fuelDatePicker.Location = new System.Drawing.Point(120, 90);
            this.fuelDatePicker.Name = "fuelDatePicker";
            this.fuelDatePicker.Size = new System.Drawing.Size(150, 26);
            this.fuelDatePicker.TabIndex = 3;
            // 
            // fuelTypeTextBox
            // 
            this.fuelTypeTextBox.Location = new System.Drawing.Point(120, 120);
            this.fuelTypeTextBox.Name = "fuelTypeTextBox";
            this.fuelTypeTextBox.Size = new System.Drawing.Size(150, 26);
            this.fuelTypeTextBox.TabIndex = 4;
            // 
            // odometerTextBox
            // 
            this.odometerTextBox.Location = new System.Drawing.Point(120, 150);
            this.odometerTextBox.Name = "odometerTextBox";
            this.odometerTextBox.Size = new System.Drawing.Size(150, 26);
            this.odometerTextBox.TabIndex = 5;
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
            // addFuelButton
            // 
            this.addFuelButton.FlatAppearance.BorderSize = 0;
            this.addFuelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addFuelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addFuelButton.Location = new System.Drawing.Point(120, 180);
            this.addFuelButton.Name = "addFuelButton";
            this.addFuelButton.Size = new System.Drawing.Size(120, 35);
            this.addFuelButton.TabIndex = 6;
            this.addFuelButton.Text = "Add Fuel";
            this.addFuelButton.UseVisualStyleBackColor = true;
            this.addFuelButton.Click += new System.EventHandler(this.AddFuelButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.Location = new System.Drawing.Point(250, 180);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 35);
            this.refreshButton.TabIndex = 7;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // busNumberLabel
            // 
            this.busNumberLabel.AutoSize = true;
            this.busNumberLabel.Location = new System.Drawing.Point(12, 33);
            this.busNumberLabel.Name = "busNumberLabel";
            this.busNumberLabel.Size = new System.Drawing.Size(93, 20);
            this.busNumberLabel.TabIndex = 8;
            this.busNumberLabel.Text = "Bus Number:";
            // 
            // fuelGallonsLabel
            // 
            this.fuelGallonsLabel.AutoSize = true;
            this.fuelGallonsLabel.Location = new System.Drawing.Point(12, 63);
            this.fuelGallonsLabel.Name = "fuelGallonsLabel";
            this.fuelGallonsLabel.Size = new System.Drawing.Size(92, 20);
            this.fuelGallonsLabel.TabIndex = 9;
            this.fuelGallonsLabel.Text = "Fuel Gallons:";
            // 
            // fuelDateLabel
            // 
            this.fuelDateLabel.AutoSize = true;
            this.fuelDateLabel.Location = new System.Drawing.Point(12, 93);
            this.fuelDateLabel.Name = "fuelDateLabel";
            this.fuelDateLabel.Size = new System.Drawing.Size(76, 20);
            this.fuelDateLabel.TabIndex = 10;
            this.fuelDateLabel.Text = "Fuel Date:";
            // 
            // fuelTypeLabel
            // 
            this.fuelTypeLabel.AutoSize = true;
            this.fuelTypeLabel.Location = new System.Drawing.Point(12, 123);
            this.fuelTypeLabel.Name = "fuelTypeLabel";
            this.fuelTypeLabel.Size = new System.Drawing.Size(76, 20);
            this.fuelTypeLabel.TabIndex = 11;
            this.fuelTypeLabel.Text = "Fuel Type:";
            // 
            // odometerLabel
            // 
            this.odometerLabel.AutoSize = true;
            this.odometerLabel.Location = new System.Drawing.Point(12, 153);
            this.odometerLabel.Name = "odometerLabel";
            this.odometerLabel.Size = new System.Drawing.Size(102, 20);
            this.odometerLabel.TabIndex = 12;
            this.odometerLabel.Text = "Odometer Reading:";
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.busNumberLabel);
            this.inputGroupBox.Controls.Add(this.fuelGallonsLabel);
            this.inputGroupBox.Controls.Add(this.fuelDateLabel);
            this.inputGroupBox.Controls.Add(this.fuelTypeLabel);
            this.inputGroupBox.Controls.Add(this.odometerLabel);
            this.inputGroupBox.Controls.Add(this.busNumberComboBox);
            this.inputGroupBox.Controls.Add(this.fuelGallonsTextBox);
            this.inputGroupBox.Controls.Add(this.fuelDatePicker);
            this.inputGroupBox.Controls.Add(this.fuelTypeTextBox);
            this.inputGroupBox.Controls.Add(this.odometerTextBox);
            this.inputGroupBox.Controls.Add(this.addFuelButton);
            this.inputGroupBox.Controls.Add(this.refreshButton);
            this.inputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputGroupBox.Location = new System.Drawing.Point(12, 218);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(800, 230);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Add New Fuel Record";
            this.inputGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 461);
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
            // FuelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(824, 492);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.statusStrip);
            this.Name = "FuelForm";
            this.Text = "Fuel Records";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox busNumberComboBox;
        private System.Windows.Forms.TextBox fuelGallonsTextBox;
        private System.Windows.Forms.DateTimePicker fuelDatePicker;
        private System.Windows.Forms.TextBox fuelTypeTextBox;
        private System.Windows.Forms.TextBox odometerTextBox;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button addFuelButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label busNumberLabel;
        private System.Windows.Forms.Label fuelGallonsLabel;
        private System.Windows.Forms.Label fuelDateLabel;
        private System.Windows.Forms.Label fuelTypeLabel;
        private System.Windows.Forms.Label odometerLabel;
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}