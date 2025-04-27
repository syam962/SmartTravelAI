using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class Booking
    {
        [Key]
        [Column("bookingid")]
        public int BookingID { get; set; }

        [Column("userid")]
        public int UserID { get; set; }

        [Column("flightid")]
        public int FlightID { get; set; }

        [Column("returnflightid")]
        public int? ReturnFlightID { get; set; }

        [Column("bookingdate")]
        public DateTime BookingDate { get; set; }

        [Column("numberofpassengers")]
        public int NumberOfPassengers { get; set; }

        [Column("triptype")]
        public string TripType { get; set; }

        [Column("classid")]
        public int ClassID { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Flight Flight { get; set; }
        public Flight ReturnFlight { get; set; }
        public TravelClass TravelClass { get; set; }
        public ICollection<BookingSegment> BookingSegments { get; set; }
    }
}
