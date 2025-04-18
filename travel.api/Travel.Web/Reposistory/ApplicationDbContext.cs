namespace Travel.Web.Reposistory
{
    using Microsoft.EntityFrameworkCore;
    using Travel.Web.Models;

    public class ApplicationDbContext : DbContext
    {
        // Constructor  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities  
        public DbSet<User> Users { get; set; }
        public DbSet<FlightCompany> FlightCompanies { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public DbSet<TravelClass> TravelClasses { get; set; }
        public DbSet<FlightSegmentClass> FlightSegmentClasses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSegment> BookingSegments { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        // Configure model relationships  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Specify schema names  
            modelBuilder.Entity<User>().ToTable("users", "flight");
            modelBuilder.Entity<FlightCompany>().ToTable("flightCompanies", "flight");
            modelBuilder.Entity<Flight>().ToTable("flights", "flight");
            modelBuilder.Entity<FlightSegment>().ToTable("flightSegments", "flight");
            modelBuilder.Entity<TravelClass>().ToTable("travelClasses", "flight");
            modelBuilder.Entity<FlightSegmentClass>().ToTable("flightSegmentClasses", "flight");
            modelBuilder.Entity<Booking>().ToTable("bookings", "flight");
            modelBuilder.Entity<BookingSegment>().ToTable("bookingSegments", "flight");
            modelBuilder.Entity<Passenger>().ToTable("passengers", "flight");
            modelBuilder.Entity<Payment>().ToTable("payments", "flight");
            modelBuilder.Entity<Country>().ToTable("countries", "common");
            modelBuilder.Entity<State>().ToTable("states", "common");
            modelBuilder.Entity<City>().ToTable("cities", "common");

            // Users  
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserID);

            // FlightCompany  
            modelBuilder.Entity<FlightCompany>()
                .HasMany(fc => fc.Flights)
                .WithOne(f => f.FlightCompany)
                .HasForeignKey(f => f.CompanyID);

            // Flights  
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.FlightSegments)
                .WithOne(fs => fs.Flight)
                .HasForeignKey(fs => fs.FlightID);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.SourceCity)
                .WithMany()
                .HasForeignKey(f => f.SourceCityID);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationCity)
                .WithMany()
                .HasForeignKey(f => f.DestinationCityID);

            // FlightSegments  
            modelBuilder.Entity<FlightSegment>()
                .HasOne(fs => fs.SegmentSourceCity)
                .WithMany()
                .HasForeignKey(fs => fs.SegmentSource);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(fs => fs.SegmentDestinationCity)
                .WithMany()
                .HasForeignKey(fs => fs.SegmentDestination);

            modelBuilder.Entity<FlightSegment>()
                .HasMany(fs => fs.SegmentClasses)
                .WithOne(fsc => fsc.FlightSegment)
                .HasForeignKey(fsc => fsc.SegmentID);

            // TravelClasses  
            modelBuilder.Entity<TravelClass>()
                .HasMany(tc => tc.SegmentClasses)
                .WithOne(fsc => fsc.TravelClass)
                .HasForeignKey(fsc => fsc.ClassID);

            // Bookings  
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.BookingSegments)
                .WithOne(bs => bs.Booking)
                .HasForeignKey(bs => bs.BookingID);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany()
                .HasForeignKey(b => b.FlightID);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ReturnFlight)
                .WithMany()
                .HasForeignKey(b => b.ReturnFlightID);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.TravelClass)
                .WithMany()
                .HasForeignKey(b => b.ClassID);

            // BookingSegments  
            modelBuilder.Entity<BookingSegment>()
                .HasOne(bs => bs.FlightSegment)
                .WithMany()
                .HasForeignKey(bs => bs.SegmentID);

            modelBuilder.Entity<BookingSegment>()
                .HasOne(bs => bs.TravelClass)
                .WithMany()
                .HasForeignKey(bs => bs.ClassID);

            // Countries, States, and Cities  
            modelBuilder.Entity<Country>()
                .HasMany(c => c.States)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryID);

            modelBuilder.Entity<State>()
                .HasMany(s => s.Cities)
                .WithOne(c => c.State)
                .HasForeignKey(c => c.StateID);
        }
    }
}
