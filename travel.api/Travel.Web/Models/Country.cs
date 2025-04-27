using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Web.Models
{
    public class Country
    {
        [Key]
        [Column("countryid")]
        public int CountryID { get; set; }

        [Column("countryname")]
        public string CountryName { get; set; }

        [Column("countrycode")]
        public string CountryCode { get; set; }

        [Column("timezone")]
        public string TimeZone { get; set; }

        // Navigation Property
        public ICollection<State> States { get; set; }
    }

    public class State
    {
        [Key]
        [Column("stateid")]
        public int StateID { get; set; }

        [Column("statename")]
        public string StateName { get; set; }

        [Column("countryid")]
        public int CountryID { get; set; }

        [Column("timezone")]
        public string TimeZone { get; set; }

        // Navigation Properties
        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
    }

    public class City
    {
        [Key]
        [Column("cityid")]
        public int CityID { get; set; }

        [Column("cityname")]
        public string CityName { get; set; }

        [Column("stateid")]
        public int StateID { get; set; }

        [Column("timezone")]
        public string TimeZone { get; set; }

        // Navigation Property
        public State State { get; set; }
    }
}
