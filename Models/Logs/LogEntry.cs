// LogEntry.cs
using System;

namespace BusBuddy.Models.Logs
{
    /// <summary>
    /// Represents a log entry in the system
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// The unique identifier for the log entry
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The date and time when the log was created
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// The log level
        /// </summary>
        public string Level { get; set; } = string.Empty;
        
        /// <summary>
        /// The log message
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
