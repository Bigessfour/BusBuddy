using System;
using System.IO;

namespace BusBuddy
{
    public static class LogBackup
    {
        public static void LogToErrorsFile(string message)
        {
            try
            {
                string logFile = Path.Combine(Directory.GetCurrentDirectory(), "busbuddy_errors.log");
                File.AppendAllText(logFile, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
