using Microsoft.EntityFrameworkCore;
using BusBuddy.Models.Entities;
using BusBuddy.Models.ValueObjects;
using BusBuddy.Models.Logs;

namespace BusBuddy.Data
{
    public class BusBuddyContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Decimal precision for FuelEntry
            modelBuilder.Entity<FuelEntry>()
                .Property(f => f.FuelAmount).HasPrecision(18, 2);
            modelBuilder.Entity<FuelEntry>()
                .Property(f => f.Mileage).HasPrecision(18, 2);

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
            modelBuilder.Entity<RouteData>()
                .HasOne(rd => rd.AMDriver)
                .WithMany()
                .HasForeignKey(rd => rd.AMDriverId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RouteData>()
                .HasOne(rd => rd.PMDriver)
                .WithMany()
                .HasForeignKey(rd => rd.PMDriverId)
                .OnDelete(DeleteBehavior.SetNull);

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
        }
    }
}
