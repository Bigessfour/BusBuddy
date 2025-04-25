// filepath: c:\Users\steve.mckitrick\Desktop\BusBuddy\UI\Forms\ActivityForm.Designer.cs
#nullable enable

using System.Windows.Forms;
using System.Drawing;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    partial class ActivityForm
    {
        private System.ComponentModel.IContainer? components = null;
        private System.Windows.Forms.DataGridView? activitiesDataGridView;
        private System.Windows.Forms.Label? activityDateLabel;
        private System.Windows.Forms.DateTimePicker? activityDatePicker;
        private System.Windows.Forms.Label? activityBusNumberLabel;
        private System.Windows.Forms.ComboBox? activityBusNumberComboBox;
        private System.Windows.Forms.Label? activityDestinationLabel;
        private System.Windows.Forms.TextBox? activityDestinationTextBox;
        private System.Windows.Forms.Label? activityLeaveTimeLabel;
        private System.Windows.Forms.DateTimePicker? activityLeaveTimePicker;
        private System.Windows.Forms.Label? activityDriverLabel;
        private System.Windows.Forms.ComboBox? activityDriverComboBox;
        private System.Windows.Forms.Label? activityHoursDrivenLabel;
        private System.Windows.Forms.NumericUpDown? activityHoursNumericUpDown;
        private System.Windows.Forms.NumericUpDown? activityMinutesNumericUpDown;
        private System.Windows.Forms.Label? activityStudentsLabel;
        private System.Windows.Forms.NumericUpDown? activityStudentsNumericUpDown;
        private System.Windows.Forms.Button? addActivityButton;
        private System.Windows.Forms.Button? clearActivityButton;
        private System.Windows.Forms.Button? exitButton;
        private System.Windows.Forms.GroupBox? inputGroupBox;
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
            this.components = new System.ComponentModel.Container();
            this.activitiesDataGridView = new System.Windows.Forms.DataGridView();
            this.activityDateLabel = new System.Windows.Forms.Label();
            this.activityDatePicker = new System.Windows.Forms.DateTimePicker();
            this.activityBusNumberLabel = new System.Windows.Forms.Label();
            this.activityBusNumberComboBox = new System.Windows.Forms.ComboBox();
            this.activityDestinationLabel = new System.Windows.Forms.Label();
            this.activityDestinationTextBox = new System.Windows.Forms.TextBox();
            this.activityLeaveTimeLabel = new System.Windows.Forms.Label();
            this.activityLeaveTimePicker = new System.Windows.Forms.DateTimePicker();
            this.activityDriverLabel = new System.Windows.Forms.Label();
            this.activityDriverComboBox = new System.Windows.Forms.ComboBox();
            this.activityHoursDrivenLabel = new System.Windows.Forms.Label();
            this.activityHoursNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.activityMinutesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.activityStudentsLabel = new System.Windows.Forms.Label();
            this.activityStudentsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.addActivityButton = new System.Windows.Forms.Button();
            this.clearActivityButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            ((System.ComponentModel.ISupportInitialize)(this.activitiesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityHoursNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityMinutesNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityStudentsNumericUpDown)).BeginInit();
            this.inputGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // activitiesDataGridView
            this.activitiesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.activitiesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.activitiesDataGridView.Location = new System.Drawing.Point(12, 12);
            this.activitiesDataGridView.Name = "activitiesDataGridView";
            this.activitiesDataGridView.ReadOnly = true;
            this.activitiesDataGridView.RowHeadersWidth = 51;
            this.activitiesDataGridView.RowTemplate.Height = 24;
            this.activitiesDataGridView.Size = new System.Drawing.Size(760, 250);
            this.activitiesDataGridView.TabIndex = 0;
            this.activitiesDataGridView.AllowUserToAddRows = false;
            this.activitiesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.activitiesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // inputGroupBox
            this.inputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputGroupBox.Controls.Add(this.activityDateLabel);
            this.inputGroupBox.Controls.Add(this.activityDatePicker);
            this.inputGroupBox.Controls.Add(this.activityBusNumberLabel);
            this.inputGroupBox.Controls.Add(this.activityBusNumberComboBox);
            this.inputGroupBox.Controls.Add(this.activityDestinationLabel);
            this.inputGroupBox.Controls.Add(this.activityDestinationTextBox);
            this.inputGroupBox.Controls.Add(this.activityLeaveTimeLabel);
            this.inputGroupBox.Controls.Add(this.activityLeaveTimePicker);
            this.inputGroupBox.Controls.Add(this.activityDriverLabel);
            this.inputGroupBox.Controls.Add(this.activityDriverComboBox);
            this.inputGroupBox.Controls.Add(this.activityHoursDrivenLabel);
            this.inputGroupBox.Controls.Add(this.activityHoursNumericUpDown);
            this.inputGroupBox.Controls.Add(this.activityMinutesNumericUpDown);
            this.inputGroupBox.Controls.Add(this.activityStudentsLabel);
            this.inputGroupBox.Controls.Add(this.activityStudentsNumericUpDown);
            this.inputGroupBox.Controls.Add(this.addActivityButton);
            this.inputGroupBox.Controls.Add(this.clearActivityButton);
            this.inputGroupBox.Controls.Add(this.exitButton);
            this.inputGroupBox.Location = new System.Drawing.Point(12, 278);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(760, 240);
            this.inputGroupBox.TabIndex = 1;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Add/Edit Activity";

            // activityDateLabel
            this.activityDateLabel.AutoSize = true;
            this.activityDateLabel.Location = new System.Drawing.Point(15, 30);
            this.activityDateLabel.Name = "activityDateLabel";
            this.activityDateLabel.Size = new System.Drawing.Size(41, 17);
            this.activityDateLabel.TabIndex = 0;
            this.activityDateLabel.Text = "Date:";

            // activityDatePicker
            this.activityDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.activityDatePicker.Location = new System.Drawing.Point(125, 27);
            this.activityDatePicker.Name = "activityDatePicker";
            this.activityDatePicker.Size = new System.Drawing.Size(150, 22);
            this.activityDatePicker.TabIndex = 1;

            // activityBusNumberLabel
            this.activityBusNumberLabel.AutoSize = true;
            this.activityBusNumberLabel.Location = new System.Drawing.Point(15, 60);
            this.activityBusNumberLabel.Name = "activityBusNumberLabel";
            this.activityBusNumberLabel.Size = new System.Drawing.Size(88, 17);
            this.activityBusNumberLabel.TabIndex = 2;
            this.activityBusNumberLabel.Text = "Bus Number:";

            // activityBusNumberComboBox
            this.activityBusNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activityBusNumberComboBox.FormattingEnabled = true;
            this.activityBusNumberComboBox.Location = new System.Drawing.Point(125, 57);
            this.activityBusNumberComboBox.Name = "activityBusNumberComboBox";
            this.activityBusNumberComboBox.Size = new System.Drawing.Size(150, 24);
            this.activityBusNumberComboBox.TabIndex = 3;

            // activityDestinationLabel
            this.activityDestinationLabel.AutoSize = true;
            this.activityDestinationLabel.Location = new System.Drawing.Point(15, 90);
            this.activityDestinationLabel.Name = "activityDestinationLabel";
            this.activityDestinationLabel.Size = new System.Drawing.Size(83, 17);
            this.activityDestinationLabel.TabIndex = 4;
            this.activityDestinationLabel.Text = "Destination:";

            // activityDestinationTextBox
            this.activityDestinationTextBox.Location = new System.Drawing.Point(125, 87);
            this.activityDestinationTextBox.Name = "activityDestinationTextBox";
            this.activityDestinationTextBox.Size = new System.Drawing.Size(250, 22);
            this.activityDestinationTextBox.TabIndex = 5;

            // activityLeaveTimeLabel
            this.activityLeaveTimeLabel.AutoSize = true;
            this.activityLeaveTimeLabel.Location = new System.Drawing.Point(15, 120);
            this.activityLeaveTimeLabel.Name = "activityLeaveTimeLabel";
            this.activityLeaveTimeLabel.Size = new System.Drawing.Size(84, 17);
            this.activityLeaveTimeLabel.TabIndex = 6;
            this.activityLeaveTimeLabel.Text = "Leave Time:";

            // activityLeaveTimePicker
            this.activityLeaveTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.activityLeaveTimePicker.Location = new System.Drawing.Point(125, 117);
            this.activityLeaveTimePicker.Name = "activityLeaveTimePicker";
            this.activityLeaveTimePicker.ShowUpDown = true;
            this.activityLeaveTimePicker.Size = new System.Drawing.Size(150, 22);
            this.activityLeaveTimePicker.TabIndex = 7;

            // activityDriverLabel
            this.activityDriverLabel.AutoSize = true;
            this.activityDriverLabel.Location = new System.Drawing.Point(15, 150);
            this.activityDriverLabel.Name = "activityDriverLabel";
            this.activityDriverLabel.Size = new System.Drawing.Size(50, 17);
            this.activityDriverLabel.TabIndex = 8;
            this.activityDriverLabel.Text = "Driver:";

            // activityDriverComboBox
            this.activityDriverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activityDriverComboBox.FormattingEnabled = true;
            this.activityDriverComboBox.Location = new System.Drawing.Point(125, 147);
            this.activityDriverComboBox.Name = "activityDriverComboBox";
            this.activityDriverComboBox.Size = new System.Drawing.Size(250, 24);
            this.activityDriverComboBox.TabIndex = 9;

            // activityHoursDrivenLabel
            this.activityHoursDrivenLabel.AutoSize = true;
            this.activityHoursDrivenLabel.Location = new System.Drawing.Point(400, 30);
            this.activityHoursDrivenLabel.Name = "activityHoursDrivenLabel";
            this.activityHoursDrivenLabel.Size = new System.Drawing.Size(95, 17);
            this.activityHoursDrivenLabel.TabIndex = 10;
            this.activityHoursDrivenLabel.Text = "Hours Driven:";

            // activityHoursNumericUpDown
            this.activityHoursNumericUpDown.Location = new System.Drawing.Point(510, 28);
            this.activityHoursNumericUpDown.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            this.activityHoursNumericUpDown.Name = "activityHoursNumericUpDown";
            this.activityHoursNumericUpDown.Size = new System.Drawing.Size(70, 22);
            this.activityHoursNumericUpDown.TabIndex = 11;

            // activityMinutesNumericUpDown
            this.activityMinutesNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            this.activityMinutesNumericUpDown.Location = new System.Drawing.Point(590, 28);
            this.activityMinutesNumericUpDown.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            this.activityMinutesNumericUpDown.Name = "activityMinutesNumericUpDown";
            this.activityMinutesNumericUpDown.Size = new System.Drawing.Size(70, 22);
            this.activityMinutesNumericUpDown.TabIndex = 12;

            // activityStudentsLabel
            this.activityStudentsLabel.AutoSize = true;
            this.activityStudentsLabel.Location = new System.Drawing.Point(400, 60);
            this.activityStudentsLabel.Name = "activityStudentsLabel";
            this.activityStudentsLabel.Size = new System.Drawing.Size(112, 17);
            this.activityStudentsLabel.TabIndex = 13;
            this.activityStudentsLabel.Text = "Students Driven:";

            // activityStudentsNumericUpDown
            this.activityStudentsNumericUpDown.Location = new System.Drawing.Point(510, 58);
            this.activityStudentsNumericUpDown.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.activityStudentsNumericUpDown.Name = "activityStudentsNumericUpDown";
            this.activityStudentsNumericUpDown.Size = new System.Drawing.Size(150, 22);
            this.activityStudentsNumericUpDown.TabIndex = 14;

            // addActivityButton
            this.addActivityButton.Location = new System.Drawing.Point(403, 190);
            this.addActivityButton.Name = "addActivityButton";
            this.addActivityButton.Size = new System.Drawing.Size(100, 35);
            this.addActivityButton.TabIndex = 15;
            this.addActivityButton.Text = "Add";
            this.addActivityButton.UseVisualStyleBackColor = true;
            this.addActivityButton.Click += new System.EventHandler(this.AddActivityButton_Click);

            // clearActivityButton
            this.clearActivityButton.Location = new System.Drawing.Point(513, 190);
            this.clearActivityButton.Name = "clearActivityButton";
            this.clearActivityButton.Size = new System.Drawing.Size(100, 35);
            this.clearActivityButton.TabIndex = 16;
            this.clearActivityButton.Text = "Clear";
            this.clearActivityButton.UseVisualStyleBackColor = true;
            this.clearActivityButton.Click += new System.EventHandler(this.ClearActivityButton_Click);

            // exitButton
            this.exitButton.Location = new System.Drawing.Point(623, 190);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(100, 35);
            this.exitButton.TabIndex = 17;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);

            // statusStrip
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });
            this.statusStrip.Location = new System.Drawing.Point(0, 530);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";

            // statusLabel
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 16);

            // ActivityForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 552);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.inputGroupBox);
            this.Controls.Add(this.activitiesDataGridView);
            this.Name = "ActivityForm";
            this.Text = "Manage Activities - BusBuddy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.activitiesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityHoursNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityMinutesNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityStudentsNumericUpDown)).EndInit();
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}