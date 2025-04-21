using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Serilog;
using BusBuddy.Models;

namespace BusBuddy.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly ILogger _logger;
        private readonly string _connectionString = "Data Source=WileySchool.db;Version=3;";

        public DatabaseManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Trip> GetTrips()
        {
            return ExecuteWithRetry(connection =>
            {
                var requiredColumns = new List<string>
                {
                    "TripID", "TripType", "Date", "BusNumber", "DriverName", "StartTime", "EndTime", "TotalHoursDriven", "Destination"
                };

                if (!ValidateTableSchema(connection, "Trips", requiredColumns))
                {
                    _logger.Error("Trips table schema is invalid. Returning empty list.");
                    return new List<Trip>();
                }

                // Query raw data from database
                var rawTrips = connection.Query<dynamic>("SELECT * FROM Trips").ToList();
                var trips = new List<Trip>();
                
                // Convert string dates and times to appropriate types
                foreach (var rawTrip in rawTrips)
                {
                    try
                    {
                        var trip = new Trip
                        {
                            TripID = rawTrip.TripID,
                            TripType = rawTrip.TripType,
                            BusNumber = rawTrip.BusNumber,
                            DriverName = rawTrip.DriverName,
                            TotalHoursDriven = rawTrip.TotalHoursDriven,
                            Destination = rawTrip.Destination,
                            // Initialize with default values to avoid unassigned variables
                            Date = new DateOnly(),
                            StartTime = new TimeOnly(), 
                            EndTime = new TimeOnly(),
                            // Add support for new payment-related fields
                            MilesDriven = rawTrip.MilesDriven != null ? (double)rawTrip.MilesDriven : 0,
                            TripCategory = rawTrip.TripCategory != null ? (string)rawTrip.TripCategory : "Route"
                        };

                        // Convert IsCDLRoute from integer to boolean
                        if (rawTrip.IsCDLRoute != null)
                        {
                            trip.IsCDLRoute = Convert.ToInt32(rawTrip.IsCDLRoute) != 0;
                        }
                        
                        // Parse date from string
                        DateOnly date = DateOnly.FromDateTime(DateTime.Today); // Default value
                        if (rawTrip.Date != null && DateOnly.TryParse(rawTrip.Date.ToString(), out date))
                        {
                            trip.Date = date;
                        }
                        else
                        {
                            trip.Date = date; // Use default if parsing fails
                        }
                        
                        // Parse start time from string
                        TimeOnly startTime = new TimeOnly(0, 0); // Default value
                        if (rawTrip.StartTime != null && TimeOnly.TryParse(rawTrip.StartTime.ToString(), out startTime))
                        {
                            trip.StartTime = startTime;
                        }
                        else
                        {
                            trip.StartTime = startTime; // Use default if parsing fails
                        }
                        
                        // Parse end time from string
                        TimeOnly endTime = new TimeOnly(0, 0); // Default value
                        if (rawTrip.EndTime != null && TimeOnly.TryParse(rawTrip.EndTime.ToString(), out endTime))
                        {
                            trip.EndTime = endTime;
                        }
                        else
                        {
                            trip.EndTime = endTime; // Use default if parsing fails
                        }
                        
                        trips.Add(trip);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error converting trip data from database");
                    }
                }

                return trips;
            }, "fetch trips");
        }

        public List<string> GetDriverNames()
        {
            return ExecuteWithRetry(connection =>
            {
                // Check if the table exists
                var tableExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Drivers'") > 0;
                
                if (!tableExists)
                {
                    _logger.Error("Drivers table does not exist.");
                    return new List<string>();
                }
                
                // Get column names to determine which one to use
                var columnInfo = connection.Query<dynamic>("PRAGMA table_info(Drivers)").ToList();
                var columns = columnInfo.Select(row => (string)row.name).ToList();
                
                // Log column information instead of using Console.WriteLine
                _logger.Debug("Drivers table columns: {Columns}", string.Join(", ", columns));
                
                string query;
                if (columns.Contains("Driver_Name"))
                {
                    query = "SELECT Driver_Name FROM Drivers";
                }
                else if (columns.Contains("Name"))
                {
                    query = "SELECT Name FROM Drivers";
                }
                else
                {
                    _logger.Error("Neither Name nor Driver_Name column found in Drivers table.");
                    return new List<string>();
                }
                
                return connection.Query<string>(query).ToList();
            }, "fetch driver names");
        }

        public List<int> GetBusNumbers()
        {
            return ExecuteWithRetry(connection =>
            {
                return connection.Query<int>("SELECT BusNumber FROM Buses").ToList();
            }, "fetch bus numbers");
        }

        public List<Route> GetRoutes()
        {
            return ExecuteWithRetry(connection =>
            {
                // Check if the table exists
                var tableExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Routes'") > 0;
                
                if (!tableExists)
                {
                    _logger.Error("Routes table does not exist.");
                    return new List<Route>();
                }
                
                // Get column names to determine which ones to use
                var columnInfo = connection.Query<dynamic>("PRAGMA table_info(Routes)").ToList();
                var columns = columnInfo.Select(row => (string)row.name).ToList();
                
                // Log column information instead of using Console.WriteLine
                _logger.Debug("Routes table columns: {Columns}", string.Join(", ", columns));
                
                // Check if the Name column exists
                if (!columns.Contains("Name"))
                {
                    _logger.Error("Name column not found in Routes table.");
                    return new List<Route>();
                }
                
                var query = @"SELECT RouteId, Name as RouteName, Description";
                
                // Add DefaultBusNumber and DefaultDriverName if they exist
                if (columns.Contains("DefaultBusNumber"))
                {
                    query += ", DefaultBusNumber";
                }
                else
                {
                    query += ", 0 as DefaultBusNumber";
                }
                
                if (columns.Contains("DefaultDriverName"))
                {
                    query += ", DefaultDriverName";
                }
                else
                {
                    query += ", '' as DefaultDriverName";
                }
                
                query += " FROM Routes";
                
                return connection.Query<Route>(query).ToList();
            }, "fetch routes");
        }

        public void AddFuelRecord(BusBuddy.Models.FuelRecord record)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"INSERT INTO FuelRecords (RecordId, BusNumber, Date, Gallons, Cost)
                            VALUES (@RecordId, @BusNumber, @Date, @Gallons, @Cost)";
                connection.Execute(sql, record);
                return 0;
            }, "add fuel record");
        }

        public List<BusBuddy.Models.Driver> GetDrivers()
        {
            return ExecuteWithRetry(connection =>
            {
                return connection.Query<BusBuddy.Models.Driver>("SELECT * FROM Drivers").ToList();
            }, "fetch drivers");
        }

        public void AddOrUpdateCalendarDay(SchoolCalendarDay day)
        {
            ExecuteWithRetry(connection =>
            {
                // Begin transaction to ensure both operations succeed or fail together
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, insert or update the calendar day
                        var sql = @"INSERT OR REPLACE INTO CalendarDays (CalendarDayId, Date, IsSchoolDay, DayType, Notes)
                                    VALUES (@CalendarDayId, @Date, @IsSchoolDay, @DayType, @Notes)";
                        connection.Execute(sql, new
                        {
                            day.CalendarDayId,
                            Date = day.Date.ToString("yyyy-MM-dd"),
                            day.IsSchoolDay,
                            day.DayType,
                            day.Notes
                        }, transaction);

                        // If this is a new calendar day, get the generated ID
                        if (day.CalendarDayId == 0)
                        {
                            day.CalendarDayId = connection.ExecuteScalar<int>("SELECT last_insert_rowid()", transaction: transaction);
                        }

                        // Delete existing active routes for this calendar day
                        connection.Execute(
                            "DELETE FROM CalendarDayRoutes WHERE CalendarDayId = @CalendarDayId",
                            new { day.CalendarDayId }, transaction);

                        // Insert new active routes
                        if (day.ActiveRouteIds != null && day.ActiveRouteIds.Count > 0)
                        {
                            var routeInsertSql = @"INSERT INTO CalendarDayRoutes (CalendarDayId, RouteId)
                                                VALUES (@CalendarDayId, @RouteId)";
                            foreach (var routeId in day.ActiveRouteIds)
                            {
                                connection.Execute(routeInsertSql, new
                                {
                                    day.CalendarDayId,
                                    RouteId = routeId
                                }, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return 0;
            }, "add or update calendar day");
        }

        public SchoolCalendarDay GetCalendarDay(DateTime date)
        {
            return ExecuteWithRetry(connection =>
            {
                // Get the calendar day
                var calendarDay = connection.QuerySingleOrDefault<SchoolCalendarDay>(
                    "SELECT * FROM CalendarDays WHERE Date = @Date",
                    new { Date = date.ToString("yyyy-MM-dd") });

                if (calendarDay != null)
                {
                    // Initialize the ActiveRouteIds list
                    calendarDay.ActiveRouteIds = new List<int>();

                    // Get the active routes for this calendar day
                    var activeRouteIds = connection.Query<int>(
                        "SELECT RouteId FROM CalendarDayRoutes WHERE CalendarDayId = @CalendarDayId",
                        new { calendarDay.CalendarDayId });

                    // Add the active route IDs to the list
                    calendarDay.ActiveRouteIds.AddRange(activeRouteIds);
                }

                return calendarDay;
            }, "fetch calendar day");
        }

        public List<ScheduledRoute> GetScheduledRoutes(int calendarDayId)
        {
            return ExecuteWithRetry(connection =>
            {
                return connection.Query<ScheduledRoute>(
                    "SELECT * FROM ScheduledRoutes WHERE CalendarDayId = @CalendarDayId",
                    new { CalendarDayId = calendarDayId }).ToList();
            }, "fetch scheduled routes by calendar day id");
        }

        public List<ScheduledRoute> GetScheduledRoutes(DateTime date)
        {
            return ExecuteWithRetry(connection =>
            {
                var calendarDay = GetCalendarDay(date);
                if (calendarDay == null)
                {
                    return new List<ScheduledRoute>();
                }

                return GetScheduledRoutes(calendarDay.CalendarDayId);
            }, "fetch scheduled routes by date");
        }

        public void AddDriver(BusBuddy.Models.Driver driver)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"INSERT INTO Drivers (DriverId, Name, Driver_Name, Address, City, State, Zip_Code, Phone_Number, Email_Address, Is_Stipend_Paid, DL_Type)
                            VALUES (@DriverID, @Driver_Name, @Driver_Name, @Address, @City, @State, @Zip_Code, @Phone_Number, @Email_Address, @Is_Stipend_Paid, @DL_Type)";
                connection.Execute(sql, driver);
                return 0;
            }, "add driver");
        }

        public List<BusBuddy.Models.Activity> GetActivities()
        {
            return ExecuteWithRetry(connection =>
            {
                return connection.Query<BusBuddy.Models.Activity>("SELECT * FROM Activities").ToList();
            }, "fetch activities");
        }

        public void UpdateScheduledRoute(ScheduledRoute route)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"UPDATE ScheduledRoutes
                            SET CalendarDayId = @CalendarDayId, RouteId = @RouteId, 
                                AssignedBusNumber = @AssignedBusNumber, AssignedDriverName = @AssignedDriverName
                            WHERE ScheduledRouteId = @ScheduledRouteId";
                connection.Execute(sql, route);
                return 0;
            }, "update scheduled route");
        }

        public void AddActivity(BusBuddy.Models.Activity activity)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"INSERT INTO Activities (ActivityId, Name, Description)
                            VALUES (@ActivityId, @Name, @Description)";
                connection.Execute(sql, activity);
                return 0;
            }, "add activity");
        }

        public void AddTrip(Trip trip)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"INSERT INTO Trips (TripID, TripType, Date, BusNumber, DriverName, StartTime, EndTime, TotalHoursDriven, Destination, IsCDLRoute, MilesDriven, TripCategory)
                            VALUES (@TripID, @TripType, @Date, @BusNumber, @DriverName, @StartTime, @EndTime, @TotalHoursDriven, @Destination, @IsCDLRoute, @MilesDriven, @TripCategory)";
                connection.Execute(sql, new
                {
                    trip.TripID,
                    trip.TripType,
                    Date = trip.Date.ToString("yyyy-MM-dd"),
                    trip.BusNumber,
                    trip.DriverName,
                    StartTime = trip.StartTime.ToString("HH:mm"),
                    EndTime = trip.EndTime.ToString("HH:mm"),
                    trip.TotalHoursDriven,
                    trip.Destination,
                    IsCDLRoute = trip.IsCDLRoute ? 1 : 0, // Convert boolean to integer for SQLite
                    trip.MilesDriven,
                    trip.TripCategory
                });
                return 0;
            }, "add trip");
        }

        public List<Trip> GetTripsByDate(DateOnly date)
        {
            return ExecuteWithRetry(connection =>
            {
                var requiredColumns = new List<string>
                {
                    "TripID", "TripType", "Date", "BusNumber", "DriverName", "StartTime", "EndTime", "TotalHoursDriven", "Destination"
                };

                if (!ValidateTableSchema(connection, "Trips", requiredColumns))
                {
                    _logger.Error("Trips table schema is invalid. Returning empty list.");
                    return new List<Trip>();
                }
                
                var dateString = date.ToString("yyyy-MM-dd");
                var rawTrips = connection.Query<dynamic>(
                    "SELECT * FROM Trips WHERE Date = @Date",
                    new { Date = dateString }).ToList();
                    
                var trips = new List<Trip>();
                
                // Convert string dates and times to appropriate types
                foreach (var rawTrip in rawTrips)
                {
                    try
                    {
                        var trip = new Trip
                        {
                            TripID = rawTrip.TripID,
                            TripType = rawTrip.TripType,
                            BusNumber = rawTrip.BusNumber,
                            DriverName = rawTrip.DriverName,
                            TotalHoursDriven = rawTrip.TotalHoursDriven,
                            Destination = rawTrip.Destination,
                            // Initialize with default values to avoid unassigned variables
                            Date = date,
                            // Add support for new payment-related fields
                            MilesDriven = rawTrip.MilesDriven != null ? (double)rawTrip.MilesDriven : 0,
                            TripCategory = rawTrip.TripCategory != null ? (string)rawTrip.TripCategory : "Route"
                        };

                        // Convert IsCDLRoute from integer to boolean
                        if (rawTrip.IsCDLRoute != null)
                        {
                            trip.IsCDLRoute = Convert.ToInt32(rawTrip.IsCDLRoute) != 0;
                        }
                        
                        // Parse start time from string
                        TimeOnly startTime = new TimeOnly(0, 0); // Default value
                        if (rawTrip.StartTime != null && TimeOnly.TryParse(rawTrip.StartTime.ToString(), out startTime))
                        {
                            trip.StartTime = startTime;
                        }
                        else
                        {
                            trip.StartTime = startTime; // Use default if parsing fails
                        }
                        
                        // Parse end time from string
                        TimeOnly endTime = new TimeOnly(0, 0); // Default value
                        if (rawTrip.EndTime != null && TimeOnly.TryParse(rawTrip.EndTime.ToString(), out endTime))
                        {
                            trip.EndTime = endTime;
                        }
                        else
                        {
                            trip.EndTime = endTime; // Use default if parsing fails
                        }
                        
                        trips.Add(trip);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error converting trip data from database for date {Date}", dateString);
                    }
                }
                
                return trips;
            }, "fetch trips by date");
        }

        public DatabaseStatistics GetDatabaseStatistics()
        {
            return ExecuteWithRetry(connection =>
            {
                var stats = new DatabaseStatistics();
                
                stats.TripCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Trips");
                stats.DriverCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Drivers");
                stats.BusCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Buses");
                stats.RouteCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Routes");
                stats.FuelRecordCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM FuelRecords");
                stats.ActivityCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Activities");
                
                return stats;
            }, "get database statistics");
        }

        public List<BusBuddy.Models.FuelRecord> GetFuelRecords()
        {
            return ExecuteWithRetry(connection =>
            {
                return connection.Query<BusBuddy.Models.FuelRecord>("SELECT * FROM FuelRecords").ToList();
            }, "fetch fuel records");
        }

        public void AddRoute(Route route)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"INSERT INTO Routes (RouteId, Name, Description, DefaultBusNumber, DefaultDriverName)
                            VALUES (@RouteId, @RouteName, @Description, @DefaultBusNumber, @DefaultDriverName)";
                connection.Execute(sql, new {
                    RouteId = route.RouteId,
                    RouteName = route.RouteName,
                    Description = route.Description,
                    DefaultBusNumber = route.DefaultBusNumber,
                    DefaultDriverName = route.DefaultDriverName
                });
                return 0;
            }, "add route");
        }

        public void UpdateRoute(Route route)
        {
            ExecuteWithRetry(connection =>
            {
                var sql = @"UPDATE Routes
                            SET Name = @RouteName, Description = @Description, 
                                DefaultBusNumber = @DefaultBusNumber, DefaultDriverName = @DefaultDriverName
                            WHERE RouteId = @RouteId";
                connection.Execute(sql, new {
                    RouteId = route.RouteId,
                    RouteName = route.RouteName,
                    Description = route.Description,
                    DefaultBusNumber = route.DefaultBusNumber,
                    DefaultDriverName = route.DefaultDriverName
                });
                return 0;
            }, "update route");
        }

        /// <summary>
        /// Validates if a string is a valid SQL identifier (table or column name)
        /// </summary>
        /// <param name="identifier">The identifier to validate</param>
        /// <returns>True if the identifier is valid, false otherwise</returns>
        private bool IsValidIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return false;
                
            // Check if the identifier contains only alphanumeric characters and underscores
            // and starts with a letter or underscore
            return System.Text.RegularExpressions.Regex.IsMatch(
                identifier, 
                @"^[a-zA-Z_][a-zA-Z0-9_]*$");
        }
        
        protected virtual T ExecuteWithRetry<T>(Func<SQLiteConnection, T> operation, string operationName)
        {
            const int maxRetries = 3;
            int attempt = 0;

            while (true)
            {
                try
                {
                    using (var connection = new SQLiteConnection(_connectionString))
                    {
                        connection.Open();
                        return operation(connection);
                    }
                }
                catch (SQLiteException ex) when (attempt < maxRetries)
                {
                    attempt++;
                    _logger.Warning("Attempt {Attempt} failed for {OperationName}: {Exception}. Retrying...", attempt, operationName, ex.Message);
                    System.Threading.Thread.Sleep(1000 * attempt);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to {OperationName} after {Attempt} attempts.", operationName, attempt);
                    throw new InvalidOperationException($"Failed to {operationName}", ex);
                }
            }
        }
        
        /// <summary>
        /// Validates that a table exists and has all required columns
        /// </summary>
        /// <param name="connection">The database connection</param>
        /// <param name="tableName">Name of the table to validate</param>
        /// <param name="requiredColumns">List of required column names</param>
        /// <returns>True if the table exists and has all required columns, false otherwise</returns>
        protected bool ValidateTableSchema(SQLiteConnection connection, string tableName, List<string> requiredColumns)
        {
            try
            {
                // Check if table exists - use parameterized query to prevent SQL injection
                var tableExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@TableName", 
                    new { TableName = tableName }) > 0;

                if (!tableExists)
                {
                    _logger.Error("{TableName} table does not exist.", tableName);
                    return false;
                }

                // Get all columns in the table - SQLite PRAGMA doesn't support parameters, but we can validate the table name
                if (!IsValidIdentifier(tableName))
                {
                    _logger.Error("Invalid table name: {TableName}", tableName);
                    return false;
                }
                
                var columnInfo = connection.Query<dynamic>($"PRAGMA table_info({tableName})").ToList();
                var columns = columnInfo.Select(row => (string)row.name).ToList();

                // Check for required columns
                foreach (var column in requiredColumns)
                {
                    if (!columns.Contains(column))
                    {
                        _logger.Error("Required column {Column} is missing in table {TableName}.", column, tableName);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error validating schema for table {TableName}", tableName);
                return false;
            }
        }

        public static void EnsureDatabaseExists()
        {
            // Check if the database file exists
            if (!System.IO.File.Exists("WileySchool.db"))
            {
                // Create a new database file
                SQLiteConnection.CreateFile("WileySchool.db");
            }
            
            using (var connection = new SQLiteConnection("Data Source=WileySchool.db;Version=3;"))
            {
                connection.Open();
                var sql = @"
                    CREATE TABLE IF NOT EXISTS Trips (
                        TripID INTEGER PRIMARY KEY,
                        TripType TEXT,
                        Date TEXT,
                        BusNumber INTEGER,
                        DriverName TEXT,
                        StartTime TEXT,
                        EndTime TEXT,
                        TotalHoursDriven REAL,
                        Destination TEXT,
                        IsCDLRoute INTEGER DEFAULT 0,
                        MilesDriven REAL DEFAULT 0,
                        TripCategory TEXT DEFAULT 'Route'
                    );
                    CREATE TABLE IF NOT EXISTS Drivers (
                        DriverId INTEGER PRIMARY KEY,
                        Name TEXT,
                        Driver_Name TEXT,
                        Address TEXT,
                        City TEXT,
                        State TEXT,
                        Zip_Code TEXT,
                        Phone_Number TEXT,
                        Email_Address TEXT,
                        Is_Stipend_Paid INTEGER DEFAULT 0,
                        DL_Type TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Buses (
                        BusNumber INTEGER PRIMARY KEY
                    );
                    CREATE TABLE IF NOT EXISTS Routes (
                        RouteId INTEGER PRIMARY KEY,
                        Name TEXT,
                        Description TEXT,
                        DefaultBusNumber INTEGER DEFAULT 0,
                        DefaultDriverName TEXT DEFAULT ''
                    );
                    CREATE TABLE IF NOT EXISTS FuelRecords (
                        RecordId INTEGER PRIMARY KEY,
                        BusNumber INTEGER,
                        Date TEXT,
                        Gallons REAL,
                        Cost REAL,
                        Fuel_Type TEXT,
                        Odometer_Reading INTEGER
                    );
                    CREATE TABLE IF NOT EXISTS CalendarDays (
                        CalendarDayId INTEGER PRIMARY KEY,
                        Date TEXT,
                        IsSchoolDay INTEGER,
                        DayType TEXT,
                        Notes TEXT
                    );
                    CREATE TABLE IF NOT EXISTS ScheduledRoutes (
                        ScheduledRouteId INTEGER PRIMARY KEY,
                        CalendarDayId INTEGER,
                        RouteId INTEGER,
                        AssignedBusNumber INTEGER,
                        AssignedDriverName TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Activities (
                        ActivityId INTEGER PRIMARY KEY,
                        Name TEXT,
                        Description TEXT
                    );
                    CREATE TABLE IF NOT EXISTS CalendarDayRoutes (
                        Id INTEGER PRIMARY KEY,
                        CalendarDayId INTEGER,
                        RouteId INTEGER,
                        FOREIGN KEY (CalendarDayId) REFERENCES CalendarDays(CalendarDayId),
                        FOREIGN KEY (RouteId) REFERENCES Routes(RouteId)
                    );";
                connection.Execute(sql);
                
                // Add sample data if tables are empty
                AddSampleDataIfEmpty(connection);
            }
        }
        
        private static void AddSampleDataIfEmpty(SQLiteConnection connection)
        {
            try
            {
                // Check if Drivers table is empty
                var driverCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Drivers");
                if (driverCount == 0)
                {
                    // Add a sample driver
                    connection.Execute(@"
                        INSERT INTO Drivers (DriverId, Name, Driver_Name, Address, City, State, Zip_Code, Phone_Number, Email_Address, Is_Stipend_Paid, DL_Type)
                        VALUES (1, 'John Doe', 'John Doe', '123 Main St', 'Anytown', 'NY', '12345', '555-123-4567', 'john.doe@example.com', 0, 'CDL')
                    ");
                }
                
                // Check if Buses table is empty
                var busCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Buses");
                if (busCount == 0)
                {
                    // Add sample buses
                    connection.Execute(@"
                        INSERT INTO Buses (BusNumber) VALUES (1);
                        INSERT INTO Buses (BusNumber) VALUES (2);
                        INSERT INTO Buses (BusNumber) VALUES (3)
                    ");
                }
                
                // Check if Routes table is empty
                var routeCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Routes");
                if (routeCount == 0)
                {
                    // Add sample routes
                    connection.Execute(@"
                        INSERT INTO Routes (RouteId, Name, Description, DefaultBusNumber, DefaultDriverName)
                        VALUES (1, 'Morning Route', 'Morning pickup route', 1, 'John Doe');
                        
                        INSERT INTO Routes (RouteId, Name, Description, DefaultBusNumber, DefaultDriverName)
                        VALUES (2, 'Afternoon Route', 'Afternoon dropoff route', 2, 'John Doe')
                    ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding sample data: {ex.Message}");
            }
        }

        public static void Cleanup()
        {
            using (var connection = new SQLiteConnection("Data Source=WileySchool.db;Version=3;"))
            {
                connection.Open();
                var sql = @"
                    DELETE FROM Trips WHERE Date < date('now', '-1 year');
                    DELETE FROM FuelRecords WHERE Date < date('now', '-1 year');
                    VACUUM;";
                connection.Execute(sql);
            }
        }
    }

    // Placeholder model classes (replace with actual definitions if they exist)
    public class DatabaseStatistics
    {
        public int TripCount { get; set; }
        public int DriverCount { get; set; }
        public int BusCount { get; set; }
        public int RouteCount { get; set; }
        public int FuelRecordCount { get; set; }
        public int ActivityCount { get; set; }
    }
}