using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Extensions.Logging;
using BusBuddy.Forms;
using BusBuddy.Data;
using BusBuddy.Data.Interfaces;

namespace BusBuddy
{
    static class Program
    {
        [STAThread]
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
                
                // Set up dependency injection
                var services = new ServiceCollection();
                ConfigureServices(services);
                var serviceProvider = services.BuildServiceProvider();
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(serviceProvider.GetRequiredService<Dashboard>());
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
        
        private static void ConfigureServices(IServiceCollection services)
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
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            }, ServiceLifetime.Scoped);
            
            // Register data access helper
            services.AddScoped<IDatabaseHelper, SqlServerDatabaseHelper>();
            
            // Register forms
            services.AddTransient<Dashboard>();
            
            Log.Information("Services configured");
        }
    }
}