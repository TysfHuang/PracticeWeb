using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Practice.Domain.Entities
{
    public class Category
    {
        public int ID { get; set; }

        [Display(Name = "商品類別")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
