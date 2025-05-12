using BusBuddy.Data;

namespace BusBuddy.Services
{
    public class BusScheduleHelper : IBusScheduleHelper
    {
        public string FormatSchedule(BusSchedule schedule)
        {
            return $"{schedule.RouteName} at {schedule.Time:HH:mm}";
        }
    }
}
