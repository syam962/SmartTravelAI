using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class FlightSegmentClass
    {
        [Key]
        [Column("segmentclassid")]
        public int SegmentClassID { get; set; }

        [Column("segmentid")]
        public int SegmentID { get; set; }

        [Column("classid")]
        public int ClassID { get; set; }

        [Column("totalseats")]
        public int TotalSeats { get; set; }

        [Column("availableseats")]
        public int AvailableSeats { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        // Navigation Properties
        public FlightSegment FlightSegment { get; set; }
        public TravelClass TravelClass { get; set; }
    }
}
