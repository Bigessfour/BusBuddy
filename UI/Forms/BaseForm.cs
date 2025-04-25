using System;
using System.Windows.Forms;
using Serilog;
using BusBuddy.Utilities;
using MaterialSkin.Controls;
using System.Drawing;

namespace BusBuddy.UI.Forms
{
    public class BaseForm : MaterialForm
    {
        protected readonly Serilog.ILogger _logger;
        
        // Theme toggle button
        protected Button themeToggleButton;
        
        public BaseForm()
        {
            // Use centralized logger from FormManager
            _logger = FormManager.GetLogger(GetType().Name);
            
            // Set common form properties
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            
            // Initialize material skin
            ThemeManager.Initialize();
            ThemeManager.ApplyTheme(this);
            
            // Add theme toggle button
            InitializeThemeToggleButton();
            
            // Register common events
            this.Load += BaseForm_Load;
            this.FormClosing += BaseForm_FormClosing;
        }
        
        private void InitializeThemeToggleButton()
        {
            themeToggleButton = new Button();
            themeToggleButton.Size = new Size(40, 40);
            themeToggleButton.FlatStyle = FlatStyle.Flat;
            themeToggleButton.FlatAppearance.BorderSize = 0;
            themeToggleButton.Text = "🌙"; // Moon emoji for dark mode toggle
            themeToggleButton.Font = new Font("Segoe UI", 14);
            themeToggleButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            themeToggleButton.Location = new Point(this.ClientSize.Width - 50, 10);
            themeToggleButton.Cursor = Cursors.Hand;
            themeToggleButton.BackColor = Color.Transparent;
            themeToggleButton.Click += ThemeToggleButton_Click;
            themeToggleButton.ForeColor = Color.FromArgb(60, 60, 60);
            
            this.Controls.Add(themeToggleButton);
        }
        
        private void ThemeToggleButton_Click(object sender, EventArgs e)
        {
            ThemeManager.ToggleTheme();
            themeToggleButton.Text = ThemeManager.IsDarkMode ? "☀️" : "🌙"; // Sun or moon emoji
            
            // Refresh form styling
            ApplyThemingToControls();
            
            _logger.Information($"{this.Name}: Theme toggled to {(ThemeManager.IsDarkMode ? "dark" : "light")} mode");
        }
        
        /// <summary>
        /// Apply theming to all controls in the form
        /// </summary>
        protected virtual void ApplyThemingToControls()
        {
            // Apply styling to all controls in this form
            foreach (Control control in this.Controls)
            {
                ThemeManager.StyleControl(control);
            }
            
            this.Refresh();
        }
        
        protected virtual void BaseForm_Load(object sender, EventArgs e)
        {
            _logger.Information($"{this.Name}: Form loaded");
            
            // Apply theming to controls on load
            ApplyThemingToControls();
        }
        
        protected virtual void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logger.Information($"{this.Name}: Form closing");
        }
        
        /// <summary>
        /// Common method to navigate to another form
        /// </summary>
        /// <param name="formName">Name of the form to navigate to</param>
        /// <param name="hideCurrentForm">Whether to hide the current form (defaults to true)</param>
        protected void NavigateToForm(string formName, bool hideCurrentForm = true)
        {
            _logger.Information($"{this.Name}: Navigating to {formName}");
            FormManager.DisplayForm(formName, hideCurrentForm ? this : null);
        }
        
        /// <summary>
        /// Save the current record. Override in derived forms to implement specific saving logic.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Method is meant to be overridden")]
        protected virtual void SaveRecord()
        {
            _logger.Information($"{this.Name}: SaveRecord called but not implemented");
        }
        
        /// <summary>
        /// Edit the current record. Override in derived forms to implement specific editing logic.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Method is meant to be overridden")]
        protected virtual void EditRecord()
        {
            _logger.Information($"{this.Name}: EditRecord called but not implemented");
        }
        
        /// <summary>
        /// Refresh the data displayed in the form. Override in derived forms to implement specific refresh logic.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Method is meant to be overridden")]
        protected virtual void RefreshData()
        {
            _logger.Information($"{this.Name}: RefreshData called but not implemented");
        }
        
        /// <summary>
        /// Delete the current record. Override in derived forms to implement specific deletion logic.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Method is meant to be overridden")]
        protected virtual void DeleteRecord()
        {
            _logger.Information($"{this.Name}: DeleteRecord called but not implemented");
        }
    }
}