// BusBuddy/Services/RouteService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Data.Repositories;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger _logger;
        
        public RouteService(IRouteRepository routeRepository, ILogger logger)
        {
            _routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<Route>> GetAllRoutesAsync()
        {
            try
            {
                return await _routeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all routes");
                throw;
            }
        }
        
        public async Task<Route> GetRouteByIdAsync(int id)
        {
            try
            {
                return await _routeRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting route by ID {RouteId}", id);
                throw;
            }
        }
        
        public async Task<Route> GetRouteByNameAsync(string name)
        {
            try
            {
                return await _routeRepository.GetRouteByNameAsync(name);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting route by name {Name}", name);
                throw;
            }
        }
        
        public async Task<(bool Success, string Message, int RouteId)> AddRouteAsync(Route route)
        {
            try
            {
                // Validate route data
                if (string.IsNullOrWhiteSpace(route.RouteName))
                {
                    _logger.Warning("Route validation failed: Name is required");
                    return (false, "Route name is required", 0);
                }
                
                // Add the route
                int routeId = await _routeRepository.AddAsync(route);
                _logger.Information("Route added successfully with ID {RouteId}", routeId);
                return (true, "Route added successfully", routeId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding route");
                return (false, $"Error adding route: {ex.Message}", 0);
            }
        }
        
        public async Task<(bool Success, string Message)> UpdateRouteAsync(Route route)
        {
            try
            {
                // Validate route data
                if (string.IsNullOrWhiteSpace(route.RouteName))
                {
                    _logger.Warning("Route validation failed: Name is required");
                    return (false, "Route name is required");
                }
                
                // Update the route
                bool success = await _routeRepository.UpdateAsync(route);
                if (success)
                {
                    _logger.Information("Route updated successfully with ID {RouteId}", route.RouteId);
                    return (true, "Route updated successfully");
                }
                else
                {
                    _logger.Warning("Route update failed for ID {RouteId}", route.RouteId);
                    return (false, "Route update failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating route");
                return (false, $"Error updating route: {ex.Message}");
            }
        }
    }
}