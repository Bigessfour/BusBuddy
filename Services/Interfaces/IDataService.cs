using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models.Entities;
using RouteEntity = BusBuddy.Models.Entities.Route;

namespace BusBuddy.Services.Interfaces
{
    /// <summary>
    /// Interface for data services that provide access to application data
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Gets all routes asynchronously
        /// </summary>
        /// <returns>Collection of routes</returns>
        Task<IEnumerable<RouteEntity>> GetAllRoutesAsync();
        
        /// <summary>
        /// Gets a route by ID asynchronously
        /// </summary>
        /// <param name="routeId">The route ID</param>
        /// <returns>The requested route or null if not found</returns>
        Task<RouteEntity> GetRouteByIdAsync(int routeId);
        
        /// <summary>
        /// Gets all drivers asynchronously
        /// </summary>
        /// <returns>Collection of drivers</returns>
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        
        /// <summary>
        /// Gets all vehicles asynchronously
        /// </summary>
        /// <returns>Collection of vehicles</returns>
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
    }
}
