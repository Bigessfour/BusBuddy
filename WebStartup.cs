using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BusBuddy.Data;
using BusBuddy.Hubs;
using BusBuddy.Models.Entities;
using BusBuddy.Services.Dashboard;

namespace BusBuddy
{
    /// <summary>
    /// Startup class for configuring ASP.NET Core application
    /// </summary>
    public class WebStartup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebStartup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        public WebStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Configures services for the ASP.NET Core application
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add database context
            services.AddDbContext<BusBuddyContext>(options =>
            {
                // Use the same connection string as the main application
                var useDocker = Environment.GetEnvironmentVariable("USE_DOCKER_DB")?.ToLower() == "true";
                var connectionName = useDocker ? "DockerConnection" : "DefaultConnection";
                var connectionString = _configuration.GetConnectionString(connectionName);
                options.UseSqlServer(connectionString);
            });

            // Add controllers
            services.AddControllers();
            
            // Add dashboard services
            services.AddScoped<DashboardService>();
            services.AddScoped<DashboardUpdatesService>();
            
            // Add Blazor Server
            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Add memory cache
            services.AddMemoryCache();

            // Add SignalR
            services.AddSignalR();

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("DashboardCorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // React frontend
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
        }

        /// <summary>
        /// Configures the HTTP request pipeline
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Web hosting environment</param>
        /// <param name="logger">Logger</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<WebStartup> logger)
        {
            logger.LogInformation("Configuring ASP.NET Core request pipeline");

            // Ensure database is created and migrations are applied
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<BusBuddyContext>();
                if (dbContext != null)
                {
                    dbContext.Database.EnsureCreated();
                    logger.LogInformation("Database created or verified");
                    
                    // Seed data for dashboard demo if needed
                    SeedDashboardData(dbContext, logger);
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }            
            // Enable CORS
            app.UseCors("DashboardCorsPolicy");
            
            // Serve static files
            app.UseStaticFiles();
            
            // Add HTTPS redirection
            app.UseHttpsRedirection();

            // Configure routing
            app.UseRouting();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                // Map controllers
                endpoints.MapControllers();

                // Map SignalR hub
                endpoints.MapHub<DashboardHub>("/hub/dashboard");
                
                // Map Blazor Hub
                endpoints.MapBlazorHub();
                
                // Map fallback to _Host.cshtml
                endpoints.MapFallbackToPage("/_Host");
            });

            logger.LogInformation("ASP.NET Core request pipeline configured");
        }

        /// <summary>
        /// Seeds dashboard data for demonstration
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="logger">Logger</param>
        private void SeedDashboardData(BusBuddyContext dbContext, ILogger logger)
        {
            try
            {
                // Check if we have any routes
                if (!dbContext.Routes.Any())
                {
                    logger.LogInformation("Seeding route data for dashboard");
                    
                    // Add a main route
                    var mainRoute = new Models.Entities.Route
                    {
                        RouteName = "Main Route",
                        StartLocation = "School",
                        EndLocation = "Downtown",
                        Distance = 5.2m,
                        CreatedDate = DateTime.Now,
                        LastModified = DateTime.Now,
                        Description = "Main school route"
                    };
                    
                    dbContext.Routes.Add(mainRoute);
                    dbContext.SaveChanges();
                    
                    // Get the newly added route ID
                    int routeId = mainRoute.Id;
                    
                    // Add trips for this route
                    dbContext.Trips.AddRange(
                        new Models.Entities.Trip
                        {
                            RouteId = routeId,
                            PassengerCount = 25,
                            DelayMinutes = 0,
                            Status = "OnTime",
                            IsActive = true,
                            LastUpdated = DateTime.Now
                        },
                        new Models.Entities.Trip
                        {
                            RouteId = routeId,
                            PassengerCount = 18,
                            DelayMinutes = 10,
                            Status = "Delayed",
                            IsActive = true,
                            LastUpdated = DateTime.Now
                        }
                    );
                    
                    // Add an alert for this route
                    dbContext.Alerts.Add(
                        new Models.Entities.Alert
                        {
                            RouteId = routeId,
                            Message = "Traffic congestion on Main St",
                            Severity = "Warning",
                            IsActive = true,
                            CreatedAt = DateTime.Now
                        }
                    );
                    
                    dbContext.SaveChanges();
                    logger.LogInformation("Dashboard demo data seeded successfully");
                }
                else
                {
                    logger.LogInformation("Routes already exist, skipping dashboard data seeding");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding dashboard data");
            }
        }
    }
}
