using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using BusBuddy.Forms;

namespace BusBuddy.Forms
{    
    public partial class RouteManagementForm : Form
    {
        private IDatabaseHelper dbHelper;
        private BindingSource bindingSource;
        private DataGridView dgvRoutes;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private System.Windows.Forms.Timer refreshTimer;
        private readonly ILogger<RouteManagementForm> logger;

        public RouteManagementForm(IDatabaseHelper dbHelper, ILogger<RouteManagementForm> logger)
        {
            this.dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.Text = "Manage Routes";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            bindingSource = new BindingSource();

            Label lblTitle = new Label
            {
                Text = "Routes",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(20, 10),
                Size = new Size(200, 30)
            };
            this.Controls.Add(lblTitle);

            dgvRoutes = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(640, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                DataSource = bindingSource
            };
            this.Controls.Add(dgvRoutes);

            btnAdd = new Button { Text = "Add", Location = new Point(20, 370), Size = new Size(80, 30) };
            btnEdit = new Button { Text = "Edit", Location = new Point(110, 370), Size = new Size(80, 30) };
            btnDelete = new Button { Text = "Delete", Location = new Point(200, 370), Size = new Size(80, 30) };
            btnRefresh = new Button { Text = "Refresh", Location = new Point(290, 370), Size = new Size(80, 30) };
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += async (s, e) => await LoadRoutesDataAsync();

            refreshTimer = new System.Windows.Forms.Timer
            {
                Interval = 10000 // 10 seconds
            };
            refreshTimer.Tick += async (s, e) => await LoadRoutesDataAsync();
            this.Activated += (s, e) => refreshTimer.Start();
            this.Deactivate += (s, e) => refreshTimer.Stop();
            this.FormClosing += (s, e) => refreshTimer.Stop();
            refreshTimer.Start();

            _ = LoadRoutesDataAsync(); // Fire and forget with proper async handling
        }

        private async System.Threading.Tasks.Task LoadRoutesDataAsync()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await dbHelper.GetRoutesAsync();
                bindingSource.DataSource = result ?? new List<BusBuddy.Models.Entities.Route>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error loading routes: {ex.Message}");
                MessageBox.Show($"Error loading routes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// TODO: Integrate GMap.NET when validated.
        /// </summary>
        public void InitializeMapPanel()
        {
            // Placeholder for future map panel integration
            logger.LogInformation("InitializeMapPanel placeholder called");
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            using (var dialog = new RouteEditorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var route = new BusBuddy.Models.Entities.Route
                        {
                            RouteName = dialog.RouteName,
                            StartLocation = dialog.StartLocation,
                            EndLocation = dialog.EndLocation,
                            Distance = (decimal)dialog.Distance
                        };
                        var addedRoute = await dbHelper.AddRouteAsync(route);
                        MessageBox.Show(addedRoute != null ? "Route added successfully" : $"Failed to add route.",
                            addedRoute != null ? "Success" : "Error",
                            MessageBoxButtons.OK,
                            addedRoute != null ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                        await LoadRoutesDataAsync();
                        bindingSource.ResetBindings(false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error adding route: {ex.Message}");
                        MessageBox.Show($"Error adding route: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgvRoutes.SelectedRows.Count != 1) return;

            var row = dgvRoutes.SelectedRows[0];
            if (row.Cells["Id"].Value == null)
            {
                MessageBox.Show("Invalid route selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(row.Cells["Id"].Value);
            string routeName = row.Cells["RouteName"].Value?.ToString() ?? string.Empty;
            string start = row.Cells["StartLocation"].Value?.ToString() ?? string.Empty;
            string end = row.Cells["EndLocation"].Value?.ToString() ?? string.Empty;
            double distance = double.TryParse(row.Cells["Distance"].Value?.ToString(), out var d) ? d : 0;

            using (var dialog = new RouteEditorDialog(routeName, start, end, (decimal)distance))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var route = new BusBuddy.Models.Entities.Route
                        {
                            Id = id,
                            RouteName = dialog.RouteName,
                            StartLocation = dialog.StartLocation,
                            EndLocation = dialog.EndLocation,
                            Distance = (decimal)dialog.Distance
                        };
                        var updated = await dbHelper.UpdateRouteAsync(route);
                        MessageBox.Show(updated ? "Route updated successfully" : $"Failed to update route.",
                            updated ? "Success" : "Error",
                            MessageBoxButtons.OK,
                            updated ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                        await LoadRoutesDataAsync();
                        bindingSource.ResetBindings(false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error updating route: {ex.Message}");
                        MessageBox.Show($"Error updating route: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvRoutes.SelectedRows.Count != 1) return;

            var row = dgvRoutes.SelectedRows[0];
            if (row.Cells["Id"].Value == null)
            {
                MessageBox.Show("Invalid route selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(row.Cells["Id"].Value);
            if (MessageBox.Show("Delete this route?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                var success = await dbHelper.DeleteRouteAsync(id);
                MessageBox.Show(success ? "Route deleted successfully" : $"Failed to delete route.",
                                success ? "Success" : "Error",
                                MessageBoxButtons.OK,
                                success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                await LoadRoutesDataAsync();
                bindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error deleting route: {ex.Message}");
                MessageBox.Show($"Error deleting route: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                refreshTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
