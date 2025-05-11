using System;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace BusBuddy.Forms
{
    public class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;

        public MainForm(ILogger<MainForm> logger)
        {
            _logger = logger;
            InitializeComponent();
            _logger.LogInformation("MainForm initialized");
        }

        private void InitializeComponent()
        {
            Text = "Bus Buddy";
            Size = new System.Drawing.Size(1024, 768);
            StartPosition = FormStartPosition.CenterScreen;
            
            // Add a welcome label
            var welcomeLabel = new Label
            {
                Text = "Welcome to Bus Buddy",
                Font = new System.Drawing.Font("Arial", 16),
                AutoSize = true,
                Location = new System.Drawing.Point(50, 50)
            };
            
            Controls.Add(welcomeLabel);
        }
    }
}
