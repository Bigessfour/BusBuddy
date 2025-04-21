// BusBuddy/Data/Repositories/IFuelRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface IFuelRepository : IRepository<FuelRecord, int>
    {
        /// <summary>
        /// Gets fuel records by bus number
        /// </summary>
        Task<IEnumerable<FuelRecord>> GetFuelRecordsByBusAsync(int busNumber);
    }
}