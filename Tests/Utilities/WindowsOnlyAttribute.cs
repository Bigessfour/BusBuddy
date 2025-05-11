using System;
using Xunit;

namespace BusBuddy.Tests.Utilities
{
    /// <summary>
    /// Attribute to mark tests that require Windows and should be skipped in containers
    /// </summary>
    public class WindowsOnlyAttribute : FactAttribute
    {
        public WindowsOnlyAttribute()
        {
            // Skip test if running in a container
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
            {
                Skip = "Test requires Windows and cannot run in a container";
            }
        }
    }
}
