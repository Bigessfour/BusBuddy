// BusBuddy/Data/IDatabaseManager.cs
using System;
using System.Collections.Generic;
using BusBuddy.Models;

namespace BusBuddy.Data
{
    /// <summary>
    /// Interface for database operations in the BusBuddy application
    /// </summary>
    public interface IDatabaseManager
    {
        /// <summary>Gets all trips from the database</summary>
        /// <returns>A list of trips, or an empty list if none found</returns>
        List<Trip> GetTrips();
        
        /// <summary>Gets all driver names from the database</summary>
        /// <returns>A list of driver names, or an empty list if none found</returns>
        List<string> GetDriverNames();
        
        /// <summary>Gets all bus numbers from the database</summary>
        /// <returns>A list of bus numbers, or an empty list if none found</returns>
        List<int> GetBusNumbers();
        
        /// <summary>Gets all routes from the database</summary>
        /// <returns>A list of routes, or an empty list if none found</returns>
        List<Route> GetRoutes();
        
        /// <summary>Adds a new fuel record to the database</summary>
        /// <param name="record">The fuel record to add</param>
        /// <returns>True if successful, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if record is null</exception>
        bool AddFuelRecord(FuelRecord record);
        
        /// <summary>Gets all drivers from the database</summary>
        /// <returns>A list of drivers, or an empty list if none found</returns>
        List<Driver> GetDrivers();
        
        /// <summary>Adds or updates a calendar day in the database</summary>
        /// <param name="day">The calendar day to add or update</param>
        /// <exception cref="ArgumentNullException">Thrown if day is null</exception>
        void AddOrUpdateCalendarDay(SchoolCalendarDay day);
        
        /// <summary>Adds or updates multiple calendar days in the database</summary>
        /// <param name="days">The calendar days to add or update</param>
        /// <exception cref="ArgumentNullException">Thrown if days is null</exception>
        void AddOrUpdateCalendarDays(IEnumerable<SchoolCalendarDay> days);
        
        /// <summary>Gets a calendar day for the specified date</summary>
        /// <param name="date">The date to retrieve</param>
        /// <returns>The calendar day, or null if not found</returns>
        SchoolCalendarDay? GetCalendarDay(DateTime date);
        
        /// <summary>Gets scheduled routes for a specific calendar day ID</summary>
        /// <param name="calendarDayId">The calendar day ID</param>
        /// <returns>A list of scheduled routes, or an empty list if none found</returns>
        List<ScheduledRoute> GetScheduledRoutes(int calendarDayId);
        
        /// <summary>Gets scheduled routes for a specific date</summary>
        /// <param name="date">The date to retrieve routes for</param>
        /// <returns>A list of scheduled routes, or an empty list if none found</returns>
        List<ScheduledRoute> GetScheduledRoutes(DateTime date);
        
        /// <summary>Adds a new driver to the database</summary>
        /// <param name="driver">The driver to add</param>
        /// <returns>True if successful, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if driver is null</exception>
        bool AddDriver(Driver driver);
        
        /// <summary>Gets all activities from the database</summary>
        /// <returns>A list of activities, or an empty list if none found</returns>
        List<Activity> GetActivities();
        
        /// <summary>Updates a scheduled route in the database</summary>
        /// <param name="route">The route to update</param>
        /// <exception cref="ArgumentNullException">Thrown if route is null</exception>
        void UpdateScheduledRoute(ScheduledRoute route);
        
        /// <summary>Adds a new activity to the database</summary>
        /// <param name="activity">The activity to add</param>
        /// <exception cref="ArgumentNullException">Thrown if activity is null</exception>
        void AddActivity(Activity activity);
        
        /// <summary>Adds a new trip to the database</summary>
        /// <param name="trip">The trip to add</param>
        /// <exception cref="ArgumentNullException">Thrown if trip is null</exception>
        void AddTrip(Trip trip);
        
        /// <summary>Gets trips for a specific date</summary>
        /// <param name="date">The date to retrieve trips for</param>
        /// <returns>A list of trips, or an empty list if none found</returns>
        List<Trip> GetTripsByDate(DateOnly date);
        
        /// <summary>Gets database statistics</summary>
        /// <returns>Database statistics</returns>
        DatabaseStatistics GetDatabaseStatistics();
        
        /// <summary>Gets all fuel records from the database</summary>
        /// <returns>A list of fuel records, or an empty list if none found</returns>
        List<FuelRecord> GetFuelRecords();
        
        /// <summary>Updates a fuel record in the database</summary>
        /// <param name="record">The fuel record to update</param>
        /// <returns>True if successful, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if record is null</exception>
        bool UpdateFuelRecord(FuelRecord record);
        
        /// <summary>Deletes a fuel record from the database</summary>
        /// <param name="recordId">The ID of the fuel record to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        bool DeleteFuelRecord(int recordId);
        
        /// <summary>Adds a new route to the database</summary>
        /// <param name="route">The route to add</param>
        /// <exception cref="ArgumentNullException">Thrown if route is null</exception>
        void AddRoute(Route route);
        
        /// <summary>Updates a route in the database</summary>
        /// <param name="route">The route to update</param>
        /// <exception cref="ArgumentNullException">Thrown if route is null</exception>
        void UpdateRoute(Route route);
        
        /// <summary>Deletes a route from the database</summary>
        /// <param name="routeId">The ID of the route to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        bool DeleteRoute(int routeId);
        
        /// <summary>Updates a driver in the database</summary>
        /// <param name="driver">The driver to update</param>
        /// <returns>True if successful, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown if driver is null</exception>
        bool UpdateDriver(Driver driver);
        
        /// <summary>Deletes a driver from the database</summary>
        /// <param name="driverId">The ID of the driver to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        bool DeleteDriver(int driverId);
        
        // Maintenance methods
        
        /// <summary>Gets all maintenance records from the database</summary>
        /// <returns>A list of maintenance records, or an empty list if none found</returns>
        List<Maintenance> GetMaintenanceRecords();
        
        /// <summary>Adds a new maintenance record to the database</summary>
        /// <param name="maintenance">The maintenance record to add</param>
        /// <exception cref="ArgumentNullException">Thrown if maintenance is null</exception>
        void AddMaintenanceRecord(Maintenance maintenance);
        
        /// <summary>Updates a maintenance record in the database</summary>
        /// <param name="maintenance">The maintenance record to update</param>
        /// <exception cref="ArgumentNullException">Thrown if maintenance is null</exception>
        void UpdateMaintenanceRecord(Maintenance maintenance);
        
        /// <summary>Deletes a maintenance record from the database</summary>
        /// <param name="maintenanceId">The ID of the maintenance record to delete</param>
        void DeleteMaintenanceRecord(int maintenanceId);
    }
}