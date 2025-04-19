// new-project-files/Program.cs
using System;
using System.Windows.Forms;
using BusBuddy.UI.Forms; // Add this namespace reference

namespace BusBuddy
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); // Launch MainForm
        }
    }
}