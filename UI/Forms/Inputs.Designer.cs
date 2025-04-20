using System.Windows.Forms;

namespace BusBuddy.UI.Forms
{
    partial class Inputs
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tripsTabPage = new System.Windows.Forms.TabPage();
            this.vehiclesTabPage = new System.Windows.Forms.TabPage();
            this.fuelTabPage = new System.Windows.Forms.TabPage();
            this.driversTabPage = new System.Windows.Forms.TabPage();
            this.activitiesTabPage = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            // Trips Tab Controls
            this.tripsDataGridView = new System.Windows.Forms.DataGridView();
            this.tripTypeLabel = new System.Windows.Forms.Label();
            this.tripTypeComboBox = new System.Windows.Forms.ComboBox();
            this.tripDateLabel = new System.Windows.Forms.Label();
            this.tripDatePicker = new System.Windows.Forms.DateTimePicker();
            this.tripBusNumberLabel = new System.Windows.Forms.Label();
            this.tripBusNumberComboBox = new System.Windows.Forms.ComboBox();
            this.tripDriverNameLabel = new System.Windows.Forms.Label();
            this.tripDriverNameComboBox = new System.Windows.Forms.ComboBox();
            this.tripStartTimeLabel = new System.Windows.Forms.Label();
            this.tripStartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.tripEndTimeLabel = new System.Windows.Forms.Label();
            this.tripEndTimePicker = new System.Windows.Forms.DateTimePicker();
            this.tripDestinationLabel = new System.Windows.Forms.Label();
            this.tripDestinationTextBox = new System.Windows.Forms.TextBox();
            this.tripAddButton = new System.Windows.Forms.Button();
            this.tripClearButton = new System.Windows.Forms.Button();

            // Vehicles Tab Controls
            this.vehiclesDataGridView = new System.Windows.Forms.DataGridView();
            this.vehicleBusNumberLabel = new System.Windows.Forms.Label();
            this.vehicleBusNumberTextBox = new System.Windows.Forms.TextBox();
            this.vehicleAddButton = new System.Windows.Forms.Button();
            this.vehicleClearButton = new System.Windows.Forms.Button();

            // Fuel Tab Controls
            this.fuelDataGridView = new System.Windows.Forms.DataGridView();
            this.fuelBusNumberLabel = new System.Windows.Forms.Label();
            this.fuelBusNumberComboBox = new System.Windows.Forms.ComboBox();
            this.fuelGallonsLabel = new System.Windows.Forms.Label();
            this.fuelGallonsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fuelDateLabel = new System.Windows.Forms.Label();
            this.fuelDatePicker = new System.Windows.Forms.DateTimePicker();
            this.fuelTypeLabel = new System.Windows.Forms.Label();
            this.fuelTypeComboBox = new System.Windows.Forms.ComboBox();
            this.fuelOdometerLabel = new System.Windows.Forms.Label();
            this.fuelOdometerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.fuelAddButton = new System.Windows.Forms.Button();
            this.fuelClearButton = new System.Windows.Forms.Button();

            // Drivers Tab Controls
            this.driversDataGridView = new System.Windows.Forms.DataGridView();
            this.driverNameLabel = new System.Windows.Forms.Label();
            this.driverNameTextBox = new System.Windows.Forms.TextBox();
            this.driverAddressLabel = new System.Windows.Forms.Label();
            this.driverAddressTextBox = new System.Windows.Forms.TextBox();
            this.driverCityLabel = new System.Windows.Forms.Label();
            this.driverCityTextBox = new System.Windows.Forms.TextBox();
            this.driverStateLabel = new System.Windows.Forms.Label();
            this.driverStateTextBox = new System.Windows.Forms.TextBox();
            this.driverZipLabel = new System.Windows.Forms.Label();
            this.driverZipTextBox = new System.Windows.Forms.TextBox();
            this.driverPhoneLabel = new System.Windows.Forms.Label();
            this.driverPhoneTextBox = new System.Windows.Forms.TextBox();
            this.driverEmailLabel = new System.Windows.Forms.Label();
            this.driverEmailTextBox = new System.Windows.Forms.TextBox();
            this.driverStipendLabel = new System.Windows.Forms.Label();
            this.driverStipendComboBox = new System.Windows.Forms.ComboBox();
            this.driverDLTypeLabel = new System.Windows.Forms.Label();
            this.driverDLTypeComboBox = new System.Windows.Forms.ComboBox();
            this.driverAddButton = new System.Windows.Forms.Button();
            this.driverClearButton = new System.Windows.Forms.Button();

            // Activities Tab Controls
            this.maintenanceDataGridView = new System.Windows.Forms.DataGridView();
            this.activityDateLabel = new System.Windows.Forms.Label();
            this.activityDatePicker = new System.Windows.Forms.DateTimePicker();
            this.activityBusNumberLabel = new System.Windows.Forms.Label();
            this.activityBusNumberComboBox = new System.Windows.Forms.ComboBox();
            this.activityDestinationLabel = new System.Windows.Forms.Label();
            this.activityDestinationTextBox = new System.Windows.Forms.TextBox();
            this.activityLeaveTimeLabel = new System.Windows.Forms.Label();
            this.activityLeaveTimePicker = new System.Windows.Forms.DateTimePicker();
            this.activityDriverLabel = new System.Windows.Forms.Label();
            this.activityDriverComboBox = new System.Windows.Forms.ComboBox();
            this.activityHoursDrivenLabel = new System.Windows.Forms.Label();
            this.activityHoursDrivenTextBox = new System.Windows.Forms.TextBox();
            this.activityStudentsLabel = new System.Windows.Forms.Label();
            this.activityStudentsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.maintenanceAddButton = new System.Windows.Forms.Button();
            this.maintenanceClearButton = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.tripsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vehiclesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelGallonsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelOdometerNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityStudentsNumericUpDown)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tripsTabPage.SuspendLayout();
            this.vehiclesTabPage.SuspendLayout();
            this.fuelTabPage.SuspendLayout();
            this.driversTabPage.SuspendLayout();
            this.activitiesTabPage.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // TabControl
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.Size = new System.Drawing.Size(860, 500);
            this.tabControl.TabIndex = 0;
            this.tabControl.Controls.Add(this.tripsTabPage);
            this.tabControl.Controls.Add(this.vehiclesTabPage);
            this.tabControl.Controls.Add(this.fuelTabPage);
            this.tabControl.Controls.Add(this.driversTabPage);
            this.tabControl.Controls.Add(this.activitiesTabPage);

            // Trips TabPage
            this.tripsTabPage.Text = "Trips";
            this.tripsTabPage.Controls.Add(this.tripsDataGridView);
            this.tripsTabPage.Controls.Add(this.tripTypeLabel);
            this.tripsTabPage.Controls.Add(this.tripTypeComboBox);
            this.tripsTabPage.Controls.Add(this.tripDateLabel);
            this.tripsTabPage.Controls.Add(this.tripDatePicker);
            this.tripsTabPage.Controls.Add(this.tripBusNumberLabel);
            this.tripsTabPage.Controls.Add(this.tripBusNumberComboBox);
            this.tripsTabPage.Controls.Add(this.tripDriverNameLabel);
            this.tripsTabPage.Controls.Add(this.tripDriverNameComboBox);
            this.tripsTabPage.Controls.Add(this.tripStartTimeLabel);
            this.tripsTabPage.Controls.Add(this.tripStartTimePicker);
            this.tripsTabPage.Controls.Add(this.tripEndTimeLabel);
            this.tripsTabPage.Controls.Add(this.tripEndTimePicker);
            this.tripsTabPage.Controls.Add(this.tripDestinationLabel);
            this.tripsTabPage.Controls.Add(this.tripDestinationTextBox);
            this.tripsTabPage.Controls.Add(this.tripAddButton);
            this.tripsTabPage.Controls.Add(this.tripClearButton);

            this.tripsDataGridView.Location = new System.Drawing.Point(10, 10);
            this.tripsDataGridView.Size = new System.Drawing.Size(840, 200);
            this.tripsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tripsDataGridView.ReadOnly = true;
            this.tripsDataGridView.AllowUserToAddRows = false;
            this.tripsDataGridView.Name = "tripsDataGridView";
            this.tripsDataGridView.Columns.Add("TripID", "TripID");
            this.tripsDataGridView.Columns.Add("TripType", "TripType");
            this.tripsDataGridView.Columns.Add("Date", "Date");
            this.tripsDataGridView.Columns.Add("BusNumber", "BusNumber");
            this.tripsDataGridView.Columns.Add("DriverName", "DriverName");
            this.tripsDataGridView.Columns.Add("StartTime", "StartTime");
            this.tripsDataGridView.Columns.Add("EndTime", "EndTime");

            this.tripTypeLabel.Text = "Trip Type:";
            this.tripTypeLabel.Location = new System.Drawing.Point(10, 220);
            this.tripTypeLabel.AutoSize = true;
            this.tripTypeLabel.Font = AppSettings.Theme.LabelFont;
            this.tripTypeComboBox.Location = new System.Drawing.Point(120, 217);
            this.tripTypeComboBox.Size = new System.Drawing.Size(150, 28);
            this.tripTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tripTypeComboBox.Items.AddRange(new object[] { "Route", "Activity" });
            this.tripTypeComboBox.Name = "tripTypeComboBox";

            this.tripDateLabel.Text = "Date:";
            this.tripDateLabel.Location = new System.Drawing.Point(10, 250);
            this.tripDateLabel.AutoSize = true;
            this.tripDateLabel.Font = AppSettings.Theme.LabelFont;
            this.tripDatePicker.Location = new System.Drawing.Point(120, 247);
            this.tripDatePicker.Size = new System.Drawing.Size(150, 28);
            this.tripDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tripDatePicker.Name = "tripDatePicker";

            this.tripBusNumberLabel.Text = "Bus Number:";
            this.tripBusNumberLabel.Location = new System.Drawing.Point(10, 280);
            this.tripBusNumberLabel.AutoSize = true;
            this.tripBusNumberLabel.Font = AppSettings.Theme.LabelFont;
            this.tripBusNumberComboBox.Location = new System.Drawing.Point(120, 277);
            this.tripBusNumberComboBox.Size = new System.Drawing.Size(150, 28);
            this.tripBusNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tripBusNumberComboBox.Name = "tripBusNumberComboBox";

            this.tripDriverNameLabel.Text = "Driver Name:";
            this.tripDriverNameLabel.Location = new System.Drawing.Point(10, 310);
            this.tripDriverNameLabel.AutoSize = true;
            this.tripDriverNameLabel.Font = AppSettings.Theme.LabelFont;
            this.tripDriverNameComboBox.Location = new System.Drawing.Point(120, 307);
            this.tripDriverNameComboBox.Size = new System.Drawing.Size(150, 28);
            this.tripDriverNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tripDriverNameComboBox.Name = "tripDriverNameComboBox";

            this.tripStartTimeLabel.Text = "Start Time:";
            this.tripStartTimeLabel.Location = new System.Drawing.Point(10, 340);
            this.tripStartTimeLabel.AutoSize = true;
            this.tripStartTimeLabel.Font = AppSettings.Theme.LabelFont;
            this.tripStartTimePicker.Location = new System.Drawing.Point(120, 337);
            this.tripStartTimePicker.Size = new System.Drawing.Size(150, 28);
            this.tripStartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tripStartTimePicker.ShowUpDown = true;
            this.tripStartTimePicker.Name = "tripStartTimePicker";

            this.tripEndTimeLabel.Text = "End Time:";
            this.tripEndTimeLabel.Location = new System.Drawing.Point(10, 370);
            this.tripEndTimeLabel.AutoSize = true;
            this.tripEndTimeLabel.Font = AppSettings.Theme.LabelFont;
            this.tripEndTimePicker.Location = new System.Drawing.Point(120, 367);
            this.tripEndTimePicker.Size = new System.Drawing.Size(150, 28);
            this.tripEndTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tripEndTimePicker.ShowUpDown = true;
            this.tripEndTimePicker.Name = "tripEndTimePicker";

            this.tripDestinationLabel.Text = "Destination:";
            this.tripDestinationLabel.Location = new System.Drawing.Point(10, 400);
            this.tripDestinationLabel.AutoSize = true;
            this.tripDestinationLabel.Font = AppSettings.Theme.LabelFont;
            this.tripDestinationTextBox.Location = new System.Drawing.Point(120, 397);
            this.tripDestinationTextBox.Size = new System.Drawing.Size(150, 28);
            this.tripDestinationTextBox.Name = "tripDestinationTextBox";

            this.tripAddButton.Text = "Add";
            this.tripAddButton.Location = new System.Drawing.Point(120, 427);
            this.tripAddButton.Size = new System.Drawing.Size(100, 35);
            this.tripAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.tripAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.tripAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tripAddButton.FlatAppearance.BorderSize = 2;
            this.tripAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.tripAddButton.Font = AppSettings.Theme.ButtonFont;
            this.tripAddButton.Name = "tripAddButton";
            this.tripAddButton.Click += new System.EventHandler(this.TripAddButton_Click);

            this.tripClearButton.Text = "Clear";
            this.tripClearButton.Location = new System.Drawing.Point(230, 427);
            this.tripClearButton.Size = new System.Drawing.Size(100, 35);
            this.tripClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.tripClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.tripClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tripClearButton.FlatAppearance.BorderSize = 2;
            this.tripClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.tripClearButton.Font = AppSettings.Theme.ButtonFont;
            this.tripClearButton.Name = "tripClearButton";
            this.tripClearButton.Click += new System.EventHandler(this.TripClearButton_Click);

            // Vehicles TabPage
            this.vehiclesTabPage.Text = "Vehicles";
            this.vehiclesTabPage.Controls.Add(this.vehiclesDataGridView);
            this.vehiclesTabPage.Controls.Add(this.vehicleBusNumberLabel);
            this.vehiclesTabPage.Controls.Add(this.vehicleBusNumberTextBox);
            this.vehiclesTabPage.Controls.Add(this.vehicleAddButton);
            this.vehiclesTabPage.Controls.Add(this.vehicleClearButton);

            this.vehiclesDataGridView.Location = new System.Drawing.Point(10, 10);
            this.vehiclesDataGridView.Size = new System.Drawing.Size(840, 200);
            this.vehiclesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.vehiclesDataGridView.ReadOnly = true;
            this.vehiclesDataGridView.AllowUserToAddRows = false;
            this.vehiclesDataGridView.Name = "vehiclesDataGridView";
            this.vehiclesDataGridView.Columns.Add("BusNumber", "Bus Number");

            this.vehicleBusNumberLabel.Text = "Bus Number:";
            this.vehicleBusNumberLabel.Location = new System.Drawing.Point(10, 220);
            this.vehicleBusNumberLabel.AutoSize = true;
            this.vehicleBusNumberLabel.Font = AppSettings.Theme.LabelFont;
            this.vehicleBusNumberTextBox.Location = new System.Drawing.Point(120, 217);
            this.vehicleBusNumberTextBox.Size = new System.Drawing.Size(150, 28);
            this.vehicleBusNumberTextBox.Name = "vehicleBusNumberTextBox";

            this.vehicleAddButton.Text = "Add";
            this.vehicleAddButton.Location = new System.Drawing.Point(120, 247);
            this.vehicleAddButton.Size = new System.Drawing.Size(100, 35);
            this.vehicleAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.vehicleAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.vehicleAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vehicleAddButton.FlatAppearance.BorderSize = 2;
            this.vehicleAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.vehicleAddButton.Font = AppSettings.Theme.ButtonFont;
            this.vehicleAddButton.Name = "vehicleAddButton";
            this.vehicleAddButton.Click += new System.EventHandler(this.VehicleAddButton_Click);

            this.vehicleClearButton.Text = "Clear";
            this.vehicleClearButton.Location = new System.Drawing.Point(230, 247);
            this.vehicleClearButton.Size = new System.Drawing.Size(100, 35);
            this.vehicleClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.vehicleClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.vehicleClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vehicleClearButton.FlatAppearance.BorderSize = 2;
            this.vehicleClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.vehicleClearButton.Font = AppSettings.Theme.ButtonFont;
            this.vehicleClearButton.Name = "vehicleClearButton";
            this.vehicleClearButton.Click += new System.EventHandler(this.VehicleClearButton_Click);

            // Fuel TabPage
            this.fuelTabPage.Text = "Fuel";
            this.fuelTabPage.Controls.Add(this.fuelDataGridView);
            this.fuelTabPage.Controls.Add(this.fuelBusNumberLabel);
            this.fuelTabPage.Controls.Add(this.fuelBusNumberComboBox);
            this.fuelTabPage.Controls.Add(this.fuelGallonsLabel);
            this.fuelTabPage.Controls.Add(this.fuelGallonsNumericUpDown);
            this.fuelTabPage.Controls.Add(this.fuelDateLabel);
            this.fuelTabPage.Controls.Add(this.fuelDatePicker);
            this.fuelTabPage.Controls.Add(this.fuelTypeLabel);
            this.fuelTabPage.Controls.Add(this.fuelTypeComboBox);
            this.fuelTabPage.Controls.Add(this.fuelOdometerLabel);
            this.fuelTabPage.Controls.Add(this.fuelOdometerNumericUpDown);
            this.fuelTabPage.Controls.Add(this.fuelAddButton);
            this.fuelTabPage.Controls.Add(this.fuelClearButton);

            this.fuelDataGridView.Location = new System.Drawing.Point(10, 10);
            this.fuelDataGridView.Size = new System.Drawing.Size(840, 200);
            this.fuelDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.fuelDataGridView.ReadOnly = true;
            this.fuelDataGridView.AllowUserToAddRows = false;
            this.fuelDataGridView.Name = "fuelDataGridView";
            this.fuelDataGridView.Columns.Add("FuelID", "Fuel ID");
            this.fuelDataGridView.Columns.Add("BusNumber", "Bus Number");
            this.fuelDataGridView.Columns.Add("FuelGallons", "Fuel Gallons");
            this.fuelDataGridView.Columns.Add("FuelDate", "Fuel Date");
            this.fuelDataGridView.Columns.Add("FuelType", "Fuel Type");
            this.fuelDataGridView.Columns.Add("OdometerReading", "Odometer Reading");

            this.fuelBusNumberLabel.Text = "Bus Number:";
            this.fuelBusNumberLabel.Location = new System.Drawing.Point(10, 220);
            this.fuelBusNumberLabel.AutoSize = true;
            this.fuelBusNumberLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelBusNumberComboBox.Location = new System.Drawing.Point(120, 217);
            this.fuelBusNumberComboBox.Size = new System.Drawing.Size(150, 28);
            this.fuelBusNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelBusNumberComboBox.Name = "fuelBusNumberComboBox";

            this.fuelGallonsLabel.Text = "Fuel Gallons:";
            this.fuelGallonsLabel.Location = new System.Drawing.Point(10, 250);
            this.fuelGallonsLabel.AutoSize = true;
            this.fuelGallonsLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelGallonsNumericUpDown.Location = new System.Drawing.Point(120, 247);
            this.fuelGallonsNumericUpDown.Size = new System.Drawing.Size(150, 28);
            this.fuelGallonsNumericUpDown.Minimum = 0;
            this.fuelGallonsNumericUpDown.Maximum = 1000;
            this.fuelGallonsNumericUpDown.Name = "fuelGallonsNumericUpDown";

            this.fuelDateLabel.Text = "Fuel Date:";
            this.fuelDateLabel.Location = new System.Drawing.Point(10, 280);
            this.fuelDateLabel.AutoSize = true;
            this.fuelDateLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelDatePicker.Location = new System.Drawing.Point(120, 277);
            this.fuelDatePicker.Size = new System.Drawing.Size(150, 28);
            this.fuelDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fuelDatePicker.Name = "fuelDatePicker";

            this.fuelTypeLabel.Text = "Fuel Type:";
            this.fuelTypeLabel.Location = new System.Drawing.Point(10, 310);
            this.fuelTypeLabel.AutoSize = true;
            this.fuelTypeLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelTypeComboBox.Location = new System.Drawing.Point(120, 307);
            this.fuelTypeComboBox.Size = new System.Drawing.Size(150, 28);
            this.fuelTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelTypeComboBox.Items.AddRange(new object[] { "Diesel", "Gasoline" });
            this.fuelTypeComboBox.Name = "fuelTypeComboBox";

            this.fuelOdometerLabel.Text = "Odometer Reading:";
            this.fuelOdometerLabel.Location = new System.Drawing.Point(10, 340);
            this.fuelOdometerLabel.AutoSize = true;
            this.fuelOdometerLabel.Font = AppSettings.Theme.LabelFont;
            this.fuelOdometerNumericUpDown.Location = new System.Drawing.Point(120, 337);
            this.fuelOdometerNumericUpDown.Size = new System.Drawing.Size(150, 28);
            this.fuelOdometerNumericUpDown.Minimum = 0;
            this.fuelOdometerNumericUpDown.Maximum = 1000000;
            this.fuelOdometerNumericUpDown.Name = "fuelOdometerNumericUpDown";

            this.fuelAddButton.Text = "Add";
            this.fuelAddButton.Location = new System.Drawing.Point(120, 367);
            this.fuelAddButton.Size = new System.Drawing.Size(100, 35);
            this.fuelAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.fuelAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.fuelAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelAddButton.FlatAppearance.BorderSize = 2;
            this.fuelAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.fuelAddButton.Font = AppSettings.Theme.ButtonFont;
            this.fuelAddButton.Name = "fuelAddButton";
            this.fuelAddButton.Click += new System.EventHandler(this.FuelAddButton_Click);

            this.fuelClearButton.Text = "Clear";
            this.fuelClearButton.Location = new System.Drawing.Point(230, 367);
            this.fuelClearButton.Size = new System.Drawing.Size(100, 35);
            this.fuelClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.fuelClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.fuelClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fuelClearButton.FlatAppearance.BorderSize = 2;
            this.fuelClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.fuelClearButton.Font = AppSettings.Theme.ButtonFont;
            this.fuelClearButton.Name = "fuelClearButton";
            this.fuelClearButton.Click += new System.EventHandler(this.FuelClearButton_Click);

            // Drivers TabPage
            this.driversTabPage.Text = "Drivers";
            this.driversTabPage.Controls.Add(this.driversDataGridView);
            this.driversTabPage.Controls.Add(this.driverNameLabel);
            this.driversTabPage.Controls.Add(this.driverNameTextBox);
            this.driversTabPage.Controls.Add(this.driverAddressLabel);
            this.driversTabPage.Controls.Add(this.driverAddressTextBox);
            this.driversTabPage.Controls.Add(this.driverCityLabel);
            this.driversTabPage.Controls.Add(this.driverCityTextBox);
            this.driversTabPage.Controls.Add(this.driverStateLabel);
            this.driversTabPage.Controls.Add(this.driverStateTextBox);
            this.driversTabPage.Controls.Add(this.driverZipLabel);
            this.driversTabPage.Controls.Add(this.driverZipTextBox);
            this.driversTabPage.Controls.Add(this.driverPhoneLabel);
            this.driversTabPage.Controls.Add(this.driverPhoneTextBox);
            this.driversTabPage.Controls.Add(this.driverEmailLabel);
            this.driversTabPage.Controls.Add(this.driverEmailTextBox);
            this.driversTabPage.Controls.Add(this.driverStipendLabel);
            this.driversTabPage.Controls.Add(this.driverStipendComboBox);
            this.driversTabPage.Controls.Add(this.driverDLTypeLabel);
            this.driversTabPage.Controls.Add(this.driverDLTypeComboBox);
            this.driversTabPage.Controls.Add(this.driverAddButton);
            this.driversTabPage.Controls.Add(this.driverClearButton);

            this.driversDataGridView.Location = new System.Drawing.Point(10, 10);
            this.driversDataGridView.Size = new System.Drawing.Size(840, 200);
            this.driversDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.driversDataGridView.ReadOnly = true;
            this.driversDataGridView.AllowUserToAddRows = false;
            this.driversDataGridView.Name = "driversDataGridView";
            this.driversDataGridView.Columns.Add("DriverID", "Driver ID");
            this.driversDataGridView.Columns.Add("DriverName", "Driver Name");
            this.driversDataGridView.Columns.Add("Address", "Address");
            this.driversDataGridView.Columns.Add("City", "City");
            this.driversDataGridView.Columns.Add("State", "State");
            this.driversDataGridView.Columns.Add("ZipCode", "Zip Code");
            this.driversDataGridView.Columns.Add("PhoneNumber", "Phone Number");
            this.driversDataGridView.Columns.Add("EmailAddress", "Email Address");
            this.driversDataGridView.Columns.Add("IsStipendPaid", "Is Stipend Paid");
            this.driversDataGridView.Columns.Add("DLType", "DL Type");

            this.driverNameLabel.Text = "Driver Name:";
            this.driverNameLabel.Location = new System.Drawing.Point(10, 220);
            this.driverNameLabel.AutoSize = true;
            this.driverNameLabel.Font = AppSettings.Theme.LabelFont;
            this.driverNameTextBox.Location = new System.Drawing.Point(120, 217);
            this.driverNameTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverNameTextBox.Name = "driverNameTextBox";

            this.driverAddressLabel.Text = "Address:";
            this.driverAddressLabel.Location = new System.Drawing.Point(10, 250);
            this.driverAddressLabel.AutoSize = true;
            this.driverAddressLabel.Font = AppSettings.Theme.LabelFont;
            this.driverAddressTextBox.Location = new System.Drawing.Point(120, 247);
            this.driverAddressTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverAddressTextBox.Name = "driverAddressTextBox";

            this.driverCityLabel.Text = "City:";
            this.driverCityLabel.Location = new System.Drawing.Point(10, 280);
            this.driverCityLabel.AutoSize = true;
            this.driverCityLabel.Font = AppSettings.Theme.LabelFont;
            this.driverCityTextBox.Location = new System.Drawing.Point(120, 277);
            this.driverCityTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverCityTextBox.Name = "driverCityTextBox";

            this.driverStateLabel.Text = "State:";
            this.driverStateLabel.Location = new System.Drawing.Point(10, 310);
            this.driverStateLabel.AutoSize = true;
            this.driverStateLabel.Font = AppSettings.Theme.LabelFont;
            this.driverStateTextBox.Location = new System.Drawing.Point(120, 307);
            this.driverStateTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverStateTextBox.Name = "driverStateTextBox";

            this.driverZipLabel.Text = "Zip Code:";
            this.driverZipLabel.Location = new System.Drawing.Point(10, 340);
            this.driverZipLabel.AutoSize = true;
            this.driverZipLabel.Font = AppSettings.Theme.LabelFont;
            this.driverZipTextBox.Location = new System.Drawing.Point(120, 337);
            this.driverZipTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverZipTextBox.Name = "driverZipTextBox";

            this.driverPhoneLabel.Text = "Phone Number:";
            this.driverPhoneLabel.Location = new System.Drawing.Point(10, 370);
            this.driverPhoneLabel.AutoSize = true;
            this.driverPhoneLabel.Font = AppSettings.Theme.LabelFont;
            this.driverPhoneTextBox.Location = new System.Drawing.Point(120, 367);
            this.driverPhoneTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverPhoneTextBox.Name = "driverPhoneTextBox";

            this.driverEmailLabel.Text = "Email Address:";
            this.driverEmailLabel.Location = new System.Drawing.Point(10, 400);
            this.driverEmailLabel.AutoSize = true;
            this.driverEmailLabel.Font = AppSettings.Theme.LabelFont;
            this.driverEmailTextBox.Location = new System.Drawing.Point(120, 397);
            this.driverEmailTextBox.Size = new System.Drawing.Size(150, 28);
            this.driverEmailTextBox.Name = "driverEmailTextBox";

            this.driverStipendLabel.Text = "Is Stipend Paid:";
            this.driverStipendLabel.Location = new System.Drawing.Point(10, 430);
            this.driverStipendLabel.AutoSize = true;
            this.driverStipendLabel.Font = AppSettings.Theme.LabelFont;
            this.driverStipendComboBox.Location = new System.Drawing.Point(120, 427);
            this.driverStipendComboBox.Size = new System.Drawing.Size(150, 28);
            this.driverStipendComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverStipendComboBox.Items.AddRange(new object[] { "Yes", "No" });
            this.driverStipendComboBox.Name = "driverStipendComboBox";

            this.driverDLTypeLabel.Text = "DL Type:";
            this.driverDLTypeLabel.Location = new System.Drawing.Point(10, 460);
            this.driverDLTypeLabel.AutoSize = true;
            this.driverDLTypeLabel.Font = AppSettings.Theme.LabelFont;
            this.driverDLTypeComboBox.Location = new System.Drawing.Point(120, 457);
            this.driverDLTypeComboBox.Size = new System.Drawing.Size(150, 28);
            this.driverDLTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.driverDLTypeComboBox.Items.AddRange(new object[] { "CDL", "Regular" });
            this.driverDLTypeComboBox.Name = "driverDLTypeComboBox";

            this.driverAddButton.Text = "Add";
            this.driverAddButton.Location = new System.Drawing.Point(120, 487);
            this.driverAddButton.Size = new System.Drawing.Size(100, 35);
            this.driverAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.driverAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.driverAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverAddButton.FlatAppearance.BorderSize = 2;
            this.driverAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.driverAddButton.Font = AppSettings.Theme.ButtonFont;
            this.driverAddButton.Name = "driverAddButton";
            this.driverAddButton.Click += new System.EventHandler(this.DriverAddButton_Click);

            this.driverClearButton.Text = "Clear";
            this.driverClearButton.Location = new System.Drawing.Point(230, 487);
            this.driverClearButton.Size = new System.Drawing.Size(100, 35);
            this.driverClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.driverClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.driverClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.driverClearButton.FlatAppearance.BorderSize = 2;
            this.driverClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.driverClearButton.Font = AppSettings.Theme.ButtonFont;
            this.driverClearButton.Name = "driverClearButton";
            this.driverClearButton.Click += new System.EventHandler(this.DriverClearButton_Click);

            // Activities TabPage
            this.activitiesTabPage.Text = "Activities";
            this.activitiesTabPage.Controls.Add(this.maintenanceDataGridView);
            this.activitiesTabPage.Controls.Add(this.activityDateLabel);
            this.activitiesTabPage.Controls.Add(this.activityDatePicker);
            this.activitiesTabPage.Controls.Add(this.activityBusNumberLabel);
            this.activitiesTabPage.Controls.Add(this.activityBusNumberComboBox);
            this.activitiesTabPage.Controls.Add(this.activityDestinationLabel);
            this.activitiesTabPage.Controls.Add(this.activityDestinationTextBox);
            this.activitiesTabPage.Controls.Add(this.activityLeaveTimeLabel);
            this.activitiesTabPage.Controls.Add(this.activityLeaveTimePicker);
            this.activitiesTabPage.Controls.Add(this.activityDriverLabel);
            this.activitiesTabPage.Controls.Add(this.activityDriverComboBox);
            this.activitiesTabPage.Controls.Add(this.activityHoursDrivenLabel);
            this.activitiesTabPage.Controls.Add(this.activityHoursDrivenTextBox);
            this.activitiesTabPage.Controls.Add(this.activityStudentsLabel);
            this.activitiesTabPage.Controls.Add(this.activityStudentsNumericUpDown);
            this.activitiesTabPage.Controls.Add(this.maintenanceAddButton);
            this.activitiesTabPage.Controls.Add(this.maintenanceClearButton);

            this.maintenanceDataGridView.Location = new System.Drawing.Point(10, 10);
            this.maintenanceDataGridView.Size = new System.Drawing.Size(840, 200);
            this.maintenanceDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.maintenanceDataGridView.ReadOnly = true;
            this.maintenanceDataGridView.AllowUserToAddRows = false;
            this.maintenanceDataGridView.Name = "maintenanceDataGridView";
            this.maintenanceDataGridView.Columns.Add("ActivityID", "Activity ID");
            this.maintenanceDataGridView.Columns.Add("Date", "Date");
            this.maintenanceDataGridView.Columns.Add("BusNumber", "Bus Number");
            this.maintenanceDataGridView.Columns.Add("Destination", "Destination");
            this.maintenanceDataGridView.Columns.Add("LeaveTime", "Leave Time");
            this.maintenanceDataGridView.Columns.Add("Driver", "Driver");
            this.maintenanceDataGridView.Columns.Add("HoursDriven", "Hours Driven");
            this.maintenanceDataGridView.Columns.Add("StudentsDriven", "Students Driven");

            this.activityDateLabel.Text = "Date:";
            this.activityDateLabel.Location = new System.Drawing.Point(10, 220);
            this.activityDateLabel.AutoSize = true;
            this.activityDateLabel.Font = AppSettings.Theme.LabelFont;
            this.activityDatePicker.Location = new System.Drawing.Point(120, 217);
            this.activityDatePicker.Size = new System.Drawing.Size(150, 28);
            this.activityDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.activityDatePicker.Name = "activityDatePicker";

            this.activityBusNumberLabel.Text = "Bus Number:";
            this.activityBusNumberLabel.Location = new System.Drawing.Point(10, 250);
            this.activityBusNumberLabel.AutoSize = true;
            this.activityBusNumberLabel.Font = AppSettings.Theme.LabelFont;
            this.activityBusNumberComboBox.Location = new System.Drawing.Point(120, 247);
            this.activityBusNumberComboBox.Size = new System.Drawing.Size(150, 28);
            this.activityBusNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activityBusNumberComboBox.Name = "activityBusNumberComboBox";

            this.activityDestinationLabel.Text = "Destination:";
            this.activityDestinationLabel.Location = new System.Drawing.Point(10, 280);
            this.activityDestinationLabel.AutoSize = true;
            this.activityDestinationLabel.Font = AppSettings.Theme.LabelFont;
            this.activityDestinationTextBox.Location = new System.Drawing.Point(120, 277);
            this.activityDestinationTextBox.Size = new System.Drawing.Size(150, 28);
            this.activityDestinationTextBox.Name = "activityDestinationTextBox";

            this.activityLeaveTimeLabel.Text = "Leave Time:";
            this.activityLeaveTimeLabel.Location = new System.Drawing.Point(10, 310);
            this.activityLeaveTimeLabel.AutoSize = true;
            this.activityLeaveTimeLabel.Font = AppSettings.Theme.LabelFont;
            this.activityLeaveTimePicker.Location = new System.Drawing.Point(120, 307);
            this.activityLeaveTimePicker.Size = new System.Drawing.Size(150, 28);
            this.activityLeaveTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.activityLeaveTimePicker.ShowUpDown = true;
            this.activityLeaveTimePicker.Name = "activityLeaveTimePicker";

            this.activityDriverLabel.Text = "Driver:";
            this.activityDriverLabel.Location = new System.Drawing.Point(10, 340);
            this.activityDriverLabel.AutoSize = true;
            this.activityDriverLabel.Font = AppSettings.Theme.LabelFont;
            this.activityDriverComboBox.Location = new System.Drawing.Point(120, 337);
            this.activityDriverComboBox.Size = new System.Drawing.Size(150, 28);
            this.activityDriverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activityDriverComboBox.Name = "activityDriverComboBox";

            this.activityHoursDrivenLabel.Text = "Hours Driven:";
            this.activityHoursDrivenLabel.Location = new System.Drawing.Point(10, 370);
            this.activityHoursDrivenLabel.AutoSize = true;
            this.activityHoursDrivenLabel.Font = AppSettings.Theme.LabelFont;
            this.activityHoursDrivenTextBox.Location = new System.Drawing.Point(120, 367);
            this.activityHoursDrivenTextBox.Size = new System.Drawing.Size(150, 28);
            this.activityHoursDrivenTextBox.Name = "activityHoursDrivenTextBox";

            this.activityStudentsLabel.Text = "Students Driven:";
            this.activityStudentsLabel.Location = new System.Drawing.Point(10, 400);
            this.activityStudentsLabel.AutoSize = true;
            this.activityStudentsLabel.Font = AppSettings.Theme.LabelFont;
            this.activityStudentsNumericUpDown.Location = new System.Drawing.Point(120, 397);
            this.activityStudentsNumericUpDown.Size = new System.Drawing.Size(150, 28);
            this.activityStudentsNumericUpDown.Minimum = 0;
            this.activityStudentsNumericUpDown.Maximum = 100;
            this.activityStudentsNumericUpDown.Name = "activityStudentsNumericUpDown";

            this.maintenanceAddButton.Text = "Add";
            this.maintenanceAddButton.Location = new System.Drawing.Point(120, 427);
            this.maintenanceAddButton.Size = new System.Drawing.Size(100, 35);
            this.maintenanceAddButton.BackColor = AppSettings.Theme.SuccessColor;
            this.maintenanceAddButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.maintenanceAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maintenanceAddButton.FlatAppearance.BorderSize = 2;
            this.maintenanceAddButton.FlatAppearance.BorderColor = AppSettings.Theme.SuccessColor;
            this.maintenanceAddButton.Font = AppSettings.Theme.ButtonFont;
            this.maintenanceAddButton.Name = "maintenanceAddButton";
            this.maintenanceAddButton.Click += new System.EventHandler(this.MaintenanceAddButton_Click);

            this.maintenanceClearButton.Text = "Clear";
            this.maintenanceClearButton.Location = new System.Drawing.Point(230, 427);
            this.maintenanceClearButton.Size = new System.Drawing.Size(100, 35);
            this.maintenanceClearButton.BackColor = AppSettings.Theme.InfoColor;
            this.maintenanceClearButton.ForeColor = AppSettings.Theme.TextLightColor;
            this.maintenanceClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maintenanceClearButton.FlatAppearance.BorderSize = 2;
            this.maintenanceClearButton.FlatAppearance.BorderColor = AppSettings.Theme.InfoColor;
            this.maintenanceClearButton.Font = AppSettings.Theme.ButtonFont;
            this.maintenanceClearButton.Name = "maintenanceClearButton";
            this.maintenanceClearButton.Click += new System.EventHandler(this.MaintenanceClearButton_Click);

            // Status Strip
            this.statusStrip.Location = new System.Drawing.Point(0, 522);
            this.statusStrip.Size = new System.Drawing.Size(884, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });

            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Text = "Ready.";

            // Form Properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 544);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Name = "Inputs";
            this.Text = "Data Entry - BusBuddy";

            ((System.ComponentModel.ISupportInitialize)(this.tripsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vehiclesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.driversDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maintenanceDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelGallonsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelOdometerNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.activityStudentsNumericUpDown)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tripsTabPage.ResumeLayout(false);
            this.tripsTabPage.PerformLayout();
            this.vehiclesTabPage.ResumeLayout(false);
            this.vehiclesTabPage.PerformLayout();
            this.fuelTabPage.ResumeLayout(false);
            this.fuelTabPage.PerformLayout();
            this.driversTabPage.ResumeLayout(false);
            this.driversTabPage.PerformLayout();
            this.activitiesTabPage.ResumeLayout(false);
            this.activitiesTabPage.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tripsTabPage;
        private System.Windows.Forms.TabPage vehiclesTabPage;
        private System.Windows.Forms.TabPage fuelTabPage;
        private System.Windows.Forms.TabPage driversTabPage;
        private System.Windows.Forms.TabPage activitiesTabPage;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

        // Trips Tab Controls
        private System.Windows.Forms.DataGridView tripsDataGridView;
        private System.Windows.Forms.Label tripTypeLabel;
        private System.Windows.Forms.ComboBox tripTypeComboBox;
        private System.Windows.Forms.Label tripDateLabel;
        private System.Windows.Forms.DateTimePicker tripDatePicker;
        private System.Windows.Forms.Label tripBusNumberLabel;
        private System.Windows.Forms.ComboBox tripBusNumberComboBox;
        private System.Windows.Forms.Label tripDriverNameLabel;
        private System.Windows.Forms.ComboBox tripDriverNameComboBox;
        private System.Windows.Forms.Label tripStartTimeLabel;
        private System.Windows.Forms.DateTimePicker tripStartTimePicker;
        private System.Windows.Forms.Label tripEndTimeLabel;
        private System.Windows.Forms.DateTimePicker tripEndTimePicker;
        private System.Windows.Forms.Label tripDestinationLabel;
        private System.Windows.Forms.TextBox tripDestinationTextBox;
        private System.Windows.Forms.Button tripAddButton;
        private System.Windows.Forms.Button tripClearButton;

        // Vehicles Tab Controls
        private System.Windows.Forms.DataGridView vehiclesDataGridView;
        private System.Windows.Forms.Label vehicleBusNumberLabel;
        private System.Windows.Forms.TextBox vehicleBusNumberTextBox;
        private System.Windows.Forms.Button vehicleAddButton;
        private System.Windows.Forms.Button vehicleClearButton;

        // Fuel Tab Controls
        private System.Windows.Forms.DataGridView fuelDataGridView;
        private System.Windows.Forms.Label fuelBusNumberLabel;
        private System.Windows.Forms.ComboBox fuelBusNumberComboBox;
        private System.Windows.Forms.Label fuelGallonsLabel;
        private System.Windows.Forms.NumericUpDown fuelGallonsNumericUpDown;
        private System.Windows.Forms.Label fuelDateLabel;
        private System.Windows.Forms.DateTimePicker fuelDatePicker;
        private System.Windows.Forms.Label fuelTypeLabel;
        private System.Windows.Forms.ComboBox fuelTypeComboBox;
        private System.Windows.Forms.Label fuelOdometerLabel;
        private System.Windows.Forms.NumericUpDown fuelOdometerNumericUpDown;
        private System.Windows.Forms.Button fuelAddButton;
        private System.Windows.Forms.Button fuelClearButton;

        // Drivers Tab Controls
        private System.Windows.Forms.DataGridView driversDataGridView;
        private System.Windows.Forms.Label driverNameLabel;
        private System.Windows.Forms.TextBox driverNameTextBox;
        private System.Windows.Forms.Label driverAddressLabel;
        private System.Windows.Forms.TextBox driverAddressTextBox;
        private System.Windows.Forms.Label driverCityLabel;
        private System.Windows.Forms.TextBox driverCityTextBox;
        private System.Windows.Forms.Label driverStateLabel;
        private System.Windows.Forms.TextBox driverStateTextBox;
        private System.Windows.Forms.Label driverZipLabel;
        private System.Windows.Forms.TextBox driverZipTextBox;
        private System.Windows.Forms.Label driverPhoneLabel;
        private System.Windows.Forms.TextBox driverPhoneTextBox;
        private System.Windows.Forms.Label driverEmailLabel;
        private System.Windows.Forms.TextBox driverEmailTextBox;
        private System.Windows.Forms.Label driverStipendLabel;
        private System.Windows.Forms.ComboBox driverStipendComboBox;
        private System.Windows.Forms.Label driverDLTypeLabel;
        private System.Windows.Forms.ComboBox driverDLTypeComboBox;
        private System.Windows.Forms.Button driverAddButton;
        private System.Windows.Forms.Button driverClearButton;

        // Activities Tab Controls
        private System.Windows.Forms.DataGridView maintenanceDataGridView;
        private System.Windows.Forms.Label activityDateLabel;
        private System.Windows.Forms.DateTimePicker activityDatePicker;
        private System.Windows.Forms.Label activityBusNumberLabel;
        private System.Windows.Forms.ComboBox activityBusNumberComboBox;
        private System.Windows.Forms.Label activityDestinationLabel;
        private System.Windows.Forms.TextBox activityDestinationTextBox;
        private System.Windows.Forms.Label activityLeaveTimeLabel;
        private System.Windows.Forms.DateTimePicker activityLeaveTimePicker;
        private System.Windows.Forms.Label activityDriverLabel;
        private System.Windows.Forms.ComboBox activityDriverComboBox;
        private System.Windows.Forms.Label activityHoursDrivenLabel;
        private System.Windows.Forms.TextBox activityHoursDrivenTextBox;
        private System.Windows.Forms.Label activityStudentsLabel;
        private System.Windows.Forms.NumericUpDown activityStudentsNumericUpDown;
        private System.Windows.Forms.Button maintenanceAddButton;
        private System.Windows.Forms.Button maintenanceClearButton;
    }
}