// Copyright (c) YourCompanyName. All rights reserved.
// BusBuddy/Services/SchoolCalendarService.cs
namespace BusBuddy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusBuddy.Data;
    using BusBuddy.Data.Repositories;
    using BusBuddy.Models;
    using Serilog;

    /// <summary>
    /// Service for managing school calendar operations.
    /// </summary>
    public class SchoolCalendarService : ISchoolCalendarService
    {
        private readonly ISchoolCalendarRepository calendarRepository;
        private readonly IRouteRepository routeRepository;
        private readonly IDatabaseManager databaseManager;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolCalendarService"/> class.
        /// </summary>
        /// <param name="calendarRepository">The calendar repository.</param>
        /// <param name="routeRepository">The route repository.</param>
        /// <param name="databaseManager">The database manager.</param>
        /// <param name="logger">The Serilog logger.</param>
        public SchoolCalendarService(
            ISchoolCalendarRepository calendarRepository,
            IRouteRepository routeRepository,
            IDatabaseManager databaseManager,
            ILogger logger)
        {
            this.calendarRepository = calendarRepository ?? throw new ArgumentNullException(nameof(calendarRepository));
            this.routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
            this.databaseManager = databaseManager ?? throw new ArgumentNullException(nameof(databaseManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        /// <inheritdoc/>
        public async Task<SchoolCalendarDay> GetCalendarDayByDateAsync(DateTime date)
        {
            try
            {
                return await this.calendarRepository.GetCalendarDayByDateAsync(date);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error getting calendar day by date {Date}", date);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesAsync(int calendarDayId)
        {
            try
            {
                return await this.calendarRepository.GetScheduledRoutesAsync(calendarDayId);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error getting scheduled routes by calendar day ID {CalendarDayId}", calendarDayId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesByDateAsync(DateTime date)
        {
            try
            {
                return await this.calendarRepository.GetScheduledRoutesByDateAsync(date);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error getting scheduled routes by date {Date}", date);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message, int CalendarDayId)> AddOrUpdateCalendarDayAsync(SchoolCalendarDay calendarDay)
        {
            try
            {
                // Validate calendar day data
                if (calendarDay.Date == default)
                {
                    this.logger.Warning("Calendar day validation failed: Date is required");
                    return (false, "Date is required", 0);
                }

                // Add or update the calendar day
                await this.calendarRepository.UpdateAsync(calendarDay);
                this.logger.Information("Calendar day added/updated successfully with ID {CalendarDayId}", calendarDay.CalendarDayId);
                return (true, "Calendar day added/updated successfully", calendarDay.CalendarDayId);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error adding/updating calendar day");
                return (false, $"Error adding/updating calendar day: {ex.Message}", 0);
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message)> UpdateScheduledRouteAsync(ScheduledRoute scheduledRoute)
        {
            try
            {
                // Validate scheduled route data
                if (scheduledRoute.RouteId <= 0)
                {
                    this.logger.Warning("Scheduled route validation failed: Route ID is required");
                    return (false, "Route ID is required");
                }

                // Update the scheduled route
                await this.calendarRepository.UpdateScheduledRouteAsync(scheduledRoute);
                this.logger.Information("Scheduled route updated successfully with ID {ScheduledRouteId}", scheduledRoute.ScheduledRouteId);
                return (true, "Scheduled route updated successfully");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error updating scheduled route");
                return (false, $"Error updating scheduled route: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a calendar day for the specified date.
        /// </summary>
        /// <param name="date">The date to retrieve.</param>
        /// <returns>The calendar day or a new default instance if not found.</returns>
        public SchoolCalendarDay GetCalendarDay(DateTime date)
        {
            try
            {
                var day = this.databaseManager.GetCalendarDay(date);
                
                if (day == null)
                {
                    this.logger.Information("No calendar day found for {Date}, creating default", date);
                    return new SchoolCalendarDay
                    {
                        Date = date,
                        IsSchoolDay = true,
                        DayType = SchoolDayType.Regular,
                        Notes = string.Empty,
                        ActiveRouteIds = new List<int>()
                    };
                }
                
                return day;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error retrieving calendar day for {Date}", date);
                throw;
            }
        }

        /// <summary>
        /// Adds or updates a calendar day.
        /// </summary>
        /// <param name="day">The calendar day to add or update.</param>
        public void AddOrUpdateCalendarDay(SchoolCalendarDay day)
        {
            try
            {
                if (day == null)
                {
                    throw new ArgumentNullException(nameof(day));
                }

                this.ValidateCalendarDay(day);
                this.databaseManager.AddOrUpdateCalendarDay(day);
                this.logger.Information("Calendar day saved for {Date}", day.Date);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error saving calendar day for {Date}", day?.Date);
                throw;
            }
        }

        /// <summary>
        /// Adds or updates multiple calendar days.
        /// </summary>
        /// <param name="days">The collection of calendar days to add or update.</param>
        public void AddOrUpdateCalendarDays(IEnumerable<SchoolCalendarDay> days)
        {
            try
            {
                if (days == null)
                {
                    throw new ArgumentNullException(nameof(days));
                }

                var daysList = days.ToList();
                foreach (var day in daysList)
                {
                    this.ValidateCalendarDay(day);
                }

                this.databaseManager.AddOrUpdateCalendarDays(daysList);
                this.logger.Information("Saved {Count} calendar days", daysList.Count);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error saving multiple calendar days");
                throw;
            }
        }

        /// <summary>
        /// Gets scheduled routes for a specific date.
        /// </summary>
        /// <param name="date">The date to retrieve scheduled routes for.</param>
        /// <returns>A list of scheduled routes for the specified date.</returns>
        public List<ScheduledRoute> GetScheduledRoutes(DateTime date)
        {
            try
            {
                return this.databaseManager.GetScheduledRoutes(date);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error retrieving scheduled routes for {Date}", date);
                throw;
            }
        }

        /// <summary>
        /// Gets all available routes.
        /// </summary>
        /// <returns>A list of all routes.</returns>
        public List<Route> GetRoutes()
        {
            try
            {
                return this.databaseManager.GetRoutes() ?? new List<Route>();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error retrieving routes");
                throw;
            }
        }

        private void ValidateCalendarDay(SchoolCalendarDay day)
        {
            if (day.Date == default)
            {
                throw new ArgumentException("Date is required", nameof(day));
            }

            if (day.Notes == null)
            {
                day.Notes = string.Empty;
            }

            if (day.ActiveRouteIds == null)
            {
                day.ActiveRouteIds = new List<int>();
            }

            if (day.ScheduledRoutes == null)
            {
                day.ScheduledRoutes = new List<ScheduledRoute>();
            }
        }
    }
}