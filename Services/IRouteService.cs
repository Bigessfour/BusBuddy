// BusBuddy/Services/IRouteService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Services
{
    public interface IRouteService
    {
        /// <summary>
        /// Gets all routes
        /// </summary>
        Task<IEnumerable<Route>> GetAllRoutesAsync();
        
        /// <summary>
        /// Gets a route by ID
        /// </summary>
        Task<Route> GetRouteByIdAsync(int id);
        
        /// <summary>
        /// Gets a route by name
        /// </summary>
        Task<Route> GetRouteByNameAsync(string name);
        
        /// <summary>
        /// Adds a new route
        /// </summary>
        Task<(bool Success, string Message, int RouteId)> AddRouteAsync(Route route);
        
        /// <summary>
        /// Updates an existing route
        /// </summary>
        Task<(bool Success, string Message)> UpdateRouteAsync(Route route);
    }
}