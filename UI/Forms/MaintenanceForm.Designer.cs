namespace BusBuddy.UI.Forms
{
    partial class MaintenanceForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView maintenanceDataGridView;
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.Label busNumberLabel;
        private System.Windows.Forms.TextBox busNumberTextBox;
        private System.Windows.Forms.Label datePerformedLabel;
        private System.Windows.Forms.DateTimePicker datePerformedPicker;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label costLabel;
        private System.Windows.Forms.NumericUpDown costNumericUpDown;
        private System.Windows.Forms.Label odometerLabel;
        private System.Windows.Forms.NumericUpDown odometerNumericUpDown;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

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
            this.maintenanceDataGridView = new System.Windows.Forms.DataGridView();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.saveButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.busNumberLabel = new System.Windows.Forms.Label();
            this.busNumberTextBox = new System.Windows.Forms.TextBox();
            this.datePerformedLabel = new System.Windows.Forms.Label();
            this.datePerformedPicker = new System.Windows.Forms.DateTimePicker();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.costLabel = new System.Windows.Forms.Label();
            this.costNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.odometerLabel = new System.Windows.Forms.Label();
            this.odometerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceDataGridView)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.costNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.odometerNumericUpDown)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // maintenanceDataGridView
            // 
            this.maintenanceDataGridView.AllowUserToAddRows = false;
            this.maintenanceDataGridView.AllowUserToDeleteRows = false;
            this.maintenanceDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maintenanceDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.maintenanceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.maintenanceDataGridView.Location = new System.Drawing.Point(12, 12);
            this.maintenanceDataGridView.MultiSelect = false;
            this.maintenanceDataGridView.Name = "maintenanceDataGridView";
            this.maintenanceDataGridView.ReadOnly = true;
            this.maintenanceDataGridView.RowHeadersWidth = 51;
            this.maintenanceDataGridView.RowTemplate.Height = 24;
            this.maintenanceDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.maintenanceDataGridView.Size = new System.Drawing.Size(776, 250);
            this.maintenanceDataGridView.TabIndex = 0;
            this.maintenanceDataGridView.SelectionChanged += new System.EventHandler(this.MaintenanceDataGridView_SelectionChanged);
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputGroupBox.Controls.Add(this.odometerNumericUpDown);
            this.inputGroupBox.Controls.Add(this.odometerLabel);
            this.inputGroupBox.Controls.Add(this.costNumericUpDown);
            this.inputGroupBox.Controls.Add(this.costLabel);
            this.inputGroupBox.Controls.Add(this.descriptionTextBox);
            this.inputGroupBox.Controls.Add(this.descriptionLabel);
            this.inputGroupBox.Controls.Add(this.datePerformedPicker);
            this.inputGroupBox.Controls.Add(this.datePerformedLabel);
            this.inputGroupBox.Controls.Add(this.busNumberTextBox);
            this.inputGroupBox.Controls.Add(this.busNumberLabel);
            this.inputGroupBox.Controls.Add(this.buttonPanel);
            this.inputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputGroupBox.Location = new System.Drawing.Point(12, 270);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(776, 200);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Maintenance Record Details";
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.Controls.Add(this.saveButton);
            this.buttonPanel.Controls.Add(this.editButton);
            this.buttonPanel.Controls.Add(this.refreshButton);
            this.buttonPanel.Controls.Add(this.deleteButton);
            this.buttonPanel.Controls.Add(this.exitButton);
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(500, 150);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(270, 40);
            this.buttonPanel.TabIndex = 10;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(165, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 35);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(59, 3);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(100, 35);
            this.editButton.TabIndex = 1;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(3, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(100, 35);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(3, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(100, 35);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(3, 3);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(50, 35);
            this.exitButton.TabIndex = 4;
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
            // datePerformedLabel
            // 
            this.datePerformedLabel.AutoSize = true;
            this.datePerformedLabel.Location = new System.Drawing.Point(20, 60);
            this.datePerformedLabel.Name = "datePerformedLabel";
            this.datePerformedLabel.Size = new System.Drawing.Size(100, 19);
            this.datePerformedLabel.TabIndex = 2;
            this.datePerformedLabel.Text = "Date Performed:";
            // 
            // datePerformedPicker
            // 
            this.datePerformedPicker.Location = new System.Drawing.Point(120, 57);
            this.datePerformedPicker.Name = "datePerformedPicker";
            this.datePerformedPicker.Size = new System.Drawing.Size(200, 25);
            this.datePerformedPicker.TabIndex = 3;
            this.datePerformedPicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(20, 90);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(90, 19);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(120, 87);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(200, 25);
            this.descriptionTextBox.TabIndex = 5;
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
            // odometerLabel
            // 
            this.odometerLabel.AutoSize = true;
            this.odometerLabel.Location = new System.Drawing.Point(350, 60);
            this.odometerLabel.Name = "odometerLabel";
            this.odometerLabel.Size = new System.Drawing.Size(70, 19);
            this.odometerLabel.TabIndex = 8;
            this.odometerLabel.Text = "Odometer:";
            // 
            // odometerNumericUpDown
            // 
            this.odometerNumericUpDown.Location = new System.Drawing.Point(450, 57);
            this.odometerNumericUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.odometerNumericUpDown.Name = "odometerNumericUpDown";
            this.odometerNumericUpDown.Size = new System.Drawing.Size(100, 25);
            this.odometerNumericUpDown.TabIndex = 9;
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
            // MaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 510);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.maintenanceDataGridView);
            this.Name = "MaintenanceForm";
            this.Text = "Maintenance Records - BusBuddy";
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceDataGridView)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.costNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.odometerNumericUpDown)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}