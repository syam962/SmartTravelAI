using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class TravelClass
    {
        [Key]
        [Column("classid")]
        public int ClassID { get; set; }

        [Column("classname")]
        public string ClassName { get; set; }

        [Column("classdescription")]
        public string ClassDescription { get; set; }

        // Navigation Property
        public ICollection<FlightSegmentClass> SegmentClasses { get; set; }
    }
}
