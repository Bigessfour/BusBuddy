using System;
using System.Collections.Generic;
using System.Linq;
using BusBuddy.Models;

namespace BusBuddy
{
    public static class DataValidator
    {
        public static (bool IsValid, List<string> Errors) ValidateTrip(Trip trip, List<string> drivers, List<int> busNumbers)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(trip.TripType))
                errors.Add("Trip Type is required.");
            if (trip.Date == default)
                errors.Add("Date is required.");
            if (string.IsNullOrWhiteSpace(trip.DriverName))
                errors.Add("Driver Name is required.");
            else if (!drivers.Contains(trip.DriverName))
                errors.Add("Invalid Driver Name.");
            if (trip.BusNumber == 0)
                errors.Add("Bus Number is required.");
            else if (!busNumbers.Contains(trip.BusNumber))
                errors.Add("Invalid Bus Number.");
            if (trip.StartTime == default)
                errors.Add("Start Time is required.");
            if (trip.EndTime == default)
                errors.Add("End Time is required.");
            if (string.IsNullOrWhiteSpace(trip.Destination))
                errors.Add("Destination is required.");

            return (errors.Count == 0, errors);
        }

        public static (bool IsValid, List<string> Errors) ValidateFuel(Fuel fuel, List<int> busNumbers)
        {
            var errors = new List<string>();

            if (fuel.Bus_Number == 0)
                errors.Add("Bus Number is required.");
            else if (!busNumbers.Contains(fuel.Bus_Number))
                errors.Add("Invalid Bus Number.");
            if (fuel.Fuel_Gallons <= 0)
                errors.Add("Fuel Gallons must be a positive number.");
            if (string.IsNullOrWhiteSpace(fuel.Fuel_Date))
                errors.Add("Fuel Date is required.");
            if (string.IsNullOrWhiteSpace(fuel.Fuel_Type))
                errors.Add("Fuel Type is required.");
            if (fuel.Odometer_Reading < 0)
                errors.Add("Odometer Reading must be a non-negative number.");

            return (errors.Count == 0, errors);
        }

        public static (bool IsValid, List<string> Errors) ValidateDriver(Driver driver)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(driver.Driver_Name))
                errors.Add("Driver Name is required.");
            if (string.IsNullOrWhiteSpace(driver.DL_Type))
                errors.Add("DL Type is required.");

            return (errors.Count == 0, errors);
        }

        public static (bool IsValid, List<string> Errors) ValidateActivity(ActivityTrip activity, List<string> drivers, List<int> busNumbers)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(activity.Date))
                errors.Add("Date is required.");
            if (activity.BusNumber == 0)
                errors.Add("Bus Number is required.");
            else if (!busNumbers.Contains(activity.BusNumber))
                errors.Add("Invalid Bus Number.");
            if (string.IsNullOrWhiteSpace(activity.Destination))
                errors.Add("Destination is required.");
            if (string.IsNullOrWhiteSpace(activity.LeaveTime) || !TimeSpan.TryParse(activity.LeaveTime, out _))
                errors.Add("Leave Time must be in HH:mm format.");
            if (string.IsNullOrWhiteSpace(activity.Driver))
                errors.Add("Driver is required.");
            else if (!drivers.Contains(activity.Driver))
                errors.Add("Invalid Driver Name.");
            if (string.IsNullOrWhiteSpace(activity.HoursDriven) || !TimeSpan.TryParse(activity.HoursDriven, out _))
                errors.Add("Hours Driven must be in HH:mm format.");
            if (activity.StudentsDriven < 0)
                errors.Add("Students Driven must be a non-negative number.");

            return (errors.Count == 0, errors);
        }
    }
}