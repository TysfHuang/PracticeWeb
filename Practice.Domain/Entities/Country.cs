using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Practice.Domain.Entities
{
    public class Country
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int CityID { get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        public string GetNameOfCityAndCountry()
        {
            return City.Name + Name;
        }
    }
}
