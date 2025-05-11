using System;
using System.Threading;
using System.Windows.Forms;
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
            Exception exception = null;
            var thread = new Thread(() => 
            {
                try 
                {
                    // Set up Windows message pump for UI testing
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    
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
