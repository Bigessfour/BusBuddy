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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Route> GetRouteByIdAsync(int id);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets a route by name
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Route> GetRouteByNameAsync(string name);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Adds a new route
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message, int RouteId)> AddRouteAsync(Route route);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Updates an existing route
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<(bool Success, string Message)> UpdateRouteAsync(Route route);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}