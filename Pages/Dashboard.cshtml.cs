using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BusBuddy.Data;
using BusBuddy.Services;

namespace BusBuddy.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly BusBuddyContext _context;
        private readonly ILogger<DashboardModel> _logger;
        private readonly IBusScheduleHelper _busScheduleHelper;

        public DashboardModel(BusBuddyContext context, ILogger<DashboardModel> logger, IBusScheduleHelper busScheduleHelper)
        {
            _context = context;
            _logger = logger;
            _busScheduleHelper = busScheduleHelper;
        }

        public List<BusSchedule> Schedules { get; set; } = new List<BusSchedule>();
        
        [BindProperty]
        public BusSchedule NewSchedule { get; set; } = new BusSchedule { Time = DateTime.Now };

        public async Task OnGetAsync()
        {
            try
            {
                // Seed basic data if no records exist
                await SeedBasicDataIfNeeded();

                // Retrieve schedule data from database
                await LoadScheduleData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                // Add some demo data to ensure the page shows something
                AddDemoData();
            }
        }        private async Task LoadScheduleData()
        {
            // Query the database for bus schedules
            Schedules = await _context.BusSchedules
                .OrderBy(s => s.Time)
                .ToListAsync();
                
            _logger.LogInformation($"Loaded {Schedules.Count} schedules from the database");
        }        private async Task SeedBasicDataIfNeeded()
        {
            // Check if we need to seed data (empty database)
            if (!await _context.BusSchedules.AnyAsync())
            {
                _logger.LogInformation("Seeding initial bus schedule data");
                
                _context.BusSchedules.AddRange(
                    new BusSchedule
                    {
                        RouteName = "Downtown Express",
                        Time = DateTime.Now.Date.AddHours(9).AddMinutes(30)
                    },
                    new BusSchedule
                    {
                        RouteName = "Airport Shuttle",
                        Time = DateTime.Now.Date.AddHours(10).AddMinutes(15)
                    },
                    new BusSchedule
                    {
                        RouteName = "University Route",
                        Time = DateTime.Now.Date.AddHours(11).AddMinutes(0)
                    }
                );
                
                await _context.SaveChangesAsync();
                _logger.LogInformation("Database seeded with initial schedules");
            }
        }

        public async Task<IActionResult> OnPostAddScheduleAsync()
        {
            if (string.IsNullOrEmpty(NewSchedule.RouteName))
            {
                NewSchedule.RouteName = "Downtown"; // Default value
            }
            
            _context.BusSchedules.Add(NewSchedule);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Added new schedule: {NewSchedule.RouteName} at {NewSchedule.Time}");
            
            return RedirectToPage();
        }

        private void AddDemoData()
        {
            // Only add demo data if we couldn't load from database
            _logger.LogWarning("Using demo data instead of database data");
            
            Schedules = new List<BusSchedule>
            {
                new BusSchedule
                {
                    Id = -1,
                    RouteName = "Downtown Express",
                    Time = DateTime.Now.AddHours(1)
                },
                new BusSchedule
                {
                    Id = -2,
                    RouteName = "Airport Shuttle",
                    Time = DateTime.Now.AddHours(2)
                },
                new BusSchedule
                {
                    Id = -3,
                    RouteName = "University Route",
                    Time = DateTime.Now.AddHours(3)
                }
            };
        }
    }
}
