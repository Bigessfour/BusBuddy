// BusBuddy/UI/Forms/RoutesForm.cs
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
    public class RoutesForm : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private readonly ILogger _logger;
        
        // Controls
        private DataGridView routesDataGridView;
        private Label routeNameLabel;
        private TextBox routeNameTextBox;
        private Label defaultBusLabel;
        private ComboBox defaultBusComboBox;
        private Label defaultDriverLabel;
        private ComboBox defaultDriverComboBox;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Button addButton;
        private Button updateButton;
        private Button clearButton;
        private GroupBox inputGroupBox;

        // Data
        private List<BusBuddy.Models.Route> _routes;
        private List<string> _drivers;
        private List<int> _busNumbers;

        public RoutesForm() : base(new MainFormNavigator())
        {
            _logger = Log.Logger;
            _dbManager = new DatabaseManager(_logger);
            
            InitializeComponent();
            LoadComboBoxData();
            LoadRoutesDataGrid();
            
            this.Load += RoutesForm_Load;
        }

        private void RoutesForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void InitializeComponent()
        {
            // Form properties
            this.Text = "Route Management - BusBuddy";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Initialize controls
            routesDataGridView = new DataGridView
            {
                Location = new Point(12, 12),
                Size = new Size(860, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            routesDataGridView.RowHeaderMouseClick += RoutesDataGridView_RowHeaderMouseClick;

            inputGroupBox = new GroupBox
            {
                Text = "Route Information",
                Location = new Point(12, 280),
                Size = new Size(860, 220)
            };

            // Route Name
            routeNameLabel = new Label
            {
                Text = "Route Name:",
                Location = new Point(20, 30),
                AutoSize = true
            };
            routeNameTextBox = new TextBox
            {
                Location = new Point(150, 27),
                Size = new Size(200, 23)
            };

            // Default Bus
            defaultBusLabel = new Label
            {
                Text = "Default Bus Number:",
                Location = new Point(20, 70),
                AutoSize = true
            };
            defaultBusComboBox = new ComboBox
            {
                Location = new Point(150, 67),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Default Driver
            defaultDriverLabel = new Label
            {
                Text = "Default Driver:",
                Location = new Point(20, 110),
                AutoSize = true
            };
            defaultDriverComboBox = new ComboBox
            {
                Location = new Point(150, 107),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Description
            descriptionLabel = new Label
            {
                Text = "Description:",
                Location = new Point(20, 150),
                AutoSize = true
            };
            descriptionTextBox = new TextBox
            {
                Location = new Point(150, 147),
                Size = new Size(400, 23),
                Multiline = true,
                Height = 40
            };

            // Buttons
            addButton = new Button
            {
                Text = "Add Route",
                Location = new Point(600, 70),
                Size = new Size(120, 30),
                BackColor = AppSettings.Theme.SuccessColor,
                ForeColor = Color.White
            };
            addButton.Click += AddButton_Click;

            updateButton = new Button
            {
                Text = "Update Route",
                Location = new Point(600, 110),
                Size = new Size(120, 30),
                BackColor = AppSettings.Theme.PrimaryColor,
                ForeColor = Color.White,
                Enabled = false
            };
            updateButton.Click += UpdateButton_Click;

            clearButton = new Button
            {
                Text = "Clear",
                Location = new Point(600, 150),
                Size = new Size(120, 30),
                BackColor = AppSettings.Theme.InfoColor,
                ForeColor = Color.White
            };
            clearButton.Click += ClearButton_Click;

            // Add controls to form
            inputGroupBox.Controls.Add(routeNameLabel);
            inputGroupBox.Controls.Add(routeNameTextBox);
            inputGroupBox.Controls.Add(defaultBusLabel);
            inputGroupBox.Controls.Add(defaultBusComboBox);
            inputGroupBox.Controls.Add(defaultDriverLabel);
            inputGroupBox.Controls.Add(defaultDriverComboBox);
            inputGroupBox.Controls.Add(descriptionLabel);
            inputGroupBox.Controls.Add(descriptionTextBox);
            inputGroupBox.Controls.Add(addButton);
            inputGroupBox.Controls.Add(updateButton);
            inputGroupBox.Controls.Add(clearButton);

            this.Controls.Add(routesDataGridView);
            this.Controls.Add(inputGroupBox);
        }

        private void LoadComboBoxData()
        {
            try
            {
                _drivers = _dbManager.GetDriverNames();
                defaultDriverComboBox.Items.Clear();
                defaultDriverComboBox.Items.Add(string.Empty); // Add empty option
                foreach (var driver in _drivers)
                {
                    defaultDriverComboBox.Items.Add(driver);
                }

                _busNumbers = _dbManager.GetBusNumbers();
                defaultBusComboBox.Items.Clear();
                foreach (var busNumber in _busNumbers)
                {
                    defaultBusComboBox.Items.Add(busNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading combo box data");
                MessageBox.Show($"Error loading drivers and buses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoutesDataGrid()
        {
            try
            {
                _routes = _dbManager.GetRoutes();
                routesDataGridView.DataSource = null;
                routesDataGridView.DataSource = _routes;

                // Format column headers
                if (routesDataGridView.Columns.Count > 0)
                {
                    routesDataGridView.Columns["RouteId"].HeaderText = "ID";
                    routesDataGridView.Columns["RouteName"].HeaderText = "Route Name";
                    routesDataGridView.Columns["DefaultBusNumber"].HeaderText = "Default Bus";
                    routesDataGridView.Columns["DefaultDriverName"].HeaderText = "Default Driver";
                    routesDataGridView.Columns["Description"].HeaderText = "Description";
                }

                StyleDataGridView(routesDataGridView);
                UpdateStatus($"Loaded {_routes.Count} routes.", AppSettings.Theme.InfoColor);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading routes");
                MessageBox.Show($"Error loading routes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error loading routes.", Color.Red);
            }
        }

        private void ClearInputs()
        {
            routeNameTextBox.Clear();
            defaultBusComboBox.SelectedIndex = -1;
            defaultDriverComboBox.SelectedIndex = -1;
            descriptionTextBox.Clear();
            updateButton.Enabled = false;
            routesDataGridView.ClearSelection();
            UpdateStatus("Inputs cleared.", AppSettings.Theme.InfoColor);
        }

        private void RoutesDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _routes.Count)
            {
                var selectedRoute = _routes[e.RowIndex];
                PopulateRouteDetails(selectedRoute);
                updateButton.Enabled = true;
                UpdateStatus($"Selected route: {selectedRoute.RouteName}", AppSettings.Theme.InfoColor);
            }
        }

        private void PopulateRouteDetails(BusBuddy.Models.Route route)
        {
            routeNameTextBox.Text = route.RouteName;
            descriptionTextBox.Text = route.Description;

            // Set the bus number in the combo box
            for (int i = 0; i < defaultBusComboBox.Items.Count; i++)
            {
                if ((int)defaultBusComboBox.Items[i] == route.DefaultBusNumber)
                {
                    defaultBusComboBox.SelectedIndex = i;
                    break;
                }
            }

            // Set the driver name in the combo box
            for (int i = 0; i < defaultDriverComboBox.Items.Count; i++)
            {
                if (defaultDriverComboBox.Items[i].ToString() == route.DefaultDriverName)
                {
                    defaultDriverComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(routeNameTextBox.Text))
                {
                    MessageBox.Show("Route name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (defaultBusComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a default bus.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var route = new BusBuddy.Models.Route
                {
                    RouteName = routeNameTextBox.Text.Trim(),
                    DefaultBusNumber = (int)defaultBusComboBox.SelectedItem,
                    DefaultDriverName = defaultDriverComboBox.SelectedIndex > 0 ? defaultDriverComboBox.SelectedItem.ToString() : string.Empty,
                    Description = descriptionTextBox.Text.Trim()
                };

                _dbManager.AddRoute(route);
                LoadRoutesDataGrid();
                ClearInputs();
                UpdateStatus($"Route '{route.RouteName}' added successfully.", Color.Green);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding route");
                MessageBox.Show($"Error adding route: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error adding route.", Color.Red);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (routesDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a route to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(routeNameTextBox.Text))
                {
                    MessageBox.Show("Route name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (defaultBusComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a default bus.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedRowIndex = routesDataGridView.SelectedRows[0].Index;
                var selectedRoute = _routes[selectedRowIndex];

                selectedRoute.RouteName = routeNameTextBox.Text.Trim();
                selectedRoute.DefaultBusNumber = (int)defaultBusComboBox.SelectedItem;
                selectedRoute.DefaultDriverName = defaultDriverComboBox.SelectedIndex > 0 ? defaultDriverComboBox.SelectedItem.ToString() : string.Empty;
                selectedRoute.Description = descriptionTextBox.Text.Trim();

                _dbManager.UpdateRoute(selectedRoute);
                LoadRoutesDataGrid();
                ClearInputs();
                UpdateStatus($"Route '{selectedRoute.RouteName}' updated successfully.", Color.Green);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating route");
                MessageBox.Show($"Error updating route: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error updating route.", Color.Red);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}