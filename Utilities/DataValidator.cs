// BusBuddy/DataValidator.cs
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
            if (string.IsNullOrWhiteSpace(trip.Date))
                errors.Add("Date is required.");
            if (string.IsNullOrWhiteSpace(trip.DriverName))
                errors.Add("Driver Name is required.");
            else if (!drivers.Contains(trip.DriverName))
                errors.Add("Invalid Driver Name.");
            if (trip.BusNumber == 0)
                errors.Add("Bus Number is required.");
            else if (!busNumbers.Contains(trip.BusNumber))
                errors.Add("Invalid Bus Number.");
            if (string.IsNullOrWhiteSpace(trip.StartTime) || !TimeSpan.TryParse(trip.StartTime, out _))
                errors.Add("Start Time must be in HH:mm format.");
            if (string.IsNullOrWhiteSpace(trip.EndTime) || !TimeSpan.TryParse(trip.EndTime, out _))
                errors.Add("End Time must be in HH:mm format.");

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
    }
}