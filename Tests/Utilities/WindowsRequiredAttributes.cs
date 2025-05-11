using System;
using System.Runtime.InteropServices;
using Xunit;

namespace BusBuddy.Tests.Utilities
{
    /// <summary>
    /// Provides a fact attribute that skips the test when running in a container or non-Windows environment
    /// </summary>
    public class WindowsRequiredFactAttribute : FactAttribute
    {
        public WindowsRequiredFactAttribute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
            {
                Skip = "This test requires Windows and cannot run in a container";
            }
        }
    }
    
    /// <summary>
    /// Provides a theory attribute that skips the test when running in a container or non-Windows environment
    /// </summary>
    public class WindowsRequiredTheoryAttribute : TheoryAttribute
    {
        public WindowsRequiredTheoryAttribute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
            {
                Skip = "This test requires Windows and cannot run in a container";
            }
        }
    }
}
