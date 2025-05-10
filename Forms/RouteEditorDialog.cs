using System;
using System.Windows.Forms;

namespace BusBuddy.Forms
{
    public class RouteEditorDialog : Form
    {
        public string RouteName { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
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
