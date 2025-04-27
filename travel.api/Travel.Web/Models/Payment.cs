using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class Payment
    {
        [Key]
        [Column("paymentid")]
        public int PaymentID { get; set; }

        [Column("bookingid")]
        public int BookingID { get; set; }

        [Column("paymentamount")]
        public decimal PaymentAmount { get; set; }

        [Column("paymentdate")]
        public DateTime PaymentDate { get; set; }

        [Column("paymentstatus")]
        public string PaymentStatus { get; set; }

        // Navigation Property  
        public Booking Booking { get; set; }
    }
}
