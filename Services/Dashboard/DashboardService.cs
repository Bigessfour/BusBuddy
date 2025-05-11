using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BusBuddy.Data;
using BusBuddy.DTOs;
using BusBuddy.Models.Entities;

namespace BusBuddy.Services.Dashboard
{
    /// <summary>
    /// Service for dashboard-related functionality
    /// </summary>
    public class DashboardService
    {
        private readonly BusBuddyContext _dbContext;
        private readonly ILogger<DashboardService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="logger">Logger instance</param>
        public DashboardService(BusBuddyContext dbContext, ILogger<DashboardService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all bus routes
        /// </summary>
        /// <returns>A list of route DTOs</returns>
        public async Task<List<RouteDto>> GetAllRoutesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all bus routes");
                
                var routes = await _dbContext.Routes
                    .Select(r => new RouteDto
                    {
                        RouteID = r.Id,
                        RouteName = r.RouteName,
                        StartLocation = r.StartLocation,
                        EndLocation = r.EndLocation,
                        Distance = r.Distance,
                        LastUpdated = r.LastModified
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} bus routes", routes.Count);
                return routes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bus routes");
                throw;
            }
        }

        /// <summary>
        /// Gets dashboard metrics
        /// </summary>
        /// <returns>Dashboard metrics DTO</returns>
        public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard metrics");
                
                var today = DateTime.Today;
                
                // Get counts in parallel for better performance
                var totalRoutes = await _dbContext.Routes.CountAsync();
                var totalDrivers = await _dbContext.Drivers.CountAsync();
                var totalVehicles = await _dbContext.Vehicles.CountAsync();
                
                // Calculate total mileage
                var totalMileage = await _dbContext.Routes.SumAsync(r => r.Distance);
                
                // Count trips scheduled for today
                var tripsToday = await _dbContext.ActivityTrips
                    .CountAsync(t => t.Date.Date == today);
                
                // Get recent activity (last 10 trips)
                var recentTrips = await _dbContext.ActivityTrips
                    .Include(t => t.Driver)
                    .Include(t => t.Vehicle)
                    .Include(t => t.Route)
                    .OrderByDescending(t => t.Date)
                    .Take(10)
                    .Select(t => new ActivitySummaryDto
                    {
                        ActivityId = t.Id,
                        ActivityType = "Trip",
                        Description = $"{t.Route.RouteName} - {t.Driver.FullName} - {t.Vehicle.VehicleNumber}",
                        Timestamp = t.Date
                    })
                    .ToListAsync();

                var metrics = new DashboardMetricsDto
                {
                    TotalRoutes = totalRoutes,
                    TotalDrivers = totalDrivers,
                    TotalVehicles = totalVehicles,
                    TotalMileage = totalMileage,
                    TripsToday = tripsToday,
                    RecentActivity = recentTrips
                };

                _logger.LogInformation("Retrieved dashboard metrics");
                return metrics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard metrics");
                throw;
            }
        }
    }
}
