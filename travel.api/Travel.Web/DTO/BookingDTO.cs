using System;
using System.Collections.Generic;

namespace Travel.Web.Models.DTOs
{
    public class BookingDTO
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public int FlightID { get; set; }
        public int? ReturnFlightID { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfPassengers { get; set; }
        public string TripType { get; set; }
        public int ClassID { get; set; }
       // public UserDTO User { get; set; }
        public FlightDTO Flight { get; set; }
        public FlightDTO ReturnFlight { get; set; }
      //  public TravelClassDTO TravelClass { get; set; }
       // public ICollection<BookingSegmentDTO> BookingSegments { get; set; }
    }
}
