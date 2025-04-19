// BusBuddy/UI/Forms/WelcomePresenter.cs
using System;
using System.Collections.Generic;
using BusBuddy.Models;
using BusBuddy.Data;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public class WelcomePresenter
    {
        private readonly IWelcomeView _view;
        private readonly DatabaseManager _databaseManager;
        private readonly ILogger _logger;

        public WelcomePresenter(IWelcomeView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _databaseManager = new DatabaseManager();
            _logger = Log.Logger;
        }

        public List<Trip> GetTodaysTripsData()
        {
            try
            {
                var todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
                _logger.Information("Fetching trips for date: {Date}", todaysDate);
                
                // Use database manager to get trips for today
                // This is a placeholder - actual implementation would depend on your DatabaseManager
                var trips = _databaseManager.GetTripsByDate(todaysDate);
                
                _logger.Information("Found {Count} trips for today", trips.Count);
                return trips;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching today's trips: {ErrorMessage}", ex.Message);
                return new List<Trip>();
            }
        }

        public string GetDashboardStats()
        {
            try
            {
                // Example stats calculation
                var stats = _databaseManager.GetDatabaseStatistics();
                
                if (stats == null || stats.Count == 0)
                {
                    return "No statistics available";
                }
                
                return $"Trips: {stats.GetValueOrDefault("Trips", 0)}, " +
                       $"Drivers: {stats.GetValueOrDefault("Drivers", 0)}, " +
                       $"Buses: {stats.GetValueOrDefault("Buses", 0)}";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting dashboard stats: {ErrorMessage}", ex.Message);
                return "Statistics unavailable";
            }
        }
    }
}