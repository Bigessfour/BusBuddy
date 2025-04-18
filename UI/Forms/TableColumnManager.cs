// BusBuddy/UI/Forms/TableColumnManager.cs
using System;
using System.Windows.Forms;
using BusBuddy.Utilities;

namespace BusBuddy.UI.Forms
{
    /// <summary>
    /// A form for managing database table columns
    /// </summary>
    public class TableColumnManager : Form, IDisposable
    {
        private System.ComponentModel.IContainer components = null;
        
        public TableColumnManager()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "Table Column Manager";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = AppSettings.Theme.BackgroundColor;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}