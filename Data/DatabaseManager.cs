using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Configuration;
using Serilog;
using BusBuddy.Models;  // Ensures Route, SchoolCalendarDay, and ScheduledRoute are accessible

namespace BusBuddy.Data
{
    public class DatabaseManager : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private bool _disposed;
        private const int CURRENT_SCHEMA_VERSION = 2; // Start with version 2 (assuming v1 is without Activities table)

        public DatabaseManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddXmlFile("app.config", optional: true, reloadOnChange: true)
                .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "Data Source=WileySchool.db;Version=3;";
            
            // Ensure database schema is up to date
            EnsureDatabaseUpdated();
        }

        public DatabaseManager() : this(Log.Logger)
        {
        }

        private void EnsureDatabaseUpdated()
        {
            try
            {
                // First ensure the database exists with basic structure
                EnsureDatabaseExists();
                
                // Then check and upgrade the schema if needed
                CheckAndUpgradeSchema();
                
                _logger.Information("Database schema is up to date");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error ensuring database is updated");
                // Continue execution - we'll handle missing tables gracefully
            }
        }

        public static void EnsureDatabaseExists()
        {
            string dbPath = "WileySchool.db";
            if (!System.IO.File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();
                    
                    // Create schema version table first
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS SchemaVersion (
                            Version INTEGER PRIMARY KEY,
                            AppliedOn TEXT
                        )");
                    
                    // Insert initial schema version
                    connection.Execute("INSERT INTO SchemaVersion (Version, AppliedOn) VALUES (1, datetime('now'))");
                    
                    // Create necessary tables
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS Trips (
                            TripID INTEGER PRIMARY KEY AUTOINCREMENT,
                            TripType TEXT,
                            Date TEXT,
                            BusNumber INTEGER,
                            DriverName TEXT,
                            StartTime TEXT,
                            EndTime TEXT,
                            Total_Hours_Driven TEXT,
                            Destination TEXT
                        )");
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS Drivers (
                            DriverID INTEGER PRIMARY KEY AUTOINCREMENT,
                            ""Driver Name"" TEXT,
                            Address TEXT,
                            City TEXT,
                            State TEXT,
                            ""Zip Code"" TEXT,
                            ""Phone Number"" TEXT,
                            ""Email Address"" TEXT,
                            ""Is Stipend Paid"" TEXT,
                            ""DL Type"" TEXT
                        )");
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS Vehicles (
                            VehicleID INTEGER PRIMARY KEY AUTOINCREMENT,
                            ""Bus Number"" INTEGER
                        )");
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS Fuel (
                            Fuel_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            ""Bus Number"" INTEGER,
                            ""Fuel Gallons"" INTEGER,
                            ""Fuel Date"" TEXT,
                            ""Fuel Type"" TEXT,
                            ""Odometer Reading"" INTEGER
                        )");
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS Activities (
                            ActivityID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            BusNumber INTEGER,
                            Destination TEXT,
                            LeaveTime TEXT,
                            Driver TEXT,
                            HoursDriven TEXT,
                            StudentsDriven INTEGER
                        )");
                }
                Log.Logger.Information("Database created at {Path}", dbPath);
            }
        }
        
        private void CheckAndUpgradeSchema() 
        {
            ExecuteWithRetry(connection => {
                // Check if SchemaVersion table exists
                var tableExists = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'SchemaVersion'");
                
                if (tableExists == 0) 
                {
                    // Create schema version table if it doesn't exist
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS SchemaVersion (
                            Version INTEGER PRIMARY KEY,
                            AppliedOn TEXT
                        )");
                    connection.Execute("INSERT INTO SchemaVersion (Version, AppliedOn) VALUES (1, datetime('now'))");
                }
                
                // Get the current schema version
                int currentVersion = connection.ExecuteScalar<int>("SELECT IFNULL(MAX(Version), 0) FROM SchemaVersion");
                
                // Apply upgrades sequentially
                if (currentVersion < 2)
                {
                    _logger.Information("Upgrading database schema to version 2...");
                    
                    // Apply version 2 changes (add Activities table if it doesn't exist)
                    connection.Execute(@"
                        CREATE TABLE IF NOT EXISTS Activities (
                            ActivityID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            BusNumber INTEGER,
                            Destination TEXT,
                            LeaveTime TEXT,
                            Driver TEXT,
                            HoursDriven TEXT,
                            StudentsDriven INTEGER
                        )");
                    
                    // Record the upgrade
                    connection.Execute("INSERT INTO SchemaVersion (Version, AppliedOn) VALUES (2, datetime('now'))");
                    _logger.Information("Upgraded to schema version 2");
                    
                    currentVersion = 2;
                }
                
                // Future upgrades would continue here with if (currentVersion < 3), etc.
                
                return currentVersion;
            }, "check and upgrade database schema");
        }
        
        public bool EnsureTableExists(string tableName, string creationSql)
        {
            try
            {
                return ExecuteWithRetry(connection => {
                    var tableExists = connection.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @TableName", 
                        new { TableName = tableName });
                    
                    if (tableExists == 0)
                    {
                        _logger.Information("Creating missing table: {TableName}", tableName);
                        connection.Execute(creationSql);
                        return true;
                    }
                    return false;
                }, $"ensure table {tableName} exists");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error ensuring table {TableName} exists", tableName);
                return false;
            }
        }

        public static void Cleanup()
        {
            Log.Logger.Information("Database cleanup performed (no-op for SQLite).");
        }

        private T ExecuteWithRetry<T>(Func<SQLiteConnection, T> operation, string operationName)
        {
            for (int attempt = 1; attempt <= AppSettings.Database.MaxRetries; attempt++)
            {
                try
                {
                    using (var connection = new SQLiteConnection(_connectionString))
                    {
                        connection.Open();
                        return operation(connection);
                    }
                }
                catch (Exception ex)
                {
                    if (attempt == AppSettings.Database.MaxRetries)
                    {
                        _logger.Error(ex, "Failed to {Operation} after {Attempts} attempts.", operationName, AppSettings.Database.MaxRetries);
                        throw;
                    }
                    System.Threading.Thread.Sleep(AppSettings.Database.RetryDelayMs);
                }
            }
            throw new InvalidOperationException("Operation failed after maximum retries.");
        }

        private void ExecuteWithRetry(Action<SQLiteConnection> operation, string operationName)
        {
            for (int attempt = 1; attempt <= AppSettings.Database.MaxRetries; attempt++)
            {
                try
                {
                    using (var connection = new SQLiteConnection(_connectionString))
                    {
                        connection.Open();
                        operation(connection);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (attempt == AppSettings.Database.MaxRetries)
                    {
                        _logger.Error(ex, "Failed to {Operation} after {Attempts} attempts.", operationName, AppSettings.Database.MaxRetries);
                        throw;
                    }
                    System.Threading.Thread.Sleep(AppSettings.Database.RetryDelayMs);
                }
            }
        }

        public List<Trip> GetTrips()
        {
            return ExecuteWithRetry(connection =>
            {
                var trips = connection.Query<Trip>("SELECT TripID, TripType, Date, BusNumber, DriverName, StartTime, EndTime, Total_Hours_Driven FROM Trips").AsList();
                return trips ?? new List<Trip>();
            }, "retrieve trips");
        }

        public List<Trip> GetTripsByDate(string date)
        {
            return ExecuteWithRetry(connection =>
            {
                var query = "SELECT TripID, TripType, Date, BusNumber, DriverName, StartTime, EndTime, Total_Hours_Driven, Destination FROM Trips WHERE Date = @Date";
                var trips = connection.Query<Trip>(query, new { Date = date }).AsList();
                return trips ?? new List<Trip>();
            }, "retrieve trips by date");
        }

        public Dictionary<string, int> GetDatabaseStatistics()
        {
            return ExecuteWithRetry(connection =>
            {
                var stats = new Dictionary<string, int>();
                
                // Add entries for tables we know exist
                stats["Trips"] = TableRowCount(connection, "Trips");
                stats["Drivers"] = TableRowCount(connection, "Drivers");
                stats["Buses"] = TableRowCount(connection, "Vehicles");
                stats["FuelRecords"] = TableRowCount(connection, "Fuel");
                
                // Try to get Activities count, but don't crash if table doesn't exist
                stats["Activities"] = TableRowCount(connection, "Activities");
                
                return stats;
            }, "retrieve database statistics");
        }
        
        private int TableRowCount(SQLiteConnection connection, string tableName)
        {
            try
            {
                // Check if table exists before querying row count
                var tableExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = @TableName", 
                    new { TableName = tableName });
                    
                if (tableExists > 0)
                {
                    return connection.ExecuteScalar<int>($"SELECT COUNT(*) FROM {tableName}");
                }
                return 0;
            }
            catch
            {
                // If there's any error, just return 0
                return 0;
            }
        }

        public void AddTrip(Trip trip)
        {
            if (trip == null) throw new ArgumentNullException(nameof(trip));

            ExecuteWithRetry(connection =>
            {
                connection.Execute(
                    "INSERT INTO Trips (TripType, Date, BusNumber, DriverName, StartTime, EndTime, Total_Hours_Driven) VALUES (@TripType, @Date, @BusNumber, @DriverName, @StartTime, @EndTime, @Total_Hours_Driven)",
                    trip);
                _logger.Information("Trip added: {TripType} on {Date}", trip.TripType, trip.Date);
            }, "add trip");
        }

        public List<string> GetDriverNames()
        {
            return ExecuteWithRetry(connection =>
            {
                var drivers = connection.Query<string>("SELECT \"Driver Name\" FROM Drivers WHERE \"Driver Name\" IS NOT NULL").AsList();
                return drivers ?? new List<string>();
            }, "retrieve driver names");
        }

        public List<int> GetBusNumbers()
        {
            return ExecuteWithRetry(connection =>
            {
                var busNumbers = connection.Query<int>("SELECT \"Bus Number\" FROM Vehicles WHERE \"Bus Number\" IS NOT NULL").AsList();
                return busNumbers ?? new List<int>();
            }, "retrieve bus numbers");
        }

        public List<Fuel> GetFuelRecords()
        {
            return ExecuteWithRetry(connection =>
            {
                var fuelRecords = connection.Query<Fuel>("SELECT Fuel_ID, \"Bus Number\" AS Bus_Number, \"Fuel Gallons\" AS Fuel_Gallons, \"Fuel Date\" AS Fuel_Date, \"Fuel Type\" AS Fuel_Type, \"Odometer Reading\" AS Odometer_Reading FROM Fuel").AsList();
                return fuelRecords ?? new List<Fuel>();
            }, "retrieve fuel records");
        }

        public void AddFuelRecord(Fuel fuel)
        {
            if (fuel == null) throw new ArgumentNullException(nameof(fuel));

            ExecuteWithRetry(connection =>
            {
                connection.Execute(
                    "INSERT INTO Fuel (\"Bus Number\", \"Fuel Gallons\", \"Fuel Date\", \"Fuel Type\", \"Odometer Reading\") VALUES (@Bus_Number, @Fuel_Gallons, @Fuel_Date, @Fuel_Type, @Odometer_Reading)",
                    fuel);
                _logger.Information("Fuel record added for Bus {BusNumber} on {FuelDate}", fuel.Bus_Number, fuel.Fuel_Date);
            }, "add fuel record");
        }

        public List<Driver> GetDrivers()
        {
            return ExecuteWithRetry(connection =>
            {
                var drivers = connection.Query<Driver>("SELECT DriverID, \"Driver Name\" AS Driver_Name, Address, City, State, \"Zip Code\" AS Zip_Code, \"Phone Number\" AS Phone_Number, \"Email Address\" AS Email_Address, \"Is Stipend Paid\" AS Is_Stipend_Paid, \"DL Type\" AS DL_Type FROM Drivers").AsList();
                return drivers ?? new List<Driver>();
            }, "retrieve drivers");
        }

        public void AddDriver(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            ExecuteWithRetry(connection =>
            {
                connection.Execute(
                    "INSERT INTO Drivers (\"Driver Name\", Address, City, State, \"Zip Code\", \"Phone Number\", \"Email Address\", \"Is Stipend Paid\", \"DL Type\") VALUES (@Driver_Name, @Address, @City, @State, @Zip_Code, @Phone_Number, @Email_Address, @Is_Stipend_Paid, @DL_Type)",
                    driver);
                _logger.Information("Driver added: {DriverName}", driver.Driver_Name);
            }, "add driver");
        }

        public void LoadVehicleNumbers()
        {
            _logger.Information("Loaded vehicle numbers (placeholder).");
        }

        public void LoadDriverNames()
        {
            _logger.Information("Loaded driver names (placeholder).");
        }

        public List<ActivityTrip> GetActivities()
        {
            try
            {
                // Ensure Activities table exists before trying to query it
                EnsureTableExists("Activities", @"
                    CREATE TABLE IF NOT EXISTS Activities (
                        ActivityID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        BusNumber INTEGER,
                        Destination TEXT,
                        LeaveTime TEXT,
                        Driver TEXT,
                        HoursDriven TEXT,
                        StudentsDriven INTEGER
                    )");
                    
                return ExecuteWithRetry(connection =>
                {
                    var activities = connection.Query<ActivityTrip>("SELECT ActivityID, Date, BusNumber, Destination, LeaveTime, Driver, HoursDriven, StudentsDriven FROM Activities").AsList();
                    return activities ?? new List<ActivityTrip>();
                }, "retrieve activities");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving activities");
                return new List<ActivityTrip>(); // Return empty list instead of crashing
            }
        }

        public void AddActivity(ActivityTrip activity)
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity));

            try
            {
                // Ensure Activities table exists before trying to insert
                EnsureTableExists("Activities", @"
                    CREATE TABLE IF NOT EXISTS Activities (
                        ActivityID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        BusNumber INTEGER,
                        Destination TEXT,
                        LeaveTime TEXT,
                        Driver TEXT,
                        HoursDriven TEXT,
                        StudentsDriven INTEGER
                    )");
                
                ExecuteWithRetry(connection =>
                {
                    connection.Execute(
                        "INSERT INTO Activities (Date, BusNumber, Destination, LeaveTime, Driver, HoursDriven, StudentsDriven) VALUES (@Date, @BusNumber, @Destination, @LeaveTime, @Driver, @HoursDriven, @StudentsDriven)",
                        activity);
                    _logger.Information("Activity added: {Destination} on {Date}", activity.Destination, activity.Date);
                }, "add activity");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding activity {Destination} on {Date}", 
                    activity.Destination, activity.Date);
                throw; // Re-throw after logging
            }
        }

        #region Routes Management
        
        public List<Route> GetRoutes()
        {
            // Ensure Routes table exists
            EnsureTableExists("Routes", @"
                CREATE TABLE IF NOT EXISTS Routes (
                    RouteId INTEGER PRIMARY KEY AUTOINCREMENT,
                    RouteName TEXT NOT NULL,
                    DefaultBusNumber INTEGER NOT NULL,
                    DefaultDriverName TEXT NOT NULL,
                    Description TEXT,
                    FOREIGN KEY (DefaultBusNumber) REFERENCES Vehicles (""Bus Number"")
                )");
                
            return ExecuteWithRetry(connection =>
            {
                var routes = connection.Query<Route>("SELECT RouteId, RouteName, DefaultBusNumber, DefaultDriverName, Description FROM Routes").AsList();
                return routes ?? new List<Route>();
            }, "retrieve routes");
        }

        public Route GetRoute(int routeId)
        {
            return ExecuteWithRetry(connection =>
            {
                return connection.QueryFirstOrDefault<Route>(
                    "SELECT RouteId, RouteName, DefaultBusNumber, DefaultDriverName, Description FROM Routes WHERE RouteId = @RouteId",
                    new { RouteId = routeId });
            }, "get route by id");
        }

        public void AddRoute(Route route)
        {
            if (route == null) throw new ArgumentNullException(nameof(route));

            ExecuteWithRetry(connection =>
            {
                connection.Execute(
                    "INSERT INTO Routes (RouteName, DefaultBusNumber, DefaultDriverName, Description) VALUES (@RouteName, @DefaultBusNumber, @DefaultDriverName, @Description)",
                    route);
                _logger.Information("Route added: {RouteName}", route.RouteName);
            }, "add route");
        }

        public void UpdateRoute(Route route)
        {
            if (route == null) throw new ArgumentNullException(nameof(route));

            ExecuteWithRetry(connection =>
            {
                connection.Execute(
                    "UPDATE Routes SET RouteName = @RouteName, DefaultBusNumber = @DefaultBusNumber, DefaultDriverName = @DefaultDriverName, Description = @Description WHERE RouteId = @RouteId",
                    route);
                _logger.Information("Route updated: {RouteName}", route.RouteName);
            }, "update route");
        }

        public void DeleteRoute(int routeId)
        {
            ExecuteWithRetry(connection =>
            {
                connection.Execute(
                    "DELETE FROM Routes WHERE RouteId = @RouteId",
                    new { RouteId = routeId });
                _logger.Information("Route deleted: ID {RouteId}", routeId);
            }, "delete route");
        }
        
        #endregion

        #region School Calendar Management
        
        public List<SchoolCalendarDay> GetSchoolCalendar(DateTime startDate, DateTime endDate)
        {
            // Ensure Calendar table exists
            EnsureTableExists("SchoolCalendarDays", @"
                CREATE TABLE IF NOT EXISTS SchoolCalendarDays (
                    CalendarDayId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    IsSchoolDay BOOLEAN NOT NULL DEFAULT 1,
                    DayType TEXT NOT NULL DEFAULT 'Regular',
                    Notes TEXT
                )");
                
            return ExecuteWithRetry(connection =>
            {
                var calendar = connection.Query<SchoolCalendarDay>(@"
                    SELECT 
                        CalendarDayId,
                        Date,
                        IsSchoolDay,
                        DayType,
                        Notes
                    FROM SchoolCalendarDays 
                    WHERE Date BETWEEN @StartDate AND @EndDate
                    ORDER BY Date",
                    new { 
                        StartDate = startDate.ToString("yyyy-MM-dd"), 
                        EndDate = endDate.ToString("yyyy-MM-dd") 
                    }).AsList();
                
                // For each calendar day, get its scheduled routes
                foreach (var day in calendar)
                {
                    day.ActiveRouteIds = GetActiveRouteIdsForCalendarDay(connection, day.CalendarDayId);
                }
                
                return calendar ?? new List<SchoolCalendarDay>();
            }, "retrieve school calendar");
        }

        private List<int> GetActiveRouteIdsForCalendarDay(SQLiteConnection connection, int calendarDayId)
        {
            return connection.Query<int>(@"
                SELECT RouteId 
                FROM ScheduledRoutes 
                WHERE CalendarDayId = @CalendarDayId",
                new { CalendarDayId = calendarDayId }).AsList();
        }

        public SchoolCalendarDay GetCalendarDay(DateTime date)
        {
            return ExecuteWithRetry(connection =>
            {
                var day = connection.QueryFirstOrDefault<SchoolCalendarDay>(@"
                    SELECT 
                        CalendarDayId,
                        Date,
                        IsSchoolDay,
                        DayType,
                        Notes
                    FROM SchoolCalendarDays 
                    WHERE Date = @Date",
                    new { Date = date.ToString("yyyy-MM-dd") });
                
                if (day != null)
                {
                    day.ActiveRouteIds = GetActiveRouteIdsForCalendarDay(connection, day.CalendarDayId);
                }
                
                return day;
            }, "get calendar day");
        }

        public int AddOrUpdateCalendarDay(SchoolCalendarDay day)
        {
            if (day == null) throw new ArgumentNullException(nameof(day));

            return ExecuteWithRetry(connection =>
            {
                var existingDay = connection.QueryFirstOrDefault<SchoolCalendarDay>(@"
                    SELECT CalendarDayId FROM SchoolCalendarDays WHERE Date = @Date",
                    new { Date = day.Date.ToString("yyyy-MM-dd") });
                
                int calendarDayId;
                
                if (existingDay == null)
                {
                    // Insert new calendar day
                    calendarDayId = connection.ExecuteScalar<int>(@"
                        INSERT INTO SchoolCalendarDays (Date, IsSchoolDay, DayType, Notes)
                        VALUES (@Date, @IsSchoolDay, @DayType, @Notes);
                        SELECT last_insert_rowid();",
                        new { 
                            Date = day.Date.ToString("yyyy-MM-dd"),
                            day.IsSchoolDay,
                            day.DayType,
                            day.Notes
                        });
                    _logger.Information("Calendar day added for {Date}", day.Date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    // Update existing calendar day
                    calendarDayId = existingDay.CalendarDayId;
                    connection.Execute(@"
                        UPDATE SchoolCalendarDays 
                        SET IsSchoolDay = @IsSchoolDay, DayType = @DayType, Notes = @Notes
                        WHERE CalendarDayId = @CalendarDayId",
                        new {
                            CalendarDayId = calendarDayId,
                            day.IsSchoolDay,
                            day.DayType,
                            day.Notes
                        });
                    _logger.Information("Calendar day updated for {Date}", day.Date.ToString("yyyy-MM-dd"));
                }
                
                // Clear existing scheduled routes
                connection.Execute(@"DELETE FROM ScheduledRoutes WHERE CalendarDayId = @CalendarDayId",
                    new { CalendarDayId = calendarDayId });
                
                // Add new scheduled routes
                foreach (var routeId in day.ActiveRouteIds)
                {
                    // Get the default route info
                    var route = connection.QueryFirstOrDefault<Route>(@"
                        SELECT RouteId, DefaultBusNumber, DefaultDriverName
                        FROM Routes
                        WHERE RouteId = @RouteId",
                        new { RouteId = routeId });
                    
                    if (route != null)
                    {
                        connection.Execute(@"
                            INSERT INTO ScheduledRoutes 
                            (CalendarDayId, RouteId, AssignedBusNumber, AssignedDriverName)
                            VALUES 
                            (@CalendarDayId, @RouteId, @AssignedBusNumber, @AssignedDriverName)",
                            new {
                                CalendarDayId = calendarDayId,
                                RouteId = routeId,
                                AssignedBusNumber = route.DefaultBusNumber,
                                AssignedDriverName = route.DefaultDriverName
                            });
                    }
                }
                
                return calendarDayId;
            }, "add or update calendar day");
        }
        
        #endregion

        #region Scheduled Routes Management
        
        public List<ScheduledRoute> GetScheduledRoutes(DateTime date)
        {
            return ExecuteWithRetry(connection =>
            {
                var calendarDay = connection.QueryFirstOrDefault<SchoolCalendarDay>(@"
                    SELECT CalendarDayId FROM SchoolCalendarDays WHERE Date = @Date",
                    new { Date = date.ToString("yyyy-MM-dd") });
                
                if (calendarDay == null)
                {
                    return new List<ScheduledRoute>();
                }
                
                var scheduledRoutes = connection.Query<ScheduledRoute>(@"
                    SELECT 
                        sr.ScheduledRouteId,
                        sr.CalendarDayId,
                        sr.RouteId,
                        sr.AssignedBusNumber,
                        sr.AssignedDriverName
                    FROM ScheduledRoutes sr
                    JOIN Routes r ON sr.RouteId = r.RouteId
                    WHERE sr.CalendarDayId = @CalendarDayId",
                    new { CalendarDayId = calendarDay.CalendarDayId }).AsList();
                
                return scheduledRoutes ?? new List<ScheduledRoute>();
            }, "get scheduled routes for date");
        }

        public void UpdateScheduledRoute(ScheduledRoute scheduledRoute)
        {
            if (scheduledRoute == null) throw new ArgumentNullException(nameof(scheduledRoute));

            ExecuteWithRetry(connection =>
            {
                connection.Execute(@"
                    UPDATE ScheduledRoutes 
                    SET AssignedBusNumber = @AssignedBusNumber, AssignedDriverName = @AssignedDriverName
                    WHERE ScheduledRouteId = @ScheduledRouteId",
                    scheduledRoute);
                _logger.Information("Scheduled route updated: ID {ScheduledRouteId}", scheduledRoute.ScheduledRouteId);
            }, "update scheduled route");
        }
        
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // No managed resources to dispose
            }

            _disposed = true;
        }
    }
}