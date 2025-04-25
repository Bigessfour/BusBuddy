namespace BusBuddy.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        
        // Added for compatibility with RouteService
        public int RouteId 
        { 
            get => RouteID; 
            set => RouteID = value; 
        }
        
        public string RouteName { get; set; }
        public string Description { get; set; }
        public int? DefaultBusNumber { get; set; }
        public int? DefaultDriverID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}