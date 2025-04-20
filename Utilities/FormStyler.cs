using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BusBuddy
{
    /// <summary>
    /// Utility class to apply consistent styling to forms and controls
    /// </summary>
    public static class FormStyler
    {
        /// <summary>
        /// Apply base styling to a form
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = AppSettings.Theme.BackgroundColor;
            form.Font = AppSettings.Theme.NormalFont;
            form.ForeColor = AppSettings.Theme.TextColor;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;

            // Apply styling to any status strip if present
            foreach (Control control in form.Controls)
            {
                if (control is StatusStrip statusStrip)
                {
                    statusStrip.BackColor = AppSettings.Theme.PrimaryColor;
                    statusStrip.ForeColor = AppSettings.Theme.TextLightColor;
                    statusStrip.Font = AppSettings.Theme.NormalFont;
                    statusStrip.SizingGrip = false;

                    foreach (ToolStripItem item in statusStrip.Items)
                    {
                        if (item is ToolStripStatusLabel label)
                        {
                            label.ForeColor = AppSettings.Theme.TextLightColor;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Style a TabControl to use modern flat styling
        /// </summary>
        public static void StyleTabControl(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(150, 40);
            tabControl.Font = AppSettings.Theme.SubheaderFont;
            
            foreach (TabPage tab in tabControl.TabPages)
            {
                tab.BackColor = AppSettings.Theme.BackgroundColor;
                tab.BorderStyle = BorderStyle.None;
                tab.Padding = new Padding(10);
            }
        }

        /// <summary>
        /// Style a DataGridView with modern appearance
        /// </summary>
        public static void StyleDataGridView(DataGridView dataGridView)
        {
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.BackgroundColor = AppSettings.Theme.BackgroundColor;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.GridColor = AppSettings.Theme.BorderColor;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.RowTemplate.Height = 30;
            dataGridView.Font = AppSettings.Theme.DataFont;
            dataGridView.DefaultCellStyle.SelectionBackColor = AppSettings.Theme.SecondaryColor;
            dataGridView.DefaultCellStyle.SelectionForeColor = AppSettings.Theme.TextLightColor;
            dataGridView.DefaultCellStyle.BackColor = AppSettings.Theme.CardColor;
            dataGridView.DefaultCellStyle.ForeColor = AppSettings.Theme.TextColor;
            dataGridView.DefaultCellStyle.Padding = new Padding(3);

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = AppSettings.Theme.PrimaryColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = AppSettings.Theme.TextLightColor;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(AppSettings.Theme.DataFont, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.ColumnHeadersHeight = 40;
            
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }
        
        /// <summary>
        /// Apply modern styling to a Button
        /// </summary>
        public static void StyleButton(Button button, bool isPrimary = true)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.Font = AppSettings.Theme.ButtonFont;
            button.FlatAppearance.BorderSize = 0;
            button.Padding = new Padding(5);
            button.Cursor = Cursors.Hand;
            
            if (isPrimary)
            {
                button.BackColor = AppSettings.Theme.PrimaryColor;
                button.ForeColor = AppSettings.Theme.TextLightColor;
            }
            else
            {
                button.BackColor = AppSettings.Theme.SecondaryColor;
                button.ForeColor = AppSettings.Theme.TextLightColor;
            }
        }
        
        /// <summary>
        /// Style an action button (Add, Save, etc.)
        /// </summary>
        public static void StyleActionButton(Button button)
        {
            StyleButton(button);
            button.Size = new Size(120, 40);
            button.FlatAppearance.BorderSize = 0;
        }
        
        /// <summary>
        /// Style a secondary button (Cancel, Clear, etc.)
        /// </summary>
        public static void StyleSecondaryButton(Button button)
        {
            StyleButton(button, false);
            button.BackColor = AppSettings.Theme.SecondaryColor;
            button.Size = new Size(120, 40);
        }
        
        /// <summary>
        /// Style input controls for consistent look
        /// </summary>
        public static void StyleInputControls(Control container)
        {
            foreach (Control control in container.Controls)
            {
                // Style labels
                if (control is Label label)
                {
                    label.Font = AppSettings.Theme.LabelFont;
                    label.ForeColor = AppSettings.Theme.TextColor;
                }
                // Style text boxes
                else if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Font = AppSettings.Theme.NormalFont;
                    textBox.BackColor = AppSettings.Theme.CardColor;
                }
                // Style combo boxes
                else if (control is ComboBox comboBox)
                {
                    comboBox.Font = AppSettings.Theme.NormalFont;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = AppSettings.Theme.CardColor;
                }
                // Style numeric up down controls
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.Font = AppSettings.Theme.NormalFont;
                    numericUpDown.BorderStyle = BorderStyle.FixedSingle;
                    numericUpDown.BackColor = AppSettings.Theme.CardColor;
                }
                // Style date time pickers
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.Font = AppSettings.Theme.NormalFont;
                    dateTimePicker.CalendarFont = AppSettings.Theme.NormalFont;
                    dateTimePicker.CalendarTitleBackColor = AppSettings.Theme.PrimaryColor;
                    dateTimePicker.CalendarTitleForeColor = AppSettings.Theme.TextLightColor;
                }
                // Process tab controls
                else if (control is TabControl tabControl)
                {
                    StyleTabControl(tabControl);
                }
                // Process tab pages and group boxes
                else if (control is TabPage tabPage || control is GroupBox groupBox)
                {
                    StyleInputControls(control);
                }
                // Style data grids
                else if (control is DataGridView dataGridView)
                {
                    StyleDataGridView(dataGridView);
                }
                // Style buttons
                else if (control is Button button)
                {
                    if (button.Name.Contains("Add") || button.Name.Contains("Save") || button.Name.Contains("Update"))
                    {
                        StyleActionButton(button);
                    }
                    else if (button.Name.Contains("Clear") || button.Name.Contains("Cancel") || button.Name.Contains("Delete"))
                    {
                        StyleSecondaryButton(button);
                    }
                    else
                    {
                        StyleButton(button);
                    }
                }
                // Process panel and other containers recursively
                else if (control.Controls.Count > 0)
                {
                    StyleInputControls(control);
                }
            }
        }
    }
}