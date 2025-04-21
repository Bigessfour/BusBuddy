// BusBuddy/Data/IDatabaseManager.cs
using System;
using System.Collections.Generic;
using BusBuddy.Models;

namespace BusBuddy.Data
{
    public interface IDatabaseManager
    {
        List<Trip> GetTrips();
        List<string> GetDriverNames();
        List<int> GetBusNumbers();
        List<Route> GetRoutes();
        void AddFuelRecord(BusBuddy.Models.FuelRecord record);
        List<BusBuddy.Models.Driver> GetDrivers();
        void AddOrUpdateCalendarDay(SchoolCalendarDay day);
        SchoolCalendarDay GetCalendarDay(DateTime date);
        List<ScheduledRoute> GetScheduledRoutes(int calendarDayId);
        List<ScheduledRoute> GetScheduledRoutes(DateTime date);
        void AddDriver(BusBuddy.Models.Driver driver);
        List<BusBuddy.Models.Activity> GetActivities();
        void UpdateScheduledRoute(ScheduledRoute route);
        void AddActivity(BusBuddy.Models.Activity activity);
        void AddTrip(Trip trip);
        List<Trip> GetTripsByDate(DateOnly date);
        DatabaseStatistics GetDatabaseStatistics();
        List<BusBuddy.Models.FuelRecord> GetFuelRecords();
        void AddRoute(Route route);
        void UpdateRoute(Route route);
    }
}