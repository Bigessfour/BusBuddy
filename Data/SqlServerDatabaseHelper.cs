using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BusBuddy.Data.Interfaces;
using BusBuddy.Models.Entities;

namespace BusBuddy.Data
{
    /// <summary>
    /// SQL Server implementation of the database helper
    /// </summary>
    public class SqlServerDatabaseHelper : IDatabaseHelper
    {
        private readonly ILogger<SqlServerDatabaseHelper> _logger;
        private readonly BusBuddyContext _context;

        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString => _context.Database.GetConnectionString();

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDatabaseHelper"/> class
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="logger">The logger</param>
        public SqlServerDatabaseHelper(
            BusBuddyContext context,
            ILogger<SqlServerDatabaseHelper> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Initializes the database if it doesn't exist
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Ensuring database is created");
                await _context.Database.EnsureCreatedAsync();
                _logger.LogInformation("Database initialization complete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing database");
                throw;
            }
        }

        /// <summary>
        /// Gets all routes from the database
        /// </summary>
        public async Task<IEnumerable<Route>> GetRoutesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all routes");
                return await _context.Routes.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting routes");
                throw;
            }
        }

        /// <summary>
        /// Gets a route by ID
        /// </summary>
        public async Task<Route> GetRouteByIdAsync(int routeId)
        {
            try
            {
                _logger.LogInformation("Getting route with ID: {RouteId}", routeId);
                return await _context.Routes.FindAsync(routeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting route with ID: {RouteId}", routeId);
                throw;
            }
        }

        /// <summary>
        /// Adds a new route to the database
        /// </summary>
        public async Task<Route> AddRouteAsync(Route route)
        {
            try
            {
                if (route == null)
                {
                    throw new ArgumentNullException(nameof(route));
                }

                _logger.LogInformation("Adding route: {RouteName}", route.RouteName);
                
                route.CreatedDate = DateTime.Now;
                route.LastModified = DateTime.Now;
                
                _context.Routes.Add(route);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Added route with ID: {RouteId}", route.Id);
                return route;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding route: {RouteName}", route?.RouteName);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing route
        /// </summary>
        public async Task<bool> UpdateRouteAsync(Route route)
        {
            try
            {
                if (route == null)
                {
                    throw new ArgumentNullException(nameof(route));
                }

                _logger.LogInformation("Updating route with ID: {RouteId}", route.Id);
                
                route.LastModified = DateTime.Now;
                
                _context.Entry(route).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Route updated successfully");
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating route ID: {RouteId}", route?.Id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating route with ID: {RouteId}", route?.Id);
                throw;
            }
        }

        /// <summary>
        /// Deletes a route
        /// </summary>
        public async Task<bool> DeleteRouteAsync(int routeId)
        {
            try
            {
                _logger.LogInformation("Deleting route with ID: {RouteId}", routeId);
                
                var route = await _context.Routes.FindAsync(routeId);
                if (route == null)
                {
                    _logger.LogWarning("Route with ID {RouteId} not found for deletion", routeId);
                    return false;
                }
                
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Route deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting route with ID: {RouteId}", routeId);
                throw;
            }
        }
    }
}
