using System;
using Microsoft.Extensions.DependencyInjection;

namespace BusBuddy.Forms
{
    /// <summary>
    /// Service registration module for the Windows Forms components
    /// </summary>
    public static class FormsModule
    {
        public static IServiceCollection AddFormsServices(this IServiceCollection services)
        {
            // Register forms as transient services
            services.AddTransient<Dashboard>();
            services.AddTransient<MainForm>();
            services.AddTransient<DriverManagementForm>();
            services.AddTransient<RouteManagementForm>();
            services.AddTransient<VehiclesManagementForm>();
            services.AddTransient<ReportsForm>();
            
            return services;
        }
    }
}
