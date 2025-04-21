using Xunit;
using Moq;
using System.Data.SQLite;
using Dapper;
using Serilog;
using BusBuddy.Data;
using BusBuddy.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BusBuddy.Tests
{
    public class DatabaseManagerTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<SQLiteConnection> _connectionMock;
        private readonly TestableDatabaseManager _dbManager;
        private readonly List<string> _requiredColumns = new List<string>
        {
            "TripID", "TripType", "Date", "BusNumber", "DriverName", "StartTime", "EndTime", "TotalHoursDriven", "Destination"
        };

        public DatabaseManagerTests()
        {
            _loggerMock = new Mock<ILogger>();
            _connectionMock = new Mock<SQLiteConnection>();
            _dbManager = new TestableDatabaseManager(_loggerMock.Object, _connectionMock.Object);
        }

        [Fact]
        public void GetTrips_TableMissing_ReturnsEmptyList()
        {
            // Arrange
            _connectionMock.Setup(c => c.ExecuteScalar<int>(It.Is<string>(s => s.Contains("sqlite_master")), It.IsAny<object>())).Returns(0);

            // Act
            var trips = _dbManager.GetTrips();

            // Assert
            Assert.Empty(trips);
            _loggerMock.Verify(l => l.Error("Trips table schema is invalid. Returning empty list."), Times.Once());
        }

        [Fact]
        public void GetTrips_MissingColumns_ReturnsEmptyList()
        {
            // Arrange
            _connectionMock.Setup(c => c.ExecuteScalar<int>(It.Is<string>(s => s.Contains("sqlite_master")), It.IsAny<object>())).Returns(1);
            _connectionMock.Setup(c => c.Query<string>(It.Is<string>(s => s.Contains("PRAGMA")), It.IsAny<object>())).Returns(new List<string> { "TripID" });

            // Act
            var trips = _dbManager.GetTrips();

            // Assert
            Assert.Empty(trips);
            _loggerMock.Verify(l => l.Error("Required column {Column} is missing in table {TableName}.", It.IsAny<string>(), "Trips"), Times.AtLeastOnce());
        }

        [Fact]
        public void GetTrips_ValidSchema_ReturnsTrips()
        {
            // Arrange
            _connectionMock.Setup(c => c.ExecuteScalar<int>(It.Is<string>(s => s.Contains("sqlite_master")), It.IsAny<object>())).Returns(1);
            _connectionMock.Setup(c => c.Query<string>(It.Is<string>(s => s.Contains("PRAGMA")), It.IsAny<object>())).Returns(_requiredColumns);
            var expectedTrips = new List<Trip>
            {
                new Trip
                {
                    TripID = 1,
                    TripType = "Morning",
                    Date = DateOnly.Parse("2025-04-20"),
                    BusNumber = 101,
                    DriverName = "John Doe",
                    StartTime = TimeOnly.Parse("08:00"),
                    EndTime = TimeOnly.Parse("09:00"),
                    TotalHoursDriven = 1.0,
                    Destination = "Downtown"
                }
            };
            _connectionMock.Setup(c => c.Query<Trip>("SELECT * FROM Trips", null, null, true, null)).Returns(expectedTrips);

            // Act
            var trips = _dbManager.GetTrips();

            // Assert
            Assert.Equal(expectedTrips, trips);
            Assert.Equal("Downtown", trips.First().Destination);
        }

        [Fact]
        public void GetTrips_DatabaseException_ThrowsException()
        {
            // Arrange
            _connectionMock.Setup(c => c.ExecuteScalar<int>(It.IsAny<string>(), It.IsAny<object>())).Throws(new SQLiteException("Database error", 1));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _dbManager.GetTrips());
            Assert.Contains("Failed to check table existence", exception.Message);
        }

        private class TestableDatabaseManager : DatabaseManager
        {
            private readonly SQLiteConnection _connection;

            public TestableDatabaseManager(ILogger logger, SQLiteConnection connection) : base(logger)
            {
                _connection = connection;
            }

            protected override T ExecuteWithRetry<T>(Func<SQLiteConnection, T> operation, string operationName)
            {
                try
                {
                    return operation(_connection);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to {operationName}", ex);
                }
            }
        }
    }
}