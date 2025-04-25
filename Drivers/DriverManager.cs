using System;
using System.Collections.Generic;
using Serilog;
using BusBuddy.Data;

namespace BusBuddy.Drivers
{
    /// <summary>
    /// Manages driver information, hours, and availability
    /// </summary>
    public class DriverManager
    {
        private readonly IDatabaseManager _dbManager;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverManager"/> class.
        /// </summary>
        /// <param name="dbManager">The database manager instance.</param>
        /// <param name="logger">The logger instance.</param>
        public DriverManager(IDatabaseManager dbManager, ILogger logger)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get a driver by ID from the database
        /// </summary>
        public Driver GetDriverById(string driverId)
        {
            try
            {
                Log.Information("Getting driver with ID: {0}", driverId);

                // In a real implementation, query the database for the driver
                // var driver = _dbManager.ExecuteQuerySingle<Driver>("SELECT * FROM Drivers WHERE Id = @Id", new { Id = driverId });

                // Placeholder implementation for demonstration
                return new Driver
                {
                    Id = driverId,
                    Name = GetDriverNameById(driverId),
                    LicenseNumber = "CDL-" + driverId.PadLeft(5, '0'),
                    HoursThisWeek = 32,
                    HoursLastWeek = 40,
                    CertificationExpiration = DateTime.Now.AddMonths(6)
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting driver with ID {0}", driverId);
                throw;
            }
        }

        /// <summary>
        /// Helper method to get a driver name from ID
        /// </summary>
        private string GetDriverNameById(string id)
        {
            // This would normally query the database, but for demo purposes we'll use placeholders
            switch (id)
            {
                case "001": return "John Smith";
                case "002": return "Mary Johnson";
                case "003": return "Robert Davis";
                default: return $"Driver {id}";
            }
        }

        /// <summary>
        /// Get a list of all active drivers
        /// </summary>
        public List<Driver> GetAllDrivers()
        {
            try
            {
                // In a real implementation, fetch drivers from the database
                // return _dbManager.GetAllDrivers();

                // Placeholder data for demonstration
                var drivers = new List<Driver>
                {
                    new Driver { Id = "D001", Name = "John Smith", LicenseNumber = "CDL-12345", HoursThisWeek = 32, HoursLastWeek = 40, CertificationExpiration = DateTime.Now.AddMonths(6) },
                    new Driver { Id = "D002", Name = "Mary Johnson", LicenseNumber = "CDL-67890", HoursThisWeek = 38, HoursLastWeek = 35, CertificationExpiration = DateTime.Now.AddMonths(9) },
                    new Driver { Id = "D003", Name = "Robert Davis", LicenseNumber = "CDL-24680", HoursThisWeek = 28, HoursLastWeek = 42, CertificationExpiration = DateTime.Now.AddMonths(3) }
                };

                return drivers;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all drivers");
                throw;
            }
        }

        /// <summary>
        /// Record hours worked by a driver
        /// </summary>
        public void RecordHoursWorked(string driverId, DateTime date, double hours, string routeIds)
        {
            try
            {
                // In a real implementation, insert the record into the database
                // _dbManager.ExecuteNonQuery("INSERT INTO WorkRecords (DriverId, Date, HoursWorked, RouteIds) VALUES (@DriverId, @Date, @HoursWorked, @RouteIds)", 
                //     new { DriverId = driverId, Date = date, HoursWorked = hours, RouteIds = routeIds });

                Log.Information("Recorded {0} hours worked for driver {1} on {2}", hours, driverId, date.ToShortDateString());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error recording hours worked for driver {0}", driverId);
                throw;
            }
        }

        /// <summary>
        /// Check if a driver is available for assignment on a specific date and time
        /// </summary>
        public bool IsDriverAvailable(string driverId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                // In a real implementation, check the database for scheduling conflicts
                // var conflicts = _dbManager.ExecuteQuery<int>(@"
                //     SELECT COUNT(*) FROM Assignments 
                //     WHERE DriverId = @DriverId AND Date = @Date 
                //     AND NOT (EndTime <= @StartTime OR StartTime >= @EndTime)", 
                //     new { DriverId = driverId, Date = date, StartTime = startTime, EndTime = endTime });

                // Simplified check - in reality, you'd check against the database for existing assignments
                // Demo implementation just returns true 80% of the time
                var random = new Random();
                bool isAvailable = random.Next(100) < 80;

                Log.Information("Driver {0} availability check for {1} at {2}: {3}", 
                    driverId, date.ToShortDateString(), startTime, isAvailable ? "Available" : "Not available");

                return isAvailable;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking driver {0} availability", driverId);
                // Default to unavailable on error to prevent scheduling conflicts
                return false;
            }
        }

        /// <summary>
        /// Get work records for a driver within a date range
        /// </summary>
        public List<WorkRecord> GetDriverWorkRecords(string driverId, DateTime startDate, DateTime endDate)
        {
            try
            {
                Log.Information("Getting work records for driver {0} from {1} to {2}", 
                    driverId, startDate.ToShortDateString(), endDate.ToShortDateString());

                // In a real implementation, query the database for work records
                // var records = _dbManager.ExecuteQuery<WorkRecord>(
                //     "SELECT * FROM WorkRecords WHERE DriverId = @DriverId AND Date BETWEEN @StartDate AND @EndDate ORDER BY Date DESC", 
                //     new { DriverId = driverId, StartDate = startDate, EndDate = endDate });

                // Placeholder implementation for demonstration
                var records = new List<WorkRecord>();
                DateTime currentDate = startDate;
                var random = new Random();

                while (currentDate <= endDate)
                {
                    // Skip weekends in our demo data
                    if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        records.Add(new WorkRecord
                        {
                            DriverId = driverId,
                            Date = currentDate,
                            HoursWorked = 6 + random.NextDouble() * 4, // Random hours between 6 and 10
                            RouteIds = "R" + random.Next(1, 5).ToString() + ",R" + random.Next(5, 10).ToString()
                        });
                    }
                    currentDate = currentDate.AddDays(1);
                }

                return records;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting work records for driver {0}", driverId);
                throw;
            }
        }
    }

    /// <summary>
    /// Represents a bus driver
    /// </summary>
    public class Driver
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public double HoursThisWeek { get; set; }
        public double HoursLastWeek { get; set; }
        public DateTime CertificationExpiration { get; set; }
    }

    /// <summary>
    /// Represents a work record for a driver
    /// </summary>
    public class WorkRecord
    {
        public string DriverId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double HoursWorked { get; set; }
        public string RouteIds { get; set; } = string.Empty;
    }
}