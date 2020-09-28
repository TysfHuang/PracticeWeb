using System.Collections.Generic;
using Practice.Domain.Entities;

namespace PracticeWeb.WebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCatrgory { get; set; }
    }

    public class ProductIntroViewModel
    {
        public string ProductUrl { get; set; }
        public string Name { get; set; }
        public string CoverImagePath { get; set; }
        public string Price { get; set; }
    }

    public class ProductsListJsonViewModel
    {
        public IEnumerable<ProductIntroViewModel> Products { get; set; }
        public IEnumerable<string> CategoryList { get; set; }
        public int TotalPage { get; set; }
    }
}