using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusBuddy.Tests.Utilities
{    /// <summary>
    /// Utility class to automatically handle MessageBox dialogs during tests.
    /// This prevents tests from hanging when MessageBox.Show() is called.
    /// </summary>
    [SupportedOSPlatform("windows6.1")]  // Indicate this class requires Windows 6.1 (Windows 7) or later
    public class MessageBoxHandler
    {
        // Win32 API imports
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // Windows message constants
        private const int WM_COMMAND = 0x0111;
        private const int IDOK = 1;      // OK button
        private const int IDCANCEL = 2;  // Cancel button
        private const int IDYES = 6;     // Yes button
        private const int IDNO = 7;      // No button

        // Singleton instance
        private static MessageBoxHandler _instance;
        private static readonly object _lock = new object();

        // Fields to track message box state
        private readonly List<MessageBoxInfo> _capturedDialogs = new List<MessageBoxInfo>();
        private CancellationTokenSource _cts;
        private Task _monitorTask;
        private bool _isMonitoring;        /// <summary>
        /// Structure to hold information about a captured MessageBox
        /// </summary>
        [SupportedOSPlatform("windows6.1")]  // Indicate this class requires Windows 6.1 (Windows 7) or later
        public class MessageBoxInfo
        {
            public string Title { get; set; }
            public MessageBoxButtons Buttons { get; set; }
            public MessageBoxIcon Icon { get; set; }
            public DateTime Timestamp { get; set; }
            public DialogResult Result { get; set; }
        }

        private MessageBoxHandler()
        {
            // Private constructor for singleton pattern
        }

        /// <summary>
        /// Gets the singleton instance of the MessageBoxHandler
        /// </summary>
        public static MessageBoxHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MessageBoxHandler();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Returns any message boxes that were captured while monitoring was active
        /// </summary>
        public IReadOnlyList<MessageBoxInfo> CapturedDialogs => _capturedDialogs.AsReadOnly();

        /// <summary>
        /// Starts monitoring for MessageBox dialogs. Any dialogs shown will be automatically closed.
        /// </summary>
        /// <param name="autoCloseButton">Specifies which button to click when a dialog appears (IDOK = OK, IDYES = Yes, etc.)</param>
        public void StartMonitoring(int autoCloseButton = IDOK)
        {
            if (_isMonitoring)
                return;

            lock (_lock)
            {
                if (_isMonitoring)
                    return;

                _cts = new CancellationTokenSource();
                _capturedDialogs.Clear();
                _isMonitoring = true;

                // Start a background thread to monitor for message boxes
                _monitorTask = Task.Run(async () =>
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {                        try
                        {
                            // Look for MessageBox dialogs - the class name is "#32770" for standard dialog boxes
                            IntPtr hWnd = FindWindow("#32770", null!); // Null forgiving operator for P/Invoke
                            if (hWnd != IntPtr.Zero)
                            {
                                // Get the window title
                                StringBuilder title = new StringBuilder(256);
                                GetWindowText(hWnd, title, title.Capacity);                                // Capture information about the dialog
                                var info = new MessageBoxInfo
                                {
                                    Title = title.ToString(),
                                    Timestamp = DateTime.Now,
                                    Result = DialogResult.OK // Default result - OK on Windows
                                };

                                lock (_capturedDialogs)
                                {
                                    _capturedDialogs.Add(info);
                                }

                                // Bring the window to the foreground
                                SetForegroundWindow(hWnd);

                                // Find the OK/Yes button and click it
                                IntPtr button = FindWindowEx(hWnd, IntPtr.Zero, "Button", "&OK");
                                if (button == IntPtr.Zero)
                                    button = FindWindowEx(hWnd, IntPtr.Zero, "Button", "&Yes");
                                if (button == IntPtr.Zero)
                                    button = FindWindowEx(hWnd, IntPtr.Zero, "Button", "OK");
                                if (button == IntPtr.Zero)
                                    button = FindWindowEx(hWnd, IntPtr.Zero, "Button", "Yes");

                                // If we found a button, click it
                                if (button != IntPtr.Zero)
                                {
                                    SendMessage(hWnd, WM_COMMAND, autoCloseButton, 0);
                                    Debug.WriteLine($"Automatically closed MessageBox with title: {info.Title}");
                                }
                                else
                                {
                                    // Try closing the dialog directly
                                    SendMessage(hWnd, WM_COMMAND, autoCloseButton, 0);
                                    Debug.WriteLine($"Attempted to close MessageBox with title: {info.Title} using default button ID");
                                }

                                // Give the UI thread time to process the message
                                await Task.Delay(100);
                            }

                            // Don't check too frequently
                            await Task.Delay(100, _cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            // Normal cancellation
                            break;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error in MessageBoxHandler monitor: {ex.Message}");
                        }
                    }
                }, _cts.Token);
            }
        }

        /// <summary>
        /// Stops monitoring for MessageBox dialogs
        /// </summary>
        public void StopMonitoring()
        {
            if (!_isMonitoring)
                return;

            lock (_lock)
            {
                if (!_isMonitoring)
                    return;

                _cts?.Cancel();
                try
                {
                    _monitorTask?.Wait(2000); // Wait up to 2 seconds for the task to complete
                }
                catch (AggregateException)
                {
                    // Task was properly cancelled
                }                _isMonitoring = false;
                _cts?.Dispose();
                _cts = null!; // Using null-forgiving operator since we know it's intentionally nulled
            }
        }

        /// <summary>
        /// Clears the list of captured dialogs
        /// </summary>
        public void ClearCapturedDialogs()
        {
            lock (_capturedDialogs)
            {
                _capturedDialogs.Clear();
            }
        }
    }
}
