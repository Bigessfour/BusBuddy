// BusBuddy/Services/ServiceRegistration.cs
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using BusBuddy.Data;
using BusBuddy.Data.Repositories;
using BusBuddy.Services;

namespace BusBuddy.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusBuddyServices(this IServiceCollection services)
        {
            // Register logger
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/busbuddy-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            services.AddSingleton<ILogger>(logger);
            
            // Register database services
            services.AddSingleton<IDatabaseManager, DatabaseManager>();
            
            // Register repositories
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IFuelRepository, FuelRepository>();
            services.AddScoped<ISchoolCalendarRepository, SchoolCalendarRepository>();
            
            // Register services
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IFuelService, FuelService>();
            services.AddScoped<ISchoolCalendarService, SchoolCalendarService>();
            
            return services;
        }
    }
}