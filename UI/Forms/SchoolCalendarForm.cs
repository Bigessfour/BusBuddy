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
    public class SchoolCalendarForm : BaseForm
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
        
        // Data
        private List<Route> _routes;
        private SchoolCalendarDay _currentDay;

        public SchoolCalendarForm() : base(new MainFormNavigator())
        {
            _dbManager = new DatabaseManager();
            _logger = Log.Logger;
            
            InitializeComponent();
            LoadRoutes();
            
            this.Load += SchoolCalendarForm_Load;
        }

        private void SchoolCalendarForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready. Select a date to view or edit.", AppSettings.Theme.InfoColor);
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "School Calendar - BusBuddy";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Calendar picker
            calendarPicker = new MonthCalendar
            {
                Location = new Point(12, 12),
                MaxSelectionCount = 1,
                CalendarDimensions = new Size(2, 2)
            };
            calendarPicker.DateChanged += CalendarPicker_DateChanged;

            // Selected date label
            selectedDateLabel = new Label
            {
                Location = new Point(400, 20),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            UpdateSelectedDateLabel(DateTime.Now);

            // Day details group
            dayDetailsGroupBox = new GroupBox
            {
                Text = "Day Details",
                Location = new Point(400, 60),
                Size = new Size(360, 170)
            };

            isSchoolDayCheckBox = new CheckBox
            {
                Text = "Is School Day (buses run)",
                Location = new Point(20, 30),
                AutoSize = true,
                Checked = true
            };
            isSchoolDayCheckBox.CheckedChanged += IsSchoolDayCheckBox_CheckedChanged;

            dayTypeLabel = new Label
            {
                Text = "Day Type:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            
            dayTypeComboBox = new ComboBox
            {
                Location = new Point(120, 57),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            dayTypeComboBox.Items.AddRange(new object[] { "Regular", "Early Release", "Late Start", "Half Day" });
            dayTypeComboBox.SelectedIndex = 0;

            notesLabel = new Label
            {
                Text = "Notes:",
                Location = new Point(20, 90),
                AutoSize = true
            };
            
            notesTextBox = new TextBox
            {
                Location = new Point(120, 87),
                Size = new Size(220, 23),
                Multiline = true,
                Height = 60
            };

            dayDetailsGroupBox.Controls.Add(isSchoolDayCheckBox);
            dayDetailsGroupBox.Controls.Add(dayTypeLabel);
            dayDetailsGroupBox.Controls.Add(dayTypeComboBox);
            dayDetailsGroupBox.Controls.Add(notesLabel);
            dayDetailsGroupBox.Controls.Add(notesTextBox);

            // Routes group
            routesGroupBox = new GroupBox
            {
                Text = "Active Routes",
                Location = new Point(400, 240),
                Size = new Size(360, 160)
            };

            truckPlazaRouteCheckBox = new CheckBox
            {
                Text = "Truck Plaza Route",
                Location = new Point(20, 30),
                AutoSize = true,
                Enabled = true
            };

            eastRouteCheckBox = new CheckBox
            {
                Text = "East Route",
                Location = new Point(20, 60),
                AutoSize = true,
                Enabled = true
            };

            westRouteCheckBox = new CheckBox
            {
                Text = "West Route",
                Location = new Point(20, 90),
                AutoSize = true,
                Enabled = true
            };

            spedRouteCheckBox = new CheckBox
            {
                Text = "SPED Route",
                Location = new Point(20, 120),
                AutoSize = true,
                Enabled = true
            };

            routesGroupBox.Controls.Add(truckPlazaRouteCheckBox);
            routesGroupBox.Controls.Add(eastRouteCheckBox);
            routesGroupBox.Controls.Add(westRouteCheckBox);
            routesGroupBox.Controls.Add(spedRouteCheckBox);

            // Buttons
            saveButton = new Button
            {
                Text = "Save Calendar Day",
                Location = new Point(460, 420),
                Size = new Size(150, 40),
                BackColor = AppSettings.Theme.SuccessColor,
                ForeColor = Color.White
            };
            saveButton.Click += SaveButton_Click;

            clearButton = new Button
            {
                Text = "Clear",
                Location = new Point(620, 420),
                Size = new Size(100, 40),
                BackColor = AppSettings.Theme.InfoColor,
                ForeColor = Color.White
            };
            clearButton.Click += ClearButton_Click;

            // Add controls to form
            this.Controls.Add(calendarPicker);
            this.Controls.Add(selectedDateLabel);
            this.Controls.Add(dayDetailsGroupBox);
            this.Controls.Add(routesGroupBox);
            this.Controls.Add(saveButton);
            this.Controls.Add(clearButton);
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
                    // Populate form with calendar day data
                    isSchoolDayCheckBox.Checked = _currentDay.IsSchoolDay;
                    notesTextBox.Text = _currentDay.Notes;
                    
                    // Set day type
                    for (int i = 0; i < dayTypeComboBox.Items.Count; i++)
                    {
                        if (dayTypeComboBox.Items[i].ToString() == _currentDay.DayType)
                        {
                            dayTypeComboBox.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    // Set route checkboxes
                    truckPlazaRouteCheckBox.Checked = _currentDay.IsRunningTruckPlazaRoute;
                    eastRouteCheckBox.Checked = _currentDay.IsRunningEastRoute;
                    westRouteCheckBox.Checked = _currentDay.IsRunningWestRoute;
                    spedRouteCheckBox.Checked = _currentDay.IsRunningSPEDRoute;
                }
                else
                {
                    // Create new calendar day with defaults
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
                
                // Set active routes
                _currentDay.ActiveRouteIds.Clear();
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