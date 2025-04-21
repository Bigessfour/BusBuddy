// BusBuddy/Utilities/DataManager.cs
using System;
using System.Threading.Tasks;
using Serilog;
using BusBuddy.Models;
using BusBuddy.Data;

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
        
        // Helper methods for specific record types to resolve delegate conversion issues
        
        // For Fuel records
        public static async Task<bool> AddFuelRecordAsync(Fuel fuel, DatabaseManager dbManager, ILogger logger, Action<string, System.Drawing.Color> updateStatus)
        {
            try
            {
                updateStatus("Adding fuel record...", AppSettings.Theme.InfoColor);
                
                // Convert from Fuel to FuelRecord
                var fuelRecord = new FuelRecord
                {
                    RecordId = fuel.Fuel_ID,
                    BusNumber = fuel.Bus_Number,
                    Fuel_Date = fuel.Fuel_Date,
                    Gallons = fuel.Fuel_Gallons,
                    Fuel_Type = fuel.Fuel_Type,
                    Odometer_Reading = fuel.Odometer_Reading
                };
                
                dbManager.AddFuelRecord(fuelRecord);
                updateStatus("Fuel record added.", AppSettings.Theme.SuccessColor);
                logger.Information("Successfully added fuel record.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to add fuel record.");
                updateStatus("Error adding fuel record.", AppSettings.Theme.ErrorColor);
                return false;
            }
        }
        
        // For Activity records
        public static async Task<bool> AddActivityRecordAsync(ActivityTrip activity, DatabaseManager dbManager, ILogger logger, Action<string, System.Drawing.Color> updateStatus)
        {
            try
            {
                updateStatus("Adding activity record...", AppSettings.Theme.InfoColor);
                
                // Convert from ActivityTrip to Activity
                var activityRecord = new Activity
                {
                    ActivityId = activity.ActivityID,
                    Name = activity.Destination,
                    Description = $"Bus: {activity.BusNumber}, Driver: {activity.Driver}",
                    Date = activity.Date,
                    BusNumber = activity.BusNumber,
                    Destination = activity.Destination,
                    LeaveTime = activity.LeaveTime,
                    Driver = activity.Driver,
                    HoursDriven = Convert.ToDouble(activity.HoursDriven),
                    StudentsDriven = activity.StudentsDriven
                };
                
                dbManager.AddActivity(activityRecord);
                updateStatus("Activity record added.", AppSettings.Theme.SuccessColor);
                logger.Information("Successfully added activity record.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to add activity record.");
                updateStatus("Error adding activity record.", AppSettings.Theme.ErrorColor);
                return false;
            }
        }
        
        // For Driver records
        public static async Task<bool> AddDriverAsync(Driver driver, DatabaseManager dbManager, ILogger logger, Action<string, System.Drawing.Color> updateStatus)
        {
            try
            {
                updateStatus("Adding driver record...", AppSettings.Theme.InfoColor);
                
                dbManager.AddDriver(driver);
                updateStatus("Driver record added.", AppSettings.Theme.SuccessColor);
                logger.Information("Successfully added driver record.");
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to add driver record.");
                updateStatus("Error adding driver record.", AppSettings.Theme.ErrorColor);
                return false;
            }
        }
    }
}