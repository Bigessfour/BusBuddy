using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BusBuddy.Forms;
using System.Threading;
using System.Diagnostics;

namespace BusBuddy
{    public class BusBuddyApplicationContext : ApplicationContext
    {
        private IServiceProvider _serviceProvider;
        private Form _mainForm;
        private readonly ILogger<BusBuddyApplicationContext> _logger;

        public BusBuddyApplicationContext(IServiceProvider serviceProvider, Type startupFormType)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            try
            {
                // Get logger if available
                _logger = _serviceProvider.GetService<ILogger<BusBuddyApplicationContext>>();
                _logger?.LogInformation("BusBuddyApplicationContext initializing");

                // Create the startup form
                _mainForm = (Form)_serviceProvider.GetRequiredService(startupFormType)
                    ?? throw new InvalidOperationException($"Failed to resolve form type: {startupFormType.Name}");

                // Handle the form's events
                _mainForm.FormClosed += OnFormClosed;
                _mainForm.Load += OnFormLoad;

                // Show the form after initialization
                _logger?.LogInformation("Showing main form: {0}", _mainForm.GetType().Name);
                _mainForm.Show();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error during application context initialization");
                MessageBox.Show($"Application startup error: {ex.Message}", "Startup Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // Exit the application when the main form is closed
            ExitThread();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            // This method runs when the main form is loaded but before it's shown
            _logger?.LogInformation("Main form loaded: {0}", _mainForm.GetType().Name);
        }
    }
}
