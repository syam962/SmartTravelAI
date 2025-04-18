using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class Passenger
    {
        [Key]
        [Column("passengerid")]
        public int PassengerID { get; set; }

        [Column("bookingid")]
        public int BookingID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("seatnumber")]
        public string SeatNumber { get; set; }

        // Navigation Property  
        public Booking Booking { get; set; }
    }
}
