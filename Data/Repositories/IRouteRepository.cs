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
        Task<Route> GetRouteByNameAsync(string name);
    }
}