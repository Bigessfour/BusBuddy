using System;
using System.Drawing;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace BusBuddy.Utilities
{
    /// <summary>
    /// Manages application-wide theming using Material Design principles
    /// Supports light and dark modes with consistent color schemes
    /// </summary>
    public static class ThemeManager
    {
        // Material Skin Manager instance
        private static readonly MaterialSkinManager MaterialSkinManager = MaterialSkinManager.Instance;
        
        // Theme state
        public static bool IsDarkMode { get; private set; } = false;
        
        // Primary color schemes (using BusBuddy blue as a base)
        private static readonly Color PrimaryColor = Color.FromArgb(65, 139, 202);
        private static readonly Color PrimaryDarkColor = Color.FromArgb(41, 98, 155);
        private static readonly Color PrimaryLightColor = Color.FromArgb(121, 180, 226);
        
        // Accent color schemes 
        private static readonly Color AccentColor = Color.FromArgb(33, 150, 243);  // Material blue
        
        // Text colors for dark/light themes
        private static readonly Color LightModeTextColor = Color.FromArgb(68, 68, 68);
        private static readonly Color DarkModeTextColor = Color.FromArgb(255, 255, 255);
        
        // Warning/danger color
        public static readonly Color WarningColor = Color.FromArgb(211, 84, 0);

        /// <summary>
        /// Initialize the MaterialSkin theme manager with default theme
        /// </summary>
        public static void Initialize()
        {
            MaterialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            
            // Set color scheme
            MaterialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue600,
                Primary.Blue800, 
                Primary.Blue500,
                Accent.LightBlue200,
                TextShade.BLACK
            );
            
            IsDarkMode = false;
        }
        
        /// <summary>
        /// Apply Material theme to a form
        /// </summary>
        /// <param name="form">The MaterialForm to apply theming to</param>
        public static void ApplyTheme(MaterialForm form)
        {
            if (form == null) return;
            
            // Register form with MaterialSkinManager
            MaterialSkinManager.AddFormToManage(form);
        }
        
        /// <summary>
        /// Apply consistent styling to standard WinForms controls
        /// </summary>
        /// <param name="control">The control to style</param>
        public static void StyleControl(Control control)
        {
            if (control == null) return;
            
            // Apply styles based on control type
            if (control is Button button && !(button is MaterialButton))
            {
                StyleButton(button);
            }
            else if (control is TextBox textBox && !(textBox is MaterialTextBox))
            {
                StyleTextBox(textBox);
            }
            else if (control is Label label)
            {
                StyleLabel(label);
            }
            else if (control is DataGridView grid)
            {
                StyleDataGridView(grid);
            }
            else if (control is GroupBox groupBox)
            {
                StyleGroupBox(groupBox);
            }
            
            // Recursively apply to child controls
            foreach (Control child in control.Controls)
            {
                StyleControl(child);
            }
        }
        
        /// <summary>
        /// Toggle between light and dark mode
        /// </summary>
        public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            
            if (IsDarkMode)
            {
                MaterialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                MaterialSkinManager.ColorScheme = new ColorScheme(
                    Primary.BlueGrey700,
                    Primary.BlueGrey900, 
                    Primary.BlueGrey500,
                    Accent.LightBlue200,
                    TextShade.WHITE
                );
            }
            else
            {
                MaterialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                MaterialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Blue600,
                    Primary.Blue800, 
                    Primary.Blue500,
                    Accent.LightBlue200,
                    TextShade.BLACK
                );
            }
        }
        
        /// <summary>
        /// Style a standard Button control with Material-like appearance
        /// </summary>
        private static void StyleButton(Button button)
        {
            if (button == null) return;
            
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = PrimaryColor;
            button.BackColor = PrimaryColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            button.Cursor = Cursors.Hand;
            button.Padding = new Padding(8, 5, 8, 5);
            
            // Add hover effects
            button.MouseEnter -= Button_MouseEnter;
            button.MouseLeave -= Button_MouseLeave;
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            
            // Store default colors to restore on leave
            button.Tag = new Tuple<Color, Color>(button.BackColor, button.ForeColor);
        }
        
        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                // Darken on hover
                btn.BackColor = PrimaryDarkColor;
                // Add slight shadow effect
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            }
        }
        
        private static void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Tuple<Color, Color> colors)
            {
                // Restore original colors
                btn.BackColor = colors.Item1;
                btn.FlatAppearance.BorderColor = PrimaryColor;
            }
        }
        
        /// <summary>
        /// Style a TextBox control with Material-like appearance
        /// </summary>
        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("Segoe UI", 10f);
            textBox.BackColor = IsDarkMode ? Color.FromArgb(55, 55, 55) : Color.White;
            textBox.ForeColor = IsDarkMode ? Color.White : Color.FromArgb(33, 33, 33);
            
            // Add a Panel under the textbox to create a line effect
            Panel underline = new Panel
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = PrimaryColor
            };
            
            // Add the underline if it doesn't exist already
            bool hasUnderline = false;
            foreach (Control c in textBox.Parent.Controls)
            {
                if (c is Panel p && p.Name == $"{textBox.Name}_underline")
                {
                    hasUnderline = true;
                    break;
                }
            }
            
            if (!hasUnderline)
            {
                Panel panel = new Panel
                {
                    Name = $"{textBox.Name}_underline",
                    Location = new Point(textBox.Left, textBox.Bottom + 2),
                    Width = textBox.Width,
                    Height = 2,
                    BackColor = PrimaryColor
                };
                
                if (textBox.Parent != null)
                {
                    textBox.Parent.Controls.Add(panel);
                    panel.BringToFront();
                }
            }
        }
        
        /// <summary>
        /// Style a Label control with Material-like appearance
        /// </summary>
        private static void StyleLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 10f);
            label.ForeColor = IsDarkMode ? Color.FromArgb(240, 240, 240) : Color.FromArgb(50, 50, 50);
            
            // If it's a heading (typically a larger font or bold)
            if (label.Font.Size > 12 || label.Font.Bold)
            {
                label.ForeColor = PrimaryColor;
            }
        }
        
        /// <summary>
        /// Style a DataGridView with Material-like appearance
        /// </summary>
        private static void StyleDataGridView(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = IsDarkMode ? Color.FromArgb(50, 50, 50) : Color.FromArgb(245, 245, 245);
            grid.GridColor = IsDarkMode ? Color.FromArgb(80, 80, 80) : Color.FromArgb(230, 230, 230);
            grid.EnableHeadersVisualStyles = false;
            
            // Style headers
            grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.ColumnHeadersHeight = 40;
            
            // Style row headers
            grid.RowHeadersDefaultCellStyle.BackColor = IsDarkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(240, 240, 240);
            
            // Style rows
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            grid.RowsDefaultCellStyle.BackColor = IsDarkMode ? Color.FromArgb(60, 60, 60) : Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = IsDarkMode ? Color.FromArgb(70, 70, 70) : Color.FromArgb(245, 245, 245);
            
            // Style selection
            grid.DefaultCellStyle.SelectionBackColor = PrimaryLightColor;
            grid.DefaultCellStyle.SelectionForeColor = IsDarkMode ? Color.White : Color.Black;
        }
        
        /// <summary>
        /// Style a GroupBox with Material-like appearance
        /// </summary>
        private static void StyleGroupBox(GroupBox groupBox)
        {
            groupBox.FlatStyle = FlatStyle.Flat;
            groupBox.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            groupBox.ForeColor = PrimaryColor;
            groupBox.BackColor = IsDarkMode ? Color.FromArgb(50, 50, 50) : Color.White;
            groupBox.Padding = new Padding(10);
            
            // Style child controls
            foreach (Control control in groupBox.Controls)
            {
                StyleControl(control);
            }
        }
    }
}