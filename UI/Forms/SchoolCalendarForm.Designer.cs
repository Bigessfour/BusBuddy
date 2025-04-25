// BusBuddy/UI/Forms/SchoolCalendarForm.Designer.cs
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    partial class SchoolCalendarForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "School Calendar - BusBuddy";
            this.Name = "SchoolCalendarForm";
            this.Size = new System.Drawing.Size(950, 600);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Resize += SchoolCalendarForm_Resize;

            instructionLabel1 = new System.Windows.Forms.Label
            {
                Text = "1. Select a date below:",
                Location = new System.Drawing.Point(12, 12),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            calendarPicker = new System.Windows.Forms.MonthCalendar
            {
                Location = new System.Drawing.Point(12, 35),
                MaxSelectionCount = 31,
                Size = new System.Drawing.Size(460, 320),
                CalendarDimensions = new System.Drawing.Size(2, 2),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom
            };
            calendarPicker.DateChanged += CalendarPicker_DateChanged;

            int rightColumnX = calendarPicker.Right + 30;
            int rightColumnWidth = this.ClientSize.Width - rightColumnX - 20;

            selectedDateLabel = new System.Windows.Forms.Label
            {
                Location = new System.Drawing.Point(rightColumnX, 20),
                Size = new System.Drawing.Size(rightColumnWidth, 30),
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            instructionLabel2 = new System.Windows.Forms.Label
            {
                Text = "2. Set details for the selected date:",
                Location = new System.Drawing.Point(rightColumnX, 60),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            dayDetailsGroupBox = new System.Windows.Forms.GroupBox
            {
                Text = "Day Details",
                Location = new System.Drawing.Point(rightColumnX, 85),
                Size = new System.Drawing.Size(rightColumnWidth, 170),
                Name = "dayDetailsGroupBox",
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            int labelX = 15;
            int controlX = 130;

            isSchoolDayCheckBox = new System.Windows.Forms.CheckBox
            {
                Text = "Is School Day (buses run)",
                Location = new System.Drawing.Point(labelX, 30),
                AutoSize = true,
                Checked = true,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };
            isSchoolDayCheckBox.CheckedChanged += IsSchoolDayCheckBox_CheckedChanged;

            dayTypeLabel = new System.Windows.Forms.Label
            {
                Text = "Day Type:",
                Location = new System.Drawing.Point(labelX, 65),
                AutoSize = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            dayTypeComboBox = new System.Windows.Forms.ComboBox
            {
                Location = new System.Drawing.Point(controlX, 62),
                Size = new System.Drawing.Size(200, 23),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };
            dayTypeComboBox.Items.AddRange(new object[] { "Regular", "Early Release", "Late Start", "Half Day" });
            dayTypeComboBox.SelectedIndex = 0;

            notesLabel = new System.Windows.Forms.Label
            {
                Text = "Notes:",
                Location = new System.Drawing.Point(labelX, 95),
                AutoSize = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            notesTextBox = new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(controlX, 92),
                Size = new System.Drawing.Size(dayDetailsGroupBox.Width - controlX - 15, 60),
                Multiline = true,
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom
            };
            notesTextBox.Height = dayDetailsGroupBox.ClientSize.Height - notesTextBox.Top - 10;

            dayDetailsGroupBox.Controls.Add(isSchoolDayCheckBox);
            dayDetailsGroupBox.Controls.Add(dayTypeLabel);
            dayDetailsGroupBox.Controls.Add(dayTypeComboBox);
            dayDetailsGroupBox.Controls.Add(notesLabel);
            dayDetailsGroupBox.Controls.Add(notesTextBox);

            routesGroupBox = new System.Windows.Forms.GroupBox
            {
                Text = "Active Routes (if School Day)",
                Size = new System.Drawing.Size(rightColumnWidth, 160),
                Name = "routesGroupBox",
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right
            };

            int checkboxX = 15;
            int checkboxYStart = 30;
            int checkboxYSpacing = 30;

            truckPlazaRouteCheckBox = new System.Windows.Forms.CheckBox
            {
                Text = "Truck Plaza Route",
                Location = new System.Drawing.Point(checkboxX, checkboxYStart),
                AutoSize = true,
                Enabled = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            eastRouteCheckBox = new System.Windows.Forms.CheckBox
            {
                Text = "East Route",
                Location = new System.Drawing.Point(checkboxX, checkboxYStart + checkboxYSpacing),
                AutoSize = true,
                Enabled = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            westRouteCheckBox = new System.Windows.Forms.CheckBox
            {
                Text = "West Route",
                Location = new System.Drawing.Point(checkboxX, checkboxYStart + 2 * checkboxYSpacing),
                AutoSize = true,
                Enabled = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            spedRouteCheckBox = new System.Windows.Forms.CheckBox
            {
                Text = "SPED Route",
                Location = new System.Drawing.Point(checkboxX, checkboxYStart + 3 * checkboxYSpacing),
                AutoSize = true,
                Enabled = true,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            routesGroupBox.Controls.Add(truckPlazaRouteCheckBox);
            routesGroupBox.Controls.Add(eastRouteCheckBox);
            routesGroupBox.Controls.Add(westRouteCheckBox);
            routesGroupBox.Controls.Add(spedRouteCheckBox);

            instructionLabel3 = new System.Windows.Forms.Label
            {
                Text = "3. Save changes for this date:",
                Location = new System.Drawing.Point(rightColumnX, routesGroupBox.Bottom + 15),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };

            saveButton = new System.Windows.Forms.Button
            {
                Text = "Save Calendar Day",
                Location = new System.Drawing.Point(rightColumnX, instructionLabel3.Bottom + 5),
                Size = new System.Drawing.Size(150, 40),
                BackColor = System.Drawing.Color.FromArgb(0, 128, 0),
                ForeColor = System.Drawing.Color.White,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };
            saveButton.Click += SaveButton_Click;

            clearButton = new System.Windows.Forms.Button
            {
                Text = "Clear",
                Location = new System.Drawing.Point(saveButton.Right + 10, saveButton.Top),
                Size = new System.Drawing.Size(100, 40),
                BackColor = System.Drawing.Color.FromArgb(0, 99, 177),
                ForeColor = System.Drawing.Color.White,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
            };
            clearButton.Click += ClearButton_Click;

            this.Controls.Add(instructionLabel1);
            this.Controls.Add(calendarPicker);
            this.Controls.Add(selectedDateLabel);
            this.Controls.Add(instructionLabel2);
            this.Controls.Add(dayDetailsGroupBox);
            this.Controls.Add(routesGroupBox);
            this.Controls.Add(instructionLabel3);
            this.Controls.Add(saveButton);
            this.Controls.Add(clearButton);
        }

        #endregion

        // Control Declarations
        private System.Windows.Forms.Label instructionLabel1;
        private System.Windows.Forms.MonthCalendar calendarPicker;
        private System.Windows.Forms.Label selectedDateLabel;
        private System.Windows.Forms.Label instructionLabel2;
        private System.Windows.Forms.GroupBox dayDetailsGroupBox;
        private System.Windows.Forms.CheckBox isSchoolDayCheckBox;
        private System.Windows.Forms.Label dayTypeLabel;
        private System.Windows.Forms.ComboBox dayTypeComboBox;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.GroupBox routesGroupBox;
        private System.Windows.Forms.CheckBox truckPlazaRouteCheckBox;
        private System.Windows.Forms.CheckBox eastRouteCheckBox;
        private System.Windows.Forms.CheckBox westRouteCheckBox;
        private System.Windows.Forms.CheckBox spedRouteCheckBox;
        private System.Windows.Forms.Label instructionLabel3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button clearButton;
    }
}