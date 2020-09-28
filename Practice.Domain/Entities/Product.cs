using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Practice.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "請輸入商品名稱")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請上傳商品圖檔")]
        public string CoverImagePath { get; set; }

        [Required(ErrorMessage = "請輸入商品簡介")]
        [Display(Name = "簡介")]
        public string Summary { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "請輸入商品介紹")]
        [Display(Name = "商品介紹")]
        public string Description { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "請輸入商品金額（1～100000）")]
        [Display(Name = "價格")]
        public int Price { get; set; }

        [Required(ErrorMessage = "請選擇商品所屬廠商")]
        public int BrandID { get; set; }

        [Required(ErrorMessage = "請輸入商品類別")]
        public int CategoryID { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
    }
}
