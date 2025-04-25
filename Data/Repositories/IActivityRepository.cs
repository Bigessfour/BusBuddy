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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Activity>> GetActivitiesByDateAsync(string date);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets activities by bus number
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Activity>> GetActivitiesByBusAsync(int busNumber);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}