using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Practice.Domain.Entities
{
    public class City
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}
