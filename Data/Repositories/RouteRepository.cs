// BusBuddy/Data/Repositories/RouteRepository.cs
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Data.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IDatabaseManager _dbManager;
        private readonly ILogger _logger;
        
        public RouteRepository(IDatabaseManager dbManager, ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
#pragma warning disable CS8603 // Possible null reference return.
        public async Task<IEnumerable<Route>> GetAllAsync()
        {
            try
            {
                return await Task.FromResult(_dbManager.GetRoutes());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all routes");
                throw;
            }
        }
        
        public async Task<Route> GetByIdAsync(int id)
        {
            try
            {
                var routes = _dbManager.GetRoutes();
                return await Task.FromResult(routes.FirstOrDefault(r => r.RouteID == id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting route by ID {RouteId}", id);
                throw;
            }
        }
        
        public async Task<int> AddAsync(Route entity)
        {
            try
            {
                _dbManager.AddRoute(entity);
                return await Task.FromResult(entity.RouteID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding route");
                throw;
            }
        }
        
        public async Task<bool> UpdateAsync(Route entity)
        {
            try
            {
                _logger.Information("Updating route with ID {RouteId}", entity.RouteID);
                _dbManager.UpdateRoute(entity);
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating route");
                throw;
            }
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.Information("Deleting route with ID {RouteId}", id);
                var result = _dbManager.DeleteRoute(id);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting route");
                throw;
            }
        }
        
        public async Task<Route> GetRouteByNameAsync(string name)
        {
            try
            {
                var routes = _dbManager.GetRoutes();
                return await Task.FromResult(routes.FirstOrDefault(r => r.RouteName == name));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting route by name {Name}", name);
                throw;
            }
        }
#pragma warning restore CS8603
    }
}
#pragma warning restore CS1591