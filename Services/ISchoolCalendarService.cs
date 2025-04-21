// BusBuddy/Services/ISchoolCalendarService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Services
{
    public interface ISchoolCalendarService
    {
        /// <summary>
        /// Gets a calendar day by date
        /// </summary>
        Task<SchoolCalendarDay> GetCalendarDayByDateAsync(DateTime date);
        
        /// <summary>
        /// Gets scheduled routes for a calendar day
        /// </summary>
        Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesAsync(int calendarDayId);
        
        /// <summary>
        /// Gets scheduled routes for a date
        /// </summary>
        Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesByDateAsync(DateTime date);
        
        /// <summary>
        /// Adds or updates a calendar day
        /// </summary>
        Task<(bool Success, string Message, int CalendarDayId)> AddOrUpdateCalendarDayAsync(SchoolCalendarDay calendarDay);
        
        /// <summary>
        /// Updates a scheduled route
        /// </summary>
        Task<(bool Success, string Message)> UpdateScheduledRouteAsync(ScheduledRoute scheduledRoute);
    }
}