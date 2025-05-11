using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using BusBuddy.Data;
using BusBuddy.DTOs;
using BusBuddy.Entities;

namespace BusBuddy.Services.Dashboard
{
    /// <summary>
    /// Service interface for dashboard functionality
    /// </summary>
    public interface IDashboardService
    {
        Task<List<RouteDto>> GetAllRoutesAsync();
        Task<DashboardDto> GetDashboardMetricsAsync();
        Task<List<TripDto>> GetActiveTripsAsync();
        Task<List<AlertDto>> GetActiveAlertsAsync();
        Task<List<RouteDto>> GetRoutesAsync();
        Task<(DashboardDto, Dictionary<string, int>)> GetDashboardOverviewAsync();
    }

    /// <summary>
    /// Service for dashboard-related functionality
    /// </summary>    
    public class DashboardService : IDashboardService
    {
        private readonly BusBuddyContext _dbContext;
        private readonly ILogger<DashboardService> _logger;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="cache">Memory cache</param>
        public DashboardService(BusBuddyContext dbContext, ILogger<DashboardService> logger, IMemoryCache cache)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
        }        /// <summary>
        /// Gets dashboard metrics
        /// </summary>
        /// <returns>Dashboard metrics DTO</returns>
        public async Task<DashboardDto> GetDashboardMetricsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard metrics");
                
                var today = DateTime.Today;
                
                // Get counts in parallel for better performance
                var totalRoutes = await _dbContext.Routes.CountAsync();
                var totalDrivers = await _dbContext.Drivers.CountAsync();
                var totalVehicles = await _dbContext.Vehicles.CountAsync();
                var activeAlerts = await _dbContext.Alerts.CountAsync(a => a.IsActive);
                
                // Calculate total mileage
                var totalMileage = await _dbContext.Routes.SumAsync(r => r.Distance);
                  // Count trips scheduled for today
                var tripsToday = await _dbContext.ActivityTrips
                    .CountAsync(t => t.DepartureTime.Date == today);
                
                // Calculate on-time performance
                var totalTrips = await _dbContext.Trips.CountAsync();
                var onTimeTrips = await _dbContext.Trips.CountAsync(t => t.DelayMinutes <= 5);
                decimal onTimePercentage = totalTrips > 0 
                    ? Math.Round((decimal)onTimeTrips / totalTrips * 100, 1) 
                    : 100;
                
                // Get route status counts for pie chart
                var trips = await GetActiveTripsAsync();                var routeStatusCounts = trips
                    .GroupBy(t => t.Status)
                    .ToDictionary(g => g.Key, g => g.Count());
                
                // Get recent activity (last 10 trips)
                var recentTrips = await _dbContext.ActivityTrips
                    .Include(t => t.Driver)
                    .Include(t => t.Vehicle)
                    .Include(t => t.Route)
                    .OrderByDescending(t => t.DepartureTime)
                    .Take(10)
                    .Select(t => new ActivitySummaryDto
                    {
                        ActivityId = t.Id,
                        ActivityType = "Trip",
                        Description = $"{t.ActivityName} with {t.Driver.FullName}",
                        Timestamp = t.DepartureTime
                    })
                    .ToListAsync();

                var metrics = new DashboardDto
                {
                    TotalRoutes = totalRoutes,
                    TotalDrivers = totalDrivers,
                    TotalVehicles = totalVehicles,
                    TotalMileage = totalMileage,
                    TripsToday = tripsToday,
                    ActiveAlertCount = activeAlerts,
                    OnTimePerformance = onTimePercentage,
                    RouteStatusCounts = routeStatusCounts,
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

        /// <summary>
        /// Gets active trips
        /// </summary>
        /// <returns>List of active trips</returns>
        public async Task<List<TripDto>> GetActiveTripsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active trips");
                
                // Check cache first
                if (_cache.TryGetValue("ActiveTrips", out List<TripDto>? cachedTrips))
                {
                    _logger.LogInformation("Retrieved active trips from cache");
                    return cachedTrips!;
                }
                
                var trips = await _dbContext.Trips
                    .Include(t => t.Route)
                    .Where(t => t.IsActive)
                    .Select(t => new TripDto
                    {
                        TripId = t.TripId,
                        RouteId = t.RouteId,
                        RouteName = t.Route!.RouteName,
                        PassengerCount = t.PassengerCount,
                        DelayMinutes = t.DelayMinutes,
                        Status = t.Status,
                        IsActive = t.IsActive,
                        LastUpdated = t.LastUpdated
                    })
                    .ToListAsync();

                // Cache the results for 1 minute
                _cache.Set("ActiveTrips", trips, TimeSpan.FromMinutes(1));
                
                _logger.LogInformation("Retrieved {Count} active trips", trips.Count);
                return trips;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active trips");
                return new List<TripDto>();
            }
        }

        /// <summary>
        /// Gets active alerts
        /// </summary>
        /// <returns>List of active alerts</returns>
        public async Task<List<AlertDto>> GetActiveAlertsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active alerts");
                
                // Check cache first
                if (_cache.TryGetValue("ActiveAlerts", out List<AlertDto>? cachedAlerts))
                {
                    _logger.LogInformation("Retrieved active alerts from cache");
                    return cachedAlerts!;
                }
                
                var alerts = await _dbContext.Alerts
                    .Include(a => a.Route)
                    .Where(a => a.IsActive)
                    .Select(a => new AlertDto
                    {
                        AlertId = a.AlertId,
                        RouteId = a.RouteId,
                        Route = new RouteDto
                        {
                            RouteID = a.Route!.Id,
                            RouteName = a.Route.RouteName,
                            StartLocation = a.Route.StartLocation,
                            EndLocation = a.Route.EndLocation,
                            Distance = a.Route.Distance,
                            LastUpdated = a.Route.LastModified
                        },
                        Message = a.Message,
                        Severity = a.Severity,
                        IsActive = a.IsActive,
                        CreatedAt = a.CreatedAt
                    })
                    .ToListAsync();

                // Cache the results for 1 minute
                _cache.Set("ActiveAlerts", alerts, TimeSpan.FromMinutes(1));
                
                _logger.LogInformation("Retrieved {Count} active alerts", alerts.Count);
                return alerts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active alerts");
                return new List<AlertDto>();
            }
        }

        /// <summary>
        /// Gets active bus routes with status information
        /// </summary>
        /// <returns>A list of route DTOs with status information</returns>
        public async Task<List<RouteDto>> GetRoutesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving route data with status information");
                
                // First get the base route data
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
                
                // Then get the latest trips to determine route status
                var activeTrips = await _dbContext.Trips
                    .Where(t => t.Status != "Completed" && t.Status != "Cancelled")
                    .ToListAsync();
                
                // Update route status based on associated trips
                foreach (var route in routes)
                {
                    var routeTrips = activeTrips.Where(t => t.RouteId == route.RouteID);
                    if (!routeTrips.Any())
                    {
                        route.Status = "Inactive";
                    }
                    else if (routeTrips.Any(t => t.Status == "Delayed"))
                    {
                        route.Status = "Delayed";
                    }
                    else if (routeTrips.Any(t => t.Status == "InProgress"))
                    {
                        route.Status = "Active";
                    }
                    else
                    {
                        route.Status = "Scheduled";
                    }
                }

                _logger.LogInformation("Retrieved {Count} routes with status information", routes.Count);
                return routes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving routes with status");
                throw;
            }
        }

        /// <summary>
        /// Gets dashboard overview data (metrics and route status)
        /// </summary>
        /// <returns>Dashboard overview data with metrics and route status counts</returns>
        public async Task<(DashboardDto, Dictionary<string, int>)> GetDashboardOverviewAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving dashboard overview");
                
                // Get metrics
                var metrics = await GetDashboardMetricsAsync();
                
                // Get active trips
                var trips = await GetActiveTripsAsync();
                
                // Calculate route status counts
                var routeStatusCounts = trips
                    .GroupBy(t => t.Status)
                    .ToDictionary(g => g.Key, g => g.Count());
                
                _logger.LogInformation("Retrieved dashboard overview");
                return (metrics, routeStatusCounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard overview");
                return (new DashboardDto(), new Dictionary<string, int>());
            }
        }
    }
}
