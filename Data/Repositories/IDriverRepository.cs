// BusBuddy/Data/Repositories/IDriverRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface IDriverRepository : IRepository<Driver, int>
    {
        /// <summary>
        /// Gets all driver names
        /// </summary>
        Task<IEnumerable<string>> GetDriverNamesAsync();
        
        /// <summary>
        /// Gets a driver by name
        /// </summary>
        Task<Driver> GetDriverByNameAsync(string name);
    }
}