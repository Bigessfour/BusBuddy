using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;
using BusBuddy.Forms;
using BusBuddy.Data;
using BusBuddy.Data.Interfaces;
using BusBuddy.Hubs;
using BusBuddy.Services.Dashboard;

namespace BusBuddy
{
    static class Program
    {        [STAThread]
        static void Main()
        {
            // Ensure logs directory exists
            Directory.CreateDirectory("logs");
            
            // Initialize Serilog first thing in the application
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/busbuddy.log", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("Application starting up");
                
                // Start ASP.NET Core web server in a background thread
                var webServer = StartWebServer();
                
                // Set up dependency injection for WinForms app
                var services = new ServiceCollection();
                ConfigureServices(services);
                var serviceProvider = services.BuildServiceProvider();
                
                // Ensure database exists and is created
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BusBuddyContext>();
                    dbContext.Database.EnsureCreated();
                    Log.Information("Database created or verified");
                }
                
                // Start Windows Forms UI
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(serviceProvider.GetRequiredService<Dashboard>());
                
                // When WinForms app closes, stop the web server
                webServer.StopAsync().Wait();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                MessageBox.Show($"A fatal error occurred: {ex.Message}\n\nPlease check the logs for details.",
                    "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        /// <summary>
        /// Starts the ASP.NET Core web server
        /// </summary>
        /// <returns>The host</returns>
        private static IHost StartWebServer()
        {
            Log.Information("Starting ASP.NET Core web server");
            
            // Build web host
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<WebStartup>();
                    webBuilder.UseUrls("http://localhost:5000");
                    webBuilder.UseSerilog();
                })
                .Build();
                
            // Start web host in background thread
            var webServerThread = new System.Threading.Thread(() =>
            {
                host.Run();
            });
            webServerThread.IsBackground = true;
            webServerThread.Start();
            
            Log.Information("ASP.NET Core web server started on http://localhost:5000");
            return host;
        }          private static void ConfigureServices(IServiceCollection services)
        {
            // Configure logging
            services.AddLogging(builder => 
            {
                builder.AddSerilog(dispose: true);
            });
              // Register Entity Framework Core DbContext
            services.AddDbContext<BusBuddyContext>(options => 
            {
                // Get connection string from configuration
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                
                // Check for environment variable to use Docker connection
                var useDocker = Environment.GetEnvironmentVariable("USE_DOCKER_DB")?.ToLower() == "true";
                var connectionName = useDocker ? "DockerConnection" : "DefaultConnection";
                
                var connectionString = configuration.GetConnectionString(connectionName);
                Log.Information($"Using database connection: {connectionName}");
                
                options.UseSqlServer(connectionString);
            }, ServiceLifetime.Scoped);
              // Register data access helper
            services.AddScoped<IDatabaseHelper, SqlServerDatabaseHelper>();
            
            // Register services
            services.AddScoped<Services.DriverService>();
            services.AddScoped<Services.Dashboard.DashboardService>();
            
            // Register forms
            services.AddTransient<Dashboard>();
            
            Log.Information("Services configured");
        }
    }
}