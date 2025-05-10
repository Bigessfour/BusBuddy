using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BusBuddy.Forms;
using BusBuddy.Services;
using BusBuddy.Models.Entities;

namespace BusBuddy.Extensions
{
    /// <summary>
    /// Extension methods for handling driver management
    /// </summary>
    public static class DriverManagementExtensions
    {
        /// <summary>
        /// Shows a dialog to safely delete a driver
        /// </summary>
        /// <param name="parent">The parent form</param>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="driver">The driver to delete</param>
        /// <returns>DialogResult indicating the result of the operation</returns>
        public static DialogResult ShowSafeDeleteDriverDialog(this Form parent, IServiceProvider serviceProvider, Driver driver)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
                
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));
                
            // Get the required services
            var driverService = serviceProvider.GetRequiredService<DriverService>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<DeleteDriverDialog>();

            // Create and show the dialog
            using var dialog = new DeleteDriverDialog(driverService, logger, driver);
            return dialog.ShowDialog(parent);
        }
    }
}
