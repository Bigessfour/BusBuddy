using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BusBuddy.Data;
using BusBuddy.Hubs;
using BusBuddy.Services.Dashboard;

namespace BusBuddy
{
    /// <summary>
    /// Startup class for configuring ASP.NET Core application with Blazor
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
        {            // Add database context
            services.AddDbContext<BusBuddyContext>(options =>
            {
                // Check if we're running in Docker and should use SQLite
                var useSqlite = Environment.GetEnvironmentVariable("USE_SQLITE") == "true";
                
                if (useSqlite)
                {
                    // Use SQLite for Docker
                    var sqliteConnectionString = _configuration.GetConnectionString("SqliteConnection") ?? "Data Source=/app/data/BusBuddy.db";
                    Console.WriteLine("Using SQLite connection: " + sqliteConnectionString);
                    options.UseSqlite(sqliteConnectionString);
                }
                else
                {
                    // Use SQL Server for local development
                    var connectionString = _configuration.GetConnectionString("DefaultConnection");
                    Console.WriteLine("Using SQL Server connection string from configuration");
                    options.UseSqlServer(connectionString);
                }
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
                    // Allow CORS from both localhost and Docker container hostnames
                    builder.WithOrigins(
                            "http://localhost:3000",     // Local development
                            "http://dashboard:3000",     // Docker container dashboard service
                            "http://localhost:5050",     // External mapped dashboard API port
                            "http://dashboard:5000"      // Docker container dashboard API
                        )
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
            try
            {
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
            }            catch (Exception ex) when (ex is Microsoft.Data.SqlClient.SqlException || ex is SqliteException)
            {
                logger.LogError(ex, "Could not connect to the database. The dashboard will still start, but data will not be available. Please check your connection string and database server.");
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            // Serve static files
            app.UseStaticFiles();
            
            // Add HTTPS redirection (skip in Docker environment)
            if (Environment.GetEnvironmentVariable("USE_DOCKER_DB")?.ToLower() != "true")
            {
                app.UseHttpsRedirection();
            }

            // Configure routing
            app.UseRouting();
            
            // Enable CORS - should be after UseRouting and before UseEndpoints
            app.UseCors("DashboardCorsPolicy");            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                // Map controllers
                endpoints.MapControllers();

                // Map SignalR hub
                endpoints.MapHub<DashboardHub>("/hub/dashboard");
                
                // Map Blazor Hub
                endpoints.MapBlazorHub();
                
                // Map Razor Pages
                endpoints.MapRazorPages();
                
                // Map fallback to _Host.cshtml (required for Blazor Server)
                endpoints.MapFallbackToPage("/_Host");
            });

            // Log the dashboard URL for VS Code auto-open
            logger.LogInformation("Now listening on: http://localhost:5500/dashboard");
            logger.LogInformation("Now listening on: http://localhost:5500/modern-dashboard");
            logger.LogInformation("ASP.NET Core request pipeline configured");
        }        private void SeedDashboardData(BusBuddyContext dbContext, ILogger logger)
        {
            try
            {
                // Sample data for the dashboard demo
                if (dbContext.Routes != null && !dbContext.Routes.Any())
                {
                    logger.LogInformation("Seeding sample data for dashboard demo");
                      // Add sample routes
                    var routes = new[]
                    {
                        new Models.Entities.Route { RouteName = "Downtown Express", Description = "Express route through downtown area" },
                        new Models.Entities.Route { RouteName = "Airport Shuttle", Description = "Route to the airport" },
                        new Models.Entities.Route { RouteName = "Campus Loop", Description = "Loop around university campus" }
                    };
                    
                    dbContext.Routes.AddRange(routes);
                    dbContext.SaveChanges();
                    
                    logger.LogInformation("Sample data seeded successfully");
                }
                else
                {
                    logger.LogInformation("Sample data already exists or Routes table not available");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding sample data");
            }
        }
    }
}
