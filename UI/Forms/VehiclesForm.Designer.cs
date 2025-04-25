#nullable enable

namespace BusBuddy.UI.Forms
{
    /// <summary>
    /// Form for managing the fleet of school buses and their details.
    /// </summary>
    partial class VehiclesForm
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.DataGridView? vehiclesDataGridView;
        private System.Windows.Forms.GroupBox? inputGroupBox;
        private System.Windows.Forms.Label? busNumberLabel;
        private System.Windows.Forms.TextBox? busNumberTextBox;
        private System.Windows.Forms.Label? makeLabel;
        private System.Windows.Forms.TextBox? makeTextBox;
        private System.Windows.Forms.Label? modelLabel;
        private System.Windows.Forms.TextBox? modelTextBox;
        private System.Windows.Forms.Label? yearLabel;
        private System.Windows.Forms.NumericUpDown? yearNumericUpDown;
        private System.Windows.Forms.Label? vinLabel;
        private System.Windows.Forms.TextBox? vinTextBox;
        private System.Windows.Forms.Label? capacityLabel;
        private System.Windows.Forms.NumericUpDown? capacityNumericUpDown;
        private System.Windows.Forms.CheckBox? isOperationalCheckBox;
        private System.Windows.Forms.Label? odometerLabel;
        private System.Windows.Forms.NumericUpDown? odometerNumericUpDown;
        private System.Windows.Forms.Label? plateNumberLabel;
        private System.Windows.Forms.TextBox? plateNumberTextBox;
        private System.Windows.Forms.Label? purchaseDateLabel;
        private System.Windows.Forms.DateTimePicker? purchaseDateTimePicker;
        private System.Windows.Forms.Label? lastInspectionDateLabel;
        private System.Windows.Forms.DateTimePicker? lastInspectionDateTimePicker;
        private System.Windows.Forms.FlowLayoutPanel? flowLayoutPanelButtons;
        private System.Windows.Forms.Button? saveButton;
        private System.Windows.Forms.Button? editButton;
        private System.Windows.Forms.Button? refreshButton;
        private System.Windows.Forms.Button? deleteButton;
        private System.Windows.Forms.Button? exitButton;
        private System.Windows.Forms.StatusStrip? statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel? statusLabel;

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
            // Initialize controls
            this.vehiclesDataGridView = new System.Windows.Forms.DataGridView();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.saveButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.busNumberLabel = new System.Windows.Forms.Label();
            this.busNumberTextBox = new System.Windows.Forms.TextBox();
            this.makeLabel = new System.Windows.Forms.Label();
            this.makeTextBox = new System.Windows.Forms.TextBox();
            this.modelLabel = new System.Windows.Forms.Label();
            this.modelTextBox = new System.Windows.Forms.TextBox();
            this.yearLabel = new System.Windows.Forms.Label();
            this.yearNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.vinLabel = new System.Windows.Forms.Label();
            this.vinTextBox = new System.Windows.Forms.TextBox();
            this.capacityLabel = new System.Windows.Forms.Label();
            this.capacityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.isOperationalCheckBox = new System.Windows.Forms.CheckBox();
            this.odometerLabel = new System.Windows.Forms.Label();
            this.odometerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.plateNumberLabel = new System.Windows.Forms.Label();
            this.plateNumberTextBox = new System.Windows.Forms.TextBox();
            this.purchaseDateLabel = new System.Windows.Forms.Label();
            this.purchaseDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.lastInspectionDateLabel = new System.Windows.Forms.Label();
            this.lastInspectionDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.vehiclesDataGridView)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yearNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.capacityNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.odometerNumericUpDown)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // vehiclesDataGridView
            // 
            this.vehiclesDataGridView.AllowUserToAddRows = false;
            this.vehiclesDataGridView.AllowUserToDeleteRows = false;
            this.vehiclesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vehiclesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.vehiclesDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.vehiclesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vehiclesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vehiclesDataGridView.Location = new System.Drawing.Point(12, 12);
            this.vehiclesDataGridView.MultiSelect = false;
            this.vehiclesDataGridView.Name = "vehiclesDataGridView";
            this.vehiclesDataGridView.ReadOnly = true;
            this.vehiclesDataGridView.RowHeadersWidth = 51;
            this.vehiclesDataGridView.RowTemplate.Height = 24;
            this.vehiclesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.vehiclesDataGridView.Size = new System.Drawing.Size(776, 250);
            this.vehiclesDataGridView.TabIndex = 0;
            this.vehiclesDataGridView.SelectionChanged += new System.EventHandler(this.VehiclesDataGridView_SelectionChanged);
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.inputGroupBox.Controls.Add(this.lastInspectionDateLabel);
            this.inputGroupBox.Controls.Add(this.lastInspectionDateTimePicker);
            this.inputGroupBox.Controls.Add(this.purchaseDateLabel);
            this.inputGroupBox.Controls.Add(this.purchaseDateTimePicker);
            this.inputGroupBox.Controls.Add(this.plateNumberLabel);
            this.inputGroupBox.Controls.Add(this.plateNumberTextBox);
            this.inputGroupBox.Controls.Add(this.odometerLabel);
            this.inputGroupBox.Controls.Add(this.odometerNumericUpDown);
            this.inputGroupBox.Controls.Add(this.isOperationalCheckBox);
            this.inputGroupBox.Controls.Add(this.capacityLabel);
            this.inputGroupBox.Controls.Add(this.capacityNumericUpDown);
            this.inputGroupBox.Controls.Add(this.vinLabel);
            this.inputGroupBox.Controls.Add(this.vinTextBox);
            this.inputGroupBox.Controls.Add(this.yearLabel);
            this.inputGroupBox.Controls.Add(this.yearNumericUpDown);
            this.inputGroupBox.Controls.Add(this.modelLabel);
            this.inputGroupBox.Controls.Add(this.modelTextBox);
            this.inputGroupBox.Controls.Add(this.makeLabel);
            this.inputGroupBox.Controls.Add(this.makeTextBox);
            this.inputGroupBox.Controls.Add(this.busNumberLabel);
            this.inputGroupBox.Controls.Add(this.busNumberTextBox);
            this.inputGroupBox.Controls.Add(this.flowLayoutPanelButtons);
            this.inputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputGroupBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.inputGroupBox.Location = new System.Drawing.Point(12, 270);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(776, 250);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Vehicle Details";
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelButtons.Controls.Add(this.saveButton);
            this.flowLayoutPanelButtons.Controls.Add(this.editButton);
            this.flowLayoutPanelButtons.Controls.Add(this.refreshButton);
            this.flowLayoutPanelButtons.Controls.Add(this.deleteButton);
            this.flowLayoutPanelButtons.Controls.Add(this.exitButton);
            this.flowLayoutPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(226, 195);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(544, 45);
            this.flowLayoutPanelButtons.TabIndex = 16;
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(441, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 35);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // editButton
            // 
            this.editButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(99)))), ((int)(((byte)(177)))));
            this.editButton.Enabled = false;
            this.editButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editButton.ForeColor = System.Drawing.Color.White;
            this.editButton.Location = new System.Drawing.Point(335, 3);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(100, 35);
            this.editButton.TabIndex = 1;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = false;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(99)))), ((int)(((byte)(177)))));
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.ForeColor = System.Drawing.Color.White;
            this.refreshButton.Location = new System.Drawing.Point(229, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(100, 35);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.deleteButton.Enabled = false;
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.ForeColor = System.Drawing.Color.White;
            this.deleteButton.Location = new System.Drawing.Point(123, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(100, 35);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.ForeColor = System.Drawing.Color.White;
            this.exitButton.Location = new System.Drawing.Point(17, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(100, 35);
            this.exitButton.TabIndex = 4;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // busNumberLabel
            // 
            this.busNumberLabel.AutoSize = true;
            this.busNumberLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.busNumberLabel.Location = new System.Drawing.Point(20, 30);
            this.busNumberLabel.Name = "busNumberLabel";
            this.busNumberLabel.Size = new System.Drawing.Size(80, 19);
            this.busNumberLabel.TabIndex = 0;
            this.busNumberLabel.Text = "Bus Number:";
            // 
            // busNumberTextBox
            // 
            this.busNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.busNumberTextBox.Location = new System.Drawing.Point(120, 27);
            this.busNumberTextBox.Name = "busNumberTextBox";
            this.busNumberTextBox.Size = new System.Drawing.Size(100, 25);
            this.busNumberTextBox.TabIndex = 1;
            // 
            // makeLabel
            // 
            this.makeLabel.AutoSize = true;
            this.makeLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.makeLabel.Location = new System.Drawing.Point(20, 60);
            this.makeLabel.Name = "makeLabel";
            this.makeLabel.Size = new System.Drawing.Size(44, 19);
            this.makeLabel.TabIndex = 2;
            this.makeLabel.Text = "Make:";
            // 
            // makeTextBox
            // 
            this.makeTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.makeTextBox.Location = new System.Drawing.Point(120, 57);
            this.makeTextBox.Name = "makeTextBox";
            this.makeTextBox.Size = new System.Drawing.Size(200, 25);
            this.makeTextBox.TabIndex = 3;
            // 
            // modelLabel
            // 
            this.modelLabel.AutoSize = true;
            this.modelLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.modelLabel.Location = new System.Drawing.Point(20, 90);
            this.modelLabel.Name = "modelLabel";
            this.modelLabel.Size = new System.Drawing.Size(48, 19);
            this.modelLabel.TabIndex = 4;
            this.modelLabel.Text = "Model:";
            // 
            // modelTextBox
            // 
            this.modelTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.modelTextBox.Location = new System.Drawing.Point(120, 87);
            this.modelTextBox.Name = "modelTextBox";
            this.modelTextBox.Size = new System.Drawing.Size(200, 25);
            this.modelTextBox.TabIndex = 5;
            // 
            // yearLabel
            // 
            this.yearLabel.AutoSize = true;
            this.yearLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.yearLabel.Location = new System.Drawing.Point(20, 120);
            this.yearLabel.Name = "yearLabel";
            this.yearLabel.Size = new System.Drawing.Size(39, 19);
            this.yearLabel.TabIndex = 6;
            this.yearLabel.Text = "Year:";
            // 
            // yearNumericUpDown
            // 
            this.yearNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.yearNumericUpDown.Location = new System.Drawing.Point(120, 117);
            this.yearNumericUpDown.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.yearNumericUpDown.Minimum = new decimal(new int[] {
            1980,
            0,
            0,
            0});
            this.yearNumericUpDown.Name = "yearNumericUpDown";
            this.yearNumericUpDown.Size = new System.Drawing.Size(100, 25);
            this.yearNumericUpDown.TabIndex = 7;
            this.yearNumericUpDown.Value = new decimal(new int[] {
            2024,
            0,
            0,
            0});
            // 
            // vinLabel
            // 
            this.vinLabel.AutoSize = true;
            this.vinLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.vinLabel.Location = new System.Drawing.Point(350, 30);
            this.vinLabel.Name = "vinLabel";
            this.vinLabel.Size = new System.Drawing.Size(32, 19);
            this.vinLabel.TabIndex = 8;
            this.vinLabel.Text = "VIN:";
            // 
            // vinTextBox
            // 
            this.vinTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.vinTextBox.Location = new System.Drawing.Point(450, 27);
            this.vinTextBox.Name = "vinTextBox";
            this.vinTextBox.Size = new System.Drawing.Size(250, 25);
            this.vinTextBox.TabIndex = 9;
            // 
            // capacityLabel
            // 
            this.capacityLabel.AutoSize = true;
            this.capacityLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.capacityLabel.Location = new System.Drawing.Point(350, 60);
            this.capacityLabel.Name = "capacityLabel";
            this.capacityLabel.Size = new System.Drawing.Size(63, 19);
            this.capacityLabel.TabIndex = 10;
            this.capacityLabel.Text = "Capacity:";
            // 
            // capacityNumericUpDown
            // 
            this.capacityNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.capacityNumericUpDown.Location = new System.Drawing.Point(450, 57);
            this.capacityNumericUpDown.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.capacityNumericUpDown.Name = "capacityNumericUpDown";
            this.capacityNumericUpDown.Size = new System.Drawing.Size(100, 25);
            this.capacityNumericUpDown.TabIndex = 11;
            // 
            // isOperationalCheckBox
            // 
            this.isOperationalCheckBox.AutoSize = true;
            this.isOperationalCheckBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.isOperationalCheckBox.Location = new System.Drawing.Point(450, 89);
            this.isOperationalCheckBox.Name = "isOperationalCheckBox";
            this.isOperationalCheckBox.Size = new System.Drawing.Size(107, 23);
            this.isOperationalCheckBox.TabIndex = 12;
            this.isOperationalCheckBox.Text = "Operational?";
            this.isOperationalCheckBox.UseVisualStyleBackColor = true;
            // 
            // odometerLabel
            // 
            this.odometerLabel.AutoSize = true;
            this.odometerLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.odometerLabel.Location = new System.Drawing.Point(350, 120);
            this.odometerLabel.Name = "odometerLabel";
            this.odometerLabel.Size = new System.Drawing.Size(70, 19);
            this.odometerLabel.TabIndex = 13;
            this.odometerLabel.Text = "Odometer:";
            // 
            // odometerNumericUpDown
            // 
            this.odometerNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.odometerNumericUpDown.Location = new System.Drawing.Point(450, 117);
            this.odometerNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.odometerNumericUpDown.Name = "odometerNumericUpDown";
            this.odometerNumericUpDown.Size = new System.Drawing.Size(120, 25);
            this.odometerNumericUpDown.TabIndex = 14;
            // 
            // plateNumberLabel
            // 
            this.plateNumberLabel.AutoSize = true;
            this.plateNumberLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.plateNumberLabel.Location = new System.Drawing.Point(350, 150);
            this.plateNumberLabel.Name = "plateNumberLabel";
            this.plateNumberLabel.Size = new System.Drawing.Size(90, 19);
            this.plateNumberLabel.TabIndex = 15;
            this.plateNumberLabel.Text = "Plate Number:";
            // 
            // plateNumberTextBox
            // 
            this.plateNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.plateNumberTextBox.Location = new System.Drawing.Point(450, 147);
            this.plateNumberTextBox.Name = "plateNumberTextBox";
            this.plateNumberTextBox.Size = new System.Drawing.Size(120, 25);
            this.plateNumberTextBox.TabIndex = 16;
            // 
            // purchaseDateLabel
            // 
            this.purchaseDateLabel.AutoSize = true;
            this.purchaseDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.purchaseDateLabel.Location = new System.Drawing.Point(20, 150);
            this.purchaseDateLabel.Name = "purchaseDateLabel";
            this.purchaseDateLabel.Size = new System.Drawing.Size(98, 19);
            this.purchaseDateLabel.TabIndex = 17;
            this.purchaseDateLabel.Text = "Purchase Date:";
            // 
            // purchaseDateTimePicker
            // 
            this.purchaseDateTimePicker.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.purchaseDateTimePicker.Location = new System.Drawing.Point(120, 147);
            this.purchaseDateTimePicker.Name = "purchaseDateTimePicker";
            this.purchaseDateTimePicker.Size = new System.Drawing.Size(200, 25);
            this.purchaseDateTimePicker.TabIndex = 18;
            this.purchaseDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // 
            // lastInspectionDateLabel
            // 
            this.lastInspectionDateLabel.AutoSize = true;
            this.lastInspectionDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lastInspectionDateLabel.Location = new System.Drawing.Point(350, 180);
            this.lastInspectionDateLabel.Name = "lastInspectionDateLabel";
            this.lastInspectionDateLabel.Size = new System.Drawing.Size(134, 19);
            this.lastInspectionDateLabel.TabIndex = 19;
            this.lastInspectionDateLabel.Text = "Last Inspection Date:";
            // 
            // lastInspectionDateTimePicker
            // 
            this.lastInspectionDateTimePicker.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lastInspectionDateTimePicker.Location = new System.Drawing.Point(490, 177);
            this.lastInspectionDateTimePicker.Name = "lastInspectionDateTimePicker";
            this.lastInspectionDateTimePicker.Size = new System.Drawing.Size(200, 25);
            this.lastInspectionDateTimePicker.TabIndex = 20;
            this.lastInspectionDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 538);
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
            // VehiclesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(800, 560);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.vehiclesDataGridView);
            this.Name = "VehiclesForm";
            this.Text = "Vehicle Management - BusBuddy";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.vehiclesDataGridView)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.yearNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.capacityNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.odometerNumericUpDown)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SaveButton_Click(object? sender, System.EventArgs e) => SaveRecord();
        private void EditButton_Click(object? sender, System.EventArgs e) => EditRecord();
        private void RefreshButton_Click(object? sender, System.EventArgs e) => RefreshData();
        private void DeleteButton_Click(object? sender, System.EventArgs e) => DeleteRecord();
        private void ExitButton_Click(object? sender, System.EventArgs e) => this.Close();
    }
}
#nullable restore