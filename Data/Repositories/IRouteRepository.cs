// BusBuddy/Data/Repositories/IRouteRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BusBuddy.Models;

namespace BusBuddy.Data.Repositories
{
    public interface IRouteRepository : IRepository<Route, int>
    {
        /// <summary>
        /// Gets a route by name
        /// </summary>
#pragma warning disable SA1611 // Element parameters should be documented
        Task<Route> GetRouteByNameAsync(string name);
#pragma warning restore SA1611 // Element parameters should be documented
    }
}