using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practice.Domain.Entities;
using Moq;
using Practice.Domain.Abstract;
using PracticeWeb.WebUI.Controllers;
using System.Web.Mvc;
using PracticeWeb.WebUI.Models;

namespace PracticeWeb.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product p1 = new Product { ID = 1, Name = "P1" };
            Product p2 = new Product { ID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ID = 1, Name = "P1" };
            Product p2 = new Product { ID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ID).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ID = 1, Name = "P1" };
            Product p2 = new Product { ID = 2, Name = "P2" };
            Product p3 = new Product { ID = 3, Name = "P3" };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ID = 1, Name = "P1", Price = 100 };
            Product p2 = new Product { ID = 2, Name = "P2", Price = 50 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ID = 1, Name = "P1", Price = 100 };
            Product p2 = new Product { ID = 2, Name = "P2", Price = 50 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            target.Clear();

            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null, null, null, null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1", CategoryID = 1},
            }.AsQueryable());
            Cart cart = new Cart();
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, null);

            target.AddToCart(cart, 1, 1);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ID, 1);
        }

        [TestMethod]
        public void Cannot_Add_Invalid_Product_To_Cart()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1", CategoryID = 1},
            }.AsQueryable());
            Cart cart = new Cart();
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, null);

            target.AddToCart(cart, 2, 1);

            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Then_Not_Redirect()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1", CategoryID = 1},
                new Product {ID = 2, Name = "P2", CategoryID = 1}
            }.AsQueryable());
            Cart cart = new Cart();
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, null);

            RedirectToRouteResult result = target.AddToCart(cart, 2, 1);

            Assert.AreEqual(result.RouteValues["action"], "ProductDetail");
            Assert.AreEqual(result.RouteValues["productID"], 2);
        }

        [TestMethod]
        public void Can_Remove_From_Cart()
        {
            var myMock = new MyMock();
            Product product = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product,
            }.AsQueryable());
            Cart cart = new Cart();
            cart.AddItem(product, 2);
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, null);

            target.RemoveFromCart(cart, product.ID, "ThisIsReturnUrl");

            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Cannot_Remove_Product_That_Not_In_Cart()
        {
            var myMock = new MyMock();
            Product product1 = new Product { ID = 1, Name = "P1" };
            Product product2 = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product1, product2
            }.AsQueryable());
            Cart cart = new Cart();
            cart.AddItem(product1, 2);
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, null);

            target.RemoveFromCart(cart, 2, "ThisIsReturnUrl");

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.First().Product.ID, 1);
            Assert.AreEqual(cart.Lines.First().Quantity, 2);
        }

        [TestMethod]
        public void Remove_Product_From_Cart_Goes_ReturnUrl()
        {
            var myMock = new MyMock();
            Product product = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product,
            }.AsQueryable());
            Cart cart = new Cart();
            cart.AddItem(product, 2);
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, null);

            RedirectToRouteResult result = target.RemoveFromCart(cart, product.ID, "ThisIsReturnUrl");

            Assert.AreEqual(result.RouteValues["returnUrl"], "ThisIsReturnUrl");
        }

        [TestMethod]
        public void Can_Get_CountrySelectListItem_Json()
        {
            var myMock = new MyMock();
            HomeController controller = new HomeController(myMock.ProductRepository.Object, null);

            dynamic data = controller.GetCountrySelectListItemJson("2").Data;

            Assert.AreEqual("Country2", (data[0]).Text);
            Assert.AreEqual("2", (data[0]).Value);
            Assert.AreEqual("Country3", (data[1]).Text);
            Assert.AreEqual("3", (data[1]).Value);
        }

        [TestMethod]
        public void Cannot_Get_Nonexistent_CountrySelectListItem_Json()
        {
            var myMock = new MyMock();
            HomeController controller = new HomeController(myMock.ProductRepository.Object, null);

            dynamic data = controller.GetCountrySelectListItemJson("4").Data;

            Assert.AreEqual(0, data.Length);
        }

        [TestMethod]
        public void Can_Get_Initial_Data_In_Checkout()
        {
            var myMock = new MyMock();
            Product product1 = new Product { ID = 1, Name = "P1" };
            Product product2 = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product1, product2
            }.AsQueryable());
            Cart cart = new Cart();
            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, myMock.UserManager.Object);

            var result = target.Checkout(cart) as ViewResult;
            ShippingDetails data = result.Model as ShippingDetails;

            Assert.AreEqual(myMock.AppUser.CityID, data.CityID);
            Assert.AreEqual(myMock.AppUser.CountryID, data.CountryID);
            Assert.AreEqual(myMock.AppUser.ShippingAddress, data.Line);
            Assert.AreEqual("", data.ReceiverName);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            var myMock = new MyMock();
            Product product1 = new Product { ID = 1, Name = "P1" };
            Product product2 = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product1, product2
            }.AsQueryable());
            Cart cart = new Cart();
            CartController target = new CartController(myMock.ProductRepository.Object, null, null, myMock.UserManager.Object);

            var result = target.Checkout(cart) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart_With_Post()
        {
            var myMock = new MyMock();
            Product product1 = new Product { ID = 1, Name = "P1" };
            Product product2 = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product1, product2
            }.AsQueryable());
            Cart cart = new Cart();
            ShippingDetails sd = new ShippingDetails { CityID = 1, CountryID = 1 };
            CartController target = new CartController(myMock.ProductRepository.Object, myMock.OrderProcessor.Object, null, null);

            ViewResult result = target.Checkout(cart, sd) as ViewResult;

            myMock.OrderProcessor.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            Assert.AreEqual("", result.ViewName);   // 因為是回傳一物件，沒有設定ViewName所以為空字串
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            var myMock = new MyMock();
            Product product1 = new Product { ID = 1, Name = "P1" };
            Product product2 = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product1, product2
            }.AsQueryable());
            Cart cart = new Cart();
            ShippingDetails sd = new ShippingDetails { CityID = 1, CountryID = 1 };
            CartController target = new CartController(myMock.ProductRepository.Object, myMock.OrderProcessor.Object, null, null);
            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(cart, sd) as ViewResult;

            myMock.OrderProcessor.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            var myMock = new MyMock();
            Product product1 = new Product { ID = 1, Name = "P1" };
            Product product2 = new Product { ID = 1, Name = "P1" };
            myMock.ProductRepository.Setup(m => m.GetCityAndCountryName(It.IsAny<int>())).Returns("城市鄉鎮");
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                product1, product2
            }.AsQueryable());
            Cart cart = new Cart();
            cart.AddItem(product1, 1);
            ShippingDetails sd = new ShippingDetails { CityID = 1, CountryID = 1, ReceiverName = "receiverName" };
            CartController target = new CartController(
                myMock.ProductRepository.Object, myMock.OrderProcessor.Object, 
                myMock.OrderLogger.Object, myMock.UserManager.Object);

            ViewResult result = target.Checkout(cart, sd) as ViewResult;

            myMock.OrderProcessor.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            myMock.OrderLogger.Verify(m => m.RecordOrder(It.IsAny<Cart>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
