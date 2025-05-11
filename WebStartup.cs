using System;
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
            services.AddControllers();            // Add dashboard services
            services.AddScoped<DashboardService>();
            services.AddScoped<DashboardUpdatesService>();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<WebStartup> logger)
        {
            logger.LogInformation("Configuring ASP.NET Core request pipeline");

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

            // Configure routing
            app.UseRouting();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                // Map controllers
                endpoints.MapControllers();

                // Map SignalR hub
                endpoints.MapHub<DashboardHub>("/hub/dashboard");
            });

            logger.LogInformation("ASP.NET Core request pipeline configured");
        }
    }
}
