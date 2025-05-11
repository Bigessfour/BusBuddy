using System;
using Xunit;

namespace BusBuddy.Tests.Utilities
{
    /// <summary>
    /// Trait attribute to mark tests that require Windows or cannot run in containers
    /// </summary>
    public sealed class WindowsOnlyTraitAttribute : Attribute, ITraitAttribute
    {
        public const string Category = "WindowsOnly";
        public const string Value = "true";

        public WindowsOnlyTraitAttribute()
        {
        }
    }
}
