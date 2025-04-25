using System;
using System.Drawing;
using System.Windows.Forms;

namespace BusBuddy.Utilities
{
    public static class FormStyler
    {
        // Theme colors
        public static readonly Color PrimaryColor = Color.FromArgb(65, 139, 202);      // Main blue
        public static readonly Color SecondaryColor = Color.FromArgb(41, 98, 155);     // Darker blue for hover/focus
        public static readonly Color AccentColor = Color.FromArgb(245, 247, 250);      // Very light blue for backgrounds
        public static readonly Color NeutralColor = Color.FromArgb(88, 88, 88);        // Dark gray for text
        public static readonly Color WarningColor = Color.FromArgb(211, 84, 0);        // Orange-red for warning/delete

        public static void ApplyFormStyle(Form form)
        {
            if (form == null) return;
            
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = true; // Allow maximizing for better usability
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = AccentColor;
            form.Font = new Font("Segoe UI", 10f);
            
            // Add a subtle border effect to the form
            form.Padding = new Padding(3);
            
            // Style the form's header text
            if (form.Text.Length > 0)
            {
                form.ForeColor = NeutralColor;
            }
            
            // Force a refresh to ensure styles are applied
            form.Refresh();
        }

        public static void StyleButton(Button button, bool isPrimary = true)
        {
            if (button == null) return;
            
            // Remove default visual styles that might override our custom styling
            button.UseVisualStyleBackColor = false;
            
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = isPrimary ? PrimaryColor : WarningColor;
            button.Padding = new Padding(8, 5, 8, 5);
            button.Margin = new Padding(5);
            button.Cursor = Cursors.Hand;
            button.BackColor = isPrimary ? PrimaryColor : Color.White;
            button.ForeColor = isPrimary ? Color.White : WarningColor;
            
            // Preserve existing size if it's already set, otherwise use default size
            if (button.Size.Width < 100 || button.Size.Height < 30)
            {
                button.Size = new Size(120, 40);
            }
            
            button.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // Clear existing event handlers to prevent duplicates
            // We need to do this more carefully since Button doesn't have GetInvocationList
            button.MouseEnter -= Button_MouseEnter;
            button.MouseLeave -= Button_MouseLeave;
            
            // Add hover effect handler
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            
            // Store button type as tag for the event handlers
            button.Tag = isPrimary;
        }
        
        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                bool isPrimary = btn.Tag is bool ? (bool)btn.Tag : true;
                btn.BackColor = isPrimary ? SecondaryColor : Color.FromArgb(252, 235, 233);
            }
        }
        
        private static void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                bool isPrimary = btn.Tag is bool ? (bool)btn.Tag : true;
                btn.BackColor = isPrimary ? PrimaryColor : Color.White;
            }
        }

        public static void StyleDataGridView(DataGridView grid)
        {
            if (grid == null) return;
            
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = AccentColor;
            grid.GridColor = Color.FromArgb(230, 230, 230);
            grid.EnableHeadersVisualStyles = false;
            
            // Style the column headers
            grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 35;
            
            // Style alternating rows
            grid.RowsDefaultCellStyle.BackColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 247, 252);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(182, 214, 237);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            
            // Ensure the grid is refreshed with new styling
            grid.Refresh();
        }

        public static void StyleGroupBox(GroupBox groupBox)
        {
            if (groupBox == null) return;
            
            groupBox.FlatStyle = FlatStyle.Flat;
            groupBox.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            groupBox.ForeColor = PrimaryColor;
            groupBox.BackColor = Color.White;
            groupBox.Padding = new Padding(10);
        }

        public static void StyleLabel(Label label, bool isHeading = false)
        {
            if (label == null) return;
            
            if (isHeading)
            {
                label.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
                label.ForeColor = PrimaryColor;
            }
            else
            {
                label.Font = new Font("Segoe UI", 9.5f);
                label.ForeColor = NeutralColor;
            }
        }

        public static void StyleInputControls(Control container)
        {
            if (container == null || container.Controls == null) return;
            
            foreach (Control control in container.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Font = new Font("Segoe UI", 9.5f);
                    textBox.BackColor = Color.White;
                    textBox.Padding = new Padding(3);
                }
                else if (control is Button button)
                {
                    StyleButton(button, !button.Name.Contains("Delete") && !button.Name.Contains("Cancel"));
                }
                else if (control is Label label)
                {
                    StyleLabel(label, label.Name.Contains("Header") || label.Name.Contains("Title") || label.Font.Bold);
                }
                else if (control is DataGridView dataGridView)
                {
                    StyleDataGridView(dataGridView);
                }
                else if (control is GroupBox groupBox)
                {
                    StyleGroupBox(groupBox);
                    
                    // Process the controls inside the group box recursively
                    StyleInputControls(groupBox);
                }
                else if (control.Controls.Count > 0)
                {
                    StyleInputControls(control);
                }
                
                // Special case for panels - preserve their transparency but style children
                if (control is Panel panel)
                {
                    panel.BackColor = Color.Transparent;
                    StyleInputControls(panel);
                }
            }
        }

        public static void ApplyControlStyles(Form form)
        {
            // Apply form basic style
            ApplyFormStyle(form);
            
            try
            {
                // Style all controls recursively
                StyleInputControls(form);
                
                // Force refresh to ensure styling is displayed
                form.PerformLayout();
                form.Refresh();
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                System.Diagnostics.Debug.WriteLine($"Error applying styles: {ex.Message}");
            }
        }
    }
}