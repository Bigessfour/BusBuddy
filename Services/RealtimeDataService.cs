using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;
using BusBuddy.Services.Interfaces;
using Microsoft.Extensions.Logging;
using RouteEntity = BusBuddy.Models.Entities.Route;

namespace BusBuddy.Services
{
    /// <summary>
    /// Implementation of IDataService that fetches real-time data directly from the database
    /// </summary>
    public class RealtimeDataService : IDataService
    {
        private readonly IDatabaseHelper _dbHelper;
        private readonly ILogger<RealtimeDataService> _logger;

        public RealtimeDataService(IDatabaseHelper dbHelper, ILogger<RealtimeDataService> logger)
        {
            _dbHelper = dbHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<RouteEntity>> GetAllRoutesAsync()
        {
            _logger.LogInformation("Getting all routes from database (real-time)");
            return await _dbHelper.GetRoutesAsync();
        }

        public async Task<RouteEntity> GetRouteByIdAsync(int routeId)
        {
            _logger.LogInformation("Getting route {RouteId} from database (real-time)", routeId);
            return await _dbHelper.GetRouteByIdAsync(routeId);
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            _logger.LogInformation("Getting all drivers from database (real-time)");
            return await _dbHelper.GetDriversAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            _logger.LogInformation("Getting all vehicles from database (real-time)");
            return await _dbHelper.GetVehiclesAsync();
        }
    }
}
