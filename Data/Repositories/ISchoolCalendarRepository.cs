// BusBuddy/Data/Repositories/ISchoolCalendarRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface ISchoolCalendarRepository : IRepository<SchoolCalendarDay, int>
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
    }
}