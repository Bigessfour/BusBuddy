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
        {
            // Add database context
            services.AddDbContext<BusBuddyContext>(options =>
            {
                // Check if we're running in Docker environment
                var useDocker = Environment.GetEnvironmentVariable("USE_DOCKER_DB")?.ToLower() == "true";
                var connectionString = "";
                
                // If CONNECTION_STRING environment variable is set, use it directly (Docker container)
                var envConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                if (!string.IsNullOrEmpty(envConnectionString))
                {
                    connectionString = envConnectionString;
                    Console.WriteLine("Using connection string from environment variable");
                }
                else
                {
                    // Otherwise use from appsettings.json
                    var connectionName = useDocker ? "DockerConnection" : "DefaultConnection";
                    connectionString = _configuration.GetConnectionString(connectionName);
                    Console.WriteLine($"Using {connectionName} connection string from configuration");
                }
                
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
            app.UseCors("DashboardCorsPolicy");

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

        // Seed method remains the same
        private void SeedDashboardData(BusBuddyContext dbContext, ILogger logger)
        {
            // Same implementation as before
        }
    }
}
