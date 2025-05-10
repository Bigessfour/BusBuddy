using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using System.Threading.Tasks;

namespace BusBuddy.Forms
{
    /// <summary>
    /// Form for viewing and generating analytics reports
    /// </summary>
    public class ReportsForm : Form
    {
        private readonly IDatabaseHelper _dbHelper;
        private readonly ILogger<ReportsForm> _logger;
        
        // UI Controls
        private Panel chartContainerPanel;
        private ComboBox reportTypeComboBox;
        private DateTimePicker startDatePicker;
        private DateTimePicker endDatePicker;
        private Button generateReportButton;
        private Button exportReportButton;
        private Label noDataLabel;
        
        public ReportsForm(IDatabaseHelper dbHelper, ILogger<ReportsForm> logger)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            InitializeComponent();
            
            // Log initialization
            _logger.LogInformation("ReportsForm initialized");
            
            // Also log to error file for tracking purposes (as required)
            LogToFile("ReportsForm initialized");
        }
        
        private void InitializeComponent()
        {
            this.Text = "BusBuddy - Analytics Reports";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Top panel for report controls
            Panel controlPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.AliceBlue
            };
            
            // Report type dropdown
            Label reportTypeLabel = new Label
            {
                Text = "Report Type:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            reportTypeComboBox = new ComboBox
            {
                Location = new Point(120, 16),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            reportTypeComboBox.Items.AddRange(new string[] 
            {
                "Fuel Consumption",
                "Maintenance Costs",
                "Vehicle Usage",
                "Route Efficiency"
            });
            reportTypeComboBox.SelectedIndex = 0;
            
            // Date range pickers
            Label dateRangeLabel = new Label
            {
                Text = "Date Range:",
                Location = new Point(20, 50),
                AutoSize = true
            };
            
            startDatePicker = new DateTimePicker
            {
                Location = new Point(120, 46),
                Width = 110,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(-1)
            };
            
            Label toLabel = new Label
            {
                Text = "to",
                Location = new Point(240, 50),
                AutoSize = true
            };
            
            endDatePicker = new DateTimePicker
            {
                Location = new Point(260, 46),
                Width = 110,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };
            
            // Buttons
            generateReportButton = new Button
            {
                Text = "Generate Report",
                Location = new Point(400, 20),
                Size = new Size(120, 30)
            };
            generateReportButton.Click += GenerateReportButton_Click;
            
            exportReportButton = new Button
            {
                Text = "Export Report",
                Location = new Point(400, 60),
                Size = new Size(120, 30),
                Enabled = false
            };
            exportReportButton.Click += ExportReportButton_Click;
            
            // Add controls to panel
            controlPanel.Controls.Add(reportTypeLabel);
            controlPanel.Controls.Add(reportTypeComboBox);
            controlPanel.Controls.Add(dateRangeLabel);
            controlPanel.Controls.Add(startDatePicker);
            controlPanel.Controls.Add(toLabel);
            controlPanel.Controls.Add(endDatePicker);
            controlPanel.Controls.Add(generateReportButton);
            controlPanel.Controls.Add(exportReportButton);
            
            // Chart container panel
            chartContainerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            
            // No data placeholder
            noDataLabel = new Label
            {
                Text = "No data available for the selected parameters.\nGenerate a report to view analytics.",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Regular)
            };
            chartContainerPanel.Controls.Add(noDataLabel);
            
            // Add panels to form
            this.Controls.Add(chartContainerPanel);
            this.Controls.Add(controlPanel);
            
            LogToFile("ReportsForm UI components initialized");
        }
        
        private void GenerateReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                string reportType = reportTypeComboBox.SelectedItem.ToString();
                DateTime startDate = startDatePicker.Value;
                DateTime endDate = endDatePicker.Value;
                
                _logger.LogInformation($"Generating {reportType} report from {startDate:d} to {endDate:d}");
                LogToFile($"User requested {reportType} report from {startDate:d} to {endDate:d}");
                
                // In a real implementation, we would fetch actual data
                // For now, we'll just update the UI with a placeholder message
                
                noDataLabel.Text = $"Chart placeholder for {reportType}\nPeriod: {startDate:d} - {endDate:d}\n\n" +
                    "This is a placeholder for future analytics functionality.\n" +
                    "Actual data visualization will be implemented in a future update.";
                
                // Enable export after generating
                exportReportButton.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report");
                LogToFile($"Error generating report: {ex.Message}");
                MessageBox.Show($"Error generating report: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ExportReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("User clicked Export Report button");
                LogToFile("User clicked Export Report button");
                
                MessageBox.Show("Export functionality will be implemented in a future update.", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting report");
                LogToFile($"Error exporting report: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Helper method to log to busbuddy_errors.log file
        /// </summary>
        private void LogToFile(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [ReportsForm] {message}";
                System.IO.File.AppendAllText("busbuddy_errors.log", logMessage + Environment.NewLine);
            }
            catch
            {
                // Silently fail if unable to write to log
            }
        }
    }
}
