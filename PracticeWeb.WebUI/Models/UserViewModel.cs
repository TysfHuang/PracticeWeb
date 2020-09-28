using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Practice.Domain.Entities;
using System.Web.Mvc;
using System;

namespace PracticeWeb.WebUI.Models
{
    public class CreateAccountModel
    {
        [Required(ErrorMessage = "請輸入帳戶名稱")]
        [StringLength(50, ErrorMessage = "帳號長度至少為{2}！", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(50, ErrorMessage = "密碼長度至少為{2}！", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "與密碼不相符！")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "請輸入手機號碼")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請選擇縣市")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "請選擇鄉鎮市區")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "請輸入地址")]
        public string ShippingAddress { get; set; }
    }

    // 通常是在用戶端傳送給Server前檢查有無帳密資料
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RoleEditModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }

    public class UserInfoEditModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "密碼長度至少為{2}！", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "與密碼不相符！")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "請輸入手機號碼")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請選擇縣市")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "請選擇鄉鎮市區")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "請輸入地址")]
        public string ShippingAddress { get; set; }
    }

    public class UserOrderViewModel
    {
        public DateTime Date { get; set; }
        public string ReceiverName { get; set; }
        public string ShippingAddress { get; set; }
        public List<List<string>> ProductList { get; set; } //[0]productID [1]productName [2]productQuantity [3]productPrice 
        public int ComputeTotalPrice()
        {
            int total = 0;
            foreach(List<string> product in ProductList)
            {
                total += Convert.ToInt32(product[2]) * Convert.ToInt32(product[3]);
            }
            return total;
        }
    }
}