using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class FlightSegment
    {
        [Key]
        [Column("segmentid")]
        public int SegmentID { get; set; }

        [Column("flightid")]
        public int FlightID { get; set; }

        [Column("segmentsource")]
        public int SegmentSource { get; set; }

        [Column("segmentdestination")]
        public int SegmentDestination { get; set; }

        [Column("departuretime")]
        public DateTime DepartureTime { get; set; }

        [Column("arrivaltime")]
        public DateTime ArrivalTime { get; set; }

        // Navigation Properties  
        public Flight Flight { get; set; }
        public City SegmentSourceCity { get; set; }
        public City SegmentDestinationCity { get; set; }
        public ICollection<FlightSegmentClass> SegmentClasses { get; set; }
    }
}
