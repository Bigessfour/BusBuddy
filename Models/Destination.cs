using System;

namespace BusBuddy.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Route? Route { get; set; }
        public int? RouteId { get; set; }
    }
}
