using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeWeb.WebUI.Models
{
    public class AdminMenuViewModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string DisplayName { get; set; }
    }

    public class AdminUserInfoViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string AddressLine { get; set; }
        public IEnumerable<UserOrderViewModel> OrderList { get; set; }
    }

    public class AdminCateAndBrandModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }

    public class AdminCateAndBrandViewModel
    {
        public IEnumerable<AdminCateAndBrandModel> CategoryList { get; set; }
        public IEnumerable<AdminCateAndBrandModel> BrandList { get; set; }
    }
}