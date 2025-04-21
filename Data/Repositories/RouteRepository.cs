// BusBuddy/Data/Repositories/RouteRepository.cs
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
                return await Task.FromResult(routes.FirstOrDefault(r => r.RouteId == id));
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
                return await Task.FromResult(entity.RouteId);
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
                // Implement delete logic when available in DatabaseManager
                _logger.Warning("Route deletion not implemented in DatabaseManager");
                return await Task.FromResult(false);
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
    }
}