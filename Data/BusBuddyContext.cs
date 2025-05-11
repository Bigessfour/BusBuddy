using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BusBuddy.Models.Entities;
using BusBuddy.Models.ValueObjects;
using BusBuddy.Models.Logs;

namespace BusBuddy.Data
{    public class BusBuddyContext : DbContext
    {
        public BusBuddyContext(DbContextOptions<BusBuddyContext> options) : base(options) { }

        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<ActivityTrip> ActivityTrips { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<FuelEntry> FuelEntries { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<RouteData> RouteData { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Decimal precision for FuelEntry
            modelBuilder.Entity<FuelEntry>()
                .Property(f => f.FuelAmount).HasPrecision(18, 2);
            modelBuilder.Entity<FuelEntry>()
                .Property(f => f.Mileage).HasPrecision(18, 2);
            modelBuilder.Entity<FuelEntry>()
                .Property(f => f.PricePerGallon).HasPrecision(18, 2);
            modelBuilder.Entity<FuelEntry>()
                .Property(f => f.TotalCost).HasPrecision(18, 2);

            // Decimal precision for Part
            modelBuilder.Entity<Part>()
                .Property(p => p.UnitPrice).HasPrecision(18, 2);

            // Decimal precision for Route
            modelBuilder.Entity<Route>()
                .Property(r => r.Distance).HasPrecision(18, 2);

            // Decimal precision for Destination
            modelBuilder.Entity<Destination>()
                .Property(d => d.TotalMiles).HasPrecision(18, 2);

            // Decimal precision for Vehicle
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Odometer).HasPrecision(18, 2);

            // Decimal precision for RouteData
            modelBuilder.Entity<RouteData>()
                .Property(r => r.AMStartMileage).HasPrecision(18, 2);
            modelBuilder.Entity<RouteData>()
                .Property(r => r.AMEndMileage).HasPrecision(18, 2);
            modelBuilder.Entity<RouteData>()
                .Property(r => r.PMStartMileage).HasPrecision(18, 2);
            modelBuilder.Entity<RouteData>()
                .Property(r => r.PMEndMileage).HasPrecision(18, 2);

            // Configure cascade behavior for RouteData driver relationships
            // First relationship uses SetNull
            modelBuilder.Entity<RouteData>()
                .HasOne(rd => rd.AMDriver)
                .WithMany()
                .HasForeignKey(rd => rd.AMDriverId)
                .OnDelete(DeleteBehavior.SetNull);

            // Second relationship uses Restrict/NoAction to avoid multiple cascade paths
            modelBuilder.Entity<RouteData>()
                .HasOne(rd => rd.PMDriver)
                .WithMany()
                .HasForeignKey(rd => rd.PMDriverId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from SetNull to Restrict to avoid multiple cascade paths

            // Configure cascade behavior for ActivityTrip relationships
            modelBuilder.Entity<ActivityTrip>()
                .HasOne(at => at.Driver)
                .WithMany()
                .HasForeignKey(at => at.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityTrip>()
                .HasOne(at => at.Vehicle)
                .WithMany()
                .HasForeignKey(at => at.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityTrip>()
                .HasOne(at => at.Route)
                .WithMany()
                .HasForeignKey(at => at.RouteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ActivityTrip>()
                .HasOne(at => at.Destination)
                .WithMany(d => d.ActivityTrips)
                .HasForeignKey(at => at.DestinationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure cascade behavior for Vehicle relationships
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.AssignedDriver)
                .WithMany()
                .HasForeignKey(v => v.AssignedDriverId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure cascade behavior for FuelEntry
            modelBuilder.Entity<FuelEntry>()
                .HasOne(fe => fe.Vehicle)
                .WithMany()
                .HasForeignKey(fe => fe.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure cascade behavior for Maintenance
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Vehicle)
                .WithMany()
                .HasForeignKey(m => m.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for Route and Destination
            modelBuilder.Entity<Route>()
                .HasMany(r => r.Destinations)
                .WithOne(d => d.Route)  // Assuming Destination has a Route navigation property
                .HasForeignKey("RouteId")  // Using string-based column name without requiring property
                .IsRequired(false);  // Make it optional
        }        /// <summary>
        /// Safely deletes a driver by handling all dependent records to avoid foreign key constraint violations.
        /// </summary>        /// <param name="driverId">The ID of the driver to delete.</param>
        /// <param name="reassignToDriverId">Optional. If provided, reassigns RouteData to this driver instead of setting to null.</param>
        /// <param name="logger">Optional. ILogger for logging errors.</param>
        /// <returns>True if deleted, false if not found or error.</returns>
        public async Task<bool> DeleteDriverSafelyAsync(int driverId, int? reassignToDriverId = null, ILogger? logger = null)
        {
            // Check if we're using an in-memory database for testing
            bool isInMemoryDatabase = Database.ProviderName?.Contains("InMemory") ?? false;
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = null;
            
            // Only start a transaction if we're not using in-memory database (which doesn't support transactions)
            if (!isInMemoryDatabase)
            {
                transaction = await Database.BeginTransactionAsync();
            }
            
            try
            {
                // Find the driver
                var driver = await Drivers.FindAsync(driverId);
                if (driver == null)
                {
                    logger?.LogWarning("Driver with ID {DriverId} not found for deletion", driverId);
                    return false;
                }

                // Validate reassignment driver if specified
                Driver? reassignToDriver = null;
                if (reassignToDriverId.HasValue)
                {
                    reassignToDriver = await Drivers.FindAsync(reassignToDriverId.Value);
                    if (reassignToDriver == null)
                    {
                        logger?.LogWarning("Reassignment driver with ID {ReassignDriverId} not found", reassignToDriverId.Value);
                        return false;
                    }
                }

                // Handle RouteData references
                var affectedRouteData = await RouteData
                    .Where(rd => rd.AMDriverId == driverId || rd.PMDriverId == driverId)
                    .ToListAsync();

                if (affectedRouteData.Any())
                {
                    logger?.LogInformation("Processing {Count} RouteData records referring to driver {DriverId}", affectedRouteData.Count, driverId);
                    
                    foreach (var rd in affectedRouteData)
                    {
                        // For AMDriver (configured with SetNull), we can reassign or set to null
                        if (rd.AMDriverId == driverId)
                        {
                            rd.AMDriverId = reassignToDriverId;
                            logger?.LogInformation("Updated RouteData {Id} AMDriverId from {OldDriver} to {NewDriver}", 
                                rd.Id, driverId, reassignToDriverId.HasValue ? reassignToDriverId.Value.ToString() : "null");
                        }
                        
                        // For PMDriver (configured with Restrict), we MUST handle this to prevent constraint violations
                        if (rd.PMDriverId == driverId)
                        {
                            if (reassignToDriverId.HasValue)
                            {
                                rd.PMDriverId = reassignToDriverId;
                                logger?.LogInformation("Updated RouteData {Id} PMDriverId from {OldDriver} to {NewDriver}", 
                                    rd.Id, driverId, reassignToDriverId.Value);
                            }
                            else
                            {
                                rd.PMDriverId = null;
                                logger?.LogInformation("Set RouteData {Id} PMDriverId to null (was {OldDriver})", 
                                    rd.Id, driverId);
                            }
                        }
                    }
                }

                // Handle FuelEntry references (if they exist)
                var affectedFuelEntries = await FuelEntries
                    .Where(fe => fe.DriverId == driverId)
                    .ToListAsync();
                
                if (affectedFuelEntries.Any())
                {
                    logger?.LogInformation("Processing {Count} FuelEntry records referring to driver {DriverId}", affectedFuelEntries.Count, driverId);
                    
                    foreach (var fe in affectedFuelEntries)
                    {
                        if (reassignToDriverId.HasValue)
                        {
                            fe.DriverId = reassignToDriverId.Value;
                        }
                        else
                        {
                            fe.DriverId = null;
                        }
                    }
                }

                // Handle Vehicle's AssignedDriver references
                var affectedVehicles = await Vehicles
                    .Where(v => v.AssignedDriverId == driverId)
                    .ToListAsync();

                if (affectedVehicles.Any())
                {
                    logger?.LogInformation("Processing {Count} Vehicle records referring to driver {DriverId}", affectedVehicles.Count, driverId);
                    
                    foreach (var v in affectedVehicles)
                    {
                        if (reassignToDriverId.HasValue)
                        {
                            v.AssignedDriverId = reassignToDriverId.Value;
                        }
                        else
                        {
                            v.AssignedDriverId = null;
                        }
                    }
                }

                // Handle ActivityTrip references (these use cascade delete by default, but we'll handle them explicitly)
                var affectedActivityTrips = await ActivityTrips
                    .Where(at => at.DriverId == driverId)
                    .ToListAsync();

                if (affectedActivityTrips.Any())
                {
                    logger?.LogInformation("Found {Count} ActivityTrip records referring to driver {DriverId}", affectedActivityTrips.Count, driverId);
                    
                    // Option 1: Reassign to another driver if provided
                    if (reassignToDriverId.HasValue)
                    {
                        foreach (var trip in affectedActivityTrips)
                        {
                            trip.DriverId = reassignToDriverId.Value;
                        }
                        logger?.LogInformation("Reassigned {Count} ActivityTrip records to driver {ReassignDriverId}", affectedActivityTrips.Count, reassignToDriverId.Value);
                    }
                    // Option 2: Delete the ActivityTrip records (if they can't be reassigned)
                    else
                    {
                        ActivityTrips.RemoveRange(affectedActivityTrips);
                        logger?.LogInformation("Removed {Count} ActivityTrip records associated with driver {DriverId}", affectedActivityTrips.Count, driverId);
                    }
                }

                // Remove the driver
                Drivers.Remove(driver);
                logger?.LogInformation("Deleting driver {DriverId} ({FullName})", driverId, driver.FullName);
                  // Save all changes
                await SaveChangesAsync();
                
                // Commit transaction if we're not using in-memory database
                if (!isInMemoryDatabase && transaction != null)
                {
                    await transaction.CommitAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Rollback transaction if we're not using in-memory database
                if (!isInMemoryDatabase && transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                logger?.LogError(ex, "Error deleting driver {DriverId}: {ErrorMessage}", driverId, ex.Message);
                return false;
            }
        }
    }
}
