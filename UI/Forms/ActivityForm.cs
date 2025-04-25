using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BusBuddy.Utilities;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public partial class ActivityForm : BaseForm
    {
        private new readonly ILogger _logger;
        private readonly List<string> _busNumbers;
        private readonly List<string> _drivers;

        public ActivityForm(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _busNumbers = new List<string>();
            _drivers = new List<string>();
            InitializeComponent();
            
            // Apply styling to ensure consistency with other forms
            ApplyCustomStyling();
        }
        
        /// <summary>
        /// Apply custom styling to all controls in this form to ensure consistency
        /// </summary>
        private void ApplyCustomStyling()
        {
            // Style all buttons
            foreach (Control control in this.Controls)
            {
                if (control is Button button)
                {
                    FormStyler.StyleButton(button, !button.Name.Contains("Clear") && !button.Name.Contains("Exit"));
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
                        else if (innerControl is ComboBox comboBox)
                        {
                            comboBox.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                            comboBox.BackColor = System.Drawing.Color.White;
                        }
                        else if (innerControl is DateTimePicker dateTimePicker)
                        {
                            dateTimePicker.Font = new System.Drawing.Font("Segoe UI", 9.5f);
                        }
                    }
                }
            }
            
            // Force a refresh to ensure all styles are applied
            this.Refresh();
        }

        private void AddActivityButton_Click(object? sender, EventArgs e)
        {
            // Placeholder for add activity logic
            statusLabel.Text = "Add activity clicked.";
            SaveRecord();
        }

        private void ClearActivityButton_Click(object? sender, EventArgs e)
        {
            // Placeholder for clear activity logic
            statusLabel.Text = "Form cleared.";
            statusLabel.ForeColor = System.Drawing.Color.RoyalBlue;
        }

        private void ExitButton_Click(object? sender, EventArgs e)
        {
            Close();
        }
        
        // Implement base form overrides
        protected override void SaveRecord()
        {
            _logger.Information("ActivityForm: SaveRecord called");
            statusLabel.Text = "Activity saved.";
            // TODO: Implement actual save logic
        }
        
        protected override void RefreshData()
        {
            _logger.Information("ActivityForm: RefreshData called");
            statusLabel.Text = "Activities refreshed.";
            // TODO: Implement actual refresh logic
        }
    }
}