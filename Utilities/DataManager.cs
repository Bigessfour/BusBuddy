// BusBuddy/Utilities/DataManager.cs
using System;
using System.Threading.Tasks;
using Serilog;

namespace BusBuddy.Utilities
{
    public static class DataManager
    {
        public static async Task<bool> AddRecordAsync<T>(T record, Action<T> addAction, ILogger logger, Action<string, System.Drawing.Color> updateStatus)
        {
            try
            {
                updateStatus("Adding record...", AppSettings.Theme.InfoColor);
                addAction(record);
                updateStatus("Record added.", AppSettings.Theme.SuccessColor);
                logger.Information("Successfully added record of type {Type}.", typeof(T).Name);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to add record of type {Type}.", typeof(T).Name);
                updateStatus("Error adding record.", AppSettings.Theme.ErrorColor);
                return false;
            }
        }
    }
}