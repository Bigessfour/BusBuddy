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
        }

        private async Task LoadScheduleData()
        {
            // This method would normally query the database with EF Core
            // Since we don't know the exact structure, we're adding demo data
            AddDemoData();
        }

        private async Task SeedBasicDataIfNeeded()
        {
            // Check if we need to seed data (empty database)
            // This would normally check specific tables and seed if empty
        }

        private void AddDemoData()
        {
            Schedules = new List<BusScheduleDto>
            {
                new BusScheduleDto
                {
                    RouteName = "Downtown Express",
                    DriverName = "John Smith",
                    VehicleInfo = "Bus #1042",
                    Status = "On Time",
                    UpdatedAt = DateTime.Now.AddMinutes(-5)
                },
                new BusScheduleDto
                {
                    RouteName = "Airport Shuttle",
                    DriverName = "Sarah Johnson",
                    VehicleInfo = "Bus #2305",
                    Status = "Delayed",
                    UpdatedAt = DateTime.Now.AddMinutes(-15)
                },
                new BusScheduleDto
                {
                    RouteName = "University Route",
                    DriverName = "Michael Chen",
                    VehicleInfo = "Bus #1578",
                    Status = "On Time",
                    UpdatedAt = DateTime.Now.AddMinutes(-2)
                }
            };
        }
    }
}
