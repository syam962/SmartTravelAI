namespace Travel.Web.Models.DTOs
{
    public class FlightDTO
    {
        public int FlightID { get; set; }
        public string FlightNumber { get; set; }
        public int CompanyID { get; set; }
        public int SourceCityID { get; set; }
        public int DestinationCityID { get; set; }
        public int TotalSeats { get; set; }
        public FlightCompanyDTO FlightCompany { get; set; }
        public CityDTO SourceCity { get; set; }
        public CityDTO DestinationCity { get; set; }
        public ICollection<FlightSegmentDTO> FlightSegments { get; set; }
    }

    public class FlightCompanyDTO
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Headquarters { get; set; }
        public string ContactNumber { get; set; }
        public string Website { get; set; }
    }

    public class FlightSegmentDTO
    {
        public int SegmentID { get; set; }
        public int FlightID { get; set; }
        public int SegmentSource { get; set; }
        public int SegmentDestination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public CityDTO SegmentSourceCity { get; set; }
        public CityDTO SegmentDestinationCity { get; set; }
        public ICollection<FlightSegmentClassDTO> SegmentClasses { get; set; }
    }

    public class FlightSegmentClassDTO
    {
        public int SegmentClassID { get; set; }
        public int SegmentID { get; set; }
        public int ClassID { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal Price { get; set; }
    }

   
}
