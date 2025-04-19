// BusBuddy/UI/Forms/Welcome.cs
using System;
using System.Windows.Forms;
using BusBuddy.UI.Interfaces;

namespace BusBuddy.UI.Forms
{
    public partial class Welcome : BaseForm, IWelcomeView
    {
        public Welcome() : base(new MainFormNavigator())
        {
            InitializeComponent();
            Logger.Information("Welcome form initialized.");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToInputs()
        {
            Logger.Information("Navigating to Inputs.");
            UpdateStatus("Opening Inputs...", AppSettings.Theme.InfoColor);
            using (var inputsForm = new Inputs())
            {
                inputsForm.ShowDialog();
            }
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToScheduler()
        {
            Logger.Information("Navigating to Trip Scheduler.");
            UpdateStatus("Opening Trip Scheduler...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Trip Scheduler");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToFuelRecords()
        {
            Logger.Information("Navigating to Fuel Records.");
            UpdateStatus("Opening Fuel Records...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Fuel Records");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToDriverManagement()
        {
            Logger.Information("Navigating to Driver Management.");
            UpdateStatus("Opening Driver Management...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Driver Management");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToReports()
        {
            Logger.Information("Navigating to Reports.");
            UpdateStatus("Opening Reports...", AppSettings.Theme.InfoColor);
            MessageBox.Show("Reports form not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        public void NavigateToSettings()
        {
            Logger.Information("Navigating to Settings.");
            UpdateStatus("Opening Settings...", AppSettings.Theme.InfoColor);
            using (var settingsForm = new Settings())
            {
                settingsForm.ShowDialog();
            }
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void SchedulerButton_Click(object sender, EventArgs e)
        {
            NavigateToScheduler();
        }

        private void FuelButton_Click(object sender, EventArgs e)
        {
            NavigateToFuelRecords();
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            NavigateToDriverManagement();
        }

        private void ReportsButton_Click(object sender, EventArgs e)
        {
            NavigateToReports();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigateToSettings();
        }

        private void InputsButton_Click(object sender, EventArgs e)
        {
            NavigateToInputs();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Exit button clicked.");
            var result = MessageBox.Show("Are you sure you want to exit BusBuddy?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Logger.Information("Exiting application.");
                Application.Exit();
            }
        }
    }
}