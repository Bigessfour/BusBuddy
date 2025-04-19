// BusBuddy/UI/Interfaces/IWelcomeView.cs
namespace BusBuddy.UI.Interfaces
{
    /// <summary>
    /// Interface defining the capabilities of the welcome screen view.
    /// </summary>
    public interface IWelcomeView
    {
        /// <summary>
        /// Navigates to the Inputs form.
        /// </summary>
        void NavigateToInputs();

        /// <summary>
        /// Navigates to the Reports form.
        /// </summary>
        void NavigateToReports();

        /// <summary>
        /// Navigates to the Settings form.
        /// </summary>
        void NavigateToSettings();

        /// <summary>
        /// Navigates to the Scheduler form.
        /// </summary>
        void NavigateToScheduler();

        /// <summary>
        /// Navigates to the Fuel Records form.
        /// </summary>
        void NavigateToFuelRecords();

        /// <summary>
        /// Navigates to the Driver Management form.
        /// </summary>
        void NavigateToDriverManagement();
    }
}