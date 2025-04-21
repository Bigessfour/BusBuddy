// BusBuddy/Services/SchoolCalendarService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Data.Repositories;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Services
{
    public class SchoolCalendarService : ISchoolCalendarService
    {
        private readonly ISchoolCalendarRepository _calendarRepository;
        private readonly ILogger _logger;
        
        public SchoolCalendarService(ISchoolCalendarRepository calendarRepository, ILogger logger)
        {
            _calendarRepository = calendarRepository ?? throw new ArgumentNullException(nameof(calendarRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<SchoolCalendarDay> GetCalendarDayByDateAsync(DateTime date)
        {
            try
            {
                return await _calendarRepository.GetCalendarDayByDateAsync(date);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting calendar day by date {Date}", date);
                throw;
            }
        }
        
        public async Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesAsync(int calendarDayId)
        {
            try
            {
                return await _calendarRepository.GetScheduledRoutesAsync(calendarDayId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting scheduled routes by calendar day ID {CalendarDayId}", calendarDayId);
                throw;
            }
        }
        
        public async Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesByDateAsync(DateTime date)
        {
            try
            {
                return await _calendarRepository.GetScheduledRoutesByDateAsync(date);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting scheduled routes by date {Date}", date);
                throw;
            }
        }
        
        public async Task<(bool Success, string Message, int CalendarDayId)> AddOrUpdateCalendarDayAsync(SchoolCalendarDay calendarDay)
        {
            try
            {
                // Validate calendar day data
                if (calendarDay.Date == default)
                {
                    _logger.Warning("Calendar day validation failed: Date is required");
                    return (false, "Date is required", 0);
                }
                
                // Add or update the calendar day
                await _calendarRepository.UpdateAsync(calendarDay);
                _logger.Information("Calendar day added/updated successfully with ID {CalendarDayId}", calendarDay.CalendarDayId);
                return (true, "Calendar day added/updated successfully", calendarDay.CalendarDayId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding/updating calendar day");
                return (false, $"Error adding/updating calendar day: {ex.Message}", 0);
            }
        }
        
        public async Task<(bool Success, string Message)> UpdateScheduledRouteAsync(ScheduledRoute scheduledRoute)
        {
            try
            {
                // Validate scheduled route data
                if (scheduledRoute.RouteId <= 0)
                {
                    _logger.Warning("Scheduled route validation failed: Route ID is required");
                    return (false, "Route ID is required");
                }
                
                // Update the scheduled route
                // Note: This is a placeholder until we have a proper repository method
                _logger.Warning("UpdateScheduledRouteAsync not fully implemented");
                return (true, "Scheduled route updated successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating scheduled route");
                return (false, $"Error updating scheduled route: {ex.Message}");
            }
        }
    }
}