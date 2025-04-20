using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using BusBuddy.Data;
using BusBuddy.UI.Forms;
using Serilog;

namespace BusBuddy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize Serilog
            string logFileName = $"busbuddy{DateTime.Now:yyyyMMdd}.log";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", logFileName), rollingInterval: RollingInterval.Day)
                .CreateLogger();
                
            try
            {
                // Register global exception handlers
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                Log.Information("Starting BusBuddy application");

                // Ensure the database file exists
                DatabaseManager.EnsureDatabaseExists();

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Welcome());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Application failed to start properly");
                MessageBox.Show($"Application failed to start properly. Error: {ex.Message}\n\nSee log for details.", 
                    "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DatabaseManager.Cleanup();
                Log.CloseAndFlush();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Error(e.Exception, "Unhandled UI thread exception");
            ShowErrorMessage(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Log.Error(ex, "Unhandled AppDomain exception");
                ShowErrorMessage(ex);
            }
            else
            {
                Log.Error("Unhandled AppDomain exception of unknown type: {ExceptionObject}", e.ExceptionObject);
                ShowErrorMessage(new Exception("An unknown error occurred. Check the log for details."));
            }
        }

        private static void ShowErrorMessage(Exception ex)
        {
            try
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}\n\nCheck the log file for details.",
                    "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception innerEx)
            {
                Log.Error(innerEx, "Failed to show error message box");
            }
        }
    }
}