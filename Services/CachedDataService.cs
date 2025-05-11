using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using BusBuddy.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RouteEntity = BusBuddy.Models.Entities.Route;

namespace BusBuddy.Services
{
    /// <summary>
    /// Implementation of IDataService that provides cached data with fallback to database
    /// </summary>
    public class CachedDataService : IDataService
    {
        private readonly IDatabaseHelper _dbHelper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedDataService> _logger;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public CachedDataService(IDatabaseHelper dbHelper, IMemoryCache cache, ILogger<CachedDataService> logger)
        {
            _dbHelper = dbHelper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<RouteEntity>> GetAllRoutesAsync()
        {
            string cacheKey = "all_routes";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<RouteEntity> routes))
            {
                _logger.LogInformation("Retrieved routes from cache");
                return routes;
            }

            _logger.LogInformation("Routes not in cache, fetching from database");
            routes = await _dbHelper.GetRoutesAsync();
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheDuration)
                .SetPriority(CacheItemPriority.Normal);
                
            _cache.Set(cacheKey, routes, cacheOptions);
            
            return routes;
        }

        public async Task<RouteEntity> GetRouteByIdAsync(int routeId)
        {
            string cacheKey = $"route_{routeId}";
            
            if (_cache.TryGetValue(cacheKey, out RouteEntity route))
            {
                _logger.LogInformation("Retrieved route {RouteId} from cache", routeId);
                return route;
            }

            _logger.LogInformation("Route {RouteId} not in cache, fetching from database", routeId);
            route = await _dbHelper.GetRouteByIdAsync(routeId);
            
            if (route != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(_cacheDuration)
                    .SetPriority(CacheItemPriority.Normal);
                    
                _cache.Set(cacheKey, route, cacheOptions);
            }
            
            return route;
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            string cacheKey = "all_drivers";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<Driver> drivers))
            {
                _logger.LogInformation("Retrieved drivers from cache");
                return drivers;
            }

            _logger.LogInformation("Drivers not in cache, fetching from database");
            drivers = await _dbHelper.GetDriversAsync();
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheDuration)
                .SetPriority(CacheItemPriority.Normal);
                
            _cache.Set(cacheKey, drivers, cacheOptions);
            
            return drivers;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            string cacheKey = "all_vehicles";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<Vehicle> vehicles))
            {
                _logger.LogInformation("Retrieved vehicles from cache");
                return vehicles;
            }

            _logger.LogInformation("Vehicles not in cache, fetching from database");
            vehicles = await _dbHelper.GetVehiclesAsync();
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheDuration)
                .SetPriority(CacheItemPriority.Normal);
                
            _cache.Set(cacheKey, vehicles, cacheOptions);
            
            return vehicles;
        }
    }
}
