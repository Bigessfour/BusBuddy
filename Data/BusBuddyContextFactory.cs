using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BusBuddy.Data;
using System;
using System.IO;

namespace BusBuddyMVP.Data
{
    public class BusBuddyContextFactory : IDesignTimeDbContextFactory<BusBuddyContext>
    {
        public BusBuddyContext CreateDbContext(string[] args)
        {
            // Get the directory where the assembly is currently executing
            string projectDir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current directory: {projectDir}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection string found: {!string.IsNullOrEmpty(connectionString)}");

            var optionsBuilder = new DbContextOptionsBuilder<BusBuddyContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BusBuddyContext(optionsBuilder.Options);
        }
    }
}
