using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using BusBuddy.DTOs;
using BusBuddy.Hubs;

namespace BusBuddy.Services.Dashboard
{
    /// <summary>
    /// Service for sending real-time dashboard updates via SignalR
    /// </summary>
    public class DashboardUpdatesService
    {
        private readonly IHubContext<DashboardHub> _hubContext;
        private readonly DashboardService _dashboardService;
        private readonly ILogger<DashboardUpdatesService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardUpdatesService"/> class.
        /// </summary>
        /// <param name="hubContext">SignalR hub context</param>
        /// <param name="dashboardService">Dashboard service</param>
        /// <param name="logger">Logger instance</param>
        public DashboardUpdatesService(
            IHubContext<DashboardHub> hubContext,
            DashboardService dashboardService,
            ILogger<DashboardUpdatesService> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Broadcasts updated metrics to all connected clients
        /// </summary>
        public async Task BroadcastMetricsUpdateAsync()
        {
            try
            {
                _logger.LogInformation("Broadcasting dashboard metrics update");
                var metrics = await _dashboardService.GetDashboardMetricsAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveMetricsUpdate", metrics);
                _logger.LogInformation("Dashboard metrics update broadcast complete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting dashboard metrics update");
            }
        }

        /// <summary>
        /// Broadcasts updated routes to all connected clients
        /// </summary>
        public async Task BroadcastRoutesUpdateAsync()
        {
            try
            {
                _logger.LogInformation("Broadcasting routes update");
                var routes = await _dashboardService.GetAllRoutesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveRoutesUpdate", routes);
                _logger.LogInformation("Routes update broadcast complete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting routes update");
            }
        }

        /// <summary>
        /// Sends an activity notification to all connected clients
        /// </summary>
        /// <param name="activity">Activity summary</param>
        public async Task SendActivityNotificationAsync(ActivitySummaryDto activity)
        {
            try
            {
                _logger.LogInformation("Sending activity notification: {ActivityType} - {Description}", activity.ActivityType, activity.Description);
                await _hubContext.Clients.All.SendAsync("ReceiveActivityNotification", activity);
                _logger.LogInformation("Activity notification sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending activity notification");
            }
        }
    }
}
