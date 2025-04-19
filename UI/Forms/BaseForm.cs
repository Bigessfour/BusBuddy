// BusBuddy/UI/Forms/BaseForm.cs
using System;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using BusBuddy.UI.Interfaces;

namespace BusBuddy.UI.Forms
{
    public class BaseForm : Form
    {
        protected readonly ILogger Logger;
        protected readonly ToolStripStatusLabel? StatusLabel; // Nullable to handle cases where not found
        protected readonly IFormNavigator FormNavigator;
        protected readonly FormFactory FormFactory;
        private bool _disposed;
        
        // Initialize the components field properly
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

        protected BaseForm() : this(new MainFormNavigator())
        {
        }

        protected BaseForm(IFormNavigator formNavigator)
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/busbuddy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            FormNavigator = formNavigator ?? throw new ArgumentNullException(nameof(formNavigator));
            FormFactory = new FormFactory();

            // Initialize status strip if not already present
            var statusStrip = Controls.OfType<StatusStrip>().FirstOrDefault();
            if (statusStrip == null)
            {
                statusStrip = new StatusStrip
                {
                    SizingGrip = false,
                    Dock = DockStyle.Bottom,
                    BackColor = AppSettings.Theme.BackgroundColor
                };
                statusStrip.Items.Add(new ToolStripStatusLabel("Ready.") { Name = "statusLabel" });
                Controls.Add(statusStrip);
            }
            StatusLabel = statusStrip.Items["statusLabel"] as ToolStripStatusLabel ?? statusStrip.Items[0] as ToolStripStatusLabel;

            // Apply theme
            BackColor = AppSettings.Theme.BackgroundColor;
        }

        protected void UpdateStatus(string message, System.Drawing.Color color)
        {
            if (StatusLabel != null)
            {
                StatusLabel.Text = message;
                StatusLabel.ForeColor = color;
                Application.DoEvents();
            }
            else
            {
                Logger.Warning("StatusLabel is null; cannot update status: {Message}", message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Are you sure you want to close this form?", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Ensure all forms are closed and the application exits cleanly
            try
            {
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (form != this)
                    {
                        form.Close();
                    }
                }
                Logger.Information("All forms closed for {FormName}.", this.Name);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error closing forms: {ErrorMessage}", ex.Message);
            }
            finally
            {
                if (e.CloseReason == CloseReason.ApplicationExitCall)
                {
                    Environment.Exit(0); // Forcefully exit the process
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            try
            {
                if (disposing)
                {
                    // Dispose managed resources
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }

                // Dispose unmanaged resources (if any)
                // Currently none in this class

                _disposed = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during disposal of BaseForm: {ErrorMessage}", ex.Message);
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}