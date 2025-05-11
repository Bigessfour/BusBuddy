using System;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// Interface for dashboard data objects that provide key metrics
    /// </summary>
    public interface IDashboardData
    {
        /// <summary>
        /// Gets a key metric value for dashboard display
        /// </summary>
        /// <param name="metricName">Name of the metric to retrieve</param>
        /// <returns>The metric value as an object that can be cast to appropriate type</returns>
        object GetKeyMetric(string metricName);
    }
}
