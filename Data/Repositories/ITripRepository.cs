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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Trip>> GetTripsByDateAsync(DateOnly date);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets trips by driver
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Trip>> GetTripsByDriverAsync(string driverName);
#pragma warning restore SA1611 // Element parameters should be documented
        
        /// <summary>
        /// Gets trips by bus number
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<IEnumerable<Trip>> GetTripsByBusAsync(int busNumber);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}