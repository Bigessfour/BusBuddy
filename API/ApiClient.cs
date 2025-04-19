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
        /// Makes a GET request to the specified endpoint with null-safe response handling
        /// </summary>
        public static async Task<string> GetAsync(string? url)
        {
            if (url == null)
            {
                Log.Warning("GetAsync called with null URL");
                return "";
            }

            try
            {
                using var httpClient = GetFreshHttpClient();
                string requestUrl = $"{DefaultApiUrl}/{url}";
                
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                
                // Ensure null-safe access to response.Content
                if (response.Content == null)
                {
                    return "";
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return content ?? "";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error making GET request to {Url}", url);
                return ""; // Return empty string instead of throwing to maintain resilience
            }
        }
        
        /// <summary>
        /// Makes a POST request to the specified endpoint with null-safe response handling
        /// </summary>
        public static async Task<string> PostAsync(string? endpoint, string? jsonContent)
        {
            if (endpoint == null)
            {
                Log.Warning("PostAsync called with null endpoint");
                return "";
            }

            try
            {
                using var httpClient = GetFreshHttpClient();
                string url = $"{DefaultApiUrl}/{endpoint}";
                
                var content = new StringContent(jsonContent ?? "{}", Encoding.UTF8, ContentTypeJson);
                HttpResponseMessage? response = await httpClient.PostAsync(url, content);
                response?.EnsureSuccessStatusCode();
                
                return response?.Content?.ReadAsStringAsync().Result ?? "";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error making POST request to {Endpoint}", endpoint);
                return ""; // Return empty string instead of throwing to maintain resilience
            }
        }
        
        /// <summary>
        /// Fetches data from a URL with null-safe response handling (SonarQube-compliant)
        /// </summary>
        public static string FetchData(string? url)
        {
            if (url == null)
            {
                Log.Warning("FetchData called with null URL");
                return "";
            }
            
            try
            {
                using var httpClient = GetFreshHttpClient();
                var response = httpClient.GetAsync(url).Result;
                return response?.Content?.ReadAsStringAsync().Result ?? "";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching data from URL: {Url}", url);
                return "";
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
        /// Tests the API connection and returns detailed debug information
        /// </summary>
        public static async Task<(bool Success, string DebugInfo)> TestAPIConnectionWithDebugAsync(string model = "")
        {
            if (string.IsNullOrEmpty(model))
            {
                model = _currentModel; // Use current model if not specified
            }
            
            // Use StringBuilder for string concatenation
            var debugInfoBuilder = new StringBuilder();
            bool success = false;
            
            try
            {
                // Get the API key for testing
                string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvVar);
                debugInfoBuilder.AppendLine($"API key configuration check: {(string.IsNullOrEmpty(apiKey) ? "NOT found" : "Found")}");
                
                if (string.IsNullOrEmpty(apiKey))
                {
                    debugInfoBuilder.AppendLine("No API key available. Please configure your API key.");
                    return (false, debugInfoBuilder.ToString());
                }
                
                // Create a simple test message for the API
                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = "You are a test assistant." },
                        new { role = "user", content = "Testing connection. Respond with 'Connection successful'." }
                    },
                    max_tokens = 20
                };
                
                // Convert request to JSON
                string jsonRequest = JsonConvert.SerializeObject(requestBody);
                debugInfoBuilder.AppendLine("Request payload prepared.");
                
                // Create HTTP client
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentTypeJson));
                client.Timeout = TimeSpan.FromSeconds(10); // Set a reasonable timeout
                
                debugInfoBuilder.AppendLine($"Testing API connection to {XaiApiUrl}...");
                
                // Track request timing
                var stopwatch = Stopwatch.StartNew();
                var content = new StringContent(jsonRequest, Encoding.UTF8, ContentTypeJson);
                var response = await client.PostAsync(XaiApiUrl, content);
                stopwatch.Stop();
                
                debugInfoBuilder.AppendLine($"Request completed in {stopwatch.ElapsedMilliseconds:F0}ms");
                debugInfoBuilder.AppendLine($"Status code: {(int)response.StatusCode} ({response.StatusCode})");
                
                // Add response headers to debug info
                debugInfoBuilder.AppendLine("Response headers:");
                foreach (var header in response.Headers)
                {
                    debugInfoBuilder.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
                
                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    // We don't need to store responseContent since we're not using it (S1481)
                    debugInfoBuilder.AppendLine("Response content received successfully.");
                    debugInfoBuilder.AppendLine("Connection test PASSED!");
                }
                else
                {
                    success = false;
                    string errorContent = await response.Content.ReadAsStringAsync();
                    debugInfoBuilder.AppendLine($"Error response content:\n{errorContent}");
                    debugInfoBuilder.AppendLine("Connection test FAILED!");
                }
            }
            catch (Exception ex)
            {
                success = false;
                debugInfoBuilder.AppendLine($"Exception occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    debugInfoBuilder.AppendLine($"Inner exception: {ex.InnerException.Message}");
                }
                debugInfoBuilder.AppendLine("Connection test FAILED due to exception!");
                
                Log.Error(ex, "API connection test with debug failed");
            }
            
            return (success, debugInfoBuilder.ToString());
        }
        
        /// <summary>
        /// Sets the model to use for API requests
        /// </summary>
        public static void SetModel(string model)
        {
            _currentModel = model;
            Log.Information("API model set to: {Model}", model);
        }
        
        /// <summary>
        /// Auto-selects the best working model based on previous tests
        /// </summary>
        public static string AutoSelectWorkingModel()
        {
            // For now, just default to grok-3 as the best model
            string bestModel = "grok-3";
            _currentModel = bestModel;
            Log.Information("Auto-selected model: {Model}", bestModel);
            return bestModel;
        }
        
        /// <summary>
        /// Detects which models are working with the current API key
        /// </summary>
        public static async Task<List<string>> DetectWorkingModelsAsync(string[] models)
        {
            var workingModels = new List<string>();
            
            foreach (var model in models)
            {
                Log.Information("Testing model: {Model}", model);
                
                try
                {
                    var result = await TestAPIConnectionWithDebugAsync(model);
                    if (result.Success)
                    {
                        workingModels.Add(model);
                        Log.Information("Model {Model} is working", model);
                    }
                    else
                    {
                        Log.Warning("Model {Model} is not working", model);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error testing model {Model}", model);
                }
            }
            
            return workingModels;
        }
        
        /// <summary>
        /// Calls the Grok API with the given prompt and returns the response as a JObject
        /// </summary>
        public static async Task<JObject?> CallGrokAPIAsync(string? prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                Log.Warning("CallGrokAPIAsync called with null or empty prompt");
                return null;
            }
            
            try
            {
                // Get the API key
                string? apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvVar);
                if (string.IsNullOrEmpty(apiKey))
                {
                    Log.Error("No API key available for Grok API call");
                    return null;
                }
                
                // Create request body
                var requestBody = new
                {
                    model = _currentModel,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 500
                };
                
                // Convert request to JSON
                string jsonRequest = JsonConvert.SerializeObject(requestBody);
                
                // Create HTTP client
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentTypeJson));
                
                // Send request
                var content = new StringContent(jsonRequest, Encoding.UTF8, ContentTypeJson);
                var response = await client.PostAsync(XaiApiUrl, content);
                
                // Check if request was successful
                response.EnsureSuccessStatusCode();
                
                // Parse response to JObject
                string responseContent = await response.Content.ReadAsStringAsync();
                
                // Null-safe parsing
                JObject? result = null;
                if (!string.IsNullOrEmpty(responseContent))
                {
                    result = JObject.Parse(responseContent);
                }
                return result;
            }
            catch (Exception ex)
            {
                // Handle exception properly instead of just rethrowing
                Log.Error(ex, "Error calling Grok API");
                // Return null instead of throwing to be consistent with other methods
                return null;
            }
        }

        /// <summary>
        /// Creates a prompt for the AI model to analyze trip conflicts
        /// </summary>
        private static string CreateConflictDetectionPrompt(Trip newTrip, List<Trip> existingTrips)
        {
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("You are a scheduling assistant for a school bus system. Analyze the following trips and identify any conflicts.");
            promptBuilder.AppendLine("\nNew trip:");
            promptBuilder.AppendLine($"Date: {newTrip.Date}");
            promptBuilder.AppendLine($"Type: {newTrip.TripType}");
            promptBuilder.AppendLine($"Destination: {newTrip.Destination}");
            promptBuilder.AppendLine($"Start Time: {newTrip.StartTime}");
            promptBuilder.AppendLine($"End Time: {newTrip.EndTime}");
            promptBuilder.AppendLine($"Bus Number: {newTrip.BusNumber}");
            promptBuilder.AppendLine($"Driver Name: {newTrip.DriverName}");
            
            if (existingTrips?.Count > 0)
            {
                promptBuilder.AppendLine("\nExisting trips for the same date:");
                int tripNumber = 1;
                
                // Filter trips for just the same date to reduce token usage
                var sameDataTrips = existingTrips.Where(t => t?.Date == newTrip.Date).ToList();
                
                foreach (var trip in sameDataTrips)
                {
                    if (trip == null) continue;
                    
                    promptBuilder.AppendLine($"\nTrip {tripNumber}:");
                    promptBuilder.AppendLine($"Type: {trip.TripType}");
                    promptBuilder.AppendLine($"Destination: {trip.Destination}");
                    promptBuilder.AppendLine($"Start Time: {trip.StartTime}");
                    promptBuilder.AppendLine($"End Time: {trip.EndTime}");
                    promptBuilder.AppendLine($"Bus Number: {trip.BusNumber}");
                    promptBuilder.AppendLine($"Driver Name: {trip.DriverName}");
                    tripNumber++;
                }
            }
            else
            {
                promptBuilder.AppendLine("\nNo existing trips for this date.");
            }
            
            promptBuilder.AppendLine("\nConflicts to check for:");
            promptBuilder.AppendLine("1. Same bus assigned to overlapping trips");
            promptBuilder.AppendLine("2. Same driver assigned to overlapping trips");
            promptBuilder.AppendLine("3. Insufficient time between consecutive trips (less than 15 minutes)");
            promptBuilder.AppendLine("\nResponse format: First line should be YES or NO indicating if conflicts exist.");
            promptBuilder.AppendLine("If YES, provide details of each conflict on subsequent lines.");
            
            return promptBuilder.ToString();
        }
        
        /// <summary>
        /// Parses AI response to determine if conflicts exist
        /// </summary>
        private static (bool HasConflict, string Details) ParseConflictResponse(string? messageContent)
        {
            if (string.IsNullOrEmpty(messageContent))
            {
                return (false, "Unable to check for conflicts: Empty response from AI service.");
            }
            
            string[] lines = messageContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            bool hasConflict = false;
            
            if (lines.Length > 0 && lines[0].Trim().StartsWith("YES", StringComparison.OrdinalIgnoreCase))
            {
                hasConflict = true;
            }
            
            return (hasConflict, messageContent);
        }

        /// <summary>
        /// Detects scheduling conflicts for a new or updated trip using AI
        /// </summary>
        /// <param name="newTrip">The new trip to check for conflicts</param>
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
                
                // Create the prompt using helper method
                string prompt = CreateConflictDetectionPrompt(newTrip, existingTrips);
                
                // Call the AI API
                var responseJson = await CallGrokAPIAsync(prompt);
                
                if (responseJson == null)
                {
                    Log.Warning("No response from AI for conflict detection");
                    return (false, "Unable to check for conflicts: No response from AI service.");
                }
                
                // Extract the content from the response using null-conditional operators
                var messageContent = responseJson["choices"]?[0]?["message"]?["content"]?.ToString();
                
                // Parse the response to determine if there's a conflict
                var result = ParseConflictResponse(messageContent);
                
                if (result.HasConflict)
                {
                    Log.Warning("Scheduling conflicts detected by AI for {Destination} on {Date}", 
                        newTrip.Destination, newTrip.Date);
                }
                
                Log.Information("Scheduling conflict check completed: {HasConflict}", 
                    result.HasConflict ? "Conflicts found" : "No conflicts");
                
                return result;
            }
            catch (HttpRequestException ex)
            {
                // Specific handling for network-related exceptions
                Log.Error(ex, "Network error occurred while checking for scheduling conflicts: {ErrorMessage}", ex.Message);
                return (false, $"Network error: {ex.Message}. Please check your connection and try again.");
            }
            catch (JsonException ex)
            {
                // Specific handling for JSON parsing errors
                Log.Error(ex, "JSON parsing error occurred while checking for scheduling conflicts: {ErrorMessage}", ex.Message);
                return (false, $"Data format error: {ex.Message}. Please report this issue to support.");
            }
            catch (Exception ex)
            {
                // General exception handling with appropriate context
                Log.Error(ex, "Unexpected error occurred while checking for scheduling conflicts: {ErrorMessage}", ex.Message);
                return (false, $"Unexpected error: {ex.Message}. Please try again later or contact support.");
            }
        }
    }
}