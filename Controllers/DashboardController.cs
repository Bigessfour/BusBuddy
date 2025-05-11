using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using BusBuddy.DTOs;
using BusBuddy.Hubs;
using BusBuddy.Services.Dashboard;

namespace BusBuddy.Controllers
{
    /// <summary>
    /// Controller for dashboard API endpoints
    /// </summary>
    [ApiController]
    [Route("api/dashboard")]
    // [Authorize] - Temporarily disabled for testing
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;
        private readonly IHubContext<DashboardHub> _hubContext;
        private readonly ILogger<DashboardController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class
        /// </summary>
        /// <param name="dashboardService">Dashboard service</param>
        /// <param name="hubContext">SignalR hub context</param>
        /// <param name="logger">Logger instance</param>
        public DashboardController(
            DashboardService dashboardService,
            IHubContext<DashboardHub> hubContext,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all bus routes
        /// </summary>
        /// <returns>List of bus routes</returns>
        [HttpGet("routes")]
        [ProducesResponseType(typeof(List<RouteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RouteDto>>> GetRoutes()
        {
            try
            {
                _logger.LogInformation("GET api/dashboard/routes called");
                var routes = await _dashboardService.GetAllRoutesAsync();
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting routes");
                return StatusCode(500, new { message = "Internal server error retrieving routes" });
            }
        }

        /// <summary>
        /// Gets dashboard metrics
        /// </summary>
        /// <returns>Dashboard metrics</returns>
        [HttpGet("metrics")]
        [ProducesResponseType(typeof(DashboardMetricsDto), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DashboardMetricsDto>> GetMetrics()
        {
            try
            {
                _logger.LogInformation("GET api/dashboard/metrics called");
                var metrics = await _dashboardService.GetDashboardMetricsAsync();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard metrics");
                return StatusCode(500, new { message = "Internal server error retrieving metrics" });
            }
        }

        /// <summary>
        /// Compatibility endpoint for the existing React dashboard
        /// </summary>
        /// <returns>List of bus routes in the format expected by the React dashboard</returns>
        [HttpGet("busroutes")]
        [ProducesResponseType(typeof(List<RouteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RouteDto>>> GetBusRoutes()
        {
            try
            {
                // This endpoint maintains compatibility with the existing React dashboard
                // which expects the route data at /api/busroutes
                _logger.LogInformation("GET api/dashboard/busroutes called (compatibility endpoint)");
                var routes = await _dashboardService.GetAllRoutesAsync();
                return Ok(routes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bus routes for compatibility endpoint");
                return StatusCode(500, new { message = "Internal server error retrieving bus routes" });
            }
        }
        
        /// <summary>
        /// Root level compatibility endpoint for the existing React dashboard
        /// </summary>
        /// <returns>List of bus routes in the format expected by the React dashboard</returns>
        [HttpGet("/api/busroutes")]
        [ProducesResponseType(typeof(List<RouteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RouteDto>>> GetBusRoutesLegacy()
        {
            // This endpoint maintains compatibility with the existing React dashboard
            // which expects the route data at /api/busroutes
            _logger.LogInformation("GET /api/busroutes called (legacy compatibility endpoint)");
            return await GetBusRoutes();
        }

        /// <summary>
        /// Gets dashboard overview data (metrics and route status)
        /// </summary>
        /// <returns>Dashboard overview data</returns>
        [HttpGet("overview")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetOverview()
        {
            try
            {
                _logger.LogInformation("GET api/dashboard/overview called");
                
                var metrics = await _dashboardService.GetDashboardMetricsAsync();
                var trips = await _dashboardService.GetActiveTripsAsync();
                
                // Calculate route status counts
                var routeStatusCounts = trips
                    .GroupBy(t => t.Status)
                    .ToDictionary(g => g.Key, g => g.Count());
                
                return Ok(new
                {
                    metrics,
                    routeStatusCounts
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard overview");
                return StatusCode(500, new { message = "Internal server error retrieving overview data" });
            }
        }

        /// <summary>
        /// Gets active alerts
        /// </summary>
        /// <returns>List of active alerts</returns>
        [HttpGet("alerts")]
        [ProducesResponseType(typeof(List<AlertDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<AlertDto>>> GetAlerts()
        {
            try
            {
                _logger.LogInformation("GET api/dashboard/alerts called");
                var alerts = await _dashboardService.GetActiveAlertsAsync();
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alerts");
                return StatusCode(500, new { message = "Internal server error retrieving alerts" });
            }
        }
    }
}
