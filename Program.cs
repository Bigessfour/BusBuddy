using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BusBuddy.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace BusBuddy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Setup Windows Forms application
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Start web host in background if needed
            var webHostCancellation = new CancellationTokenSource();
            Task.Run(() => StartWebHost(webHostCancellation.Token));

            try
            {
                // Run the main Windows Forms application
                var mainForm = serviceProvider.GetRequiredService<MainForm>();
                Application.Run(mainForm);
            }
            finally
            {
                // Ensure web host is stopped when application exits
                webHostCancellation.Cancel();
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Add logging
            services.AddLogging(configure => configure.AddConsole());
            
            // Add caching
            services.AddMemoryCache();
            
            // Add database services
            // services.AddDbContext<BusBuddyContext>(options => 
            //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            // Register forms
            services.AddTransient<MainForm>();
            
            // Add other services
            // services.AddSingleton<IDashboardService, DashboardService>();
            // services.AddSingleton<IDriverService, DriverService>();
        }

        private static async Task StartWebHost(CancellationToken cancellationToken)
        {
            try
            {
                var webHostBuilder = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<WebStartup>();
                    });

                await webHostBuilder.Build().RunAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when application is closing
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Web host error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}