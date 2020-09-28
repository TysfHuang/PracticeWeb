using System.ComponentModel.DataAnnotations;

namespace Practice.Domain.Entities
{
    public class ShippingDetails
    {
        public int CityID { get; set; }
        public int CountryID { get; set; }

        [Required(ErrorMessage = "請輸入收件人姓名")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "請輸入路口號碼")]
        [Display(Name = "路口號碼")]
        public string Line { get; set; }
    }
}
