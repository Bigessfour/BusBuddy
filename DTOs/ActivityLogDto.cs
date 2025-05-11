using System;

namespace BusBuddy.DTOs
{
    /// <summary>
    /// DTO for activity log entries shown on the dashboard
    /// </summary>
    public class ActivityLogDto
    {
        /// <summary>
        /// Unique identifier for the activity
        /// </summary>
        public int ActivityId { get; set; }
        
        /// <summary>
        /// Activity type (e.g., Route, Driver, Vehicle, Alert)
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;
        
        /// <summary>
        /// Description of the activity
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// When the activity occurred
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
