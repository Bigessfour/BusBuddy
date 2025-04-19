// BusBuddy/UI/Forms/Inputs.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Data;
using BusBuddy.API;

namespace BusBuddy.UI.Forms
{
    public partial class Inputs : BaseForm
    {
        private readonly DatabaseManager _dbManager;
        private Label? _lblApiStatus; // Removed readonly modifier
        private Button? _btnTestAPI; // Removed readonly modifier

        // Constants for common strings
        private const string ErrorCaption = "Error";
        private const string ApiStatusUnknown = "API Status: Unknown";
        private const string ApiStatusTesting = "API Status: Testing...";
        private const string ApiStatusConnected = "API Status: Connected";
        private const string ApiStatusFailed = "API Status: Failed";
        private const string ApiStatusError = "API Status: Error";
        private const string TestApiButtonText = "Test API Connection";
        private const string TestApiButtonTestingText = "Testing...";
        private const string UnknownErrorMessage = "Unknown error occurred";

        public Inputs() : base(new MainFormNavigator())
        {
            InitializeComponent();
            _dbManager = new DatabaseManager(Logger);
            InitializeApiControls();
        }

        private void InitializeApiControls()
        {
            try
            {
                _lblApiStatus = new Label
                {
                    Text = ApiStatusUnknown,
                    BackColor = Color.Red,
                    ForeColor = Color.White,
                    AutoSize = true,
                    Padding = new Padding(5),
                    Font = new Font(AppSettings.Theme.LabelFont.FontFamily, AppSettings.Theme.LabelFont.Size, FontStyle.Bold),
                    Location = new Point(10, 10)
                };

                _btnTestAPI = new Button
                {
                    Text = TestApiButtonText,
                    Location = new Point(_lblApiStatus.Right + 10, _lblApiStatus.Top - 2),
                    Size = new Size(150, 30),
                    FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                    Font = AppSettings.Theme.ButtonFont,
                    UseVisualStyleBackColor = true
                };
                _btnTestAPI.Click += BtnTestAPI_Click;

                Controls.Add(_lblApiStatus);
                Controls.Add(_btnTestAPI);

                Logger.Information("API controls initialized successfully");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Logger.Error(ex, "Error initializing API controls: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error initializing API controls: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnTestAPI_Click(object? sender, EventArgs e)
        {
            if (sender == null || _lblApiStatus == null || _btnTestAPI == null) return;

            try
            {
                Logger.Information("API test button clicked");
                _btnTestAPI.Enabled = false;
                _btnTestAPI.Text = TestApiButtonTestingText;
                _lblApiStatus.Text = ApiStatusTesting;
                _lblApiStatus.BackColor = Color.Orange;

                bool isConnected = await ApiClient.TestAPIConnectionAsync();

                _lblApiStatus.Text = isConnected ? ApiStatusConnected : ApiStatusFailed;
                _lblApiStatus.BackColor = isConnected ? Color.Green : Color.Red;
                Logger.Information(isConnected ? "API test successful" : "API test failed");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                _lblApiStatus.Text = ApiStatusError;
                _lblApiStatus.BackColor = Color.Red;
                Logger.Error(ex, "Error testing API connection: {ErrorMessage}", errorMessage);
                MessageBox.Show($"Error testing API connection: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnTestAPI.Enabled = true;
                _btnTestAPI.Text = TestApiButtonText;
            }
        }

        private void Inputs_Load(object sender, EventArgs e)
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            try
            {
                _dbManager.LoadVehicleNumbers();
                _dbManager.LoadDriverNames();
                Logger.Information("Database loaded successfully");
                UpdateStatus("Database loaded.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Logger.Error(ex, "Error loading database: {ErrorMessage}", errorMessage);
                UpdateStatus("Error loading database.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error loading database: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Logger.Information("Save button clicked");
            UpdateStatus("Saving data...", AppSettings.Theme.InfoColor);
            // Implement save logic here
            UpdateStatus("Data saved.", AppSettings.Theme.SuccessColor);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Logger.Information("Edit button clicked");
            UpdateStatus("Editing data...", AppSettings.Theme.InfoColor);
            // Implement edit logic here
            UpdateStatus("Data edited.", AppSettings.Theme.SuccessColor);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Logger.Information("Delete button clicked");
            UpdateStatus("Deleting data...", AppSettings.Theme.InfoColor);
            // Implement delete logic here
            UpdateStatus("Data deleted.", AppSettings.Theme.SuccessColor);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Logger.Information("Refresh button clicked");
            LoadDatabase();
        }

        private void btnMaintenanceScannedInvoice_Click(object sender, EventArgs e)
        {
            Logger.Information("Maintenance scanned invoice button clicked");
            UpdateStatus("Opening scanned invoice...", AppSettings.Theme.InfoColor);
            // Implement invoice logic here
            UpdateStatus("Scanned invoice opened.", AppSettings.Theme.SuccessColor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbManager?.Dispose();
                _lblApiStatus?.Dispose();
                _btnTestAPI?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}