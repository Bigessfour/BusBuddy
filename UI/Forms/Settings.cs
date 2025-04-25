using System;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    public partial class Settings : BaseForm
    {
        private readonly new ILogger _logger;

        public Settings()
        {
            _logger = Log.Logger ?? new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/busbuddy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            InitializeComponent();
            
            // Apply styling to ensure consistency with other forms
            ApplyCustomStyling();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Style buttons
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    FormStyler.StyleButton(button, !button.Name.Contains("Cancel") && !button.Name.Contains("Reset"));
                }
                else if (control is Label label && label != statusLabel)
                {
                    FormStyler.StyleLabel(label, label.Name.Contains("Header") || label.Name.Contains("Title"));
                }
                else if (control is GroupBox groupBox)
                {
                    FormStyler.StyleGroupBox(groupBox);
                    
                    // Apply styling to controls within the group box
                    foreach (Control innerControl in groupBox.Controls)
                    {
                        if (innerControl is TextBox innerTextBox)
                        {
                            innerTextBox.BorderStyle = BorderStyle.FixedSingle;
                            innerTextBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                            innerTextBox.BackColor = System.Drawing.Color.White;
                        }
                        else if (innerControl is Label innerLabel)
                        {
                            FormStyler.StyleLabel(innerLabel, false);
                        }
                        else if (innerControl is CheckBox checkBox)
                        {
                            checkBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                        }
                    }
                }
                else if (control is TabControl tabControl)
                {
                    tabControl.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                    
                    // Apply styling to controls within each tab page
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        foreach (Control tabControl in tabPage.Controls)
                        {
                            if (tabControl is TextBox tabTextBox)
                            {
                                tabTextBox.BorderStyle = BorderStyle.FixedSingle;
                                tabTextBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                                tabTextBox.BackColor = System.Drawing.Color.White;
                            }
                            else if (tabControl is Label tabLabel)
                            {
                                FormStyler.StyleLabel(tabLabel, tabLabel.Name.Contains("Header") || tabLabel.Name.Contains("Title"));
                            }
                            else if (tabControl is Button tabButton)
                            {
                                FormStyler.StyleButton(tabButton, !tabButton.Name.Contains("Cancel") && !tabButton.Name.Contains("Reset"));
                            }
                        }
                    }
                }
            }
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }

        protected override void SaveRecord()
        {
            _logger.Information("Settings: SaveRecord called.");
            statusLabel.Text = "Save clicked.";
        }

        protected override void EditRecord()
        {
            _logger.Information("Settings: EditRecord called.");
            statusLabel.Text = "Edit clicked.";
        }

        protected override void RefreshData()
        {
            _logger.Information("Settings: RefreshData called.");
            statusLabel.Text = "Refresh clicked.";
        }

        protected override void DeleteRecord()
        {
            _logger.Information("Settings: DeleteRecord called.");
            statusLabel.Text = "Delete clicked.";
        }
    }
}