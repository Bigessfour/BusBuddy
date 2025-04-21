using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Serilog;
using System.ComponentModel;
using BusBuddy.UI.Interfaces;
using System.Linq; // Needed for LINQ

namespace BusBuddy.UI.Forms
{
    public class BaseForm : Form
    {
        protected IFormNavigator FormNavigator { get; }
        protected ILogger Logger { get; }
        public ToolStripStatusLabel? statusLabel; // Make nullable with ? operator
        private IContainer? components = null; // Already nullable with ?
        private StatusStrip statusStrip = null!; // Non-nullable with null! initialization

        // Fields for dragging controls
        private bool _isDragging = false;
        private Point _dragStartPoint;
        private Control _draggedControl = null;
        protected List<Control> _draggableControls = new List<Control>(); // Made protected

        public BaseForm(IFormNavigator navigator)
        {
            FormNavigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            Logger = Log.Logger;

            // Initialize the status bar components
            InitializeStatusStrip();

            // Subscribe to events
            this.Load += BaseForm_Load;
            this.FormClosing += BaseForm_FormClosing; // Add handler for saving layout
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

            // Setup draggable controls and load layout *after* styling and controls are potentially created
            SetupDraggableControls();
            LoadLayoutSettings(); // Load saved layout
        }

        // Add FormClosing event handler to save layout
        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLayoutSettings();
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
            this.FormBorderStyle = FormBorderStyle.Sizable; // Changed from FixedSingle to Sizable
            this.MaximizeBox = true; // Enabled MaximizeBox
            this.StartPosition = FormStartPosition.CenterScreen;

            // Style status strip if present
            if (statusStrip != null)
            {
                statusStrip.BackColor = AppSettings.Theme.PrimaryColor;
                statusStrip.ForeColor = AppSettings.Theme.TextLightColor;
                statusStrip.Font = AppSettings.Theme.NormalFont;
                statusStrip.SizingGrip = true; // Changed from false to true

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
            if (statusLabel is null || this.IsDisposed)
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
                    if (statusLabel is not null && !statusLabel.IsDisposed)
                    {
                        statusLabel.ForeColor = color;
                        statusLabel.Text = message ?? string.Empty; // Null-safe assignment
                    }
                }));
            }
            catch (InvalidOperationException ex)
            {
                Logger.Warning("Could not update status: {ErrorMessage}", ex.Message);
            }
        }

        // --- Drag and Drop Implementation ---

        /// <summary>
        /// Sets up which controls should be draggable. By default, finds all GroupBoxes.
        /// Derived forms can override this to customize draggable controls.
        /// </summary>
        protected virtual void SetupDraggableControls()
        {
            _draggableControls.Clear(); // Clear any previous list
            FindAndAddDraggableControlsRecursive(this);
            foreach (var control in _draggableControls)
            {
                AttachDragEvents(control);
            }
            Logger?.Debug("Found {Count} draggable GroupBox controls on {FormName}", _draggableControls.Count, this.Name);
        }

        /// <summary>
        /// Recursively finds GroupBox controls and adds them to the draggable list.
        /// </summary>
        private void FindAndAddDraggableControlsRecursive(Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control is GroupBox gb)
                {
                    // Ensure the GroupBox has a name for saving/loading
                    if (string.IsNullOrEmpty(gb.Name))
                    {
                        Logger?.Warning("GroupBox found without a Name property on {FormName}. Its position cannot be saved.", this.Name);
                    }
                    else if (!_draggableControls.Contains(gb))
                    {
                        _draggableControls.Add(gb);
                    }
                }
                // Recurse into container controls like Panels, TabPages, etc.
                if (control.Controls.Count > 0 && !(control is GroupBox)) // Don't recurse into the GroupBoxes we just added
                {
                    FindAndAddDraggableControlsRecursive(control);
                }
            }
        }

        private void AttachDragEvents(Control control)
        {
            if (control == null) return;
            control.MouseDown += Control_MouseDown;
            control.MouseMove += Control_MouseMove;
            control.MouseUp += Control_MouseUp;

            // Attach to children (like labels) within the draggable control for easier grabbing
            foreach (Control child in control.Controls)
            {
                // Avoid attaching to controls that need their own mouse interactions
                if (!(child is Button || child is TextBox || child is ComboBox || child is CheckBox || child is DateTimePicker || child is MonthCalendar || child is DataGridView || child is ListBox))
                {
                    child.MouseDown += Control_MouseDown;
                    child.MouseMove += Control_MouseMove;
                    child.MouseUp += Control_MouseUp;
                }
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control potentialDragTarget = sender as Control;
                // Traverse up the parent hierarchy until we find a control in our draggable list
                while (potentialDragTarget != null && !_draggableControls.Contains(potentialDragTarget))
                {
                    potentialDragTarget = potentialDragTarget.Parent;
                }

                if (potentialDragTarget != null) // Found a draggable control
                {
                    _isDragging = true;
                    _draggedControl = potentialDragTarget;
                    _dragStartPoint = e.Location; // Location relative to the control where mouse went down
                    _draggedControl.Cursor = Cursors.SizeAll;
                    _draggedControl.BringToFront();
                }
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedControl != null)
            {
                // Calculate the new location of the control's top-left corner
                // relative to its parent container
                Point newLocation = _draggedControl.Location;
                newLocation.X += e.X - _dragStartPoint.X;
                newLocation.Y += e.Y - _dragStartPoint.Y;

                _draggedControl.Location = newLocation;
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isDragging)
            {
                _isDragging = false;
                if (_draggedControl != null)
                {
                    _draggedControl.Cursor = Cursors.Default;
                }
                _draggedControl = null;
            }
        }

        // --- Layout Saving and Loading ---
        protected void LoadLayoutSettings() // Made protected
        {
            // Use Form's Name as the key, ensure it's set
            string formName = this.Name;
            if (string.IsNullOrEmpty(formName))
            {
                Logger?.Warning("Form {FormType} does not have a Name property set. Layout cannot be loaded.", this.GetType().Name);
                return;
            }

            var layoutSettings = AppSettings.Layout.GetLayout(formName);
            if (layoutSettings != null)
            {
                // Apply form size (only if valid and form is sizable)
                if (this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                {
                    if (layoutSettings.FormSize.Width > 100 && layoutSettings.FormSize.Height > 100) // Basic validation
                    {
                        this.Size = layoutSettings.FormSize;
                    }
                }

                // Apply control locations
                foreach (var kvp in layoutSettings.ControlLocations)
                {
                    var control = FindControlRecursive(this, kvp.Key);
                    // Ensure the control exists and is one we intended to make draggable
                    if (control != null && _draggableControls.Any(dc => dc.Name == kvp.Key))
                    {
                        control.Location = kvp.Value;
                    }
                    else if (control != null)
                    {
                        Logger?.Debug("Control {ControlName} found but not in draggable list for {FormName}, skipping saved location.", kvp.Key, formName);
                    }
                    else
                    {
                        Logger?.Warning("Saved layout contains location for non-existent control '{ControlName}' on form {FormName}.", kvp.Key, formName);
                    }
                }
                Logger?.Information("Loaded layout settings for {FormName}", formName);
            }
            else
            {
                Logger?.Information("No saved layout settings found for {FormName}, using default layout.", formName);
            }
        }

        protected void SaveLayoutSettings() // Made protected
        {
            // Use Form's Name as the key, ensure it's set
            string formName = this.Name;
            if (string.IsNullOrEmpty(formName))
            {
                Logger?.Warning("Form {FormType} does not have a Name property set. Layout cannot be saved.", this.GetType().Name);
                return;
            }

            var controlLocations = new Dictionary<string, Point>();
            foreach (var control in _draggableControls)
            {
                // Only save controls that have a valid Name
                if (control != null && !string.IsNullOrEmpty(control.Name))
                {
                    controlLocations[control.Name] = control.Location;
                }
                else if (control != null)
                {
                    Logger?.Warning("Attempted to save layout for a draggable control without a Name on {FormName}.", formName);
                }
            }

            // Only save size if the form is sizable
            Size sizeToSave = (this.FormBorderStyle == FormBorderStyle.Sizable || this.FormBorderStyle == FormBorderStyle.SizableToolWindow) ? this.Size : Size.Empty; // Save Size.Empty if not sizable

            AppSettings.Layout.SaveLayout(formName, sizeToSave, controlLocations);
            Logger?.Information("Saved layout settings for {FormName}", formName);
        }

        // Helper to find controls by name, even if nested
        private Control FindControlRecursive(Control container, string name)
        {
            if (container == null || string.IsNullOrEmpty(name)) return null;

            // Check the container itself
            if (container.Name == name) return container;

            // Check children
            foreach (Control c in container.Controls)
            {
                var found = FindControlRecursive(c, name);
                if (found != null) return found;
            }
            return null;
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