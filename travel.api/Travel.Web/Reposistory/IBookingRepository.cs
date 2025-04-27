using Microsoft.EntityFrameworkCore;
using Travel.Web.Models;

namespace Travel.Web.Reposistory
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId);
        Task<Booking> GetBookingWithDetailsAsync(int bookingId);
    }

    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserID == userId)
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.ReturnFlight)
                .Include(b => b.TravelClass)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId)
        {
            return await _context.Bookings
                .Where(b => b.FlightID == flightId || b.ReturnFlightID == flightId)
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.ReturnFlight)
                .Include(b => b.TravelClass)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings
                .Where(b => b.BookingID == bookingId)
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.ReturnFlight)
                .Include(b => b.TravelClass)
                .Include(b => b.BookingSegments)
                    .ThenInclude(bs => bs.FlightSegment)
                .FirstOrDefaultAsync();
        }
    }
}
