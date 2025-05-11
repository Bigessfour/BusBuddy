using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite; // Using System.Data.SQLite.Core package
using System.IO;
using Microsoft.Extensions.Logging;
using Dapper;

namespace BusBuddy.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseHelper> _logger;

        public DatabaseHelper(string dbPath, ILogger<DatabaseHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentException("Database path cannot be null or empty", nameof(dbPath));
            
            // Ensure directory exists
            var directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _connectionString = $"Data Source={dbPath};Version=3;";
            
            // Initialize database if it doesn't exist
            if (!File.Exists(dbPath))
            {
                CreateDatabase();
            }
            
            _logger.LogInformation("Database initialized at {DbPath}", dbPath);
        }

        private void CreateDatabase()
        {
            _logger.LogInformation("Creating new database");
            
            SQLiteConnection.CreateFile(_connectionString.Replace("Data Source=", "").Replace(";Version=3;", ""));
            
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            
            // Create tables
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Routes (
                    RouteId INTEGER PRIMARY KEY AUTOINCREMENT,
                    RouteName TEXT NOT NULL,
                    Description TEXT,
                    StartLocation TEXT NOT NULL,
                    EndLocation TEXT NOT NULL,
                    CreatedDate TEXT NOT NULL,
                    LastModified TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();

            _logger.LogInformation("Database schema created");
        }

        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        // Example method to execute queries with error handling
        public T ExecuteWithErrorHandling<T>(Func<IDbConnection, T> query)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                return query(connection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database operation failed");
                throw;
            }
        }
    }
}
