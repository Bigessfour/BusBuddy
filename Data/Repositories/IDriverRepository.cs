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
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Driver> GetDriverByNameAsync(string name);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}