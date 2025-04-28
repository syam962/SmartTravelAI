using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Web.Models;

namespace Travel.Web.Reposistory
{
    public interface IFlightSegmentRepository : IRepository<FlightSegment>
    {
        Task<IEnumerable<FlightSegment>> GetSegmentsWithinDateRangeAsync(DateTime startDate, DateTime endDate,int sourcecity, int destinationCity);
    }

    public class FlightSegmentRepository : Repository<FlightSegment>, IFlightSegmentRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightSegmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightSegment>> GetSegmentsWithinDateRangeAsync(DateTime startDate, DateTime endDate, int sourcecity, int destinationCity)
        {
            return await _context.FlightSegments
                .Where(segment => segment.DepartureTime >= startDate && segment.DepartureTime <= endDate && segment.SegmentSource == sourcecity && segment.SegmentDestination == destinationCity)
                .Include(segment => segment.Flight) // Include related Flight entity
                .Include(segment => segment.SegmentSourceCity) // Include source city
                .Include(segment => segment.SegmentDestinationCity) 
                .Include(segment=>segment.SegmentClasses)// Include Segment classes for price and class
                .ToListAsync();
        }
    }
}
