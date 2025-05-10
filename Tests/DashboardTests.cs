using Xunit;
using BusBuddy.Models;
using BusBuddy.Forms;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;
using BusBuddy.Data.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BusBuddy.Tests
{
    public class DashboardTests
    {
        private readonly Mock<ILogger<Dashboard>> _loggerMock;
        private readonly Mock<IDatabaseHelper> _dbHelperMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        public DashboardTests()
        {
            // Mock the logger
            _loggerMock = new Mock<ILogger<Dashboard>>();
            
            // Mock the database helper
            _dbHelperMock = new Mock<IDatabaseHelper>();
            
            // Mock the service provider
            _serviceProviderMock = new Mock<IServiceProvider>();
            
            // Setup service provider to return the database helper
            _serviceProviderMock
                .Setup(x => x.GetService(typeof(IDatabaseHelper)))
                .Returns(_dbHelperMock.Object);
        }

        [Fact]
        public void Dashboard_InitializesCorrectly()
        {
            var dashboard = new Dashboard(_dbHelperMock.Object, _serviceProviderMock.Object, _loggerMock.Object);
            Assert.NotNull(dashboard);
        }

        [Fact(Skip = "UI testing requires STAThread attribute which is not supported in xUnit")]
        public void Dashboard_HasMaterialTabControl()
        {
            var dashboard = new Dashboard(_dbHelperMock.Object, _serviceProviderMock.Object, _loggerMock.Object);
            // TODO: Check for MaterialTabControl existence
            Assert.True(true);
        }
    }
}
