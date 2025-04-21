// BusBuddy/Data/Repositories/IActivityRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface IActivityRepository : IRepository<Activity, int>
    {
        /// <summary>
        /// Gets activities by date
        /// </summary>
        Task<IEnumerable<Activity>> GetActivitiesByDateAsync(string date);
        
        /// <summary>
        /// Gets activities by bus number
        /// </summary>
        Task<IEnumerable<Activity>> GetActivitiesByBusAsync(int busNumber);
    }
}