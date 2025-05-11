using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BusBuddy.Hubs
{
    /// <summary>
    /// SignalR hub for dashboard real-time updates
    /// </summary>
    public class DashboardHub : Hub
    {
        private readonly ILogger<DashboardHub> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardHub"/> class
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public DashboardHub(ILogger<DashboardHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Called when a client connects to the hub
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when a client disconnects from the hub
        /// </summary>
        /// <param name="exception">Exception that caused the disconnection, if any</param>
        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Joins a specific dashboard data group
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation("Client {ConnectionId} joined group {GroupName}", Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Leaves a specific dashboard data group
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation("Client {ConnectionId} left group {GroupName}", Context.ConnectionId, groupName);
        }
    }
}
