// BusBuddy/Data/DatabaseManager.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Configuration;
using Serilog;
using BusBuddy.Models;

namespace BusBuddy.Data
{
    public class DatabaseManager : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private bool _disposed;

        public DatabaseManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddXmlFile("app.config", optional: true, reloadOnChange: true)
                .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "Data Source=WileySchool.db;Version=3;";
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
            // Placeholder for loading vehicle numbers, used by Inputs.cs
            _logger.Information("Loaded vehicle numbers (placeholder).");
        }

        public void LoadDriverNames()
        {
            // Placeholder for loading driver names, used by Inputs.cs
            _logger.Information("Loaded driver names (placeholder).");
        }

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