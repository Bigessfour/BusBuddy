using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace BusBuddy.Forms
{
    /// <summary>
    /// Dialog for adding or editing bus routes
    /// </summary>
    public class RouteEditorDialog : Form
    {
        // Form controls
        private TextBox txtRouteName;
        private TextBox txtStartLocation;
        private TextBox txtEndLocation;
        private NumericUpDown numDistance;
        private Button btnSave;
        private Button btnCancel;
        private Label lblRouteNameError;
        private Label lblStartLocationError;
        private Label lblEndLocationError;
        
        // Public properties exposed for data binding
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RouteName { get; set; } = string.Empty;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StartLocation { get; set; } = string.Empty;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EndLocation { get; set; } = string.Empty;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Distance { get; set; }

        /// <summary>
        /// Initializes a new instance of the RouteEditorDialog
        /// </summary>
        public RouteEditorDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the RouteEditorDialog with existing data
        /// </summary>
        /// <param name="routeName">Name of the route</param>
        /// <param name="start">Starting location</param>
        /// <param name="end">Ending location</param>
        /// <param name="distance">Distance in miles</param>
        public RouteEditorDialog(string routeName, string start, string end, decimal distance)
            : this()
        {
            RouteName = routeName;
            StartLocation = start;
            EndLocation = end;
            Distance = distance;
            
            // Set control values after initialization
            txtRouteName.Text = routeName;
            txtStartLocation.Text = start;
            txtEndLocation.Text = end;
            numDistance.Value = distance;
        }

        /// <summary>
        /// Initializes form components
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Route Editor";
            this.Size = new Size(450, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = new Font("Microsoft Sans Serif", 8F);
            this.BackColor = Color.White;

            // Route Name
            Label lblRouteName = new Label
            {
                Text = "Route Name:",
                Location = new Point(20, 20),
                Size = new Size(100, 20)
            };
            this.Controls.Add(lblRouteName);

            txtRouteName = new TextBox
            {
                Location = new Point(130, 20),
                Size = new Size(280, 20)
            };
            txtRouteName.TextChanged += ValidateInput;
            this.Controls.Add(txtRouteName);

            lblRouteNameError = new Label
            {
                Text = "Route name is required",
                Location = new Point(130, 45),
                Size = new Size(280, 20),
                ForeColor = Color.Red,
                Visible = false
            };
            this.Controls.Add(lblRouteNameError);

            // Start Location
            Label lblStartLocation = new Label
            {
                Text = "Start Location:",
                Location = new Point(20, 70),
                Size = new Size(100, 20)
            };
            this.Controls.Add(lblStartLocation);

            txtStartLocation = new TextBox
            {
                Location = new Point(130, 70),
                Size = new Size(280, 20)
            };
            txtStartLocation.TextChanged += ValidateInput;
            this.Controls.Add(txtStartLocation);

            lblStartLocationError = new Label
            {
                Text = "Start location is required",
                Location = new Point(130, 95),
                Size = new Size(280, 20),
                ForeColor = Color.Red,
                Visible = false
            };
            this.Controls.Add(lblStartLocationError);

            // End Location
            Label lblEndLocation = new Label
            {
                Text = "End Location:",
                Location = new Point(20, 120),
                Size = new Size(100, 20)
            };
            this.Controls.Add(lblEndLocation);

            txtEndLocation = new TextBox
            {
                Location = new Point(130, 120),
                Size = new Size(280, 20)
            };
            txtEndLocation.TextChanged += ValidateInput;
            this.Controls.Add(txtEndLocation);

            lblEndLocationError = new Label
            {
                Text = "End location is required",
                Location = new Point(130, 145),
                Size = new Size(280, 20),
                ForeColor = Color.Red,
                Visible = false
            };
            this.Controls.Add(lblEndLocationError);

            // Distance
            Label lblDistance = new Label
            {
                Text = "Distance (miles):",
                Location = new Point(20, 170),
                Size = new Size(100, 20)
            };
            this.Controls.Add(lblDistance);

            numDistance = new NumericUpDown
            {
                Location = new Point(130, 170),
                Size = new Size(100, 20),
                Minimum = 0,
                Maximum = 1000,
                DecimalPlaces = 1,
                Increment = 0.1m
            };
            this.Controls.Add(numDistance);

            // Buttons
            btnSave = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                Location = new Point(130, 220),
                Size = new Size(75, 30)
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(220, 220),
                Size = new Size(75, 30)
            };
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;            // Do initial validation
            ValidateInput(this, EventArgs.Empty);
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            bool isValid = true;
            
            // Validate route name
            if (string.IsNullOrWhiteSpace(txtRouteName.Text))
            {
                lblRouteNameError.Visible = true;
                isValid = false;
            }
            else
            {
                lblRouteNameError.Visible = false;
            }
            
            // Validate start location
            if (string.IsNullOrWhiteSpace(txtStartLocation.Text))
            {
                lblStartLocationError.Visible = true;
                isValid = false;
            }
            else
            {
                lblStartLocationError.Visible = false;
            }
            
            // Validate end location
            if (string.IsNullOrWhiteSpace(txtEndLocation.Text))
            {
                lblEndLocationError.Visible = true;
                isValid = false;
            }
            else
            {
                lblEndLocationError.Visible = false;
            }
            
            // Enable/disable save button based on validation
            btnSave.Enabled = isValid;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Update properties before dialog closes
            RouteName = txtRouteName.Text;
            StartLocation = txtStartLocation.Text;
            EndLocation = txtEndLocation.Text;
            Distance = numDistance.Value;
        }
    }
}
