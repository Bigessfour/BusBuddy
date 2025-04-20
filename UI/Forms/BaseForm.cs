using System;
using System.Drawing;
using System.Windows.Forms;
using Serilog;
using System.ComponentModel;
using BusBuddy.UI.Interfaces; // Changed to use the correct namespace for IFormNavigator

namespace BusBuddy.UI.Forms
{
    public class BaseForm : Form
    {
        protected IFormNavigator FormNavigator { get; }
        protected ILogger Logger { get; }
        public ToolStripStatusLabel? statusLabel; // Make nullable with ? operator
        private IContainer? components = null; // Already nullable with ?

        public BaseForm(IFormNavigator navigator)
        {
            FormNavigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            Logger = Log.Logger;
        }

        protected void UpdateStatus(string message, Color color)
        {
            if (statusLabel == null || this.IsDisposed)
            {
                Logger.Warning("Cannot update status: statusLabel is null or form is disposed.");
                return;
            }

            // Check if the form's handle is created before invoking
            if (!this.IsHandleCreated)
            {
                Logger.Warning("Cannot update status: Form handle not created yet. Deferring update.");
                // Defer the update until the form is loaded
                this.Load += (s, e) =>
                {
                    UpdateStatus(message, color);
                };
                return;
            }

            // Use BeginInvoke to ensure thread safety for UI updates
            this.BeginInvoke(new Action(() =>
            {
                if (!statusLabel.IsDisposed)
                {
                    statusLabel.ForeColor = color;
                    statusLabel.Text = message;
                }
            }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                statusLabel?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}