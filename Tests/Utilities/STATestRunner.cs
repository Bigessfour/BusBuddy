using System;
using System.Threading;
using System.Runtime.InteropServices;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BusBuddy.Tests.Utilities
{
    /// <summary>
    /// Custom test runner that executes tests in a Single-Threaded Apartment (STA) thread,
    /// which is required for Windows Forms UI testing.
    /// </summary>
    public class STATestRunner
    {
        private readonly IMessageSink _diagnosticMessageSink;
        private static readonly bool IsRunningInContainer = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        
        public STATestRunner(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }
        
        /// <summary>
        /// Run a test method in an STA thread
        /// </summary>
        /// <param name="testMethod">The delegate/method to execute</param>
        /// <returns>True if the test passes, false otherwise</returns>
        public bool RunTest(Action testMethod)
        {
            if (IsRunningInContainer || !IsWindows)
            {
                throw new SkipException("This test requires Windows and cannot run in a container");
            }

            Exception exception = null;
            var thread = new Thread(() => 
            {
                try 
                {
                    #if !CONTAINER
                    if (IsWindows) 
                    {
                        // Only call Windows Forms API when actually running on Windows
                        System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.SystemAware);
                        System.Windows.Forms.Application.EnableVisualStyles();
                        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                    }
                    #endif
                    
                    // Run the test method
                    testMethod();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            
            // Set thread to STA mode required for Windows Forms
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(); // Wait for thread to complete
            
            if (exception != null)
            {
                // Re-throw the exception in the calling thread
                throw new Exception("Test failed in STA thread", exception);
            }
            
            return true;
        }
    }
}
