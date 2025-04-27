using Microsoft.EntityFrameworkCore;

namespace Travel.Web.Reposistory
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IFlightRepository Flights { get; }
        ILocationRepository Locations { get; }
        IFlightSegmentRepository FlightSegments { get; }
        IBookingRepository Bookings { get; }
        Task<int> CompleteAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Flights = new FlightRepository(context);
            Locations = new LocationRepository(context);
            FlightSegments = new FlightSegmentRepository(context);
            Bookings = new BookingRepository(context);

        }

        public IUserRepository Users { get; private set; }
        public IFlightRepository Flights { get; private set; }
        public ILocationRepository Locations { get; private set; }
        public IFlightSegmentRepository FlightSegments { get; private set; }
        public IBookingRepository Bookings { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
