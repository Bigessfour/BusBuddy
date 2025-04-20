using System;
using System.Drawing;
using System.Windows.Forms;
using Serilog;
using System.ComponentModel;
using BusBuddy.UI.Interfaces;

namespace BusBuddy.UI.Forms
{
    public class BaseForm : Form
    {
        protected IFormNavigator FormNavigator { get; }
        protected ILogger Logger { get; }
        public ToolStripStatusLabel? statusLabel; // Make nullable with ? operator
        private IContainer? components = null; // Already nullable with ?
        private StatusStrip statusStrip;

        public BaseForm(IFormNavigator navigator)
        {
            FormNavigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            Logger = Log.Logger;
            
            // Initialize the status bar components
            InitializeStatusStrip();
            
            // Subscribe to the Load event to ensure the form is fully initialized
            this.Load += BaseForm_Load;
        }
        
        private void InitializeStatusStrip()
        {
            statusStrip = new StatusStrip();
            statusStrip.Dock = DockStyle.Bottom;
            statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "Ready";
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }
        
        private void BaseForm_Load(object sender, EventArgs e)
        {
            if (statusLabel != null)
            {
                statusLabel.Text = "Ready";
            }
            
            // Apply our modern styling to the form
            ApplyStyling();
        }
        
        /// <summary>
        /// Apply consistent styling to the form and all its controls
        /// </summary>
        protected virtual void ApplyStyling()
        {
            // Apply base form styling
            ApplyFormStyle();
            
            // Apply styling to all input controls recursively
            StyleAllControls();
        }

        /// <summary>
        /// Apply base styling to the form
        /// </summary>
        protected void ApplyFormStyle()
        {
            this.BackColor = AppSettings.Theme.BackgroundColor;
            this.Font = AppSettings.Theme.NormalFont;
            this.ForeColor = AppSettings.Theme.TextColor;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Style status strip if present
            if (statusStrip != null)
            {
                statusStrip.BackColor = AppSettings.Theme.PrimaryColor;
                statusStrip.ForeColor = AppSettings.Theme.TextLightColor;
                statusStrip.Font = AppSettings.Theme.NormalFont;
                statusStrip.SizingGrip = false;

                if (statusLabel != null)
                {
                    statusLabel.ForeColor = AppSettings.Theme.TextLightColor;
                }
            }
        }

        /// <summary>
        /// Style all controls on the form recursively
        /// </summary>
        protected void StyleAllControls()
        {
            foreach (Control control in this.Controls)
            {
                // Style tab controls
                if (control is TabControl tabControl)
                {
                    StyleTabControl(tabControl);
                }
                // Style data grids
                else if (control is DataGridView dgv)
                {
                    StyleDataGridView(dgv);
                }
                // Style buttons
                else if (control is Button btn)
                {
                    StyleButton(btn);
                }
                // Style text boxes
                else if (control is TextBox txtBox)
                {
                    txtBox.BorderStyle = BorderStyle.FixedSingle;
                    txtBox.BackColor = AppSettings.Theme.CardColor;
                    txtBox.Font = AppSettings.Theme.NormalFont;
                }
                // Style combo boxes
                else if (control is ComboBox combo)
                {
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = AppSettings.Theme.CardColor;
                    combo.Font = AppSettings.Theme.NormalFont;
                }
                // Style labels
                else if (control is Label lbl)
                {
                    lbl.ForeColor = AppSettings.Theme.TextColor;
                    lbl.Font = AppSettings.Theme.LabelFont;
                }
                // Style date time pickers
                else if (control is DateTimePicker dtp)
                {
                    dtp.Font = AppSettings.Theme.NormalFont;
                }
                // Style numeric up down controls
                else if (control is NumericUpDown nud)
                {
                    nud.BorderStyle = BorderStyle.FixedSingle;
                    nud.BackColor = AppSettings.Theme.CardColor;
                    nud.Font = AppSettings.Theme.NormalFont;
                }
                // Recursively process containers
                else if (control.Controls.Count > 0)
                {
                    // Apply styling recursively to all child controls
                    ApplyStylesToContainer(control);
                }
            }
        }
        
        /// <summary>
        /// Apply styles to all controls within a container
        /// </summary>
        private void ApplyStylesToContainer(Control container)
        {
            foreach (Control control in container.Controls)
            {
                // Style tab controls
                if (control is TabControl tabControl)
                {
                    StyleTabControl(tabControl);
                }
                // Style tab pages
                else if (control is TabPage tabPage)
                {
                    tabPage.BackColor = AppSettings.Theme.BackgroundColor;
                    ApplyStylesToContainer(tabPage);
                }
                // Style data grids
                else if (control is DataGridView dgv)
                {
                    StyleDataGridView(dgv);
                }
                // Style buttons
                else if (control is Button btn)
                {
                    StyleButton(btn);
                }
                // Style text boxes
                else if (control is TextBox txtBox)
                {
                    txtBox.BorderStyle = BorderStyle.FixedSingle;
                    txtBox.BackColor = AppSettings.Theme.CardColor;
                    txtBox.Font = AppSettings.Theme.NormalFont;
                }
                // Style combo boxes
                else if (control is ComboBox combo)
                {
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = AppSettings.Theme.CardColor;
                    combo.Font = AppSettings.Theme.NormalFont;
                }
                // Style labels
                else if (control is Label lbl)
                {
                    lbl.ForeColor = AppSettings.Theme.TextColor;
                    lbl.Font = AppSettings.Theme.LabelFont;
                }
                // Style date time pickers
                else if (control is DateTimePicker dtp)
                {
                    dtp.Font = AppSettings.Theme.NormalFont;
                }
                // Style numeric up down controls
                else if (control is NumericUpDown nud)
                {
                    nud.BorderStyle = BorderStyle.FixedSingle;
                    nud.BackColor = AppSettings.Theme.CardColor;
                    nud.Font = AppSettings.Theme.NormalFont;
                }
                // Recursively style any child containers
                else if (control.Controls.Count > 0)
                {
                    ApplyStylesToContainer(control);
                }
            }
        }
        
        /// <summary>
        /// Style a tab control for modern appearance
        /// </summary>
        protected void StyleTabControl(TabControl tabControl)
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
                ApplyStylesToContainer(tab);
            }
        }
        
        /// <summary>
        /// Style a DataGridView with modern appearance
        /// </summary>
        protected void StyleDataGridView(DataGridView dataGridView)
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
        /// Style a button based on its purpose
        /// </summary>
        protected void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.Font = AppSettings.Theme.ButtonFont;
            button.FlatAppearance.BorderSize = 0;
            button.Padding = new Padding(5);
            button.Cursor = Cursors.Hand;
            
            // Apply different styles based on button's name/purpose
            if (button.Name.Contains("Add") || button.Name.Contains("Save") || button.Name.Contains("Update"))
            {
                // Primary action buttons
                button.BackColor = AppSettings.Theme.PrimaryColor;
                button.ForeColor = AppSettings.Theme.TextLightColor;
            }
            else if (button.Name.Contains("Clear") || button.Name.Contains("Cancel") || button.Name.Contains("Delete"))
            {
                // Secondary action buttons
                button.BackColor = AppSettings.Theme.SecondaryColor;
                button.ForeColor = AppSettings.Theme.TextLightColor;
            }
            else
            {
                // Default buttons
                button.BackColor = AppSettings.Theme.PrimaryColor;
                button.ForeColor = AppSettings.Theme.TextLightColor;
            }
        }

        protected void UpdateStatus(string message, Color color)
        {
            if (statusLabel == null || this.IsDisposed)
            {
                Logger.Warning("Cannot update status: statusLabel is null or form is disposed.");
                return;
            }

            // Check if the form's handle is created before invoking
            if (!this.IsHandleCreated)
            {
                Logger.Warning("Cannot update status: Form handle not created yet. Deferring update.");
                // Defer the update until the form is loaded
                this.Load += (s, e) =>
                {
                    UpdateStatus(message, color);
                };
                return;
            }

            // Use BeginInvoke to ensure thread safety for UI updates
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (statusLabel != null && !statusLabel.IsDisposed)
                    {
                        statusLabel.ForeColor = color;
                        statusLabel.Text = message;
                    }
                }));
            }
            catch (InvalidOperationException ex)
            {
                Logger.Warning("Could not update status: {ErrorMessage}", ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                statusLabel?.Dispose();
                statusStrip?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}