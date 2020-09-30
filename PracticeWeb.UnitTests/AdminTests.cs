using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;
using PracticeWeb.WebUI.Controllers;
using PracticeWeb.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PracticeWeb.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Can_Go_User_Create()
        {
            MyMock myMock = new MyMock();
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            ViewResult result = controller.UserCreate() as ViewResult;

            Assert.AreEqual("~/Views/Home/CreateUser.cshtml", result.ViewName);
        }

        [TestMethod]
        public void Can_Create_User()
        {
            var myMock = new MyMock();
            //不確認CreateAsync功能
            myMock.UserManager.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            CreateAccountModel userInfo = new CreateAccountModel
            {
                Name = "UserName",
                Email = "user@example.com",
                Password = "12sd45er78",
                PasswordConfirm = "12sd45er78",
                Phone = "0123456789",
                CityID = 2,
                CountryID = 3,
                ShippingAddress = "999"
            };
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.UserCreate(userInfo) as Task<ActionResult>;
            var viewresult = result.Result;

            Assert.AreEqual("UserIndex", (viewresult as RedirectToRouteResult).RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_Create_User_When_ModelState_Invalid()
        {
            var myMock = new MyMock();
            CreateAccountModel userInfo = new CreateAccountModel
            {
                Name = "UserName",
                Email = "user@example.com",
                Password = "12sd45er78",
                PasswordConfirm = "12sd45er78",
                Phone = "0123456789",
                CityID = 2,
                CountryID = 3,
                ShippingAddress = "999"
            };
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.ViewData.ModelState.AddModelError("error", "error");

            var result = controller.UserCreate(userInfo) as Task<ActionResult>;
            var viewresult = result.Result as ViewResult;

            Assert.AreEqual("~/Views/Home/CreateUser.cshtml", viewresult.ViewName);
        }

        [TestMethod]
        public void Can_View_Order_Detail()
        {
            var myMock = new MyMock();
            DateTime time = DateTime.Now;
            myMock.ProductRepository.Setup(u => u.ProductOrders).Returns(new ProductOrder[]
            {
                new ProductOrder{
                    ID = 1,
                    Date = time, 
                    ReceiverName = "Rec1", 
                    ShippingAddress = "address1", 
                    ProductList = "1:name1,2,1999"
                },
                new ProductOrder{
                    ID = 2,
                    Date = time,
                    ReceiverName = "Rec2",
                    ShippingAddress = "address2",
                    ProductList = "2:name2,1,3999&3:name3,1,9999"
                }
            });
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            UserOrderViewModel result = (controller.OrderDetail(2) as ViewResult).Model as UserOrderViewModel;

            Assert.AreEqual(time, result.Date);
            Assert.AreEqual("Rec2", result.ReceiverName);
            Assert.AreEqual("address2", result.ShippingAddress);
            Assert.AreEqual(2, result.ProductList.Count);
            Assert.AreEqual("2", result.ProductList[0][0]);
            Assert.AreEqual("name2", result.ProductList[0][1]);
            Assert.AreEqual("1", result.ProductList[0][2]);
            Assert.AreEqual("3999", result.ProductList[0][3]);
            Assert.AreEqual("3", result.ProductList[1][0]);
        }

        [TestMethod]
        public void Can_Add_Product_Category()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(p => p.AddCategory(It.IsAny<string>())).Returns(true);
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.ProductAddCategory("CategoryName");

            myMock.ProductRepository.Verify(p => p.AddCategory("CategoryName"), Times.Once());
        }

        [TestMethod]
        public void Cannot_Add_Empty_String_To_Product_Category()
        {
            var myMock = new MyMock();
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.ProductAddCategory("");

            myMock.ProductRepository.Verify(p => p.AddCategory(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void Cannot_Add_To_Product_Category_When_ModelState_Invalid()
        {
            var myMock = new MyMock();
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.ViewData.ModelState.AddModelError("error", "error");

            var result = controller.ProductAddCategory("");

            myMock.ProductRepository.Verify(p => p.AddCategory(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void Can_Add_Product_Brand()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(p => p.AddBrand(It.IsAny<string>())).Returns(true);
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.ProductAddBrand("BrandName");

            myMock.ProductRepository.Verify(p => p.AddBrand("BrandName"), Times.Once());
        }

        [TestMethod]
        public void Cannot_Add_Empty_String_To_Product_Brand()
        {
            var myMock = new MyMock();
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.ProductAddBrand("");

            myMock.ProductRepository.Verify(p => p.AddBrand(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void Cannot_Add_To_Product_Brand_When_ModelState_Invalid()
        {
            var myMock = new MyMock();
            var controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.ViewData.ModelState.AddModelError("error", "error");

            var result = controller.ProductAddBrand("");

            myMock.ProductRepository.Verify(p => p.AddBrand(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public void Index_Contains_All_Products()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1"},
                new Product {ID = 2, Name = "P2"},
                new Product {ID = 3, Name = "P3"},
            });
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            Product[] result = ((IEnumerable<Product>)controller.ProductIndex().
                ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Get_Initial_Data_in_ProductEdit()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1"},
                new Product {ID = 2, Name = "P2"}
            });
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            Product result = (controller.ProductEdit(1) as ViewResult).Model as Product;

            Assert.AreEqual(1, result.ID);
            Assert.AreEqual("P1", result.Name);
        }

        [TestMethod]
        public void Cannot_Get_Initial_Data_in_ProductEdit_When_Invalid_Id()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1"},
                new Product {ID = 2, Name = "P2"}
            });
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            ActionResult result = controller.ProductEdit(3);

            Assert.AreEqual(400, ((HttpStatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            var myMock = new MyMock();
            Product p = new Product { ID = 1, Name = "NewName" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product { ID = 1, Name = "P1" },
                new Product { ID = 2, Name = "P2" }
            });
            myMock.ProductRepository.Setup(m => m.SaveProduct(It.IsAny<Product>()));
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.ProductEdit(p, null) as Task<ActionResult>;
            RedirectToRouteResult viewresult = result.Result as RedirectToRouteResult;

            myMock.ProductRepository.Verify(m => m.SaveProduct(p), Times.Once());
            Assert.AreEqual("ProductIndex", viewresult.RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            var myMock = new MyMock();
            Product p = new Product { ID = 3, Name = "P3" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1"},
                new Product {ID = 2, Name = "P2"}
            });
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.ProductEdit(p, null) as Task<ActionResult>;
            ActionResult viewresult = result.Result as ActionResult;

            Assert.AreEqual(((HttpStatusCodeResult)viewresult).StatusCode, 400);
        }

        [TestMethod]
        public void Cannot_Edit_Product_When_ModelState_Invalid()
        {
            var myMock = new MyMock();
            Product p = new Product { ID = 1, Name = "NewName" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product { ID = 1, Name = "P1" },
                new Product { ID = 2, Name = "P2" }
            });
            myMock.ProductRepository.Setup(m => m.SaveProduct(It.IsAny<Product>()));
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.ViewData.ModelState.AddModelError("error", "error");

            var result = controller.ProductEdit(p, null) as Task<ActionResult>;
            ViewResult viewresult = result.Result as ViewResult;

            myMock.ProductRepository.Verify(m => m.SaveProduct(p), Times.Never());
            Assert.AreEqual("", viewresult.ViewName);
        }

        [TestMethod]
        public void Can_Go_ProductCreate()
        {
            var myMock = new MyMock();
            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            ViewResult result = controller.ProductCreate() as ViewResult;

            Assert.AreEqual("ProductEdit", result.ViewName);
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            var myMock = new MyMock();
            Product prod = new Product { ID = 2, Name = "Test" };
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ID = 1, Name = "P1"},
                prod,
                new Product {ID = 3, Name = "P3"},
            });

            AdminController controller = new AdminController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            controller.ProductDelete(prod.ID);

            myMock.ProductRepository.Verify(m => m.DeleteProduct(prod.ID), Times.Once());
        }
    }
}