using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class User
    {
        [Key]
        [Column("userid")]
        public int UserID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phonenumber")]
        public string PhoneNumber { get; set; }

        [Column("password")]
        public string Password { get; set; }

        // Navigation Property
        public ICollection<Booking> Bookings { get; set; }
    }
}
