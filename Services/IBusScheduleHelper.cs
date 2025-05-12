using BusBuddy.Data;

namespace BusBuddy.Services
{
    public interface IBusScheduleHelper
    {
        string FormatSchedule(BusSchedule schedule);
    }
}
