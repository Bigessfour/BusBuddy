// UI/Forms/WelcomePresenter.cs
using Serilog;
using BusBuddy.Data;
using BusBuddy.UI.Interfaces;
using System.Collections.Generic;
using BusBuddy.Models;
using System;

public class WelcomePresenter
{
    private readonly IDatabaseManager _dbManager;
    private readonly IWelcomeView? _view; // Make nullable with ? operator

    public WelcomePresenter()
    {
        var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        _dbManager = new DatabaseManager(logger);
    }

    public WelcomePresenter(IWelcomeView view)
    {
        var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        _dbManager = new DatabaseManager(logger);
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }
    
    public List<Trip> GetTodaysTripsData()
    {
        try
        {
            return _dbManager.GetTripsByDate(DateOnly.FromDateTime(DateTime.Today));
        }
        catch (Exception ex)
        {
            // Log error and return empty list
            Log.Error(ex, "Error getting today's trips data");
            return new List<Trip>();
        }
    }
    
    public string GetDashboardStats()
    {
        try
        {
            var stats = _dbManager.GetDatabaseStatistics();
            return $"Drivers: {stats.DriverCount} | " +
                   $"Buses: {stats.BusCount} | " +
                   $"Routes: {stats.RouteCount} | " +
                   $"Trips: {stats.TripCount}";
        }
        catch (Exception ex)
        {
            // Log error and return empty stats
            Log.Error(ex, "Error getting dashboard statistics");
            return "Statistics unavailable";
        }
    }
}