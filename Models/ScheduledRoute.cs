// BusBuddy/Models/ScheduledRoute.cs
using System;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a scheduled route for a specific calendar day
    /// </summary>
    public class ScheduledRoute
    {
        /// <summary>
        /// Unique identifier for the scheduled route
        /// </summary>
        public int ScheduledRouteId { get; set; }

        /// <summary>
        /// ID of the calendar day
        /// </summary>
        public int CalendarDayId { get; set; }

        /// <summary>
        /// ID of the route
        /// </summary>
        public int RouteId { get; set; }
        
        /// <summary>
        /// Bus assigned to this scheduled route
        /// </summary>
        public int AssignedBusNumber { get; set; }

        /// <summary>
        /// Driver assigned to this scheduled route
        /// </summary>
        public string AssignedDriverName { get; set; }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public ScheduledRoute(int scheduledRouteId, int calendarDayId, int routeId, int assignedBusNumber, string assignedDriverName)
        {
            ScheduledRouteId = scheduledRouteId;
            CalendarDayId = calendarDayId;
            RouteId = routeId;
            AssignedBusNumber = assignedBusNumber;
            AssignedDriverName = assignedDriverName;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduledRoute()
        {
            AssignedDriverName = string.Empty;
        }
    }
}