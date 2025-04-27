using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class Flight
    {
        [Key]
        [Column("flightid")]
        public int FlightID { get; set; }

        [Column("flightnumber")]
        public string FlightNumber { get; set; }

        [Column("companyid")]
        public int CompanyID { get; set; }

        [Column("sourcecityid")]
        public int SourceCityID { get; set; }

        [Column("destinationcityid")]
        public int DestinationCityID { get; set; }

        [Column("totalseats")]
        public int TotalSeats { get; set; }

        // Navigation Properties  
        public FlightCompany FlightCompany { get; set; }
        public City SourceCity { get; set; }
        public City DestinationCity { get; set; }
        public ICollection<FlightSegment> FlightSegments { get; set; }
    }
}
