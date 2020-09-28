using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Practice.Domain.Entities;
using PracticeWeb.WebUI.Controllers;
using PracticeWeb.WebUI.Models;

namespace PracticeWeb.UnitTests
{
    [TestClass]
    public class WebApiTest
    {
        private Product[] GetProductsData()
        {
            return new Product[] {
                new Product {ID = 1, Name = "smartphone1", CategoryID = 1}, 
                new Product {ID = 2, Name = "smartphone2", CategoryID = 1},
                new Product {ID = 3, Name = "smartphone3", CategoryID = 1}, 
                new Product {ID = 4, Name = "smartphone4", CategoryID = 1},
                new Product {ID = 5, Name = "cpu1", CategoryID = 2}, 
                new Product {ID = 6, Name = "cpu2", CategoryID = 2},
                new Product {ID = 7, Name = "videocard1", CategoryID = 3},
                new Product {ID = 8, Name = "videocard2", CategoryID = 3},
                new Product {ID = 9, Name = "videocard3", CategoryID = 3}, 
                new Product {ID = 10, Name = "videocard4", CategoryID = 3},
                new Product {ID = 11, Name = "smartphone5", CategoryID = 1}, 
                new Product {ID = 12, Name = "smartphone6", CategoryID = 1} };
        }

        [TestMethod]
        public void Can_Get_Data()
        {
            MyMock myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(GetProductsData());
            var controller = new WebApiController(myMock.ProductRepository.Object);

            ProductsListJsonViewModel result = controller.GetSpecifyProductIntro("ALL");

            Assert.AreEqual(5, result.CategoryList.Count());
            Assert.AreEqual("ALL", result.CategoryList.First());
            Assert.AreEqual(10, result.Products.Count());
            Assert.AreEqual(2, result.TotalPage);
        }

        [TestMethod]
        public void Can_Get_Data_With_Category()
        {
            MyMock myMock = new MyMock();
            string category = "Category1";
            myMock.ProductRepository.Setup(m => m.Products).Returns(GetProductsData());
            var controller = new WebApiController(myMock.ProductRepository.Object);

            ProductsListJsonViewModel result = controller.GetSpecifyProductIntro(category);

            Assert.AreEqual(6, result.Products.Count());
            Assert.AreEqual(1, result.TotalPage);
            Assert.AreEqual("smartphone1", result.Products.First().Name);
        }

        [TestMethod]
        public void Can_Get_Data_With_KeyString()
        {
            MyMock myMock = new MyMock();
            string key = "2";
            myMock.ProductRepository.Setup(m => m.Products).Returns(GetProductsData());
            var controller = new WebApiController(myMock.ProductRepository.Object);

            ProductsListJsonViewModel result = controller.GetSpecifyProductIntro("ALL", key);

            Assert.AreEqual(3, result.Products.Count());
            Assert.AreEqual(1, result.TotalPage);
            Assert.AreEqual("smartphone2", result.Products.First().Name);
        }

        [TestMethod]
        public void Can_Get_Data_With_Category_And_KeyString()
        {
            MyMock myMock = new MyMock();
            string category = "Category3";
            string key = "2";
            myMock.ProductRepository.Setup(m => m.Products).Returns(GetProductsData());
            var controller = new WebApiController(myMock.ProductRepository.Object);

            ProductsListJsonViewModel result = controller.GetSpecifyProductIntro(category, key);

            Assert.AreEqual(1, result.Products.Count());
            Assert.AreEqual(1, result.TotalPage);
            Assert.AreEqual("videocard2", result.Products.First().Name);
        }

        [TestMethod]
        public void Can_Get_All_Data_When_Category_Invalid()
        {
            MyMock myMock = new MyMock();
            string category = "Category999";
            myMock.ProductRepository.Setup(m => m.Products).Returns(GetProductsData());
            var controller = new WebApiController(myMock.ProductRepository.Object);

            ProductsListJsonViewModel result = controller.GetSpecifyProductIntro(category);

            Assert.AreEqual(5, result.CategoryList.Count());
            Assert.AreEqual("ALL", result.CategoryList.First());
            Assert.AreEqual(10, result.Products.Count());
            Assert.AreEqual(2, result.TotalPage);
        }

        [TestMethod]
        public void Cannot_Get_Data_When_Not_Contain_KeyString()
        {
            MyMock myMock = new MyMock();
            string key = "GPU";
            myMock.ProductRepository.Setup(m => m.Products).Returns(GetProductsData());
            var controller = new WebApiController(myMock.ProductRepository.Object);

            ProductsListJsonViewModel result = controller.GetSpecifyProductIntro("ALL", key);

            Assert.AreEqual(0, result.Products.Count());
        }
    }
}
