// BusBuddy/Models/ScheduledRoute.cs
using System;

namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a scheduled route for a specific calendar day
    /// </summary>
    public sealed class ScheduledRoute
    {
        /// <summary>
        /// Gets or sets the unique identifier for the scheduled route
        /// </summary>
        public int ScheduledRouteID { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the scheduled route (lowercase alias)
        /// </summary>
        public int ScheduledRouteId 
        { 
            get => ScheduledRouteID; 
            set => ScheduledRouteID = value; 
        }

        /// <summary>
        /// Gets or sets the ID of the calendar day
        /// </summary>
        public int CalendarDayID { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the calendar day (lowercase alias)
        /// </summary>
        public int CalendarDayId 
        { 
            get => CalendarDayID; 
            set => CalendarDayID = value; 
        }

        /// <summary>
        /// Gets or sets the ID of the route
        /// </summary>
        public int RouteID { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the route (lowercase alias)
        /// </summary>
        public int RouteId 
        { 
            get => RouteID; 
            set => RouteID = value; 
        }
        
        /// <summary>
        /// Gets or sets the bus assigned to this scheduled route
        /// </summary>
        public int AssignedBusNumber { get; set; }

        /// <summary>
        /// Gets or sets the driver ID assigned to this scheduled route
        /// </summary>
        public int AssignedDriverID { get; set; }
        
        /// <summary>
        /// Gets or sets the driver ID assigned to this scheduled route (lowercase alias)
        /// </summary>
        public int AssignedDriverId 
        { 
            get => AssignedDriverID; 
            set => AssignedDriverID = value; 
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledRoute"/> class with specified values
        /// </summary>
        /// <param name="scheduledRouteID">The scheduled route ID</param>
        /// <param name="calendarDayID">The calendar day ID</param>
        /// <param name="routeID">The route ID</param>
        /// <param name="assignedBusNumber">The assigned bus number</param>
        /// <param name="assignedDriverID">The assigned driver ID</param>
        public ScheduledRoute(int scheduledRouteID, int calendarDayID, int routeID, int assignedBusNumber, int assignedDriverID)
        {
            ScheduledRouteID = scheduledRouteID;
            CalendarDayID = calendarDayID;
            RouteID = routeID;
            AssignedBusNumber = assignedBusNumber;
            AssignedDriverID = assignedDriverID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledRoute"/> class
        /// </summary>
        public ScheduledRoute()
        {
        }
        
        /// <summary>
        /// Returns a string representation of the scheduled route
        /// </summary>
        /// <returns>A string representation of the scheduled route</returns>
        public override string ToString()
        {
            return $"Route ID: {RouteID}, Bus: {AssignedBusNumber}, Driver ID: {AssignedDriverID}";
        }
    }
}