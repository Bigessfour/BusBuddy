// BusBuddy/Data/Repositories/ITripRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface ITripRepository : IRepository<Trip, int>
    {
        /// <summary>
        /// Gets trips by date
        /// </summary>
        Task<IEnumerable<Trip>> GetTripsByDateAsync(DateOnly date);
        
        /// <summary>
        /// Gets trips by driver
        /// </summary>
        Task<IEnumerable<Trip>> GetTripsByDriverAsync(string driverName);
        
        /// <summary>
        /// Gets trips by bus number
        /// </summary>
        Task<IEnumerable<Trip>> GetTripsByBusAsync(int busNumber);
    }
}