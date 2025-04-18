using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class BookingSegment
    {
        [Key]
        [Column("bookingsegmentid")]
        public int BookingSegmentID { get; set; }

        [Column("bookingid")]
        public int BookingID { get; set; }

        [Column("segmentid")]
        public int SegmentID { get; set; }

        [Column("passengercount")]
        public int PassengerCount { get; set; }

        [Column("classid")]
        public int ClassID { get; set; }

        // Navigation Properties
        public Booking Booking { get; set; }
        public FlightSegment FlightSegment { get; set; }
        public TravelClass TravelClass { get; set; }
    }
}
