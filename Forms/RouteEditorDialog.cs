using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace BusBuddy.Forms
{
    public class RouteEditorDialog : Form
    {        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RouteName { get; set; } = string.Empty;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StartLocation { get; set; } = string.Empty;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EndLocation { get; set; } = string.Empty;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal Distance { get; set; }

        public RouteEditorDialog() { }
        public RouteEditorDialog(string routeName, string start, string end, decimal distance)
        {
            RouteName = routeName;
            StartLocation = start;
            EndLocation = end;
            Distance = distance;
        }
    }
}
