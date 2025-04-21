// BusBuddy/UI/Forms/SchoolCalendarForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.UI.Interfaces;
using BusBuddy.Utilities;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public class SchoolCalendarForm : BaseForm // Inherits from BaseForm which now handles layout/dragging
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;

        // Controls
        private MonthCalendar calendarPicker;
        private GroupBox dayDetailsGroupBox;
        private Label dayTypeLabel;
        private ComboBox dayTypeComboBox;
        private Label notesLabel;
        private TextBox notesTextBox;
        private CheckBox isSchoolDayCheckBox;
        private GroupBox routesGroupBox;
        private CheckBox truckPlazaRouteCheckBox;
        private CheckBox eastRouteCheckBox;
        private CheckBox westRouteCheckBox;
        private CheckBox spedRouteCheckBox;
        private Button saveButton;
        private Button clearButton;
        private Label selectedDateLabel;
        private Label instructionLabel1;
        private Label instructionLabel2;
        private Label instructionLabel3;

        // Data
        private List<BusBuddy.Models.Route> _routes;
        private SchoolCalendarDay _currentDay;

        private const int GroupBoxSpacing = 20; // Define spacing constant

        public SchoolCalendarForm() : base(new MainFormNavigator())
        {
            _logger = Log.Logger;
            _dbManager = new DatabaseManager(_logger);

            InitializeComponent();
            LoadRoutes();
        }

        private void SchoolCalendarForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready. Select a date to view or edit.", AppSettings.Theme.InfoColor);
            LoadCalendarDay(calendarPicker.SelectionStart);
        }

        private void InitializeComponent()
        {
            this.Text = "School Calendar - BusBuddy";
            this.Name = "SchoolCalendarForm"; // IMPORTANT: Set Form Name for layout saving
            this.Size = new Size(950, 600);
            this.MinimumSize = new Size(700, 500); // Add a minimum size
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Resize += SchoolCalendarForm_Resize; // Add resize event handler

            instructionLabel1 = new Label
            {
                Text = "1. Select a date below:",
                Location = new Point(12, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left
            };

            calendarPicker = new MonthCalendar
            {
                Location = new Point(12, 35),
                MaxSelectionCount = 1,
                Size = new Size(460, 320), // Keep initial size
                CalendarDimensions = new Size(2, 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom // Anchor to allow vertical resize
            };
            calendarPicker.DateChanged += CalendarPicker_DateChanged;

            int rightColumnX = calendarPicker.Right + 30;
            int rightColumnWidth = this.ClientSize.Width - rightColumnX - 20;

            selectedDateLabel = new Label
            {
                Location = new Point(rightColumnX, 20),
                Size = new Size(rightColumnWidth, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Anchor top, left, right
            };
            UpdateSelectedDateLabel(DateTime.Now);

            instructionLabel2 = new Label
            {
                Text = "2. Set details for the selected date:",
                Location = new Point(rightColumnX, 60),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left (relative to right column start)
            };

            dayDetailsGroupBox = new GroupBox
            {
                Text = "Day Details",
                Location = new Point(rightColumnX, 85),
                Size = new Size(rightColumnWidth, 170),
                Name = "dayDetailsGroupBox", // IMPORTANT: Name needed for saving position
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Anchor top, left, right
            };

            int labelX = 15;
            int controlX = 130;

            isSchoolDayCheckBox = new CheckBox
            {
                Text = "Is School Day (buses run)",
                Location = new Point(labelX, 30),
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };
            isSchoolDayCheckBox.CheckedChanged += IsSchoolDayCheckBox_CheckedChanged;

            dayTypeLabel = new Label
            {
                Text = "Day Type:",
                Location = new Point(labelX, 65),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };

            dayTypeComboBox = new ComboBox
            {
                Location = new Point(controlX, 62),
                Size = new Size(200, 23), // Fixed width might be okay here, or anchor right
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };
            dayTypeComboBox.Items.AddRange(new object[] { "Regular", "Early Release", "Late Start", "Half Day" });
            dayTypeComboBox.SelectedIndex = 0;

            notesLabel = new Label
            {
                Text = "Notes:",
                Location = new Point(labelX, 95),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };

            notesTextBox = new TextBox
            {
                Location = new Point(controlX, 92),
                Size = new Size(dayDetailsGroupBox.Width - controlX - 15, 60), // Initial size calculation
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom // Anchor all sides within groupbox
            };
            notesTextBox.Height = dayDetailsGroupBox.ClientSize.Height - notesTextBox.Top - 10;

            dayDetailsGroupBox.Controls.Add(isSchoolDayCheckBox);
            dayDetailsGroupBox.Controls.Add(dayTypeLabel);
            dayDetailsGroupBox.Controls.Add(dayTypeComboBox);
            dayDetailsGroupBox.Controls.Add(notesLabel);
            dayDetailsGroupBox.Controls.Add(notesTextBox);

            routesGroupBox = new GroupBox
            {
                Text = "Active Routes (if School Day)",
                Size = new Size(rightColumnWidth, 160),
                Name = "routesGroupBox", // IMPORTANT: Name needed for saving position
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Anchor top, left, right
            };

            int checkboxX = 15;
            int checkboxYStart = 30;
            int checkboxYSpacing = 30;

            truckPlazaRouteCheckBox = new CheckBox
            {
                Text = "Truck Plaza Route",
                Location = new Point(checkboxX, checkboxYStart),
                AutoSize = true,
                Enabled = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };

            eastRouteCheckBox = new CheckBox
            {
                Text = "East Route",
                Location = new Point(checkboxX, checkboxYStart + checkboxYSpacing),
                AutoSize = true,
                Enabled = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };

            westRouteCheckBox = new CheckBox
            {
                Text = "West Route",
                Location = new Point(checkboxX, checkboxYStart + 2 * checkboxYSpacing),
                AutoSize = true,
                Enabled = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };

            spedRouteCheckBox = new CheckBox
            {
                Text = "SPED Route",
                Location = new Point(checkboxX, checkboxYStart + 3 * checkboxYSpacing),
                AutoSize = true,
                Enabled = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left within groupbox
            };

            routesGroupBox.Controls.Add(truckPlazaRouteCheckBox);
            routesGroupBox.Controls.Add(eastRouteCheckBox);
            routesGroupBox.Controls.Add(westRouteCheckBox);
            routesGroupBox.Controls.Add(spedRouteCheckBox);

            instructionLabel3 = new Label
            {
                Text = "3. Save changes for this date:",
                Location = new Point(rightColumnX, routesGroupBox.Bottom + 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left (relative to right column start)
            };

            saveButton = new Button
            {
                Text = "Save Calendar Day",
                Location = new Point(rightColumnX, instructionLabel3.Bottom + 5),
                Size = new Size(150, 40),
                BackColor = AppSettings.Theme.SuccessColor,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left (relative to right column start)
            };
            saveButton.Click += SaveButton_Click;

            clearButton = new Button
            {
                Text = "Clear",
                Location = new Point(saveButton.Right + 10, saveButton.Top), // Position relative to save button
                Size = new Size(100, 40),
                BackColor = AppSettings.Theme.InfoColor,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left // Anchor top-left (relative to save button's anchored position)
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

            RecalculateLayout();
        }

        private void SchoolCalendarForm_Resize(object sender, EventArgs e)
        {
            RecalculateLayout();
        }

        private void RecalculateLayout()
        {
            // Prevent calculations if form is minimized
            if (this.WindowState == FormWindowState.Minimized) return;

            // Adjust calendar height first as other positions depend on its width
            // Ensure minimum height for calendar
            int minCalendarHeight = 200; // Example minimum height
            calendarPicker.Height = Math.Max(minCalendarHeight, this.ClientSize.Height - calendarPicker.Top - 20); // Adjust bottom margin as needed

            // Recalculate right column position/width
            int rightColumnX = calendarPicker.Right + 30;
            int rightColumnWidth = this.ClientSize.Width - rightColumnX - 20;

            // Ensure minimum width for the right column
            int minRightColumnWidth = 250; // Example minimum width
            if (rightColumnWidth < minRightColumnWidth)
            {
                rightColumnWidth = minRightColumnWidth;
                // Optional: Adjust form width if needed
                // this.Width = calendarPicker.Right + 30 + rightColumnWidth + 20;
            }

            // Use SuspendLayout/ResumeLayout for smoother resizing
            this.SuspendLayout();

            // Reposition/Resize controls in the right column
            selectedDateLabel.Left = rightColumnX;
            selectedDateLabel.Width = rightColumnWidth;

            instructionLabel2.Left = rightColumnX;
            // instructionLabel2.Top is fixed relative to selectedDateLabel

            dayDetailsGroupBox.Left = rightColumnX;
            dayDetailsGroupBox.Width = rightColumnWidth;
            dayDetailsGroupBox.Top = instructionLabel2.Bottom + 5; // Position below label

            // Adjust notesTextBox height relative to its container
            if (dayDetailsGroupBox.ClientSize.Height > notesTextBox.Top + 10)
            {
                notesTextBox.Height = dayDetailsGroupBox.ClientSize.Height - notesTextBox.Top - 10;
            }
            // Anchoring handles notesTextBox width

            routesGroupBox.Left = rightColumnX;
            routesGroupBox.Width = rightColumnWidth;
            routesGroupBox.Top = dayDetailsGroupBox.Bottom + GroupBoxSpacing; // Position below previous groupbox using the constant

            instructionLabel3.Left = rightColumnX;
            instructionLabel3.Top = routesGroupBox.Bottom + 15; // Position below groupbox

            saveButton.Left = rightColumnX;
            saveButton.Top = instructionLabel3.Bottom + 5; // Position below label

            clearButton.Left = saveButton.Right + 10; // Ensure clear button stays next to save button
            clearButton.Top = saveButton.Top; // Align top with save button

            this.ResumeLayout(true);
        }

        private void LoadRoutes()
        {
            try
            {
                _routes = _dbManager.GetRoutes();
                _logger.Information("Loaded {Count} routes", _routes.Count);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading routes");
                MessageBox.Show($"Error loading routes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSelectedDateLabel(DateTime date)
        {
            selectedDateLabel.Text = date.ToString("dddd, MMMM d, yyyy");
        }

        private void LoadCalendarDay(DateTime date)
        {
            try
            {
                _currentDay = _dbManager.GetCalendarDay(date);

                if (_currentDay != null)
                {
                    isSchoolDayCheckBox.Checked = _currentDay.IsSchoolDay;
                    notesTextBox.Text = _currentDay.Notes;

                    for (int i = 0; i < dayTypeComboBox.Items.Count; i++)
                    {
                        if (dayTypeComboBox.Items[i].ToString() == _currentDay.DayType)
                        {
                            dayTypeComboBox.SelectedIndex = i;
                            break;
                        }
                    }

                    truckPlazaRouteCheckBox.Checked = _currentDay.IsRunningTruckPlazaRoute;
                    eastRouteCheckBox.Checked = _currentDay.IsRunningEastRoute;
                    westRouteCheckBox.Checked = _currentDay.IsRunningWestRoute;
                    spedRouteCheckBox.Checked = _currentDay.IsRunningSPEDRoute;
                }
                else
                {
                    _currentDay = new SchoolCalendarDay
                    {
                        Date = date,
                        IsSchoolDay = true,
                        DayType = "Regular",
                        Notes = string.Empty,
                        ActiveRouteIds = new List<int>()
                    };

                    ClearFormInputs();
                }

                UpdateRouteCheckBoxes();
                UpdateStatus($"Loaded calendar data for {date:d}", AppSettings.Theme.InfoColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading calendar day for {Date}", date);
                MessageBox.Show($"Error loading calendar data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error loading calendar day.", Color.Red);
            }
        }

        private void ClearFormInputs()
        {
            isSchoolDayCheckBox.Checked = true;
            dayTypeComboBox.SelectedIndex = 0;
            notesTextBox.Text = string.Empty;
            truckPlazaRouteCheckBox.Checked = false;
            eastRouteCheckBox.Checked = false;
            westRouteCheckBox.Checked = false;
            spedRouteCheckBox.Checked = false;
        }

        private void UpdateRouteCheckBoxes()
        {
            bool enableRoutes = isSchoolDayCheckBox.Checked;

            truckPlazaRouteCheckBox.Enabled = enableRoutes;
            eastRouteCheckBox.Enabled = enableRoutes;
            westRouteCheckBox.Enabled = enableRoutes;
            spedRouteCheckBox.Enabled = enableRoutes;

            if (!enableRoutes)
            {
                truckPlazaRouteCheckBox.Checked = false;
                eastRouteCheckBox.Checked = false;
                westRouteCheckBox.Checked = false;
                spedRouteCheckBox.Checked = false;
            }
        }

        private void CalendarPicker_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (e.Start == null) return;

            UpdateSelectedDateLabel(e.Start);
            LoadCalendarDay(e.Start);
        }

        private void IsSchoolDayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRouteCheckBoxes();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentDay == null)
                {
                    _currentDay = new SchoolCalendarDay();
                }

                _currentDay.Date = calendarPicker.SelectionStart;
                _currentDay.IsSchoolDay = isSchoolDayCheckBox.Checked;
                _currentDay.DayType = dayTypeComboBox.SelectedItem.ToString();
                _currentDay.Notes = notesTextBox.Text.Trim();

                if (_currentDay.ActiveRouteIds == null)
                {
                    _currentDay.ActiveRouteIds = new List<int>();
                }
                else
                {
                    _currentDay.ActiveRouteIds.Clear();
                }

                if (isSchoolDayCheckBox.Checked)
                {
                    if (truckPlazaRouteCheckBox.Checked) _currentDay.ActiveRouteIds.Add(1);
                    if (eastRouteCheckBox.Checked) _currentDay.ActiveRouteIds.Add(2);
                    if (westRouteCheckBox.Checked) _currentDay.ActiveRouteIds.Add(3);
                    if (spedRouteCheckBox.Checked) _currentDay.ActiveRouteIds.Add(4);
                }

                _dbManager.AddOrUpdateCalendarDay(_currentDay);
                UpdateStatus($"Calendar day saved for {_currentDay.Date:d}", Color.Green);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving calendar day");
                MessageBox.Show($"Error saving calendar day: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error saving calendar day.", Color.Red);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearFormInputs();
            UpdateStatus("Form inputs cleared.", AppSettings.Theme.InfoColor);
        }
    }
}