// UI/Forms/WelcomePresenter.cs
using Serilog; // Changed back to Serilog
using BusBuddy.Data;
using BusBuddy.UI.Interfaces;
using System.Collections.Generic;
using BusBuddy.Models;
using System;

public class WelcomePresenter
{
    private readonly IDatabaseManager _dbManager;
    private readonly IWelcomeView? _view; // Keep nullable if view is optional or set later
    private readonly ILogger _logger; // Changed type to Serilog.ILogger

    // Constructor for DI, injecting IDatabaseManager and ILogger
    public WelcomePresenter(IDatabaseManager dbManager, ILogger logger, IWelcomeView? view = null) // Changed logger type
    {
        _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _view = view; // Assign view if provided
    }

    public List<Trip> GetTodaysTripsData()
    {
        try
        {
            return _dbManager.GetTripsByDate(DateOnly.FromDateTime(DateTime.Today));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting today's trips data"); // Changed back to Serilog format
            return new List<Trip>();
        }
    }

    public string GetDashboardStats()
    {
        try
        {
            var stats = _dbManager.GetDatabaseStatistics();
            return $"Drivers: {stats.TotalDrivers} | " +
                   $"Routes: {stats.TotalRoutes} | " +
                   $"Trips: {stats.TotalTrips} | " +
                   $"Fuel Records: {stats.TotalFuelRecords}";
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting dashboard statistics"); // Changed back to Serilog format
            return "Statistics unavailable";
        }
    }
}