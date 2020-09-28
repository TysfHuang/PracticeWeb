using System.Collections.Generic;
using Practice.Domain.Entities;

namespace Practice.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Category> Categories { get; }
        IEnumerable<Brand> Brands { get; }
        IEnumerable<ProductOrder> ProductOrders { get; }
        IEnumerable<City> Cities { get; }
        IEnumerable<Country> Countries { get; }
        IEnumerable<Country> GetCountriesByCityID(int CityID);
        void SaveProduct(Product product);
        bool AddCategory(string name);
        bool AddBrand(string name);
        Product DeleteProduct(int productID);
        void SaveProductOrder(ProductOrder productOrder);
        string GetCityAndCountryName(int CountryID);
    }
}
