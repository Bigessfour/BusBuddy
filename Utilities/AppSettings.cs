using System;
using Microsoft.Extensions.Configuration;

namespace BusBuddy.Utilities
{
    public static class AppSettings
    {
        private static readonly IConfigurationRoot Configuration;

        static AppSettings()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static string DatabasePath => Configuration["DatabaseSettings:DatabasePath"] ?? "WileySchool.db";
    }
}