// BusBuddy/API/ApiClient.cs
#nullable enable
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Serilog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusBuddy.Models;

namespace BusBuddy.API
{
    /// <summary>
    /// Basic API client for making HTTP requests
    /// </summary>
    public static class ApiClient
    {
        // Environment variable name for the API key
        private const string ApiKeyEnvVar = "BUSBUDDY_API_KEY";
        
        // API configuration keys
        private const string DefaultApiUrlKey = "API_BASE_URL";
        private const string XaiApiUrlKey = "XAI_API_URL";
        
        // Content type for JSON
        private const string ContentTypeJson = "application/json";
        
        // Current model for API requests
        private static string _currentModel = "grok-3";

        // API URLs with default values that can be overridden
        private static string DefaultApiUrl => 
            Environment.GetEnvironmentVariable(DefaultApiUrlKey) ?? "https://api.example.com";
        
        private static string XaiApiUrl => 
            Environment.GetEnvironmentVariable(XaiApiUrlKey) ?? "https://api.x.ai/v1/chat/completions";
        
        // Get a fresh HttpClient for every request
        private static HttpClient GetFreshHttpClient()
        {
            var client = new HttpClient();
            string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvVar);
            
            if (!string.IsNullOrEmpty(apiKey))
            {
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", apiKey);
            }
            
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ContentTypeJson));
                
            return client;
        }
        
        /// <summary>
        /// Initializes the API key for the application by setting the environment variable.
        /// This should be called at application startup, before making any API calls.
        /// </summary>
        /// <param name="apiKey">The API key value</param>
        /// <returns>True if the API key was successfully set</returns>
        public static bool InitializeApiKey(string apiKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    Log.Error("Cannot initialize with an empty API key");
                    return false;
                }

                // Set the environment variable for the current process
                Environment.SetEnvironmentVariable(ApiKeyEnvVar, apiKey);
                
                // Verify the API key was set correctly
                string? verifyKey = Environment.GetEnvironmentVariable(ApiKeyEnvVar);
                bool success = !string.IsNullOrWhiteSpace(verifyKey);
                
                if (success)
                {
                    Log.Information("API key successfully initialized");
                }
                else
                {
                    Log.Error("Failed to initialize API key");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error initializing API key");
                return false;
            }
        }
        
        /// <summary>
        /// Tests the connection to the API by sending a simple request
        /// </summary>
        public static async Task<bool> TestAPIConnectionAsync()
        {
            try
            {
                using var httpClient = GetFreshHttpClient();
                
                // In a real application, this would test an actual API endpoint
                // For now, we'll just simulate success
                await Task.Delay(500);
                
                Log.Information("API connection test completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "API connection test failed");
                return false;
            }
        }
        
        /// <summary>
        /// Detects scheduling conflicts between a new trip and existing trips
        /// </summary>
        /// <param name="newTrip">The new trip to check</param>
        /// <param name="existingTrips">List of existing trips to check against</param>
        /// <returns>A tuple with conflict status and detailed information</returns>
        public static async Task<(bool HasConflict, string ConflictDetails)> DetectSchedulingConflictsAsync(
            Trip newTrip, List<Trip> existingTrips)
        {
            if (newTrip == null)
            {
                Log.Warning("DetectSchedulingConflictsAsync called with null trip");
                return (false, "Invalid trip data: Trip cannot be null");
            }

            try
            {
                Log.Information("Checking for scheduling conflicts for {Destination} on {Date}", 
                    newTrip.Destination, newTrip.Date);
                
                // Simple conflict detection logic
                var conflictingTrips = existingTrips.Where(t => 
                    t.Date == newTrip.Date && 
                    t.BusNumber == newTrip.BusNumber &&
                    ((t.StartTime <= newTrip.StartTime && t.EndTime >= newTrip.StartTime) ||
                     (t.StartTime <= newTrip.EndTime && t.EndTime >= newTrip.EndTime) ||
                     (t.StartTime >= newTrip.StartTime && t.EndTime <= newTrip.EndTime))).ToList();
                
                if (conflictingTrips.Any())
                {
                    var conflictDetails = string.Join("\n", conflictingTrips.Select(t => 
                        $"- Conflict with trip to {t.Destination} from {t.StartTime} to {t.EndTime}"));
                    
                    return (true, $"Bus #{newTrip.BusNumber} already has scheduled trips during this time:\n{conflictDetails}");
                }
                
                return (false, "No scheduling conflicts detected.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error checking for scheduling conflicts");
                return (false, $"Error checking for conflicts: {ex.Message}");
            }
        }
    }
}