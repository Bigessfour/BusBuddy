using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models.Entities;

namespace BusBuddy.Data.Interfaces
{
    /// <summary>
    /// Interface for database operations
    /// </summary>
    public interface IDatabaseHelper
    {
        /// <summary>
        /// Gets the connection string
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Initializes the database if it doesn't exist
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task InitializeDatabaseAsync();

        /// <summary>
        /// Gets all routes from the database
        /// </summary>
        /// <returns>List of routes</returns>
        Task<IEnumerable<Route>> GetRoutesAsync();

        /// <summary>
        /// Gets a route by ID
        /// </summary>
        /// <param name="routeId">The ID of the route to retrieve</param>
        /// <returns>The route if found, null otherwise</returns>
        Task<Route> GetRouteByIdAsync(int routeId);

        /// <summary>
        /// Adds a new route to the database
        /// </summary>
        /// <param name="route">The route to add</param>
        /// <returns>The added route with ID populated</returns>
        Task<Route> AddRouteAsync(Route route);

        /// <summary>
        /// Updates an existing route
        /// </summary>
        /// <param name="route">The route with updated information</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateRouteAsync(Route route);

        /// <summary>
        /// Deletes a route
        /// </summary>
        /// <param name="routeId">The ID of the route to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteRouteAsync(int routeId);
    }
}
