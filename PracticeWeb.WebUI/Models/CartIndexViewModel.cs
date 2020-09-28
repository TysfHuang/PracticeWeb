using Practice.Domain.Entities;
using System.Dynamic;

namespace PracticeWeb.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}