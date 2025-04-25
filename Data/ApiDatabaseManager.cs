using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBuddy.API;
using BusBuddy.Models;
using Serilog;

namespace BusBuddy.Data
{
    public class ApiDatabaseManager : IDatabaseManager
    {
        private readonly ApiClient _apiClient;
        private readonly Serilog.ILogger _logger;

        public ApiDatabaseManager(ApiClient apiClient, Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        #region Trips
        public List<Trip> GetTrips()
        {
            try
            {
                var result = _apiClient.GetData<List<Trip>>("trips");
                return result ?? new List<Trip>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving trips from API");
                return new List<Trip>();
            }
        }

        public List<Trip> GetTripsByDate(DateOnly date)
        {
            try
            {
                var endpoint = $"trips/date/{date:yyyy-MM-dd}";
                var result = _apiClient.GetData<List<Trip>>(endpoint);
                return result ?? new List<Trip>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving trips for date {Date} from API", date);
                return new List<Trip>();
            }
        }

        public void AddTrip(Trip trip)
        {
            try
            {
                if (!_apiClient.PostData("trips", trip))
                {
                    throw new Exception("Failed to add trip");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding trip to API");
                throw;
            }
        }
        #endregion

        #region Drivers
        public List<string> GetDriverNames()
        {
            try
            {
                var result = _apiClient.GetData<List<string>>("drivers/names");
                return result ?? new List<string>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving driver names from API");
                return new List<string>();
            }
        }

        public List<Driver> GetDrivers()
        {
            try
            {
                var result = _apiClient.GetData<List<Driver>>("drivers");
                return result ?? new List<Driver>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving drivers from API");
                return new List<Driver>();
            }
        }

        public bool AddDriver(Driver driver)
        {
            try
            {
                return _apiClient.PostData("drivers", driver);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding driver to API");
                return false;
            }
        }

        public bool UpdateDriver(Driver driver)
        {
            try
            {
                return _apiClient.PutData($"drivers/{driver.DriverID}", driver);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating driver in API");
                return false;
            }
        }

        public bool DeleteDriver(int driverId)
        {
            try
            {
                return _apiClient.DeleteData($"drivers/{driverId}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting driver from API");
                return false;
            }
        }
        #endregion

        #region Vehicles
        public List<int> GetBusNumbers()
        {
            try
            {
                var result = _apiClient.GetData<List<int>>("vehicles/numbers");
                return result ?? new List<int>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving bus numbers from API");
                return new List<int>();
            }
        }
        #endregion

        #region Routes
        public List<Route> GetRoutes()
        {
            try
            {
                var result = _apiClient.GetData<List<Route>>("routes");
                return result ?? new List<Route>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving routes from API");
                return new List<Route>();
            }
        }

        public void AddRoute(Route route)
        {
            try
            {
                if (!_apiClient.PostData("routes", route))
                {
                    throw new Exception("Failed to add route");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding route to API");
                throw;
            }
        }

        public void UpdateRoute(Route route)
        {
            try
            {
                if (!_apiClient.PutData($"routes/{route.RouteID}", route))
                {
                    throw new Exception("Failed to update route");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating route in API");
                throw;
            }
        }

        public bool DeleteRoute(int routeId)
        {
            try
            {
                return _apiClient.DeleteData($"routes/{routeId}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting route from API");
                return false;
            }
        }
        #endregion

        #region School Calendar
        public SchoolCalendarDay? GetCalendarDay(DateTime date)
        {
            try
            {
                var endpoint = $"calendar/{date:yyyy-MM-dd}";
                return _apiClient.GetData<SchoolCalendarDay>(endpoint);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving calendar day from API for date {Date}", date);
                return null;
            }
        }

        public void AddOrUpdateCalendarDay(SchoolCalendarDay day)
        {
            try
            {
                if (!_apiClient.PostData("calendar", day))
                {
                    throw new Exception("Failed to add/update calendar day");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding/updating calendar day in API");
                throw;
            }
        }

        public void AddOrUpdateCalendarDays(IEnumerable<SchoolCalendarDay> days)
        {
            try
            {
                if (!_apiClient.PostData("calendar/batch", days))
                {
                    throw new Exception("Failed to batch add/update calendar days");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error batch adding/updating calendar days in API");
                throw;
            }
        }

        public List<ScheduledRoute> GetScheduledRoutes(int calendarDayId)
        {
            try
            {
                var endpoint = $"scheduled-routes/calendar/{calendarDayId}";
                var result = _apiClient.GetData<List<ScheduledRoute>>(endpoint);
                return result ?? new List<ScheduledRoute>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving scheduled routes for calendar day ID {ID} from API", calendarDayId);
                return new List<ScheduledRoute>();
            }
        }

        public List<ScheduledRoute> GetScheduledRoutes(DateTime date)
        {
            try
            {
                var endpoint = $"scheduled-routes/date/{date:yyyy-MM-dd}";
                var result = _apiClient.GetData<List<ScheduledRoute>>(endpoint);
                return result ?? new List<ScheduledRoute>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving scheduled routes for date {Date} from API", date);
                return new List<ScheduledRoute>();
            }
        }

        public void UpdateScheduledRoute(ScheduledRoute route)
        {
            try
            {
                if (!_apiClient.PutData($"scheduled-routes/{route.ScheduledRouteID}", route))
                {
                    throw new Exception("Failed to update scheduled route");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating scheduled route in API");
                throw;
            }
        }
        #endregion

        #region Activities
        public List<Activity> GetActivities()
        {
            try
            {
                var result = _apiClient.GetData<List<Activity>>("activities");
                return result ?? new List<Activity>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving activities from API");
                return new List<Activity>();
            }
        }

        public void AddActivity(Activity activity)
        {
            try
            {
                if (!_apiClient.PostData("activities", activity))
                {
                    throw new Exception("Failed to add activity");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding activity to API");
                throw;
            }
        }
        #endregion

        #region Fuel Records
        public List<FuelRecord> GetFuelRecords()
        {
            try
            {
                var result = _apiClient.GetData<List<FuelRecord>>("fuel");
                return result ?? new List<FuelRecord>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving fuel records from API");
                return new List<FuelRecord>();
            }
        }

        public bool AddFuelRecord(FuelRecord record)
        {
            try
            {
                return _apiClient.PostData("fuel", record);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding fuel record to API");
                return false;
            }
        }

        public bool UpdateFuelRecord(FuelRecord record)
        {
            try
            {
                return _apiClient.PutData($"fuel/{record.FuelID}", record);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating fuel record in API");
                return false;
            }
        }

        public bool DeleteFuelRecord(int recordId)
        {
            try
            {
                return _apiClient.DeleteData($"fuel/{recordId}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting fuel record from API");
                return false;
            }
        }
        #endregion

        #region Maintenance Records
        public List<Maintenance> GetMaintenanceRecords()
        {
            try
            {
                var result = _apiClient.GetData<List<Maintenance>>("maintenance");
                return result ?? new List<Maintenance>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving maintenance records from API");
                return new List<Maintenance>();
            }
        }

        public void AddMaintenanceRecord(Maintenance maintenance)
        {
            try
            {
                if (!_apiClient.PostData("maintenance", maintenance))
                {
                    throw new Exception("Failed to add maintenance record");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error adding maintenance record to API");
                throw;
            }
        }

        public void UpdateMaintenanceRecord(Maintenance maintenance)
        {
            try
            {
                if (!_apiClient.PutData($"maintenance/{maintenance.MaintenanceID}", maintenance))
                {
                    throw new Exception("Failed to update maintenance record");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating maintenance record in API");
                throw;
            }
        }

        public void DeleteMaintenanceRecord(int maintenanceId)
        {
            try
            {
                if (!_apiClient.DeleteData($"maintenance/{maintenanceId}"))
                {
                    throw new Exception("Failed to delete maintenance record");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting maintenance record from API");
                throw;
            }
        }
        #endregion

        public DatabaseStatistics GetDatabaseStatistics()
        {
            try
            {
                return _apiClient.GetData<DatabaseStatistics>("statistics") 
                    ?? new DatabaseStatistics();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving database statistics from API");
                return new DatabaseStatistics();
            }
        }
    }
}
