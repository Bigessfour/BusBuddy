namespace BusBuddy.UI.Forms
{
    partial class FuelForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView fuelDataGridView;
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.Label busNumberLabel;
        private System.Windows.Forms.TextBox busNumberTextBox;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Label gallonsLabel;
        private System.Windows.Forms.NumericUpDown gallonsNumericUpDown;
        private System.Windows.Forms.Label costLabel;
        private System.Windows.Forms.NumericUpDown costNumericUpDown;
        private System.Windows.Forms.Label fuelTypeLabel;
        private System.Windows.Forms.TextBox fuelTypeTextBox;
        private System.Windows.Forms.Label odometerLabel;
        private System.Windows.Forms.NumericUpDown odometerNumericUpDown;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button testGrokButton; // Added Grok test button

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
            this.fuelDataGridView = new System.Windows.Forms.DataGridView();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.saveButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.testGrokButton = new System.Windows.Forms.Button(); // Initialize Grok test button
            this.busNumberLabel = new System.Windows.Forms.Label();
            this.busNumberTextBox = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.gallonsLabel = new System.Windows.Forms.Label();
            this.gallonsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.costLabel = new System.Windows.Forms.Label();
            this.costNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fuelTypeLabel = new System.Windows.Forms.Label();
            this.fuelTypeTextBox = new System.Windows.Forms.TextBox();
            this.odometerLabel = new System.Windows.Forms.Label();
            this.odometerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.fuelDataGridView)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gallonsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.odometerNumericUpDown)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // fuelDataGridView
            // 
            this.fuelDataGridView.AllowUserToAddRows = false;
            this.fuelDataGridView.AllowUserToDeleteRows = false;
            this.fuelDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fuelDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.fuelDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fuelDataGridView.Location = new System.Drawing.Point(12, 12);
            this.fuelDataGridView.MultiSelect = false;
            this.fuelDataGridView.Name = "fuelDataGridView";
            this.fuelDataGridView.ReadOnly = true;
            this.fuelDataGridView.RowHeadersWidth = 51;
            this.fuelDataGridView.RowTemplate.Height = 24;
            this.fuelDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.fuelDataGridView.Size = new System.Drawing.Size(776, 250);
            this.fuelDataGridView.TabIndex = 0;
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputGroupBox.Controls.Add(this.odometerNumericUpDown);
            this.inputGroupBox.Controls.Add(this.odometerLabel);
            this.inputGroupBox.Controls.Add(this.fuelTypeTextBox);
            this.inputGroupBox.Controls.Add(this.fuelTypeLabel);
            this.inputGroupBox.Controls.Add(this.costNumericUpDown);
            this.inputGroupBox.Controls.Add(this.costLabel);
            this.inputGroupBox.Controls.Add(this.gallonsNumericUpDown);
            this.inputGroupBox.Controls.Add(this.gallonsLabel);
            this.inputGroupBox.Controls.Add(this.datePicker);
            this.inputGroupBox.Controls.Add(this.dateLabel);
            this.inputGroupBox.Controls.Add(this.busNumberTextBox);
            this.inputGroupBox.Controls.Add(this.busNumberLabel);
            this.inputGroupBox.Controls.Add(this.buttonPanel);
            this.inputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputGroupBox.Location = new System.Drawing.Point(12, 270);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(776, 200);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Fuel Record Details";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.Controls.Add(this.saveButton);
            this.buttonPanel.Controls.Add(this.refreshButton);
            this.buttonPanel.Controls.Add(this.testGrokButton);
            this.buttonPanel.Controls.Add(this.exitButton);
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(400, 150);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(370, 40);
            this.buttonPanel.TabIndex = 10;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(267, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 35);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(161, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(100, 35);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // testGrokButton
            // 
            this.testGrokButton.Location = new System.Drawing.Point(55, 3);
            this.testGrokButton.Name = "testGrokButton";
            this.testGrokButton.Size = new System.Drawing.Size(100, 35);
            this.testGrokButton.TabIndex = 3;
            this.testGrokButton.Text = "Test Grok";
            this.testGrokButton.UseVisualStyleBackColor = true;
            this.testGrokButton.Click += new System.EventHandler(this.TestGrokButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(3, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(46, 35);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // busNumberLabel
            // 
            this.busNumberLabel.AutoSize = true;
            this.busNumberLabel.Location = new System.Drawing.Point(20, 30);
            this.busNumberLabel.Name = "busNumberLabel";
            this.busNumberLabel.Size = new System.Drawing.Size(80, 19);
            this.busNumberLabel.TabIndex = 0;
            this.busNumberLabel.Text = "Bus Number:";
            // 
            // busNumberTextBox
            // 
            this.busNumberTextBox.Location = new System.Drawing.Point(120, 27);
            this.busNumberTextBox.Name = "busNumberTextBox";
            this.busNumberTextBox.Size = new System.Drawing.Size(100, 25);
            this.busNumberTextBox.TabIndex = 1;
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(20, 60);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(44, 19);
            this.dateLabel.TabIndex = 2;
            this.dateLabel.Text = "Date:";
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(120, 57);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(200, 25);
            this.datePicker.TabIndex = 3;
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // 
            // gallonsLabel
            // 
            this.gallonsLabel.AutoSize = true;
            this.gallonsLabel.Location = new System.Drawing.Point(20, 90);
            this.gallonsLabel.Name = "gallonsLabel";
            this.gallonsLabel.Size = new System.Drawing.Size(60, 19);
            this.gallonsLabel.TabIndex = 4;
            this.gallonsLabel.Text = "Gallons:";
            // 
            // gallonsNumericUpDown
            // 
            this.gallonsNumericUpDown.DecimalPlaces = 2;
            this.gallonsNumericUpDown.Location = new System.Drawing.Point(120, 87);
            this.gallonsNumericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.gallonsNumericUpDown.Name = "gallonsNumericUpDown";
            this.gallonsNumericUpDown.Size = new System.Drawing.Size(100, 25);
            this.gallonsNumericUpDown.TabIndex = 5;
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(350, 30);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(40, 19);
            this.costLabel.TabIndex = 6;
            this.costLabel.Text = "Cost:";
            // 
            // costNumericUpDown
            // 
            this.costNumericUpDown.DecimalPlaces = 2;
            this.costNumericUpDown.Location = new System.Drawing.Point(450, 27);
            this.costNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.costNumericUpDown.Name = "costNumericUpDown";
            this.costNumericUpDown.Size = new System.Drawing.Size(100, 25);
            this.costNumericUpDown.TabIndex = 7;
            // 
            // fuelTypeLabel
            // 
            this.fuelTypeLabel.AutoSize = true;
            this.fuelTypeLabel.Location = new System.Drawing.Point(350, 60);
            this.fuelTypeLabel.Name = "fuelTypeLabel";
            this.fuelTypeLabel.Size = new System.Drawing.Size(70, 19);
            this.fuelTypeLabel.TabIndex = 8;
            this.fuelTypeLabel.Text = "Fuel Type:";
            // 
            // fuelTypeTextBox
            // 
            this.fuelTypeTextBox.Location = new System.Drawing.Point(450, 57);
            this.fuelTypeTextBox.Name = "fuelTypeTextBox";
            this.fuelTypeTextBox.Size = new System.Drawing.Size(100, 25);
            this.fuelTypeTextBox.TabIndex = 9;
            // 
            // odometerLabel
            // 
            this.odometerLabel.AutoSize = true;
            this.odometerLabel.Location = new System.Drawing.Point(350, 90);
            this.odometerLabel.Name = "odometerLabel";
            this.odometerLabel.Size = new System.Drawing.Size(70, 19);
            this.odometerLabel.TabIndex = 10;
            this.odometerLabel.Text = "Odometer:";
            // 
            // odometerNumericUpDown
            // 
            this.odometerNumericUpDown.Location = new System.Drawing.Point(450, 87);
            this.odometerNumericUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.odometerNumericUpDown.Name = "odometerNumericUpDown";
            this.odometerNumericUpDown.Size = new System.Drawing.Size(100, 25);
            this.odometerNumericUpDown.TabIndex = 11;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });
            this.statusStrip.Location = new System.Drawing.Point(0, 488);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready.";
            // 
            // FuelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 510);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.fuelDataGridView);
            this.Name = "FuelForm";
            this.Text = "Fuel Records - BusBuddy";
            ((System.ComponentModel.ISupportInitialize)(this.fuelDataGridView)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gallonsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.odometerNumericUpDown)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}