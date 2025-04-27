using Travel.Web.Models;

namespace Travel.Web.DTO
{
    public class FlightRoute
    {
        public List<FlightSegment> Segments { get; set; } = new List<FlightSegment>();
        public TimeSpan TotalDuration { get; set; }
    }
}
