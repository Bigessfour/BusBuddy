// Copyright (c) YourCompanyName. All rights reserved.
// BusBuddy/Data/Repositories/ISchoolCalendarRepository.cs
namespace BusBuddy.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusBuddy.Models;

    /// <summary>
    /// Repository interface for school calendar operations.
    /// </summary>
    public interface ISchoolCalendarRepository : IRepository<SchoolCalendarDay, int>
    {
        /// <summary>
        /// Gets a calendar day by date.
        /// </summary>
        /// <param name="date">The date to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with the calendar day result.</returns>
        Task<SchoolCalendarDay> GetCalendarDayByDateAsync(DateTime date);
        
        /// <summary>
        /// Gets scheduled routes for a calendar day.
        /// </summary>
        /// <param name="calendarDayId">The calendar day ID.</param>
        /// <returns>A task representing the asynchronous operation with the scheduled routes result.</returns>
        Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesAsync(int calendarDayId);
        
        /// <summary>
        /// Gets scheduled routes for a date.
        /// </summary>
        /// <param name="date">The date to retrieve scheduled routes for.</param>
        /// <returns>A task representing the asynchronous operation with the scheduled routes result.</returns>
        Task<IEnumerable<ScheduledRoute>> GetScheduledRoutesByDateAsync(DateTime date);
        
        /// <summary>
        /// Updates a scheduled route.
        /// </summary>
        /// <param name="scheduledRoute">The scheduled route to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateScheduledRouteAsync(ScheduledRoute scheduledRoute);
    }
}