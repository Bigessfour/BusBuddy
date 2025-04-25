#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8605 // Unboxing a possibly null value.

namespace BusBuddy.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using BusBuddy.Data.Exceptions;
    using BusBuddy.Models;
    using Dapper;
    using Serilog;

    public static class FuelSchema
    {
        public const string TableName = "Fuel";
        public const string FuelID = "FuelID";
        public const string FuelDate = "FuelDate";
        public const string BusNumber = "BusNumber";
        public const string Gallons = "FuelGallons";
        public const string Odometer = "OdometerReading";
        public const string Notes = "Notes";
    }

    public class DatabaseManager : IDatabaseManager
    {
        private readonly Serilog.ILogger _logger;
        private readonly string _dbFilePath;
        private readonly string _connectionString;
        private readonly string _initDbScriptPath;
        
        private const string DefaultDbFileName = "WileySchool.db";
        private const string DefaultInitScriptName = "init-db.sql";

        private readonly Dictionary<string, List<string>> _requiredTableSchema = new Dictionary<string, List<string>>
        {
            { "Fuel", new List<string> { FuelSchema.FuelID, FuelSchema.FuelDate, FuelSchema.BusNumber, FuelSchema.Gallons, FuelSchema.Odometer, FuelSchema.Notes } }
        };

        private readonly SortedDictionary<int, Action<SQLiteConnection, SQLiteTransaction>> _migrations = new SortedDictionary<int, Action<SQLiteConnection, SQLiteTransaction>>
        {
            { 1, (conn, trans) =>
                {
                    conn.Execute(@"
                        DROP TABLE IF EXISTS SchemaVersion;
                        CREATE TABLE SchemaVersion (
                            RowId INTEGER PRIMARY KEY,
                            Version INTEGER NOT NULL DEFAULT 0
                        );
                        INSERT OR IGNORE INTO SchemaVersion (RowId, Version) VALUES (1, 1);
                    ", transaction: trans);
                }
            },
            { 2, (conn, trans) =>
                {
                    // Ensure Routes table exists
                    var tableExists = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Routes';", transaction: trans) > 0;
                    if (!tableExists)
                    {
                        conn.Execute(@"
                            CREATE TABLE Routes (
                                RouteID INTEGER PRIMARY KEY AUTOINCREMENT,
                                RouteName TEXT NOT NULL,
                                DefaultBusNumber INTEGER,
                                DefaultDriverID INTEGER,
                                Description TEXT,
                                StartTime TEXT NOT NULL,
                                EndTime TEXT NOT NULL,
                                FOREIGN KEY (DefaultBusNumber) REFERENCES Vehicles (BusNumber),
                                FOREIGN KEY (DefaultDriverID) REFERENCES Drivers (DriverID)
                            );", transaction: trans);
                    }

                    var vehicleColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Vehicles')", transaction: trans).ToList();
                    if (!vehicleColumns.Contains("PurchaseDate"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN PurchaseDate TEXT;", transaction: trans);
                    if (!vehicleColumns.Contains("PlateNumber"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN PlateNumber TEXT;", transaction: trans);

                    var routeColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Routes')", transaction: trans).ToList();
                    if (!routeColumns.Contains("RouteName"))
                        conn.Execute("ALTER TABLE Routes ADD COLUMN RouteName TEXT;", transaction: trans);

                    conn.Execute("UPDATE SchemaVersion SET Version = 2 WHERE RowId = 1;", transaction: trans);
                }
            },
            { 3, (conn, trans) =>
                {
                    var tripColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Trips')", transaction: trans).ToList();
                    if (!tripColumns.Contains("TripID"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN TripID INTEGER PRIMARY KEY AUTOINCREMENT;", transaction: trans);
                    if (!tripColumns.Contains("TripType"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN TripType TEXT;", transaction: trans);
                    if (!tripColumns.Contains("Date"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN Date TEXT;", transaction: trans);
                    if (!tripColumns.Contains("BusNumber"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN BusNumber INTEGER;", transaction: trans);
                    if (!tripColumns.Contains("DriverName"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN DriverName TEXT;", transaction: trans);
                    if (!tripColumns.Contains("StartTime"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN StartTime TEXT;", transaction: trans);
                    if (!tripColumns.Contains("EndTime"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN EndTime TEXT;", transaction: trans);
                    if (!tripColumns.Contains("Total_Hours_Driven"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN Total_Hours_Driven REAL;", transaction: trans);
                    if (!tripColumns.Contains("Destination"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN Destination TEXT;", transaction: trans);

                    var driverColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Drivers')", transaction: trans).ToList();
                    if (!driverColumns.Contains("DriverID"))
                        conn.Execute("ALTER TABLE Drivers ADD COLUMN DriverID INTEGER PRIMARY KEY AUTOINCREMENT;", transaction: trans);
                    if (!driverColumns.Contains("DriverName"))
                        conn.Execute("ALTER TABLE Drivers ADD COLUMN DriverName TEXT;", transaction: trans);
                    if (!driverColumns.Contains("PhoneNumber"))
                        conn.Execute("ALTER TABLE Drivers ADD COLUMN PhoneNumber TEXT;", transaction: trans);
                    if (!driverColumns.Contains("EmailAddress"))
                        conn.Execute("ALTER TABLE Drivers ADD COLUMN EmailAddress TEXT;", transaction: trans);
                    if (!driverColumns.Contains("IsStipendPaid"))
                        conn.Execute("ALTER TABLE Drivers ADD COLUMN IsStipendPaid BOOLEAN;", transaction: trans);
                    if (!driverColumns.Contains("DLType"))
                        conn.Execute("ALTER TABLE Drivers ADD COLUMN DLType TEXT;", transaction: trans);

                    var vehicleColumns3 = conn.Query<string>("SELECT name FROM pragma_table_info('Vehicles')", transaction: trans).ToList();
                    if (!vehicleColumns3.Contains("Make"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN Make TEXT;", transaction: trans);
                    if (!vehicleColumns3.Contains("Model"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN Model TEXT;", transaction: trans);
                    if (!vehicleColumns3.Contains("ModelYear"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN ModelYear INTEGER;", transaction: trans);
                    if (!vehicleColumns3.Contains("SeatingCapacity"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN SeatingCapacity INTEGER;", transaction: trans);
                    if (!vehicleColumns3.Contains("IsOperational"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN IsOperational INTEGER DEFAULT 1;", transaction: trans);

                    var maintenanceTableExists = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Maintenance';", transaction: trans) > 0;
                    
                    if (!maintenanceTableExists)
                    {
                        conn.Execute(@"
                            CREATE TABLE Maintenance (
                                MaintenanceID INTEGER PRIMARY KEY AUTOINCREMENT,
                                BusNumber INTEGER NOT NULL,
                                DatePerformed TEXT NOT NULL,
                                Description TEXT NOT NULL,
                                Cost REAL DEFAULT 0,
                                OdometerReading INTEGER DEFAULT 0,
                                FOREIGN KEY (BusNumber) REFERENCES Vehicles (BusNumber)
                            );", transaction: trans);
                    }
                    else
                    {
                        var maintenanceColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Maintenance')", transaction: trans).ToList();
                        if (!maintenanceColumns.Contains("MaintenanceID"))
                            conn.Execute("ALTER TABLE Maintenance ADD COLUMN MaintenanceID INTEGER PRIMARY KEY AUTOINCREMENT;", transaction: trans);
                        if (!maintenanceColumns.Contains("BusNumber"))
                            conn.Execute("ALTER TABLE Maintenance ADD COLUMN BusNumber INTEGER NOT NULL DEFAULT 0;", transaction: trans);
                        if (!maintenanceColumns.Contains("DatePerformed"))
                            conn.Execute("ALTER TABLE Maintenance ADD COLUMN DatePerformed TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP;", transaction: trans);
                        if (!maintenanceColumns.Contains("Description"))
                            conn.Execute("ALTER TABLE Maintenance ADD COLUMN Description TEXT NOT NULL DEFAULT '';", transaction: trans);
                        if (!maintenanceColumns.Contains("Cost"))
                            conn.Execute("ALTER TABLE Maintenance ADD COLUMN Cost REAL DEFAULT 0;", transaction: trans);
                        if (!maintenanceColumns.Contains("OdometerReading"))
                            conn.Execute("ALTER TABLE Maintenance ADD COLUMN OdometerReading INTEGER DEFAULT 0;", transaction: trans);
                        
                        if (maintenanceColumns.Contains("ID") && !maintenanceColumns.Contains("MaintenanceID"))
                            conn.Execute("ALTER TABLE Maintenance RENAME COLUMN ID TO MaintenanceID;", transaction: trans);
                        if (maintenanceColumns.Contains("Bus Number") && !maintenanceColumns.Contains("BusNumber"))
                            conn.Execute("ALTER TABLE Maintenance RENAME COLUMN \"Bus Number\" TO BusNumber;", transaction: trans);
                        if (maintenanceColumns.Contains("Date of Service") && !maintenanceColumns.Contains("DatePerformed"))
                            conn.Execute("ALTER TABLE Maintenance RENAME COLUMN \"Date of Service\" TO DatePerformed;", transaction: trans);
                        if (maintenanceColumns.Contains("Service Done") && !maintenanceColumns.Contains("Description"))
                            conn.Execute("ALTER TABLE Maintenance RENAME COLUMN \"Service Done\" TO Description;", transaction: trans);
                        if (maintenanceColumns.Contains("Mileage") && !maintenanceColumns.Contains("OdometerReading"))
                            conn.Execute("ALTER TABLE Maintenance RENAME COLUMN Mileage TO OdometerReading;", transaction: trans);
                    }

                    conn.Execute("UPDATE SchemaVersion SET Version = 3 WHERE RowId = 1;", transaction: trans);
                }
            },
            { 4, (conn, trans) =>
                {
                    // Update column names to new schema
                    var tripColumns4 = conn.Query<string>("SELECT name FROM pragma_table_info('Trips')", transaction: trans).ToList();
                    if (tripColumns4.Contains("Date") && !tripColumns4.Contains("TripDate"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN Date TO TripDate;", transaction: trans);
                    if (tripColumns4.Contains("Total_Hours_Driven") && !tripColumns4.Contains("TotalHoursDriven"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN Total_Hours_Driven TO TotalHoursDriven;", transaction: trans);
                    if (tripColumns4.Contains("AM_Begin_Mileage") && !tripColumns4.Contains("AMBeginMileage"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN AM_Begin_Mileage TO AMBeginMileage;", transaction: trans);
                    if (tripColumns4.Contains("AM_End_Mileage") && !tripColumns4.Contains("AMEndMileage"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN AM_End_Mileage TO AMEndMileage;", transaction: trans);
                    if (tripColumns4.Contains("PM_Start_Mileage") && !tripColumns4.Contains("PMStartMileage"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN PM_Start_Mileage TO PMStartMileage;", transaction: trans);
                    if (tripColumns4.Contains("PM_Ending_Mileage") && !tripColumns4.Contains("PMEndingMileage"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN PM_Ending_Mileage TO PMEndingMileage;", transaction: trans);
                    if (tripColumns4.Contains("Num_Riders") && !tripColumns4.Contains("NumRiders"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN Num_Riders TO NumRiders;", transaction: trans);
                    if (tripColumns4.Contains("Num_PM_Riders") && !tripColumns4.Contains("NumPMRiders"))
                        conn.Execute("ALTER TABLE Trips RENAME COLUMN Num_PM_Riders TO NumPMRiders;", transaction: trans);
                    if (!tripColumns4.Contains("DriverID"))
                        conn.Execute("ALTER TABLE Trips ADD COLUMN DriverID INTEGER;", transaction: trans);

                    var driverColumns4 = conn.Query<string>("SELECT name FROM pragma_table_info('Drivers')", transaction: trans).ToList();
                    if (driverColumns4.Contains("Driver Name") && !driverColumns4.Contains("DriverName"))
                        conn.Execute("ALTER TABLE Drivers RENAME COLUMN \"Driver Name\" TO DriverName;", transaction: trans);
                    if (driverColumns4.Contains("Phone Number") && !driverColumns4.Contains("PhoneNumber"))
                        conn.Execute("ALTER TABLE Drivers RENAME COLUMN \"Phone Number\" TO PhoneNumber;", transaction: trans);
                    if (driverColumns4.Contains("Email Address") && !driverColumns4.Contains("EmailAddress"))
                        conn.Execute("ALTER TABLE Drivers RENAME COLUMN \"Email Address\" TO EmailAddress;", transaction: trans);
                    if (driverColumns4.Contains("Zip Code") && !driverColumns4.Contains("ZipCode"))
                        conn.Execute("ALTER TABLE Drivers RENAME COLUMN \"Zip Code\" TO ZipCode;", transaction: trans);
                    if (driverColumns4.Contains("Is Stipend Paid") && !driverColumns4.Contains("IsStipendPaid"))
                        conn.Execute("ALTER TABLE Drivers RENAME COLUMN \"Is Stipend Paid\" TO IsStipendPaid;", transaction: trans);
                    if (driverColumns4.Contains("DL Type") && !driverColumns4.Contains("DLType"))
                        conn.Execute("ALTER TABLE Drivers RENAME COLUMN \"DL Type\" TO DLType;", transaction: trans);

                    var vehicleColumns4 = conn.Query<string>("SELECT name FROM pragma_table_info('Vehicles')", transaction: trans).ToList();
                    if (vehicleColumns4.Contains("Bus Number") && !vehicleColumns4.Contains("BusNumber"))
                        conn.Execute("ALTER TABLE Vehicles RENAME COLUMN \"Bus Number\" TO BusNumber;", transaction: trans);
                    if (vehicleColumns4.Contains("Model Year") && !vehicleColumns4.Contains("ModelYear"))
                        conn.Execute("ALTER TABLE Vehicles RENAME COLUMN \"Model Year\" TO ModelYear;", transaction: trans);
                    if (vehicleColumns4.Contains("Seating Capacity") && !vehicleColumns4.Contains("SeatingCapacity"))
                        conn.Execute("ALTER TABLE Vehicles RENAME COLUMN \"Seating Capacity\" TO SeatingCapacity;", transaction: trans);
                    if (!vehicleColumns4.Contains("PurchasePrice"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN PurchasePrice REAL;", transaction: trans);
                    if (!vehicleColumns4.Contains("AnnualInspection"))
                        conn.Execute("ALTER TABLE Vehicles ADD COLUMN AnnualInspection TEXT;", transaction: trans);

                    var fuelColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Fuel')", transaction: trans).ToList();
                    if (fuelColumns.Contains("Fuel_ID") && !fuelColumns.Contains("FuelID"))
                        conn.Execute("ALTER TABLE Fuel RENAME COLUMN Fuel_ID TO FuelID;", transaction: trans);
                    if (fuelColumns.Contains("Bus Number") && !fuelColumns.Contains("BusNumber"))
                        conn.Execute("ALTER TABLE Fuel RENAME COLUMN \"Bus Number\" TO BusNumber;", transaction: trans);
                    if (fuelColumns.Contains("Fuel Date") && !fuelColumns.Contains("FuelDate"))
                        conn.Execute("ALTER TABLE Fuel RENAME COLUMN \"Fuel Date\" TO FuelDate;", transaction: trans);
                    if (fuelColumns.Contains("Fuel Gallons") && !fuelColumns.Contains("FuelGallons"))
                        conn.Execute("ALTER TABLE Fuel RENAME COLUMN \"Fuel Gallons\" TO FuelGallons;", transaction: trans);
                    if (fuelColumns.Contains("Odometer Reading") && !fuelColumns.Contains("OdometerReading"))
                        conn.Execute("ALTER TABLE Fuel RENAME COLUMN \"Odometer Reading\" TO OdometerReading;", transaction: trans);

                    var activityColumns = conn.Query<string>("SELECT name FROM pragma_table_info('Activities')", transaction: trans).ToList();
                    if (activityColumns.Contains("Date") && !activityColumns.Contains("ActivityDate"))
                        conn.Execute("ALTER TABLE Activities RENAME COLUMN Date TO ActivityDate;", transaction: trans);
                    if (!activityColumns.Contains("DriverID"))
                        conn.Execute("ALTER TABLE Activities ADD COLUMN DriverID INTEGER;", transaction: trans);

                    var routeColumns4 = conn.Query<string>("SELECT name FROM pragma_table_info('Routes')", transaction: trans).ToList();
                    if (!routeColumns4.Contains("DefaultDriverID"))
                        conn.Execute("ALTER TABLE Routes ADD COLUMN DefaultDriverID INTEGER;", transaction: trans);

                    var calendarColumns = conn.Query<string>("SELECT name FROM pragma_table_info('SchoolCalendarDays')", transaction: trans).ToList();
                    if (calendarColumns.Contains("Date") && !calendarColumns.Contains("CalendarDate"))
                        conn.Execute("ALTER TABLE SchoolCalendarDays RENAME COLUMN Date TO CalendarDate;", transaction: trans);
                    if (calendarColumns.Contains("CalendarDayId") && !calendarColumns.Contains("CalendarDayID"))
                        conn.Execute("ALTER TABLE SchoolCalendarDays RENAME COLUMN CalendarDayId TO CalendarDayID;", transaction: trans);

                    var scheduledRouteColumns = conn.Query<string>("SELECT name FROM pragma_table_info('ScheduledRoutes')", transaction: trans).ToList();
                    if (scheduledRouteColumns.Contains("ScheduledRouteId") && !scheduledRouteColumns.Contains("ScheduledRouteID"))
                        conn.Execute("ALTER TABLE ScheduledRoutes RENAME COLUMN ScheduledRouteId TO ScheduledRouteID;", transaction: trans);
                    if (scheduledRouteColumns.Contains("CalendarDayId") && !scheduledRouteColumns.Contains("CalendarDayID"))
                        conn.Execute("ALTER TABLE ScheduledRoutes RENAME COLUMN CalendarDayId TO CalendarDayID;", transaction: trans);
                    if (scheduledRouteColumns.Contains("RouteId") && !scheduledRouteColumns.Contains("RouteID"))
                        conn.Execute("ALTER TABLE ScheduledRoutes RENAME COLUMN RouteId TO RouteID;", transaction: trans);
                    if (!scheduledRouteColumns.Contains("AssignedDriverID"))
                        conn.Execute("ALTER TABLE ScheduledRoutes ADD COLUMN AssignedDriverID INTEGER;", transaction: trans);

                    // Update SchemaVersion
                    conn.Execute("UPDATE SchemaVersion SET Version = 4 WHERE RowId = 1;", transaction: trans);
                }
            }
        };

        public DatabaseManager(Serilog.ILogger logger, Microsoft.Extensions.Configuration.IConfiguration? configuration = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            string dbFileName = DefaultDbFileName;
            string initScriptName = DefaultInitScriptName;
            if (configuration != null)
            {
                var section = configuration.GetSection("Database");
                string? fileName = section["FileName"];
                if (!string.IsNullOrEmpty(fileName))
                {
                    dbFileName = fileName;
                }
                string? scriptName = section["InitScript"];
                if (!string.IsNullOrEmpty(scriptName))
                {
                    initScriptName = scriptName;
                }
            }
            _dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName);
            _initDbScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, initScriptName);
            
            _connectionString = $"Data Source={_dbFilePath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                bool dbExists = File.Exists(_dbFilePath);
                if (!dbExists)
                {
                    _logger.Information("Database file not found at {Path}. Creating...", _dbFilePath);
                    SQLiteConnection.CreateFile(_dbFilePath);
                    _logger.Information("Database file created.");

                    using var connection = new SQLiteConnection(_connectionString);
                    connection.Open();
                    if (!File.Exists(_initDbScriptPath))
                    {
                        _logger.Warning("init-db.sql not found at {Path}.", _initDbScriptPath);
                        throw new FileNotFoundException("init-db.sql not found.", _initDbScriptPath);
                    }
                    string script = File.ReadAllText(_initDbScriptPath);
                    connection.Execute(script);
                    _logger.Information("Database initialized with init-db.sql.");
                }

                using var migrationConnection = new SQLiteConnection(_connectionString);
                migrationConnection.Open();
                ApplyPendingMigrations(migrationConnection);
                Cleanup(migrationConnection);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "A critical error occurred during database initialization.");
                throw;
            }
        }

        private void ApplyPendingMigrations(SQLiteConnection connection)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var schemaVersionRowCount = connection.ExecuteScalar<int>(
                        "SELECT COUNT(*) FROM SchemaVersion WHERE RowId = 1;", transaction: transaction);
                    if (schemaVersionRowCount == 0)
                    {
                        connection.Execute("INSERT INTO SchemaVersion (RowId, Version) VALUES (1, 0);", transaction: transaction);
                        _logger.Information("Initialized SchemaVersion with version 0.");
                    }

                    int currentVersion = connection.ExecuteScalar<int>(
                        "SELECT Version FROM SchemaVersion WHERE RowId = 1;", transaction: transaction);
                    _logger.Information("Current database schema version: {Version}", currentVersion);

                    var pendingMigrations = _migrations.Where(m => m.Key > currentVersion).ToList();
                    if (!pendingMigrations.Any())
                    {
                        _logger.Information("No pending migrations.");
                        transaction.Commit();
                        return;
                    }

                    _logger.Information("Found {Count} pending migrations.", pendingMigrations.Count);

                    foreach (var migration in pendingMigrations)
                    {
                        _logger.Information("Applying migration {Version}...", migration.Key);
                        migration.Value(connection, transaction);
                        int newVersion = connection.ExecuteScalar<int>(
                            "SELECT Version FROM SchemaVersion WHERE RowId = 1;", transaction: transaction);
                        _logger.Information("Successfully applied migration {Version}. New schema version: {NewVersion}", migration.Key, newVersion);
                        currentVersion = newVersion;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.Error(ex, "Failed to apply migrations.");
                    throw;
                }
            }
        }

        public void Cleanup(SQLiteConnection connection)
        {
            _logger.Information("Running database cleanup...");
            var tableExists = connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Fuel';") > 0;
            if (tableExists)
            {
                try
                {
                    connection.Execute(
                        "DELETE FROM Fuel WHERE FuelDate < date('now', '-1 year');" +
                        "VACUUM;");
                    _logger.Information("Successfully cleaned up old Fuel records and vacuumed database.");
                }
                catch (SQLiteException ex)
                {
                    _logger.Error(ex, "Error during database cleanup.");
                }
            }
            else
            {
                _logger.Warning("Fuel table does not exist. Skipping cleanup.");
            }
        }

        protected virtual T ExecuteWithRetry<T>(Func<IDbConnection, T> action, string operationName, int retries = 3, T defaultValue = default, bool failSilently = false)
        {
            for (int i = 0; i <= retries; i++)
            {
                try
                {
                    using (var connection = new SQLiteConnection(_connectionString))
                    {
                        connection.Open();
                        return action(connection);
                    }
                }
                catch (SQLiteException ex) when (ex.ErrorCode == (int)SQLiteErrorCode.Busy || ex.ErrorCode == (int)SQLiteErrorCode.Locked)
                {
                    if (i == retries)
                    {
                        if (failSilently) return defaultValue;
                        throw;
                    }
                    System.Threading.Thread.Sleep(100 * (i + 1));
                }
                catch (SQLiteException ex) when (ex.Message.Contains("no such table") || ex.Message.Contains("no such column"))
                {
                    if (failSilently) return defaultValue;
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error during {Operation}", operationName);
                    if (failSilently) return defaultValue;
                    throw;
                }
            }
            throw new InvalidOperationException("Should not reach here.");
        }

        protected virtual List<T> ExecuteWithRetryList<T>(Func<IDbConnection, List<T>> action, string operationName, int retries = 3, List<T>? defaultValue = null, bool failSilently = false)
        {
            return ExecuteWithRetry(action, operationName, retries, defaultValue ?? new List<T>(), failSilently);
        }

        protected virtual bool ExecuteWithRetryValue(Func<IDbConnection, bool> action, string operationName, int retries = 3, bool defaultValue = false, bool failSilently = false)
        {
            return ExecuteWithRetry(action, operationName, retries, defaultValue, failSilently);
        }

        public List<Trip> GetTrips()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<Trip>(@"SELECT 
                    TripID, 
                    TripType, 
                    TripDate, 
                    StartTime, 
                    EndTime, 
                    Destination, 
                    BusNumber, 
                    DriverID, 
                    AMBeginMileage, 
                    AMEndMileage, 
                    NumRiders, 
                    PMStartMileage, 
                    PMEndingMileage, 
                    NumPMRiders, 
                    TotalHoursDriven
                FROM Trips ORDER BY StartTime").ToList(),
                "GetTrips");
        }

        public List<string> GetDriverNames()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<string>("SELECT DriverName FROM Drivers ORDER BY DriverName").ToList(),
                "GetDriverNames");
        }

        public List<int> GetBusNumbers()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<int>("SELECT BusNumber FROM Vehicles ORDER BY BusNumber").ToList(),
                "GetBusNumbers");
        }

        public List<Vehicle> GetVehicles()
        {
            try
            {
                return ExecuteWithRetryList(conn =>
                    conn.Query<Vehicle>(@"SELECT 
                        VehicleID, 
                        BusNumber, 
                        ModelYear AS Year, 
                        VIN, 
                        Make, 
                        Model, 
                        SeatingCapacity AS Capacity
                    FROM Vehicles ORDER BY BusNumber").ToList(),
                    "GetVehicles");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping Vehicle data. Check DB column types and model properties.");
                throw;
            }
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            return ExecuteWithRetryValue(conn =>
            {
                var sql = @"
                    UPDATE Vehicles 
                    SET BusNumber = @BusNumber, 
                        Make = @Make, 
                        Model = @Model, 
                        ModelYear = @Year, 
                        VIN = @VIN, 
                        PlateNumber = @PlateNumber, 
                        SeatingCapacity = @Capacity, 
                        IsOperational = @IsOperational, 
                        PurchaseDate = @PurchaseDate, 
                        LastInspectionDate = @LastInspectionDate, 
                        CurrentOdometer = @CurrentOdometer 
                    WHERE VehicleID = @VehicleID";
                return conn.Execute(sql, vehicle) > 0;
            }, "UpdateVehicle");
        }

        public bool AddVehicle(Vehicle vehicle)
        {
            return ExecuteWithRetryValue(conn =>
            {
                var sql = @"
                    INSERT INTO Vehicles (
                        BusNumber, Make, Model, ModelYear, VIN, PlateNumber, 
                        SeatingCapacity, IsOperational, PurchaseDate, LastInspectionDate, CurrentOdometer
                    ) VALUES (
                        @BusNumber, @Make, @Model, @Year, @VIN, @PlateNumber, 
                        @Capacity, @IsOperational, @PurchaseDate, @LastInspectionDate, @CurrentOdometer
                    )";
                return conn.Execute(sql, vehicle) > 0;
            }, "AddVehicle");
        }

        public bool DeleteVehicle(int vehicleId)
        {
            return ExecuteWithRetryValue(conn =>
                conn.Execute("DELETE FROM Vehicles WHERE VehicleID = @VehicleID", new { VehicleID = vehicleId }) > 0,
                "DeleteVehicle");
        }

        public List<Route> GetRoutes()
        {
            try
            {
                return ExecuteWithRetryList(conn =>
                    conn.Query<Route>(@"SELECT 
                        RouteID, 
                        RouteName, 
                        Description, 
                        StartTime, 
                        EndTime, 
                        CAST(DefaultBusNumber AS INTEGER) AS DefaultBusNumber
                    FROM Routes ORDER BY RouteName").ToList(),
                    "GetRoutes");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping Route data. Check DB column types and model properties.");
                throw;
            }
        }

        public bool AddFuelRecord(FuelRecord record)
        {
            return ExecuteWithRetryValue(conn =>
            {
                var sql = $@"INSERT INTO {FuelSchema.TableName} 
                    ({FuelSchema.FuelDate}, {FuelSchema.BusNumber}, 
                    {FuelSchema.Gallons}, {FuelSchema.Odometer}, {FuelSchema.Notes})
                    VALUES (@FuelDate, @BusNumber, @Gallons, @Odometer, @Notes)";
                return conn.Execute(sql, record) > 0;
            }, "AddFuelRecord");
        }

        public List<Driver> GetDrivers()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<Driver>(@"SELECT 
                    DriverID, 
                    DriverName AS Name, 
                    Address, 
                    City, 
                    State, 
                    ZipCode, 
                    PhoneNumber, 
                    EmailAddress, 
                    IsStipendPaid, 
                    DLType
                FROM Drivers ORDER BY DriverName").ToList(),
                "GetDrivers");
        }

        public void AddOrUpdateCalendarDay(SchoolCalendarDay day)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = @"
                    INSERT INTO SchoolCalendarDays (CalendarDate, IsSchoolDay, DayType, Notes)
                    VALUES (@DateStr, @IsSchoolDay, @DayTypeStr, @Notes)
                    ON CONFLICT(CalendarDate) DO UPDATE SET
                        IsSchoolDay = excluded.IsSchoolDay,
                        DayType = excluded.DayType,
                        Notes = excluded.Notes";
                
                var parameters = new
                {
                    DateStr = day.Date.ToString("yyyy-MM-dd"),
                    day.IsSchoolDay,
                    DayTypeStr = day.DayType.ToString(),
                    day.Notes
                };

                var calendarDayId = conn.ExecuteScalar<int>(sql + "; SELECT last_insert_rowid();", parameters);
                return calendarDayId;
            }, "AddOrUpdateCalendarDay");
        }

        public void AddOrUpdateCalendarDays(IEnumerable<SchoolCalendarDay> days)
        {
            ExecuteWithRetry(conn =>
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var sql = @"
                            INSERT INTO SchoolCalendarDays (CalendarDate, IsSchoolDay, DayType, Notes)
                            VALUES (@DateStr, @IsSchoolDay, @DayTypeStr, @Notes)
                            ON CONFLICT(CalendarDate) DO UPDATE SET
                                IsSchoolDay = excluded.IsSchoolDay,
                                DayType = excluded.DayType,
                                Notes = excluded.Notes";

                        var parameters = days.Select(day => new
                        {
                            DateStr = day.Date.ToString("yyyy-MM-dd"),
                            day.IsSchoolDay,
                            DayTypeStr = day.DayType.ToString(),
                            day.Notes
                        }).ToList();

                        conn.Execute(sql, parameters, transaction: transaction);
                        transaction.Commit();
                        _logger.Information("Successfully saved {Count} calendar days.", parameters.Count);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error during bulk calendar day update. Rolling back transaction.");
                        transaction.Rollback();
                        throw;
                    }
                }
            }, "AddOrUpdateCalendarDays");
        }

        public SchoolCalendarDay GetCalendarDay(DateTime date)
        {
            return ExecuteWithRetry(conn =>
            {
                var rawData = conn.QueryFirstOrDefault<dynamic>(
                    "SELECT * FROM SchoolCalendarDays WHERE CalendarDate = @DateStr",
                    new { DateStr = date.ToString("yyyy-MM-dd") });

                if (rawData == null) return null;

                int calendarDayId = (int)rawData.CalendarDayID;
                string calendarDate = (string)rawData.CalendarDate;
                bool isSchoolDay = Convert.ToBoolean(rawData.IsSchoolDay);
                string dayTypeStr = rawData.DayType?.ToString();
                string notes = rawData.Notes?.ToString() ?? string.Empty;
                
                var day = new SchoolCalendarDay(calendarDayId, calendarDate, isSchoolDay, dayTypeStr, notes);

                return day;
            },
            "GetCalendarDay");
        }

        public List<ScheduledRoute> GetScheduledRoutes(int calendarDayId)
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<ScheduledRoute>(
                    "SELECT * FROM ScheduledRoutes WHERE CalendarDayID = @Id",
                    new { Id = calendarDayId }).ToList(),
                "GetScheduledRoutes by ID");
        }

        public List<ScheduledRoute> GetScheduledRoutes(DateTime date)
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<ScheduledRoute>(
                    "SELECT sr.* FROM ScheduledRoutes sr JOIN SchoolCalendarDays sc ON sr.CalendarDayID = sc.CalendarDayID WHERE sc.CalendarDate = @Date",
                    new { Date = date.ToString("yyyy-MM-dd") }).ToList(),
                "GetScheduledRoutes by Date");
        }

        public bool AddDriver(Driver driver)
        {
            return ExecuteWithRetryValue(conn =>
            {
                var sql = "INSERT INTO Drivers (DriverName, PhoneNumber, EmailAddress, IsStipendPaid, DLType) VALUES (@Name, @PhoneNumber, @EmailAddress, @IsStipendPaid, @DLType)";
                return conn.Execute(sql, driver) > 0;
            }, "AddDriver");
        }

        public List<Activity> GetActivities()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<Activity>("SELECT * FROM Activities ORDER BY ActivityDate").ToList(),
                "GetActivities");
        }

        public void UpdateScheduledRoute(ScheduledRoute route)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = @"
                    UPDATE ScheduledRoutes SET
                        RouteID = @RouteID,
                        AssignedBusNumber = @AssignedBusNumber,
                        AssignedDriverID = @AssignedDriverID
                    WHERE ScheduledRouteID = @ScheduledRouteID";
                return conn.Execute(sql, route);
            }, "UpdateScheduledRoute");
        }

        public void AddActivity(Activity activity)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = "INSERT INTO Activities (ActivityDate, BusNumber, Destination, LeaveTime, DriverID, HoursDriven, StudentsDriven) VALUES (@ActivityDate, @BusNumber, @Destination, @LeaveTime, @DriverID, @HoursDriven, @StudentsDriven)";
                return conn.Execute(sql, activity);
            }, "AddActivity");
        }

        public void AddTrip(Trip trip)
        {
            ExecuteWithRetry(conn =>
            {
                // Use property names that match the Trip model
                var sql = "INSERT INTO Trips (TripType, TripDate, BusNumber, DriverName, StartTime, EndTime, TotalHoursDriven, Destination) VALUES (@TripType, @Date, @BusNumber, @DriverName, @StartTime, @EndTime, @TotalHoursDriven, @Destination)";
                
                // Create parameters explicitly to handle property name mapping
                var parameters = new
                {
                    trip.TripType,
                    Date = trip.Date.ToString("yyyy-MM-dd"),
                    trip.BusNumber,
                    trip.DriverName,
                    StartTime = trip.StartTime.ToString("HH:mm"),
                    EndTime = trip.EndTime.ToString("HH:mm"),
                    trip.TotalHoursDriven,
                    trip.Destination
                };
                
                return conn.Execute(sql, parameters);
            }, "AddTrip");
        }

        public List<Trip> GetTripsByDate(DateOnly date)
        {
            return ExecuteWithRetryList(conn =>
            {
                var sql = @"SELECT 
                    TripID, 
                    TripType, 
                    TripDate, 
                    StartTime, 
                    EndTime, 
                    Destination, 
                    BusNumber, 
                    DriverName, 
                    AMBeginMileage, 
                    AMEndMileage, 
                    NumRiders, 
                    PMStartMileage, 
                    PMEndingMileage, 
                    NumPMRiders, 
                    TotalHoursDriven
                FROM Trips WHERE TripDate = @Date ORDER BY StartTime";
                
                var trips = conn.Query<dynamic>(sql, new { Date = date.ToString("yyyy-MM-dd") }).ToList();
                
                // Convert the dynamic results to Trip objects
                var result = trips.Select(t => new Trip
                {
                    TripID = t.TripID,
                    TripType = t.TripType,
                    Date = DateOnly.Parse(t.TripDate.ToString()),
                    BusNumber = t.BusNumber,
                    DriverName = t.DriverName?.ToString() ?? string.Empty,
                    StartTime = TimeOnly.TryParse(t.StartTime?.ToString(), out TimeOnly start) ? start : TimeOnly.MinValue,
                    EndTime = TimeOnly.TryParse(t.EndTime?.ToString(), out TimeOnly end) ? end : TimeOnly.MinValue,
                    Destination = t.Destination?.ToString() ?? string.Empty,
                    TotalHoursDriven = t.TotalHoursDriven ?? 0.0
                }).ToList();
                
                return result;
            }, "GetTripsByDate");
        }

        public DatabaseStatistics GetDatabaseStatistics()
        {
            return ExecuteWithRetry(conn =>
            {
                return new DatabaseStatistics
                {
                    TotalTrips = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Trips"),
                    TotalDrivers = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Drivers"),
                    TotalRoutes = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Routes"),
                    TotalFuelRecords = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Fuel"),
                    TotalMaintenanceRecords = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Maintenance"),
                    TotalActivities = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Activities"),
                    Timestamp = DateTime.Now
                };
            }, "GetDatabaseStatistics");
        }

        public List<FuelRecord> GetFuelRecords()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<FuelRecord>("SELECT * FROM Fuel ORDER BY FuelDate DESC").ToList(),
                "GetFuelRecords");
        }

        public bool UpdateFuelRecord(FuelRecord record)
        {
            return ExecuteWithRetryValue(conn =>
            {
                var sql = $@"UPDATE {FuelSchema.TableName} SET
                    {FuelSchema.FuelDate} = @FuelDate,
                    {FuelSchema.BusNumber} = @BusNumber,
                    {FuelSchema.Gallons} = @Gallons,
                    {FuelSchema.Odometer} = @Odometer,
                    {FuelSchema.Notes} = @Notes
                    WHERE {FuelSchema.FuelID} = @FuelID";
                return conn.Execute(sql, record) > 0;
            }, "UpdateFuelRecord");
        }

        public bool DeleteFuelRecord(int recordId)
        {
            return ExecuteWithRetryValue(conn =>
                conn.Execute($"DELETE FROM {FuelSchema.TableName} WHERE {FuelSchema.FuelID} = @Id", new { Id = recordId }) > 0,
                "DeleteFuelRecord");
        }

        public void AddRoute(Route route)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = "INSERT INTO Routes (RouteName, DefaultBusNumber, DefaultDriverID, Description, StartTime, EndTime) VALUES (@RouteName, @DefaultBusNumber, @DefaultDriverID, @Description, @StartTime, @EndTime)";
                return conn.Execute(sql, route);
            }, "AddRoute");
        }

        public void UpdateRoute(Route route)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = "UPDATE Routes SET RouteName = @RouteName, DefaultBusNumber = @DefaultBusNumber, DefaultDriverID = @DefaultDriverID, Description = @Description, StartTime = @StartTime, EndTime = @EndTime WHERE RouteID = @RouteID";
                return conn.Execute(sql, route);
            }, "UpdateRoute");
        }

        public bool DeleteRoute(int routeId)
        {
            return ExecuteWithRetryValue(conn =>
                conn.Execute("DELETE FROM Routes WHERE RouteID = @RouteID", new { RouteID = routeId }) > 0,
                "DeleteRoute");
        }

        public bool UpdateDriver(Driver driver)
        {
            return ExecuteWithRetryValue(conn =>
            {
                var sql = "UPDATE Drivers SET DriverName = @Name, PhoneNumber = @PhoneNumber, EmailAddress = @EmailAddress WHERE DriverID = @DriverID";
                return conn.Execute(sql, driver) > 0;
            }, "UpdateDriver");
        }

        public bool DeleteDriver(int driverId)
        {
            return ExecuteWithRetryValue(conn =>
                conn.Execute("DELETE FROM Drivers WHERE DriverID = @DriverID", new { DriverID = driverId }) > 0,
                "DeleteDriver");
        }

        public List<Maintenance> GetMaintenanceRecords()
        {
            return ExecuteWithRetryList(conn =>
                conn.Query<Maintenance>(
                    @"SELECT MaintenanceID, 
                        BusNumber, 
                        DatePerformed, 
                        Description, 
                        Cost, 
                        OdometerReading 
                    FROM Maintenance ORDER BY DatePerformed DESC").ToList(),
                "GetMaintenanceRecords");
        }

        public void AddMaintenanceRecord(Maintenance maintenance)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = @"INSERT INTO Maintenance 
                    (BusNumber, DatePerformed, Description, Cost, OdometerReading) 
                    VALUES (@BusNumber, @DatePerformed, @Description, @Cost, @OdometerReading)";
                return conn.Execute(sql, maintenance);
            }, "AddMaintenanceRecord");
        }

        public void UpdateMaintenanceRecord(Maintenance maintenance)
        {
            ExecuteWithRetry(conn =>
            {
                var sql = @"UPDATE Maintenance 
                    SET BusNumber = @BusNumber, 
                        DatePerformed = @DatePerformed, 
                        Description = @Description, 
                        Cost = @Cost, 
                        OdometerReading = @OdometerReading 
                    WHERE MaintenanceID = @MaintenanceID";
                return conn.Execute(sql, maintenance);
            }, "UpdateMaintenanceRecord");
        }

        public void DeleteMaintenanceRecord(int maintenanceId)
        {
            ExecuteWithRetry(conn =>
                conn.Execute("DELETE FROM Maintenance WHERE MaintenanceID = @MaintenanceID", new { MaintenanceID = maintenanceId }),
                "DeleteMaintenanceRecord");
        }
    }
}

#pragma warning restore CS8600
#pragma warning restore CS8601
#pragma warning restore CS8602
#pragma warning restore CS8603
#pragma warning restore CS8604
#pragma warning restore CS8605