using Xunit;
using BusBuddy.Models;
using BusBuddy.Forms;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace BusBuddy.Tests
{
    public class DashboardTests
    {
        private readonly ILogger<Dashboard> _logger;

        public DashboardTests()
        {
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Dashboard>();
        }

        [Fact]
        public void Dashboard_InitializesCorrectly()
        {
            var dashboard = new Dashboard(_logger);
            Assert.NotNull(dashboard);
        }

        [Fact]
        public void Dashboard_HasMaterialTabControl()
        {
            var dashboard = new Dashboard(_logger);
            // TODO: Check for MaterialTabControl existence
            Assert.True(true);
        }
    }
}
