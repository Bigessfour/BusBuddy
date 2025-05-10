using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusBuddy.Tests.Utilities;
using Xunit;

namespace BusBuddy.Tests
{    /// <summary>
    /// Base class for tests that need to handle MessageBox dialogs automatically.
    /// Inherit from this class to prevent tests from hanging due to MessageBox.Show() calls.
    /// </summary>
    [SupportedOSPlatform("windows6.1")]  // Indicate this class requires Windows 6.1 (Windows 7) or later
    public abstract class MessageBoxHandlerTestBase : IDisposable
    {
        private readonly MessageBoxHandler _messageBoxHandler;
        private bool _disposedValue;

        /// <summary>
        /// Constructor that starts MessageBox monitoring
        /// </summary>
        /// <param name="autoCloseButton">Which button to automatically click (OK = 1, Cancel = 2, Yes = 6, No = 7)</param>
        public MessageBoxHandlerTestBase(int autoCloseButton = 1) // Default to OK button
        {
            _messageBoxHandler = MessageBoxHandler.Instance;
            _messageBoxHandler.StartMonitoring(autoCloseButton);
        }

        /// <summary>
        /// Gets the list of MessageBox dialogs that have been shown and automatically closed
        /// </summary>
        protected IReadOnlyList<MessageBoxHandler.MessageBoxInfo> CapturedDialogs => 
            _messageBoxHandler.CapturedDialogs;

        /// <summary>
        /// Clears the list of captured dialogs
        /// </summary>
        protected void ClearCapturedDialogs() => _messageBoxHandler.ClearCapturedDialogs();

        /// <summary>
        /// Helper method for tests that need to await a task with a timeout
        /// </summary>
        /// <param name="task">The task to await</param>
        /// <param name="timeoutSeconds">Timeout in seconds</param>
        /// <returns>True if the task completed within the timeout, false if it timed out</returns>
        protected static async Task<bool> WaitForTaskWithTimeout(Task task, int timeoutSeconds)
        {
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
            var completedTask = await Task.WhenAny(task, timeoutTask);
            return completedTask != timeoutTask;
        }

        /// <summary>
        /// Asserts that a MessageBox with the given title was shown
        /// </summary>
        /// <param name="title">The title to search for (case-sensitive)</param>
        protected void AssertMessageBoxShown(string title)
        {
            foreach (var dialog in CapturedDialogs)
            {
                if (dialog.Title == title)
                {
                    return;
                }
            }
            
            Assert.Fail($"Expected MessageBox with title '{title}' was not shown");
        }

        /// <summary>
        /// Asserts that a MessageBox with a title containing the given text was shown
        /// </summary>
        /// <param name="titleContains">The text to search for in the title (case-insensitive)</param>
        protected void AssertMessageBoxShownContaining(string titleContains)
        {
            foreach (var dialog in CapturedDialogs)
            {
                if (dialog.Title.IndexOf(titleContains, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return;
                }
            }
            
            Assert.Fail($"Expected MessageBox with title containing '{titleContains}' was not shown");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Stop monitoring MessageBoxes when the test is done
                    _messageBoxHandler.StopMonitoring();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
