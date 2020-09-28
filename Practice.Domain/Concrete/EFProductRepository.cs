using System;
using System.Collections.Generic;
using System.Linq;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;

namespace Practice.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private AppIdentityDbContext context = new AppIdentityDbContext();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public IEnumerable<Category> Categories
        {
            get { return context.Categories; }
        }

        public IEnumerable<Brand> Brands
        {
            get { return context.Brands; }
        }

        public IEnumerable<ProductOrder> ProductOrders
        {
            get { return context.ProductOrders; }
        }

        public IEnumerable<City> Cities
        {
            get { return context.Cities; }
        }

        public IEnumerable<Country> Countries
        {
            get { return context.Countries; }
        }

        public void SaveProduct(Product product)
        {
            if(product.ID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                // 參考書本Page.264
                Product dbEntry = context.Products.Find(product.ID); //取得目標
                if(dbEntry != null)
                {
                    dbEntry.Name = product.Name;    //對目標更新
                    dbEntry.Summary = product.Summary;
                    dbEntry.CoverImagePath = product.CoverImagePath;
                    dbEntry.Description = product.Description;
                    dbEntry.BrandID = product.BrandID;
                    dbEntry.Price = product.Price;
                    dbEntry.CategoryID = product.CategoryID;
                }
            }
            context.SaveChanges();  //然後寫入資料庫
        }

        public Product DeleteProduct(int productID)
        {
            Product dbEntry = context.Products.Find(productID);
            if(dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public bool AddCategory(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if(context.Categories.Select(c => c.Name.ToLower()).Contains(name.ToLower()))
            {
                return false;
            }
            context.Categories.Add(new Category { Name = name });
            context.SaveChanges();
            return true;
        }

        public bool AddBrand(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if (context.Brands.Select(c => c.Name.ToLower()).Contains(name.ToLower()))
            {
                return false;
            }
            context.Brands.Add(new Brand { Name = name });
            context.SaveChanges();
            return true;
        }

        public void SaveProductOrder(ProductOrder productOrder)
        {
            if (productOrder == null || productOrder.ID != 0)
                return;
            context.ProductOrders.Add(productOrder);
            context.SaveChanges();
        }

        public IEnumerable<Country> GetCountriesByCityID(int CityID)
        {
            return context.Countries.Where(c => c.CityID == CityID);
        }

        public string GetCityAndCountryName(int CountryID)
        {
            Country country = context.Countries.Where(c => c.ID == CountryID).FirstOrDefault();
            return country == null ? "參數錯誤" : country.GetNameOfCityAndCountry();
        }
    }
}
