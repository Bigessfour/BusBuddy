// BusBuddy/UI/Forms/Inputs.Designer.cs
#nullable enable
using System.Windows.Forms;

namespace BusBuddy.UI.Forms
{
    partial class Inputs
    {
        private System.ComponentModel.IContainer? components;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl = new TabControl();
            tabTrips = new TabPage();
            tabVehicles = new TabPage();
            tabFuel = new TabPage();
            tabMaintenance = new TabPage();
            tabDrivers = new TabPage();

            // Trips Tab Controls
            lblTripDate = new Label();
            txtTripDate = new TextBox();
            lblBusNumberTrips = new Label();
            txtBusNumberTrips = new TextBox();
            gridTrips = new DataGridView();

            // Vehicles Tab Controls
            lblVehicleId = new Label();
            txtVehicleId = new TextBox();
            lblVehicleType = new Label();
            txtVehicleType = new TextBox();
            gridVehicles = new DataGridView();

            // Fuel Tab Controls
            lblFuelDate = new Label();
            txtFuelDate = new TextBox();
            lblFuelAmount = new Label();
            txtFuelAmount = new TextBox();
            gridFuel = new DataGridView();

            // Maintenance Tab Controls
            lblMaintDate = new Label();
            txtMaintDate = new TextBox();
            lblMaintType = new Label();
            txtMaintType = new TextBox();
            gridMaintenance = new DataGridView();

            // Drivers Tab Controls
            lblDriverName = new Label();
            txtDriverName = new TextBox();
            lblDriverLicense = new Label();
            txtDriverLicense = new TextBox();
            gridDrivers = new DataGridView();

            tabControl.SuspendLayout();
            tabTrips.SuspendLayout();
            tabVehicles.SuspendLayout();
            tabFuel.SuspendLayout();
            tabMaintenance.SuspendLayout();
            tabDrivers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridTrips).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridVehicles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridFuel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridMaintenance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridDrivers).BeginInit();

            // TabControl Setup
            tabControl.Controls.AddRange(new[] { tabTrips, tabVehicles, tabFuel, tabMaintenance, tabDrivers });
            tabControl.Location = new System.Drawing.Point(12, 12);
            tabControl.Size = new System.Drawing.Size(1000, 600);
            tabControl.Name = "tabControl";

            // Trips Tab
            tabTrips.Controls.AddRange(new Control[] { lblTripDate, txtTripDate, lblBusNumberTrips, txtBusNumberTrips, gridTrips });
            tabTrips.Name = "tabTrips";
            tabTrips.Text = "Trips";

            lblTripDate.Text = "Trip Date:";
            lblTripDate.Location = new System.Drawing.Point(20, 20);
            lblTripDate.Size = new System.Drawing.Size(100, 23);

            txtTripDate.Location = new System.Drawing.Point(120, 20);
            txtTripDate.Size = new System.Drawing.Size(150, 23);

            lblBusNumberTrips.Text = "Bus Number:";
            lblBusNumberTrips.Location = new System.Drawing.Point(20, 50);
            lblBusNumberTrips.Size = new System.Drawing.Size(100, 23);

            txtBusNumberTrips.Location = new System.Drawing.Point(120, 50);
            txtBusNumberTrips.Size = new System.Drawing.Size(150, 23);

            gridTrips.Location = new System.Drawing.Point(20, 100);
            gridTrips.Size = new System.Drawing.Size(950, 450);
            gridTrips.Name = "gridTrips";

            // Vehicles Tab
            tabVehicles.Controls.AddRange(new Control[] { lblVehicleId, txtVehicleId, lblVehicleType, txtVehicleType, gridVehicles });
            tabVehicles.Name = "tabVehicles";
            tabVehicles.Text = "Vehicles";

            lblVehicleId.Text = "Vehicle ID:";
            lblVehicleId.Location = new System.Drawing.Point(20, 20);
            lblVehicleId.Size = new System.Drawing.Size(100, 23);

            txtVehicleId.Location = new System.Drawing.Point(120, 20);
            txtVehicleId.Size = new System.Drawing.Size(150, 23);

            lblVehicleType.Text = "Vehicle Type:";
            lblVehicleType.Location = new System.Drawing.Point(20, 50);
            lblVehicleType.Size = new System.Drawing.Size(100, 23);

            txtVehicleType.Location = new System.Drawing.Point(120, 50);
            txtVehicleType.Size = new System.Drawing.Size(150, 23);

            gridVehicles.Location = new System.Drawing.Point(20, 100);
            gridVehicles.Size = new System.Drawing.Size(950, 450);
            gridVehicles.Name = "gridVehicles";

            // Fuel Tab
            tabFuel.Controls.AddRange(new Control[] { lblFuelDate, txtFuelDate, lblFuelAmount, txtFuelAmount, gridFuel });
            tabFuel.Name = "tabFuel";
            tabFuel.Text = "Fuel";

            lblFuelDate.Text = "Fuel Date:";
            lblFuelDate.Location = new System.Drawing.Point(20, 20);
            lblFuelDate.Size = new System.Drawing.Size(100, 23);

            txtFuelDate.Location = new System.Drawing.Point(120, 20);
            txtFuelDate.Size = new System.Drawing.Size(150, 23);

            lblFuelAmount.Text = "Fuel Amount (gal):";
            lblFuelAmount.Location = new System.Drawing.Point(20, 50);
            lblFuelAmount.Size = new System.Drawing.Size(100, 23);

            txtFuelAmount.Location = new System.Drawing.Point(120, 50);
            txtFuelAmount.Size = new System.Drawing.Size(150, 23);

            gridFuel.Location = new System.Drawing.Point(20, 100);
            gridFuel.Size = new System.Drawing.Size(950, 450);
            gridFuel.Name = "gridFuel";

            // Maintenance Tab
            tabMaintenance.Controls.AddRange(new Control[] { lblMaintDate, txtMaintDate, lblMaintType, txtMaintType, gridMaintenance });
            tabMaintenance.Name = "tabMaintenance";
            tabMaintenance.Text = "Maintenance";

            lblMaintDate.Text = "Maint. Date:";
            lblMaintDate.Location = new System.Drawing.Point(20, 20);
            lblMaintDate.Size = new System.Drawing.Size(100, 23);

            txtMaintDate.Location = new System.Drawing.Point(120, 20);
            txtMaintDate.Size = new System.Drawing.Size(150, 23);

            lblMaintType.Text = "Maint. Type:";
            lblMaintType.Location = new System.Drawing.Point(20, 50);
            lblMaintType.Size = new System.Drawing.Size(100, 23);

            txtMaintType.Location = new System.Drawing.Point(120, 50);
            txtMaintType.Size = new System.Drawing.Size(150, 23);

            gridMaintenance.Location = new System.Drawing.Point(20, 100);
            gridMaintenance.Size = new System.Drawing.Size(950, 450);
            gridMaintenance.Name = "gridMaintenance";

            // Drivers Tab
            tabDrivers.Controls.AddRange(new Control[] { lblDriverName, txtDriverName, lblDriverLicense, txtDriverLicense, gridDrivers });
            tabDrivers.Name = "tabDrivers";
            tabDrivers.Text = "Drivers";

            lblDriverName.Text = "Driver Name:";
            lblDriverName.Location = new System.Drawing.Point(20, 20);
            lblDriverName.Size = new System.Drawing.Size(100, 23);

            txtDriverName.Location = new System.Drawing.Point(120, 20);
            txtDriverName.Size = new System.Drawing.Size(150, 23);

            lblDriverLicense.Text = "License No.:";
            lblDriverLicense.Location = new System.Drawing.Point(20, 50);
            lblDriverLicense.Size = new System.Drawing.Size(100, 23);

            txtDriverLicense.Location = new System.Drawing.Point(120, 50);
            txtDriverLicense.Size = new System.Drawing.Size(150, 23);

            gridDrivers.Location = new System.Drawing.Point(20, 100);
            gridDrivers.Size = new System.Drawing.Size(950, 450);
            gridDrivers.Name = "gridDrivers";

            // Form Setup
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1024, 650);
            Controls.Add(tabControl);
            Name = "Inputs";
            Text = "BusBuddy - Inputs";

            tabControl.ResumeLayout(false);
            tabTrips.ResumeLayout(false);
            tabVehicles.ResumeLayout(false);
            tabFuel.ResumeLayout(false);
            tabMaintenance.ResumeLayout(false);
            tabDrivers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridTrips).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridVehicles).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridFuel).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridMaintenance).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridDrivers).EndInit();
        }

        private TabControl tabControl;
        private TabPage tabTrips;
        private TabPage tabVehicles;
        private TabPage tabFuel;
        private TabPage tabMaintenance;
        private TabPage tabDrivers;

        // Trips Tab Controls
        private Label lblTripDate;
        private TextBox txtTripDate;
        private Label lblBusNumberTrips;
        private TextBox txtBusNumberTrips;
        private DataGridView gridTrips;

        // Vehicles Tab Controls
        private Label lblVehicleId;
        private TextBox txtVehicleId;
        private Label lblVehicleType;
        private TextBox txtVehicleType;
        private DataGridView gridVehicles;

        // Fuel Tab Controls
        private Label lblFuelDate;
        private TextBox txtFuelDate;
        private Label lblFuelAmount;
        private TextBox txtFuelAmount;
        private DataGridView gridFuel;

        // Maintenance Tab Controls
        private Label lblMaintDate;
        private TextBox txtMaintDate;
        private Label lblMaintType;
        private TextBox txtMaintType;
        private DataGridView gridMaintenance;

        // Drivers Tab Controls
        private Label lblDriverName;
        private TextBox txtDriverName;
        private Label lblDriverLicense;
        private TextBox txtDriverLicense;
        private DataGridView gridDrivers;
    }
}