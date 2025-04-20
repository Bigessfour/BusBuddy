// BusBuddy/UI/Forms/ScheduledRoutesForm.cs
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
    public class ScheduledRoutesForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;

        // Controls
        private DateTimePicker datePicker;
        private Label dateLabel;
        private DataGridView scheduledRoutesGridView;
        private GroupBox editGroupBox;
        private Label routeNameLabel;
        private Label routeLabel;
        private Label busLabel;
        private ComboBox busComboBox;
        private Label driverLabel;
        private ComboBox driverComboBox;
        private Button updateButton;
        private Label statusHeaderLabel;
        private Label statusLabel;
        private Button calendarButton;
        private Button routesButton;

        // Data
        private List<ScheduledRoute> _scheduledRoutes;
        private List<Route> _routes;
        private List<string> _drivers;
        private List<int> _busNumbers;
        private SchoolCalendarDay _currentDay;
        private DateTime _selectedDate;

        public ScheduledRoutesForm() : base(new MainFormNavigator())
        {
            _dbManager = new DatabaseManager();
            _logger = Log.Logger;
            _selectedDate = DateTime.Today;

            InitializeComponent();
            LoadComboBoxData();
            LoadData(_selectedDate);

            this.Load += ScheduledRoutesForm_Load;
        }

        private void ScheduledRoutesForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "Scheduled Routes - BusBuddy";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Date picker
            dateLabel = new Label
            {
                Text = "Select Date:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            datePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(120, 20),
                Size = new Size(150, 26),
                Value = DateTime.Today
            };
            datePicker.ValueChanged += DatePicker_ValueChanged;

            // Status header
            statusHeaderLabel = new Label
            {
                Text = "Day Status:",
                Location = new Point(300, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            statusLabel = new Label
            {
                Location = new Point(390, 20),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Calendar button
            calendarButton = new Button
            {
                Text = "Open Calendar",
                Location = new Point(600, 18),
                Size = new Size(120, 30),
                BackColor = AppSettings.Theme.PrimaryColor,
                ForeColor = Color.White
            };
            calendarButton.Click += CalendarButton_Click;

            // Routes button
            routesButton = new Button
            {
                Text = "Manage Routes",
                Location = new Point(740, 18),
                Size = new Size(120, 30),
                BackColor = AppSettings.Theme.PrimaryColor,
                ForeColor = Color.White
            };
            routesButton.Click += RoutesButton_Click;

            // Scheduled routes grid
            scheduledRoutesGridView = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(840, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            scheduledRoutesGridView.RowHeaderMouseClick += ScheduledRoutesGridView_RowHeaderMouseClick;

            // Edit group box
            editGroupBox = new GroupBox
            {
                Text = "Edit Scheduled Route",
                Location = new Point(20, 330),
                Size = new Size(840, 170)
            };

            // Route name (non-editable)
            routeNameLabel = new Label
            {
                Text = "Route:",
                Location = new Point(20, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            routeLabel = new Label
            {
                Location = new Point(150, 40),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Bus selector
            busLabel = new Label
            {
                Text = "Assigned Bus:",
                Location = new Point(20, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            busComboBox = new ComboBox
            {
                Location = new Point(150, 80),
                Size = new Size(150, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Driver selector
            driverLabel = new Label
            {
                Text = "Assigned Driver:",
                Location = new Point(20, 120),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            driverComboBox = new ComboBox
            {
                Location = new Point(150, 120),
                Size = new Size(300, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Update button
            updateButton = new Button
            {
                Text = "Update Assignment",
                Location = new Point(600, 90),
                Size = new Size(170, 40),
                BackColor = AppSettings.Theme.SuccessColor,
                ForeColor = Color.White,
                Enabled = false
            };
            updateButton.Click += UpdateButton_Click;

            // Add controls to the form
            editGroupBox.Controls.Add(routeNameLabel);
            editGroupBox.Controls.Add(routeLabel);
            editGroupBox.Controls.Add(busLabel);
            editGroupBox.Controls.Add(busComboBox);
            editGroupBox.Controls.Add(driverLabel);
            editGroupBox.Controls.Add(driverComboBox);
            editGroupBox.Controls.Add(updateButton);

            this.Controls.Add(dateLabel);
            this.Controls.Add(datePicker);
            this.Controls.Add(statusHeaderLabel);
            this.Controls.Add(statusLabel);
            this.Controls.Add(calendarButton);
            this.Controls.Add(routesButton);
            this.Controls.Add(scheduledRoutesGridView);
            this.Controls.Add(editGroupBox);
        }

        private void LoadComboBoxData()
        {
            try
            {
                _drivers = _dbManager.GetDriverNames();
                driverComboBox.Items.Clear();
                driverComboBox.Items.Add(string.Empty); // Add empty option
                foreach (var driver in _drivers)
                {
                    driverComboBox.Items.Add(driver);
                }

                _busNumbers = _dbManager.GetBusNumbers();
                busComboBox.Items.Clear();
                foreach (var busNumber in _busNumbers)
                {
                    busComboBox.Items.Add(busNumber);
                }

                _routes = _dbManager.GetRoutes();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading combo box data");
                MessageBox.Show($"Error loading drivers and buses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetRouteName(int routeId)
        {
            foreach (var route in _routes)
            {
                if (route.RouteId == routeId)
                {
                    return route.RouteName;
                }
            }
            return $"Route #{routeId}";
        }

        private void LoadData(DateTime date)
        {
            try
            {
                _selectedDate = date;
                _currentDay = _dbManager.GetCalendarDay(date);
                
                if (_currentDay != null && _currentDay.IsSchoolDay)
                {
                    statusLabel.Text = $"{_currentDay.DayType} School Day";
                    statusLabel.ForeColor = Color.Green;
                    
                    // Load scheduled routes
                    _scheduledRoutes = _dbManager.GetScheduledRoutes(date);
                    
                    // Add route name property for display
                    var displayData = new List<ScheduledRouteDisplay>();
                    foreach (var route in _scheduledRoutes)
                    {
                        displayData.Add(new ScheduledRouteDisplay
                        {
                            ScheduledRouteId = route.ScheduledRouteId,
                            RouteName = GetRouteName(route.RouteId),
                            RouteId = route.RouteId,
                            AssignedBusNumber = route.AssignedBusNumber,
                            AssignedDriverName = route.AssignedDriverName
                        });
                    }
                    
                    scheduledRoutesGridView.DataSource = null;
                    scheduledRoutesGridView.DataSource = displayData;
                    
                    // Format columns
                    if (scheduledRoutesGridView.Columns.Count > 0)
                    {
                        scheduledRoutesGridView.Columns["ScheduledRouteId"].Visible = false;
                        scheduledRoutesGridView.Columns["RouteId"].Visible = false;
                        scheduledRoutesGridView.Columns["RouteName"].HeaderText = "Route";
                        scheduledRoutesGridView.Columns["AssignedBusNumber"].HeaderText = "Bus Number";
                        scheduledRoutesGridView.Columns["AssignedDriverName"].HeaderText = "Driver";
                    }
                    
                    editGroupBox.Enabled = true;
                }
                else
                {
                    statusLabel.Text = "No School Day";
                    statusLabel.ForeColor = Color.Red;
                    
                    scheduledRoutesGridView.DataSource = null;
                    editGroupBox.Enabled = false;
                }
                
                // Disable the update button until a route is selected
                updateButton.Enabled = false;
                
                StyleDataGridView(scheduledRoutesGridView);
                UpdateStatus($"Loaded scheduled routes for {date:d}", AppSettings.Theme.InfoColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading scheduled routes for {Date}", date);
                MessageBox.Show($"Error loading scheduled routes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error loading scheduled routes.", Color.Red);
            }
        }

        private void ScheduledRoutesGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && scheduledRoutesGridView.DataSource != null)
            {
                var selectedRoute = ((List<ScheduledRouteDisplay>)scheduledRoutesGridView.DataSource)[e.RowIndex];
                
                routeLabel.Text = selectedRoute.RouteName;
                
                // Set the bus number in the combo box
                for (int i = 0; i < busComboBox.Items.Count; i++)
                {
                    if ((int)busComboBox.Items[i] == selectedRoute.AssignedBusNumber)
                    {
                        busComboBox.SelectedIndex = i;
                        break;
                    }
                }
                
                // Set the driver name in the combo box
                for (int i = 0; i < driverComboBox.Items.Count; i++)
                {
                    if (driverComboBox.Items[i].ToString() == selectedRoute.AssignedDriverName)
                    {
                        driverComboBox.SelectedIndex = i;
                        break;
                    }
                }
                
                updateButton.Enabled = true;
                UpdateStatus($"Selected route: {selectedRoute.RouteName}", AppSettings.Theme.InfoColor);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (scheduledRoutesGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a route to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (busComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a bus.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedRowIndex = scheduledRoutesGridView.SelectedRows[0].Index;
                var selectedRoute = ((List<ScheduledRouteDisplay>)scheduledRoutesGridView.DataSource)[selectedRowIndex];
                
                // Find the actual ScheduledRoute object
                ScheduledRoute routeToUpdate = null;
                foreach (var route in _scheduledRoutes)
                {
                    if (route.ScheduledRouteId == selectedRoute.ScheduledRouteId)
                    {
                        routeToUpdate = route;
                        break;
                    }
                }
                
                if (routeToUpdate == null)
                {
                    MessageBox.Show("Could not find the selected route.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                routeToUpdate.AssignedBusNumber = (int)busComboBox.SelectedItem;
                routeToUpdate.AssignedDriverName = driverComboBox.SelectedIndex > 0 ? driverComboBox.SelectedItem.ToString() : string.Empty;

                _dbManager.UpdateScheduledRoute(routeToUpdate);
                LoadData(_selectedDate);
                updateButton.Enabled = false;
                UpdateStatus($"Route assignment updated successfully.", Color.Green);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating scheduled route");
                MessageBox.Show($"Error updating route assignment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error updating route assignment.", Color.Red);
            }
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadData(datePicker.Value);
        }

        private void CalendarButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var calendarForm = new SchoolCalendarForm())
                {
                    calendarForm.ShowDialog();
                }
                
                // Refresh the current view after returning from calendar
                LoadData(_selectedDate);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error opening school calendar");
                MessageBox.Show($"Error opening school calendar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoutesButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var routesForm = new RoutesForm())
                {
                    routesForm.ShowDialog();
                }
                
                // Refresh routes data and reload view
                LoadComboBoxData();
                LoadData(_selectedDate);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error opening routes management");
                MessageBox.Show($"Error opening routes management: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Helper class for display in the DataGridView
    public class ScheduledRouteDisplay
    {
        public int ScheduledRouteId { get; set; }
        public string RouteName { get; set; }
        public int RouteId { get; set; }
        public int AssignedBusNumber { get; set; }
        public string AssignedDriverName { get; set; }
    }
}