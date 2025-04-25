#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Newtonsoft.Json;

namespace BusBuddy.API
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly Serilog.ILogger _logger;
        private readonly string _apiKey;
        private readonly bool _isEnabled;

        // Constructor with direct URL and API key for simpler instantiation
        public ApiClient(string baseUrl, string apiKey, Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiKey = apiKey;
            
            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.Error("API URL is not configured");
                _isEnabled = false;
                return;
            }

            try
            {
                // Configure HttpClient directly
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(baseUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(30);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Set Authentication Header if key exists
                if (!string.IsNullOrEmpty(_apiKey))
                {
                    _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
                    _isEnabled = true;
                }
                else
                {
                    _logger.Warning("API Key is not provided. API functionality will be limited.");
                    _isEnabled = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error initializing API client");
                _isEnabled = false;
            }
        }

        public bool TestConnection()
        {
            try
            {
                _logger.Information("Testing API connection...");
                var request = new HttpRequestMessage(HttpMethod.Head, "");
                var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.Information("API connection test successful");
                    return true;
                }

                _logger.Warning("API connection test failed with status code: {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API connection test failed with exception");
                return false;
            }
        }

        public bool IsApiEnabled => _isEnabled && _httpClient != null;

        // Synchronous version for simpler consumption
        public T GetData<T>(string endpoint) where T : class
        {
            if (!IsApiEnabled)
            {
                _logger.Warning("API is disabled. Cannot perform GET request to {Endpoint}", endpoint);
                return null;
            }

            try
            {
                _logger.Information("Sending GET request to {Endpoint}", endpoint);
                var response = _httpClient.GetAsync(endpoint).GetAwaiter().GetResult();

                _logger.Debug("Received status {StatusCode} from GET {Endpoint}", response.StatusCode, endpoint);
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during GET {Endpoint}", endpoint);
                return null;
            }
        }

        public bool PostData<T>(string endpoint, T data)
        {
            if (!IsApiEnabled)
            {
                _logger.Warning("API is disabled. Cannot perform POST request to {Endpoint}", endpoint);
                return false;
            }

            try
            {
                _logger.Information("Sending POST request to {Endpoint}", endpoint);
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(endpoint, content).GetAwaiter().GetResult();
                _logger.Debug("Received status {StatusCode} from POST {Endpoint}", response.StatusCode, endpoint);
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during POST {Endpoint}", endpoint);
                return false;
            }
        }

        public bool PutData<T>(string endpoint, T data)
        {
            if (!IsApiEnabled)
            {
                _logger.Warning("API is disabled. Cannot perform PUT request to {Endpoint}", endpoint);
                return false;
            }

            try
            {
                _logger.Information("Sending PUT request to {Endpoint}", endpoint);
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PutAsync(endpoint, content).GetAwaiter().GetResult();
                _logger.Debug("Received status {StatusCode} from PUT {Endpoint}", response.StatusCode, endpoint);
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during PUT {Endpoint}", endpoint);
                return false;
            }
        }

        public bool DeleteData(string endpoint)
        {
            if (!IsApiEnabled)
            {
                _logger.Warning("API is disabled. Cannot perform DELETE request to {Endpoint}", endpoint);
                return false;
            }

            try
            {
                _logger.Information("Sending DELETE request to {Endpoint}", endpoint);
                var response = _httpClient.DeleteAsync(endpoint).GetAwaiter().GetResult();
                _logger.Debug("Received status {StatusCode} from DELETE {Endpoint}", response.StatusCode, endpoint);
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during DELETE {Endpoint}", endpoint);
                return false;
            }
        }
    }
}
#pragma warning restore CS1591