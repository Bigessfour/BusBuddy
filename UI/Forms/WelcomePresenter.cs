using System;
using System.Collections.Generic;
using BusBuddy.Data;
using BusBuddy.Models;
using BusBuddy.UI.Interfaces;
using Serilog;

namespace BusBuddy.UI.Forms
{
    public class WelcomePresenter
    {
        private readonly IWelcomeView _view;
        private readonly DatabaseManager _databaseManager;

        public WelcomePresenter(IWelcomeView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _databaseManager = new DatabaseManager();
        }

        public List<Trip> GetTodaysTripsData()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                return _databaseManager.GetTripsByDate(today);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error retrieving today's trips data: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public string GetDashboardStats()
        {
            try
            {
                var stats = _databaseManager.GetDatabaseStatistics();
                return $"Trips: {stats["Trips"]}, Drivers: {stats["Drivers"]}, Buses: {stats["Buses"]}, Fuel Records: {stats["FuelRecords"]}, Activities: {stats["Activities"]}";
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error retrieving dashboard stats: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}