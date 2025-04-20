// BusBuddy/UI/Forms/Settings.cs
using System;
using System.Windows.Forms;
using Serilog;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using BusBuddy.API;
using BusBuddy.UI.Interfaces;

namespace BusBuddy.UI.Forms
{
    /// <summary>
    /// Settings form for the BusBuddy application, allowing configuration of database and API settings.
    /// </summary>
    public class Settings : BaseForm
    {
        // UI Controls - nullable to handle initialization safely
        private GroupBox? _grpDatabaseSettings;
        private GroupBox? _grpApiStatus;
        private Button? _btnManageColumns;
        private Button? _btnCheckApiStatus;
        private Label? _lblSettingsTitle;
        private Label? _lblSettingsDescription;
        private Label? _lblApiStatusResult;

        // Constants for common strings
        private const string ErrorCaption = "Error";
        private const string ApiSuccessText = "API Connection: SUCCESS\nAPI is properly configured and working.";
        private const string ApiFailureText = "API Connection: FAILED\nPlease verify your API provider and check that the API key is correctly set in your environmental variables.";
        private const string ApiErrorText = "API Connection: ERROR\nPlease verify your API provider and check that the API key is correctly set in your environmental variables.";
        private const string FormTitle = "Settings - BusBuddy";
        private const string DatabaseGroupText = "Database Management";
        private const string ApiGroupText = "API Status";
        private const string ManageColumnsButtonText = "Manage Database Columns";
        private const string CheckApiButtonText = "Check API Status";
        private const string SettingsTitleText = "BusBuddy Settings";
        private const string SettingsDescriptionText = "Configure application settings and database options";
        private const string InitialApiStatusText = "Click the button to check API connection status";
        private const string UnknownErrorMessage = "Unknown error occurred";

        public Settings() : base(new MainFormNavigator())
        {
            InitializeComponent();
            Logger.Information("Settings form initialized");
        }

        private void InitializeComponent()
        {
            // Form setup
            ClientSize = new Size(600, 450);
            Text = FormTitle;
            StartPosition = FormStartPosition.CenterScreen;

            // Settings title
            _lblSettingsTitle = new Label
            {
                Text = SettingsTitleText,
                Location = new Point(20, 20),
                Size = new Size(560, 30),
                Font = AppSettings.Theme.HeaderFont,
                ForeColor = AppSettings.Theme.AccentColor,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Settings description
            _lblSettingsDescription = new Label
            {
                Text = SettingsDescriptionText,
                Location = new Point(20, 60),
                Size = new Size(560, 20),
                Font = AppSettings.Theme.LabelFont,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Database settings group
            _grpDatabaseSettings = new GroupBox
            {
                Text = DatabaseGroupText,
                Location = new Point(20, 100),
                Size = new Size(560, 120),
                BackColor = AppSettings.Theme.GroupBoxColor,
                FlatStyle = FlatStyle.Flat,
                Font = AppSettings.Theme.LabelFont
            };

            // Manage columns button
            _btnManageColumns = new Button
            {
                Text = ManageColumnsButtonText,
                Location = new Point(30, 40),
                Size = new Size(200, 40),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = AppSettings.Theme.ButtonFont,
                UseVisualStyleBackColor = true
            };
            _btnManageColumns.Click += BtnManageColumns_Click;

            // API Status group
            _grpApiStatus = new GroupBox
            {
                Text = ApiGroupText,
                Location = new Point(20, 240),
                Size = new Size(560, 120),
                BackColor = AppSettings.Theme.GroupBoxColor,
                FlatStyle = FlatStyle.Flat,
                Font = AppSettings.Theme.LabelFont
            };

            // API Status button
            _btnCheckApiStatus = new Button
            {
                Text = CheckApiButtonText,
                Location = new Point(30, 40),
                Size = new Size(200, 40),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = AppSettings.Theme.ButtonFont,
                UseVisualStyleBackColor = true
            };
            _btnCheckApiStatus.Click += BtnCheckApiStatus_Click;

            // API Status result label
            _lblApiStatusResult = new Label
            {
                Text = InitialApiStatusText,
                Location = new Point(250, 40),
                Size = new Size(280, 60),
                Font = AppSettings.Theme.LabelFont,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Add controls to the form
            _grpDatabaseSettings.Controls.Add(_btnManageColumns);
            _grpApiStatus.Controls.Add(_btnCheckApiStatus);
            _grpApiStatus.Controls.Add(_lblApiStatusResult);
            Controls.AddRange(new Control[] { _lblSettingsTitle, _lblSettingsDescription, _grpDatabaseSettings, _grpApiStatus });
        }

        private void BtnManageColumns_Click(object? sender, EventArgs e)
        {
            if (sender == null) return;

            try
            {
                Logger.Information("Opening Table Column Manager");
                UpdateStatus("Opening Column Manager...", AppSettings.Theme.InfoColor);
                using var columnManager = new TableColumnManager();
                columnManager.ShowDialog(this);
                UpdateStatus("Column Manager closed.", AppSettings.Theme.SuccessColor);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Logger.Error(ex, "Error opening Table Column Manager: {ErrorMessage}", errorMessage);
                UpdateStatus("Error opening Column Manager.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error opening Column Manager: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnCheckApiStatus_Click(object? sender, EventArgs e)
        {
            if (sender == null || _btnCheckApiStatus == null || _lblApiStatusResult == null) return;

            try
            {
                Logger.Information("Checking API connection status");
                UpdateStatus("Checking API connection...", AppSettings.Theme.InfoColor);
                _btnCheckApiStatus.Enabled = false;
                _lblApiStatusResult.Text = "Checking API connection...";

                bool isApiConnected = await ApiClient.TestAPIConnectionAsync();

                _lblApiStatusResult.Text = isApiConnected ? ApiSuccessText : ApiFailureText;
                _lblApiStatusResult.BackColor = isApiConnected ? Color.LightGreen : Color.LightPink;
                UpdateStatus(isApiConnected ? "API connection successful." : "API connection failed.", isApiConnected ? AppSettings.Theme.SuccessColor : AppSettings.Theme.ErrorColor);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Logger.Error(ex, "Error checking API status: {ErrorMessage}", errorMessage);
                UpdateStatus("Error checking API status.", AppSettings.Theme.ErrorColor);
                MessageBox.Show($"Error checking API status: {errorMessage}", ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _lblApiStatusResult.Text = ApiErrorText;
                _lblApiStatusResult.BackColor = Color.LightPink;
            }
            finally
            {
                _btnCheckApiStatus.Enabled = true;
            }
        }

        private async Task<bool> CheckApiConnectionAsync()
        {
            try
            {
                string? apiUrl = Environment.GetEnvironmentVariable("BUSBUDDY_API_URL");
                string? apiKey = Environment.GetEnvironmentVariable("BUSBUDDY_API_KEY");

                if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(apiKey))
                {
                    Logger.Warning("API URL or API Key not found in environment variables");
                    return false;
                }

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                var response = await httpClient.GetAsync($"{apiUrl}/status");

                if (response.IsSuccessStatusCode)
                {
                    Logger.Information("API connection test successful");
                    return true;
                }

                Logger.Warning("API connection test failed with status code: {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message ?? UnknownErrorMessage;
                Logger.Error(ex, "Exception during API connection test: {ErrorMessage}", errorMessage);
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _grpDatabaseSettings?.Dispose();
                _grpApiStatus?.Dispose();
                _btnManageColumns?.Dispose();
                _btnCheckApiStatus?.Dispose();
                _lblSettingsTitle?.Dispose();
                _lblSettingsDescription?.Dispose();
                _lblApiStatusResult?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}