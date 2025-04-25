using System;
using System.Drawing;
using System.Windows.Forms;
using MaterialSkin.Controls;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Controls
{
    /// <summary>
    /// Factory for creating modern, Material Design UI controls
    /// </summary>
    public static class ControlFactory
    {
        // Consistent padding and margin values
        private static readonly Padding StandardPadding = new Padding(8);
        private static readonly Padding StandardMargin = new Padding(6);
        
        /// <summary>
        /// Creates a Material Design button with hover effects and ripple animation
        /// </summary>
        public static MaterialButton CreateButton(string text, string name = null)
        {
            MaterialButton button = new MaterialButton();
            button.Text = text;
            button.Name = name ?? $"btn{text.Replace(" ", "")}";
            button.AutoSize = false;
            button.Size = new Size(150, 40);
            button.UseAccentColor = false;
            button.Margin = StandardMargin;
            button.Padding = StandardPadding;
            button.Cursor = Cursors.Hand;
            
            return button;
        }
        
        /// <summary>
        /// Creates a Material Design text box with floating hint
        /// </summary>
        public static MaterialTextBox2 CreateTextBox(string hint, string name = null)
        {
            MaterialTextBox2 textBox = new MaterialTextBox2();
            textBox.Hint = hint;
            textBox.Name = name ?? $"txt{hint.Replace(" ", "")}";
            textBox.Size = new Size(250, 50);
            textBox.Margin = StandardMargin;
            textBox.MaxLength = 255;
            
            return textBox;
        }
        
        /// <summary>
        /// Creates a Material Design multiline text box with floating hint
        /// </summary>
        public static MaterialMultiLineTextBox2 CreateMultiLineTextBox(string hint, string name = null)
        {
            MaterialMultiLineTextBox2 textBox = new MaterialMultiLineTextBox2();
            textBox.Hint = hint;
            textBox.Name = name ?? $"txt{hint.Replace(" ", "")}";
            textBox.Size = new Size(250, 100);
            textBox.Margin = StandardMargin;
            textBox.MaxLength = 1000;
            
            return textBox;
        }
        
        /// <summary>
        /// Creates a Material Design combo box with floating hint
        /// </summary>
        public static MaterialComboBox CreateComboBox(string hint, string[] items = null, string name = null)
        {
            MaterialComboBox comboBox = new MaterialComboBox();
            comboBox.Hint = hint;
            comboBox.Name = name ?? $"cmb{hint.Replace(" ", "")}";
            comboBox.Size = new Size(250, 50);
            comboBox.Margin = StandardMargin;
            comboBox.FormattingEnabled = true;
            
            if (items != null)
            {
                comboBox.Items.AddRange(items);
            }
            
            return comboBox;
        }
        
        /// <summary>
        /// Creates a Material Design checkbox
        /// </summary>
        public static MaterialCheckbox CreateCheckBox(string text, string name = null)
        {
            MaterialCheckbox checkbox = new MaterialCheckbox();
            checkbox.Text = text;
            checkbox.Name = name ?? $"chk{text.Replace(" ", "")}";
            checkbox.Size = new Size(250, 30);
            checkbox.Margin = StandardMargin;
            
            return checkbox;
        }
        
        /// <summary>
        /// Creates a Material Design radio button
        /// </summary>
        public static MaterialRadioButton CreateRadioButton(string text, string name = null)
        {
            MaterialRadioButton radioButton = new MaterialRadioButton();
            radioButton.Text = text;
            radioButton.Name = name ?? $"rad{text.Replace(" ", "")}";
            radioButton.Size = new Size(250, 30);
            radioButton.Margin = StandardMargin;
            
            return radioButton;
        }
        
        /// <summary>
        /// Creates a Material Design label
        /// </summary>
        public static MaterialLabel CreateLabel(string text, string name = null, bool isHeading = false)
        {
            MaterialLabel label = new MaterialLabel();
            label.Text = text;
            label.Name = name ?? $"lbl{text.Replace(" ", "")}";
            label.AutoSize = true;
            label.Margin = StandardMargin;
            
            if (isHeading)
            {
                label.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                label.Depth = 0;
            }
            
            return label;
        }
        
        /// <summary>
        /// Creates a Material Design card panel for grouping controls
        /// </summary>
        public static MaterialCard CreateCard(string title = null)
        {
            MaterialCard card = new MaterialCard();
            card.Size = new Size(500, 300);
            card.Margin = StandardMargin;
            card.Padding = StandardPadding;
            
            if (!string.IsNullOrEmpty(title))
            {
                MaterialLabel titleLabel = CreateLabel(title, null, true);
                titleLabel.Dock = DockStyle.Top;
                titleLabel.TextAlign = ContentAlignment.MiddleCenter;
                titleLabel.Padding = new Padding(0, 10, 0, 15);
                
                card.Controls.Add(titleLabel);
            }
            
            return card;
        }
        
        /// <summary>
        /// Creates a Material Design date picker
        /// </summary>
        public static DateTimePicker CreateDatePicker(string name = null)
        {
            DateTimePicker datePicker = new DateTimePicker();
            datePicker.Format = DateTimePickerFormat.Short;
            datePicker.Name = name ?? "datePicker";
            datePicker.Size = new Size(200, 30);
            datePicker.Margin = StandardMargin;
            datePicker.Font = new Font("Segoe UI", 10);
            
            return datePicker;
        }
        
        /// <summary>
        /// Creates a Material Design tab control
        /// </summary>
        public static MaterialTabControl CreateTabControl(string name = null)
        {
            MaterialTabControl tabControl = new MaterialTabControl();
            tabControl.Name = name ?? "tabControl";
            tabControl.Size = new Size(600, 400);
            tabControl.Margin = StandardMargin;
            tabControl.MouseState = MaterialSkin.MouseState.HOVER;
            tabControl.Multiline = true;
            tabControl.Dock = DockStyle.Fill;
            
            return tabControl;
        }
        
        /// <summary>
        /// Creates a Material Design tab selector
        /// </summary>
        public static MaterialTabSelector CreateTabSelector(MaterialTabControl tabControl, string name = null)
        {
            MaterialTabSelector tabSelector = new MaterialTabSelector();
            tabSelector.Name = name ?? "tabSelector";
            tabSelector.Dock = DockStyle.Top;
            tabSelector.Height = 50;
            tabSelector.TabControl = tabControl;
            tabSelector.ShrinkSizeMode = MaterialSkin.Controls.MaterialTabSelector.ShrinkMode.Shrink;
            
            return tabSelector;
        }
        
        /// <summary>
        /// Creates a Material Design tab page
        /// </summary>
        public static TabPage CreateTabPage(string text, string name = null)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = text;
            tabPage.Name = name ?? $"tab{text.Replace(" ", "")}";
            tabPage.Padding = StandardPadding;
            
            return tabPage;
        }
        
        /// <summary>
        /// Creates a modern styled DataGridView
        /// </summary>
        public static DataGridView CreateDataGridView(string name = null)
        {
            DataGridView grid = new DataGridView();
            grid.Name = name ?? "dataGridView";
            grid.Size = new Size(600, 300);
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 30;
            grid.BorderStyle = BorderStyle.None;
            grid.Font = new Font("Segoe UI", 10);
            
            // Apply theming styles
            ThemeManager.StyleControl(grid);
            
            return grid;
        }
        
        /// <summary>
        /// Creates a Floating Action Button (FAB)
        /// </summary>
        public static MaterialFloatingActionButton CreateFab(string text, string name = null)
        {
            MaterialFloatingActionButton fab = new MaterialFloatingActionButton();
            fab.Text = text;
            fab.Name = name ?? $"fab{text}";
            fab.Size = new Size(60, 60);
            fab.BackColor = Color.Transparent;
            fab.Depth = 0;
            fab.Icon = null; // You can set an icon if needed
            fab.UseAccentColor = true;
            
            return fab;
        }
        
        /// <summary>
        /// Creates a Material Design slider
        /// </summary>
        public static MaterialSlider CreateSlider(string name = null, int min = 0, int max = 100, int value = 50)
        {
            MaterialSlider slider = new MaterialSlider();
            slider.Name = name ?? "slider";
            slider.Size = new Size(250, 40);
            slider.Margin = StandardMargin;
            slider.Minimum = min;
            slider.Maximum = max;
            slider.Value = value;
            
            return slider;
        }
        
        /// <summary>
        /// Creates a Material Design progress bar
        /// </summary>
        public static MaterialProgressBar CreateProgressBar(string name = null, int value = 0)
        {
            MaterialProgressBar progressBar = new MaterialProgressBar();
            progressBar.Name = name ?? "progressBar";
            progressBar.Size = new Size(250, 5);
            progressBar.Margin = StandardMargin;
            progressBar.Value = value;
            
            return progressBar;
        }
        
        /// <summary>
        /// Creates a simple panel with modern styling
        /// </summary>
        public static Panel CreateFlowPanel(string name = null, FlowDirection direction = FlowDirection.LeftToRight)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Name = name ?? "flowPanel";
            panel.Size = new Size(500, 60);
            panel.FlowDirection = direction;
            panel.WrapContents = false;
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel.Padding = StandardPadding;
            panel.Margin = StandardMargin;
            
            return panel;
        }
    }
}