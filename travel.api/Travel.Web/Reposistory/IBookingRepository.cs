using Microsoft.EntityFrameworkCore;
using Travel.Web.Models;

namespace Travel.Web.Reposistory
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId);
        Task<Booking> GetBookingWithDetailsAsync(int bookingId);
        Task AddBookingAsync(Booking booking, IEnumerable<BookingSegment> bookingSegments);
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

        public async Task AddBookingAsync(Booking booking, IEnumerable<BookingSegment> bookingSegments)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Insert booking
                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                // Insert booking segments
                foreach (var segment in bookingSegments)
                {
                    segment.BookingID = booking.BookingID; // Set the foreign key
                    await _context.BookingSegments.AddAsync(segment);
                }

                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction in case of error
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
