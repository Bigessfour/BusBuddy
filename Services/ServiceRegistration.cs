// Copyright (c) YourCompanyName. All rights reserved.
// BusBuddy/Services/ServiceRegistration.cs
namespace BusBuddy.Services
{
  using BusBuddy.Data;
  using BusBuddy.Data.Repositories;
  using BusBuddy.UI.Forms;
  using BusBuddy.Utilities;
  using BusBuddy.API;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Serilog;

  /// <summary>
  /// Provides extension methods for registering BusBuddy services with the dependency injection container.
  /// </summary>
  public static class ServiceRegistration
  {
    /// <summary>
    /// Adds all BusBuddy services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="logger">The configured logger instance.</param>
    /// <returns>The service collection with BusBuddy services added.</returns>
    public static IServiceCollection AddBusBuddyServices(
        this IServiceCollection services,
        IConfiguration? configuration = null,
        Serilog.ILogger? logger = null)
    {
      // Register configuration if provided
      if (configuration is not null)
      {
        services.AddSingleton(configuration);
      }

      // Register logger if provided, otherwise use FormManager's logger
      if (logger is not null)
      {
        services.AddSingleton(logger);
      }
      else
      {
        // Use logger from FormManager
        services.AddSingleton(FormManager.GetLogger());
      }

      // Register HTTP client factory
      services.AddHttpClient();

      // Register API Clients
      services.AddSingleton<ApiClient>();
      services.AddSingleton<GrokApiClient>(); // Add Grok API client

      // Register database services - using API database manager
      services.AddSingleton<IDatabaseManager, BusBuddy.Data.ApiDatabaseManager>();

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

      // Register UI Components
      services.AddSingleton<IFormNavigator>(sp => new FormNavigatorImpl(form => form.Close()));

      // Register Forms (as Transient since they are typically created and disposed)
      services.AddTransient<Welcome>();
      services.AddTransient<TripSchedulerForm>();
      services.AddTransient<FuelForm>();
      services.AddTransient<DriverForm>();
      services.AddTransient<Settings>();
      services.AddTransient<ScheduledRoutesForm>();
      services.AddTransient<SchoolCalendarForm>();
      services.AddTransient<RoutesForm>();
      services.AddTransient<ActivityForm>();
      services.AddTransient<VehiclesForm>();
      services.AddTransient<MaintenanceForm>();

      // Register Presenters (Scope depends on usage, Transient is often suitable for form presenters)
      services.AddTransient<WelcomePresenter>();

      return services;
    }
  }
}