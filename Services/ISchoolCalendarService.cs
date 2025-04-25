// Copyright (c) YourCompanyName. All rights reserved.
// BusBuddy/Services/ISchoolCalendarService.cs
namespace BusBuddy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusBuddy.Models;

    /// <summary>
    /// Interface for school calendar service operations.
    /// </summary>
    public interface ISchoolCalendarService
    {
        /// <summary>
        /// Gets a calendar day by date asynchronously.
        /// </summary>
        /// <param name="date">The date to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with the calendar day result.</returns>
        Task<SchoolCalendarDay> GetCalendarDayByDateAsync(DateTime date);
        
        /// <summary>
        /// Gets scheduled routes for a calendar day asynchronously.
        /// </summary>
        /// <param name="calendarDayId">The calendar day ID.</param>
        /// <returns>A task representing the asynchronous operation with the scheduled routes result.</returns>
        Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesAsync(int calendarDayId);
        
        /// <summary>
        /// Gets scheduled routes for a date asynchronously.
        /// </summary>
        /// <param name="date">The date to retrieve scheduled routes for.</param>
        /// <returns>A task representing the asynchronous operation with the scheduled routes result.</returns>
        Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesByDateAsync(DateTime date);
        
        /// <summary>
        /// Adds or updates a calendar day asynchronously.
        /// </summary>
        /// <param name="calendarDay">The calendar day to add or update.</param>
        /// <returns>A task representing the asynchronous operation with the result tuple.</returns>
        Task<(bool Success, string Message, int CalendarDayId)> AddOrUpdateCalendarDayAsync(SchoolCalendarDay calendarDay);
        
        /// <summary>
        /// Updates a scheduled route asynchronously.
        /// </summary>
        /// <param name="scheduledRoute">The scheduled route to update.</param>
        /// <returns>A task representing the asynchronous operation with the result tuple.</returns>
        Task<(bool Success, string Message)> UpdateScheduledRouteAsync(ScheduledRoute scheduledRoute);

        /// <summary>
        /// Gets a calendar day for the specified date.
        /// </summary>
        /// <param name="date">The date to retrieve.</param>
        /// <returns>The calendar day or a new default instance if not found.</returns>
        SchoolCalendarDay GetCalendarDay(DateTime date);

        /// <summary>
        /// Adds or updates a calendar day.
        /// </summary>
        /// <param name="day">The calendar day to add or update.</param>
        void AddOrUpdateCalendarDay(SchoolCalendarDay day);

        /// <summary>
        /// Adds or updates multiple calendar days.
        /// </summary>
        /// <param name="days">The collection of calendar days to add or update.</param>
        void AddOrUpdateCalendarDays(IEnumerable<SchoolCalendarDay> days);

        /// <summary>
        /// Gets scheduled routes for a specific date.
        /// </summary>
        /// <param name="date">The date to retrieve scheduled routes for.</param>
        /// <returns>A list of scheduled routes for the specified date.</returns>
        List<ScheduledRoute> GetScheduledRoutes(DateTime date);

        /// <summary>
        /// Gets all available routes.
        /// </summary>
        /// <returns>A list of all routes.</returns>
        List<Route> GetRoutes();
    }
}