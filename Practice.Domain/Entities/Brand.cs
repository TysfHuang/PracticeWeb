using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Practice.Domain.Entities
{
    public class Brand
    {
        public int ID { get; set; }

        [Display(Name = "商品廠商")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
