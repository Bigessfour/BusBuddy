// BusBuddy/Models/Route.cs
namespace BusBuddy.Models
{
    /// <summary>
    /// Represents a bus route in the system
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Unique identifier for the route
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// Name of the route
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Default assigned bus number
        /// </summary>
        public int DefaultBusNumber { get; set; }

        /// <summary>
        /// Default assigned driver
        /// </summary>
        public string DefaultDriverName { get; set; }

        /// <summary>
        /// Description of the route
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public Route(int routeId, string routeName, int defaultBusNumber, string defaultDriverName, string description)
        {
            RouteId = routeId;
            RouteName = routeName;
            DefaultBusNumber = defaultBusNumber;
            DefaultDriverName = defaultDriverName;
            Description = description;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Route()
        {
            RouteName = string.Empty;
            DefaultDriverName = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Returns a string representation of the route
        /// </summary>
        public override string ToString()
        {
            return RouteName;
        }
    }
}