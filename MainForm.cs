using System;
using System.Windows.Forms;

namespace BusBuddy
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "Bus Buddy";
            Size = new System.Drawing.Size(800, 600);
            
            // Add initial UI components here
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
