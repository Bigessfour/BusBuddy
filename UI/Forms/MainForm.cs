// BusBuddy/UI/Forms/MainForm.cs
using System;
using System.Windows.Forms;
using BusBuddy.UI.Interfaces;

namespace BusBuddy.UI.Forms
{
    public partial class MainForm : BaseForm
    {
        public MainForm() : base(new MainFormNavigator())
        {
            InitializeComponent();
            Logger.Information("MainForm initialized.");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void SchedulerButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Navigating to Trip Scheduler.");
            UpdateStatus("Opening Trip Scheduler...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Trip Scheduler");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void FuelButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Navigating to Fuel Records.");
            UpdateStatus("Opening Fuel Records...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Fuel Records");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void DriverButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Navigating to Driver Management.");
            UpdateStatus("Opening Driver Management...", AppSettings.Theme.InfoColor);
            FormNavigator.NavigateTo("Driver Management");
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void ReportsButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Navigating to Reports.");
            UpdateStatus("Opening Reports...", AppSettings.Theme.InfoColor);
            MessageBox.Show("Reports form not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Navigating to Settings.");
            UpdateStatus("Opening Settings...", AppSettings.Theme.InfoColor);
            using (var settingsForm = new Settings())
            {
                settingsForm.ShowDialog();
            }
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
        }

        private void InputsButton_Click(object sender, EventArgs e)
        {
            Logger.Information("Navigating to Inputs.");
            UpdateStatus("Opening Inputs...", AppSettings.Theme.InfoColor);
            using (var inputsForm = new Inputs())
            {
                inputsForm.ShowDialog();
            }
            UpdateStatus("Ready.", AppSettings.Theme.InfoColor);
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