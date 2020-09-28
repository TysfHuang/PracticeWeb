using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Practice.Domain.Abstract;
using PracticeWeb.WebUI.Models;
using Practice.Domain.Entities;
using Practice.Domain.Concrete;

namespace PracticeWeb.WebUI.Controllers
{
    public class WebApiController : ApiController
    {
        private IProductRepository repository;
        private const int PageSize = 10;

        public WebApiController()
        {
            repository = new EFProductRepository();
        }

        public WebApiController(IProductRepository repository)
        {
            this.repository = repository;
        }

        private void ProcessCategoryString(ref string category)
        {
            if(category == null || (category != "ALL" && !repository.Categories.Select(c => c.Name).Contains(category)))
                category = "ALL";
        }

        private int GetIdOfCategory(string category)
        {
            // 前面有經過Category的名稱判斷，這邊不判斷是否為null
            return repository.Categories.Where(c => c.Name == category).FirstOrDefault().ID;
        }

        public ProductsListJsonViewModel GetSpecifyProductIntro(string category, string searchString = "", int page = 1)
        {
            IEnumerable<Product> products = repository.Products;
            ProcessCategoryString(ref category);
            //if (category != "ALL")    //為了單元測試，使用CategoryID來判斷
            //    products = products.Where(p => p.Category.Name == category);
            if (category != "ALL")
                products = products.Where(p => p.CategoryID == GetIdOfCategory(category));
            if (searchString != null && searchString != "")
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
            int totalPage = (int)Math.Ceiling((decimal)products.Count() / PageSize);
            products = products.Skip((page - 1) * PageSize).Take(PageSize);

            List<string> cateList = new List<string>() { "ALL" };
            cateList.AddRange(repository.Categories.Select(c => c.Name));

            ProductsListJsonViewModel data = new ProductsListJsonViewModel {
                Products = products.Select(p => new ProductIntroViewModel {
                    ProductUrl = "Home/ProductDetail/productID=" + p.ID.ToString(),
                    Name = p.Name,
                    Price = p.Price.ToString(),
                    CoverImagePath = p.CoverImagePath,
                }).ToList(),
                CategoryList = cateList,
                TotalPage = totalPage
            };
            return data;
        }
    }
}
