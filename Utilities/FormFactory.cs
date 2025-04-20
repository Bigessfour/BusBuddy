using System;
using System.Collections.Generic;
using BusBuddy.UI.Forms;

namespace BusBuddy
{
    public class FormFactory
    {
        private readonly Dictionary<string, Func<BaseForm>> _formCreators;

        public FormFactory()
        {
            _formCreators = new Dictionary<string, Func<BaseForm>>
            {
                { "Trip Scheduler", () => new TripSchedulerForm() },
                { "Fuel Records", () => new FuelForm() },
                { "Driver Management", () => new DriverForm() },
                { "Inputs", () => new Inputs() },
                { "Settings", () => new Settings() },
                { "Routes", () => new ScheduledRoutesForm() },
                { "School Calendar", () => new SchoolCalendarForm() },
                { "Route Management", () => new RoutesForm() }
                // Add more forms as needed, e.g., Reports
            };
        }

        public BaseForm CreateForm(string formName)
        {
            if (_formCreators.TryGetValue(formName, out var creator))
            {
                return creator();
            }
            throw new ArgumentException($"Form '{formName}' is not supported.");
        }
    }
}