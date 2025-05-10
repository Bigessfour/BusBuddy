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
        Task InitializeDatabaseAsync();        /// <summary>
        /// Gets all routes from the database
        /// </summary>
        /// <returns>List of routes</returns>
        Task<IEnumerable<Route>> GetRoutesAsync();

        /// <summary>
        /// Gets all vehicles from the database
        /// </summary>
        /// <returns>List of vehicles</returns>
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();

        /// <summary>
        /// Gets a vehicle by ID
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>The vehicle</returns>
        Vehicle GetVehicle(int id);

        /// <summary>
        /// Adds a new vehicle to the database
        /// </summary>
        /// <param name="vehicle">The vehicle to add</param>
        void AddVehicle(Vehicle vehicle);

        /// <summary>
        /// Checks if a vehicle exists
        /// </summary>
        /// <param name="name">Vehicle name to check</param>
        /// <returns>True if vehicle exists, false otherwise</returns>
        bool VehicleExists(string name);

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
        Task<bool> UpdateRouteAsync(Route route);        /// <summary>
        /// Deletes a route
        /// </summary>
        /// <param name="routeId">The ID of the route to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteRouteAsync(int routeId);

        /// <summary>
        /// Gets all drivers from the database
        /// </summary>
        /// <returns>List of drivers</returns>
        Task<IEnumerable<Driver>> GetDriversAsync();
        
        /// <summary>
        /// Gets a driver by ID
        /// </summary>
        /// <param name="driverId">The driver ID</param>
        /// <returns>The driver if found, null otherwise</returns>
        Task<Driver> GetDriverByIdAsync(int driverId);
        
        /// <summary>
        /// Adds a new driver to the database
        /// </summary>
        /// <param name="driver">The driver to add</param>
        /// <returns>The added driver with ID populated</returns>
        Task<Driver> AddDriverAsync(Driver driver);
        
        /// <summary>
        /// Updates an existing driver
        /// </summary>
        /// <param name="driver">The driver with updated information</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateDriverAsync(Driver driver);
        
        /// <summary>
        /// Safely deletes a driver by handling all dependent records to avoid foreign key constraint violations
        /// </summary>
        /// <param name="driverId">The ID of the driver to delete</param>
        /// <param name="reassignToDriverId">Optional. If provided, reassigns dependent records to this driver instead of setting to null</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteDriverSafelyAsync(int driverId, int? reassignToDriverId = null);
    }
}
