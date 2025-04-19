// BusBuddy/UI/Forms/SchedulerForm.Designer.cs
namespace BusBuddy.UI.Forms
{
    partial class SchedulerForm
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tripTypeComboBox = new System.Windows.Forms.ComboBox();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.busNumberComboBox = new System.Windows.Forms.ComboBox();
            this.driverNameComboBox = new System.Windows.Forms.ComboBox();
            this.startTimeTextBox = new System.Windows.Forms.TextBox();
            this.endTimeTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.addTripButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.tripTypeLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.busNumberLabel = new System.Windows.Forms.Label();
            this.driverNameLabel = new System.Windows.Forms.Label();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tripTypeComboBox
            // 
            this.tripTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tripTypeComboBox.Items.AddRange(new object[] { "Regular", "Field Trip", "Charter" });
            this.tripTypeComboBox.Location = new System.Drawing.Point(120, 30);
            this.tripTypeComboBox.Name = "tripTypeComboBox";
            this.tripTypeComboBox.Size = new System.Drawing.Size(150, 28);
            this.tripTypeComboBox.TabIndex = 1;
            // 
            // datePicker
            // 
            this.datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePicker.Location = new System.Drawing.Point(120, 60);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(150, 26);
            this.datePicker.TabIndex = 2;
            // 
            // busNumberComboBox
            // 
            this.busNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.busNumberComboBox.Location = new System.Drawing.Point(120, 90);
            this.busNumberComboBox.Name = "busNumberComboBox";
            this.busNumberComboBox.Size = new System.Drawing.Size(150, 28);
            this.busNumberComboBox.TabIndex = 3;
            // 
            // driverNameComboBox
            // 
            this.driverNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverNameComboBox.Location = new System.Drawing.Point(120, 120);
            this.driverNameComboBox.Name = "driverNameComboBox";
            this.driverNameComboBox.Size = new System.Drawing.Size(150, 28);
            this.driverNameComboBox.TabIndex = 4;
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(120, 150);
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(150, 26);
            this.startTimeTextBox.TabIndex = 5;
            // 
            // endTimeTextBox
            // 
            this.endTimeTextBox.Location = new System.Drawing.Point(120, 180);
            this.endTimeTextBox.Name = "endTimeTextBox";
            this.endTimeTextBox.Size = new System.Drawing.Size(150, 26);
            this.endTimeTextBox.TabIndex = 6;
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
            // addTripButton
            // 
            this.addTripButton.FlatAppearance.BorderSize = 0;
            this.addTripButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTripButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addTripButton.Location = new System.Drawing.Point(120, 210);
            this.addTripButton.Name = "addTripButton";
            this.addTripButton.Size = new System.Drawing.Size(120, 35);
            this.addTripButton.TabIndex = 7;
            this.addTripButton.Text = "Add Trip";
            this.addTripButton.UseVisualStyleBackColor = true;
            this.addTripButton.Click += new System.EventHandler(this.AddTripButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshButton.Location = new System.Drawing.Point(250, 210);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 35);
            this.refreshButton.TabIndex = 8;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // tripTypeLabel
            // 
            this.tripTypeLabel.AutoSize = true;
            this.tripTypeLabel.Location = new System.Drawing.Point(12, 33);
            this.tripTypeLabel.Name = "tripTypeLabel";
            this.tripTypeLabel.Size = new System.Drawing.Size(74, 20);
            this.tripTypeLabel.TabIndex = 9;
            this.tripTypeLabel.Text = "Trip Type:";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(12, 63);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(43, 20);
            this.dateLabel.TabIndex = 10;
            this.dateLabel.Text = "Date:";
            // 
            // busNumberLabel
            // 
            this.busNumberLabel.AutoSize = true;
            this.busNumberLabel.Location = new System.Drawing.Point(12, 93);
            this.busNumberLabel.Name = "busNumberLabel";
            this.busNumberLabel.Size = new System.Drawing.Size(93, 20);
            this.busNumberLabel.TabIndex = 11;
            this.busNumberLabel.Text = "Bus Number:";
            // 
            // driverNameLabel
            // 
            this.driverNameLabel.AutoSize = true;
            this.driverNameLabel.Location = new System.Drawing.Point(12, 123);
            this.driverNameLabel.Name = "driverNameLabel";
            this.driverNameLabel.Size = new System.Drawing.Size(98, 20);
            this.driverNameLabel.TabIndex = 12;
            this.driverNameLabel.Text = "Driver Name:";
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.AutoSize = true;
            this.startTimeLabel.Location = new System.Drawing.Point(12, 153);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(82, 20);
            this.startTimeLabel.TabIndex = 13;
            this.startTimeLabel.Text = "Start Time:";
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.AutoSize = true;
            this.endTimeLabel.Location = new System.Drawing.Point(12, 183);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(76, 20);
            this.endTimeLabel.TabIndex = 14;
            this.endTimeLabel.Text = "End Time:";
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.tripTypeLabel);
            this.inputGroupBox.Controls.Add(this.dateLabel);
            this.inputGroupBox.Controls.Add(this.busNumberLabel);
            this.inputGroupBox.Controls.Add(this.driverNameLabel);
            this.inputGroupBox.Controls.Add(this.startTimeLabel);
            this.inputGroupBox.Controls.Add(this.endTimeLabel);
            this.inputGroupBox.Controls.Add(this.tripTypeComboBox);
            this.inputGroupBox.Controls.Add(this.datePicker);
            this.inputGroupBox.Controls.Add(this.busNumberComboBox);
            this.inputGroupBox.Controls.Add(this.driverNameComboBox);
            this.inputGroupBox.Controls.Add(this.startTimeTextBox);
            this.inputGroupBox.Controls.Add(this.endTimeTextBox);
            this.inputGroupBox.Controls.Add(this.addTripButton);
            this.inputGroupBox.Controls.Add(this.refreshButton);
            this.inputGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inputGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputGroupBox.Location = new System.Drawing.Point(12, 218);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(800, 260);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Add New Trip";
            this.inputGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 491);
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
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(824, 522);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.statusStrip);
            this.Name = "SchedulerForm";
            this.Text = "Trip Scheduler";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox tripTypeComboBox;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.ComboBox busNumberComboBox;
        private System.Windows.Forms.ComboBox driverNameComboBox;
        private System.Windows.Forms.TextBox startTimeTextBox;
        private System.Windows.Forms.TextBox endTimeTextBox;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button addTripButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label tripTypeLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label busNumberLabel;
        private System.Windows.Forms.Label driverNameLabel;
        private System.Windows.Forms.Label startTimeLabel;
        private System.Windows.Forms.Label endTimeLabel;
        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}