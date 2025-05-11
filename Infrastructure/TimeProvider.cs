using System;

namespace BusBuddy.Infrastructure
{
    /// <summary>
    /// Custom TimeProvider implementation for .NET 6 compatibility
    /// (TimeProvider was introduced in .NET 8)
    /// </summary>
    public class TimeProvider
    {
        private static readonly TimeProvider _instance = new TimeProvider();

        /// <summary>
        /// Gets the singleton instance of the TimeProvider
        /// </summary>
        public static TimeProvider System => _instance;

        /// <summary>
        /// Gets the current UTC time
        /// </summary>
        /// <returns>Current UTC DateTime</returns>
        public virtual DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;

        /// <summary>
        /// Gets the current local time
        /// </summary>
        /// <returns>Current local DateTime</returns>
        public virtual DateTimeOffset GetLocalNow() => DateTimeOffset.Now;
        
        /// <summary>
        /// Creates a TimeProvider with a fixed time for testing
        /// </summary>
        /// <param name="fixedTime">The fixed time to use</param>
        /// <returns>A TimeProvider that returns the fixed time</returns>
        public static TimeProvider CreateFixed(DateTimeOffset fixedTime)
        {
            return new FixedTimeProvider(fixedTime);
        }

        private class FixedTimeProvider : TimeProvider
        {
            private readonly DateTimeOffset _fixedTime;

            public FixedTimeProvider(DateTimeOffset fixedTime)
            {
                _fixedTime = fixedTime;
            }

            public override DateTimeOffset GetUtcNow() => _fixedTime.ToUniversalTime();
            public override DateTimeOffset GetLocalNow() => _fixedTime.ToLocalTime();
        }
    }
}
