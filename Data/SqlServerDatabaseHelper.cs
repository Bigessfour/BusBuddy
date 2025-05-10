using System;
using System.Collections.Generic;
using System.Linq;
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
        }        /// <summary>
        /// Gets all routes from the database
        /// </summary>
        public async Task<IEnumerable<Route>> GetRoutesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all routes");
                return await _context.Routes
                    .AsNoTracking() // Performance optimization: don't track entities when just reading
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting routes");
                throw;
            }
        }        /// <summary>
        /// Gets a route by ID
        /// </summary>
        public async Task<Route> GetRouteByIdAsync(int routeId)
        {
            try
            {
                _logger.LogInformation("Getting route with ID: {RouteId}", routeId);
                // Use FindAsync for primary key lookups as it's optimized
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
        }        /// <summary>
        /// Gets all vehicles from the database
        /// </summary>
        /// <returns>List of vehicles</returns>
        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all vehicles");
                return await _context.Vehicles
                    .AsNoTracking()  // Performance optimization for read-only operations
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                throw;
            }
        }        /// <summary>
        /// Gets a vehicle by ID
        /// </summary>
        /// <param name="id">The vehicle ID</param>
        /// <returns>The vehicle</returns>
        public Vehicle GetVehicle(int id)
        {
            try
            {
                _logger.LogInformation("Getting vehicle with ID {VehicleId}", id);
                // Using Find for primary key lookup which is optimized
                return _context.Vehicles.Find(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicle with ID {VehicleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Adds a new vehicle to the database
        /// </summary>
        /// <param name="vehicle">The vehicle to add</param>
        public void AddVehicle(Vehicle vehicle)
        {
            try
            {
                _logger.LogInformation($"Adding vehicle {vehicle.VehicleNumber}");
                _context.Vehicles.Add(vehicle);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding vehicle {vehicle.VehicleNumber}");
                throw;
            }
        }

        /// <summary>
        /// Checks if a vehicle exists
        /// </summary>
        /// <param name="name">Vehicle name to check</param>
        /// <returns>True if vehicle exists, false otherwise</returns>
        public bool VehicleExists(string name)
        {
            try
            {
                _logger.LogInformation($"Checking if vehicle {name} exists");
                return _context.Vehicles.Any(v => v.VehicleNumber == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking if vehicle {name} exists");
                throw;
            }
        }
          /// <summary>
        /// Gets all drivers from the database
        /// </summary>
        /// <returns>List of drivers</returns>
        public async Task<IEnumerable<Driver>> GetDriversAsync()
        {
            try
            {
                _logger.LogInformation("Getting all drivers");
                return await _context.Drivers
                    .AsNoTracking() // Performance optimization for read-only operations
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drivers");
                throw;
            }
        }
          /// <summary>
        /// Gets a driver by ID
        /// </summary>
        /// <param name="driverId">The driver ID</param>
        /// <returns>The driver if found, null otherwise</returns>
        public async Task<Driver> GetDriverByIdAsync(int driverId)
        {
            try
            {
                _logger.LogInformation("Getting driver with ID {DriverId}", driverId);
                
                // Return a fully populated driver including any related entities if needed (uncomment next line)
                // return await _context.Drivers.Include(d => d.Licenses).FirstOrDefaultAsync(d => d.Id == driverId);
                
                // Use FindAsync for primary key lookups (best performance for simple lookups)
                return await _context.Drivers.FindAsync(driverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting driver with ID {DriverId}", driverId);
                throw;
            }
        }
        
        /// <summary>
        /// Gets a driver by ID including related entities (use this when you need more than just the driver data)
        /// </summary>
        /// <param name="driverId">The driver ID</param>
        /// <returns>The driver with related entities if found, null otherwise</returns>
        public async Task<Driver> GetDriverWithDetailsAsync(int driverId)
        {
            try
            {
                _logger.LogInformation("Getting driver with details for ID {DriverId}", driverId);
                
                // This demonstrates eager loading of related entities
                // Add Include() statements for any related entities needed
                return await _context.Drivers
                    .AsNoTracking() // Use AsNoTracking for read-only operations
                    // Uncomment and adjust when you have related entities to include
                    // .Include(d => d.Assignments)
                    // .Include(d => d.Qualifications)
                    .FirstOrDefaultAsync(d => d.Id == driverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting driver details with ID {DriverId}", driverId);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a new driver to the database
        /// </summary>
        /// <param name="driver">The driver to add</param>
        /// <returns>The added driver with ID populated</returns>
        public async Task<Driver> AddDriverAsync(Driver driver)
        {
            try
            {
                _logger.LogInformation("Adding driver {DriverName}", driver.FullName);
                _context.Drivers.Add(driver);
                await _context.SaveChangesAsync();
                return driver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding driver {DriverName}", driver.FullName);
                throw;
            }
        }
          /// <summary>
        /// Updates an existing driver
        /// </summary>
        /// <param name="driver">The driver with updated information</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> UpdateDriverAsync(Driver driver)
        {
            try
            {
                _logger.LogInformation("Updating driver with ID {DriverId}", driver.Id);
                
                // Performance optimization: attach and mark as modified
                // instead of querying for the driver first
                _context.Entry(driver).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency conflict when updating driver ID: {DriverId}", driver?.Id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating driver with ID {DriverId}", driver?.Id);
                return false;
            }
        }
        
        /// <summary>
        /// Safely deletes a driver by handling all dependent records to avoid foreign key constraint violations
        /// </summary>
        /// <param name="driverId">The ID of the driver to delete</param>
        /// <param name="reassignToDriverId">Optional. If provided, reassigns dependent records to this driver instead of setting to null</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteDriverSafelyAsync(int driverId, int? reassignToDriverId = null)
        {
            try
            {
                _logger.LogInformation("Safely deleting driver with ID {DriverId}", driverId);
                // Use the DbContext implementation with our logger
                var result = await _context.DeleteDriverSafelyAsync(driverId, reassignToDriverId, _logger);
                
                if (result)
                {
                    _logger.LogInformation("Successfully deleted driver with ID {DriverId}", driverId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete driver with ID {DriverId}", driverId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting driver with ID {DriverId}", driverId);
                return false;
            }
        }        /// <summary>
        /// Deletes a vehicle by ID
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteVehicleAsync(int vehicleId)
        {
            try
            {
                _logger.LogInformation("Deleting vehicle with ID: {VehicleId}", vehicleId);
                
                var vehicle = await _context.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                {
                    _logger.LogWarning("Vehicle with ID {VehicleId} not found for deletion", vehicleId);
                    return false;
                }
                
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Vehicle with ID {VehicleId} deleted successfully", vehicleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle with ID: {VehicleId}", vehicleId);
                return false;
            }
        }

        /// <summary>
        /// Performs a batch update of drivers (much more efficient than updating one at a time)
        /// </summary>
        /// <param name="drivers">Collection of drivers to update</param>
        /// <returns>Number of records successfully updated</returns>
        public async Task<int> BatchUpdateDriversAsync(IEnumerable<Driver> drivers)
        {
            try
            {
                if (drivers == null || !drivers.Any())
                    return 0;
                
                _logger.LogInformation("Batch updating {Count} drivers", drivers.Count());
                
                // Mark each entity as modified
                foreach (var driver in drivers)
                {
                    _context.Entry(driver).State = EntityState.Modified;
                }
                
                // Save all changes in a single transaction
                var count = await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated {Count} drivers", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing batch update on drivers");
                throw;
            }
        }

        /// <summary>
        /// Gets drivers with licenses expiring within the specified days
        /// </summary>
        /// <param name="daysThreshold">Number of days to check for expiration</param>
        /// <returns>List of drivers with expiring licenses</returns>
        public async Task<IEnumerable<Driver>> GetDriversWithExpiringLicensesAsync(int daysThreshold)
        {
            try
            {
                _logger.LogInformation("Getting drivers with licenses expiring in the next {Days} days", daysThreshold);
                
                var expirationThreshold = DateTime.Now.AddDays(daysThreshold);
                
                return await _context.Drivers
                    .AsNoTracking() // Performance optimization for read-only operations
                    .Where(d => d.LicenseExpiration > DateTime.Now && 
                               d.LicenseExpiration <= expirationThreshold)
                    .OrderBy(d => d.LicenseExpiration)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drivers with licenses expiring in next {Days} days", daysThreshold);
                throw;
            }
        }
    }
}
