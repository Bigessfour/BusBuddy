using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusBuddy.Data
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(BusBuddyContext context)
        {
            // Create database if it doesn't exist
            await context.Database.EnsureCreatedAsync();
            
            // Check if database has been seeded
            if (await context.Routes.AnyAsync())
            {
                return; // Database already seeded
            }
            
            // Add seed data here if needed
        }
    }
}
