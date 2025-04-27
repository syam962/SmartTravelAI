using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class FlightCompany
    {
        [Key]
        [Column("companyid")]
        public int CompanyID { get; set; }

        [Column("companyname")]
        public string CompanyName { get; set; }

        [Column("headquarters")]
        public string Headquarters { get; set; }

        [Column("contactnumber")]
        public string ContactNumber { get; set; }

        [Column("website")]
        public string Website { get; set; }

        // Navigation Property  
        public ICollection<Flight> Flights { get; set; }
    }
}
