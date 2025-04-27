using Microsoft.EntityFrameworkCore;
using Travel.Web.DTO;
using Travel.Web.Models;

namespace Travel.Web.Reposistory
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Task<IEnumerable<Flight>> GetFlightsByCityAsync(int sourceCityId, int destinationCityId);
        Task<IEnumerable<Flight>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId);

        Task<IEnumerable<FlightRoute>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId, bool includeConnections);
        Task<Flight> GetFlightDetailsBySegmentIdAsync(int segmentId);
        Task<FlightSegment> GetFlightSegmentDetailsAsync(int segmentId);


    }

    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetFlightsByCityAsync(int sourceCityId, int destinationCityId)
        {
            return await _context.Set<Flight>()
                .Where(f => f.SourceCityID == sourceCityId && f.DestinationCityID == destinationCityId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Flight>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId)
        {
            return await _context.Set<Flight>()
                .Where(f => f.SourceCityID == sourceCityId &&
                            f.DestinationCityID == destinationCityId &&
                            f.FlightSegments.Any(s => s.DepartureTime >= startDate && s.DepartureTime <= endDate))
                .Include(f => f.FlightSegments) // Ensure flight segments are included
                .ToListAsync();
        }



        public async Task<IEnumerable<FlightRoute>> SearchFlightsAsync(DateTime startDate, DateTime endDate, int sourceCityId, int destinationCityId, bool includeConnections)
        {
            // Direct Flights
            var directSegments = await _context.Set<FlightSegment>()
                .Where(s => s.DepartureTime >= startDate && s.DepartureTime <= endDate &&
                            s.SegmentSource == sourceCityId && s.SegmentDestination == destinationCityId)
                .ToListAsync();

            var directRoutes = directSegments.Select(segment => new FlightRoute
            {
                Segments = new List<FlightSegment> { segment },
                TotalDuration = segment.ArrivalTime - segment.DepartureTime
            }).ToList();

            // Connecting Flights
            var connectingRoutes = new List<FlightRoute>();
            if (includeConnections)
            {
                // Find intermediate connections
                var firstLegSegments = await _context.Set<FlightSegment>()
                    .Where(s => s.DepartureTime >= startDate && s.DepartureTime <= endDate &&
                                s.SegmentSource == sourceCityId)
                    .ToListAsync();

                foreach (var firstLeg in firstLegSegments)
                {
                    var secondLegSegments = await _context.Set<FlightSegment>()
                        .Where(s => s.DepartureTime >= firstLeg.ArrivalTime &&
                                    s.DepartureTime <= endDate &&
                                    s.SegmentSource == firstLeg.SegmentDestination &&
                                    s.SegmentDestination == destinationCityId)
                        .ToListAsync();

                    foreach (var secondLeg in secondLegSegments)
                    {
                        // Combine segments into a route
                        var totalDuration = (secondLeg.ArrivalTime - firstLeg.DepartureTime);
                        connectingRoutes.Add(new FlightRoute
                        {
                            Segments = new List<FlightSegment> { firstLeg, secondLeg },
                            TotalDuration = totalDuration
                        });
                    }
                }
            }

            // Combine direct and connecting routes
            return directRoutes.Concat(connectingRoutes);
        }
        public async Task<Flight> GetFlightDetailsBySegmentIdAsync(int segmentId)
        {
            return await _context.Flights
                .Include(f => f.FlightSegments)
                .ThenInclude(fs => fs.SegmentClasses)
                .FirstOrDefaultAsync(f => f.FlightSegments.Any(fs => fs.SegmentID == segmentId));
        }
       
        public async Task<FlightSegment> GetFlightSegmentDetailsAsync(int segmentId)
        {
            return await _context.FlightSegments
                .Include(fs => fs.Flight)
                .ThenInclude(f => f.FlightCompany)
                .Include(fs => fs.SegmentSourceCity)
                .Include(fs => fs.SegmentDestinationCity)
                .FirstOrDefaultAsync(fs => fs.SegmentID == segmentId);
        }

         

    }

}
