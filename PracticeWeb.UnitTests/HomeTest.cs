using System;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeWeb.WebUI.Controllers;
using PracticeWeb.WebUI.Models;
using Moq;
using System.Web.Mvc;
using System.Collections.Generic;
using Practice.Domain.Concrete;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace PracticeWeb.UnitTests
{
    [TestClass]
    public class HomeTest
    {
        [TestMethod]
        public void Can_View_Product_Detail()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ID = 1, Name = "P1"}
            });
            HomeController target = new HomeController(myMock.ProductRepository.Object, null);

            Product p1 = (target.ProductDetail(1) as ViewResult).Model as Product;

            Assert.AreEqual(1, p1.ID);
            Assert.AreEqual("P1", p1.Name);
        }

        [TestMethod]
        public void Cannot_View_Nonexistent_Product_Detail()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ID = 1, Name = "P1"}
            });
            HomeController controller = new HomeController(myMock.ProductRepository.Object, null);

            var result = controller.ProductDetail(2);

            Assert.AreEqual(400, ((HttpStatusCodeResult)result).StatusCode);
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
        public void Can_View_Order_Index()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.ProductOrders).Returns(new ProductOrder[]
            {
                new ProductOrder{
                    ID = 1,
                    AppUserID = "1",
                    Date = new DateTime(2020, 12, 12, 12, 12, 12),
                    ProductList = "1:name,2,199",
                    ReceiverName = "rec",
                    ShippingAddress = "131號"},
                new ProductOrder{
                    ID = 2,
                    AppUserID = "2",
                    Date = new DateTime(2020, 12, 12, 12, 12, 12),
                    ProductList = "10:Product,3,1199",
                    ReceiverName = "another",
                    ShippingAddress = "1號"}
            });
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            List<UserOrderViewModel> data = (controller.ViewOrder(1) as ViewResult).Model as List<UserOrderViewModel>;

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(new DateTime(2020, 12, 12, 12, 12, 12), data[0].Date);
            Assert.AreEqual("rec", data[0].ReceiverName);
            Assert.AreEqual("131號", data[0].ShippingAddress);
            Assert.AreEqual("1", data[0].ProductList[0][0]);
            Assert.AreEqual("name", data[0].ProductList[0][1]);
            Assert.AreEqual("2", data[0].ProductList[0][2]);
            Assert.AreEqual("199", data[0].ProductList[0][3]);
        }

        [TestMethod]
        public void Cannot_View_Nonexistent_Order_Index()
        {
            var myMock = new MyMock();
            myMock.ProductRepository.Setup(m => m.ProductOrders).Returns(new ProductOrder[]
            {
                new ProductOrder{
                    ID = 1,
                    AppUserID = "11",
                    Date = new DateTime(2020, 12, 12, 12, 12, 12),
                    ProductList = "1:name,2,199",
                    ReceiverName = "rec",
                    ShippingAddress = "131號"},
                new ProductOrder{
                    ID = 2,
                    AppUserID = "22",
                    Date = new DateTime(2020, 12, 12, 12, 12, 12),
                    ProductList = "10:Product,3,1199",
                    ReceiverName = "another",
                    ShippingAddress = "1號"}
            });
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            List<UserOrderViewModel> data = (controller.ViewOrder(1) as ViewResult).Model as List<UserOrderViewModel>;

            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void Can_Get_Initial_UserData_in_Edit_User()
        {
            var myMock = new MyMock();
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            UserInfoEditModel result = (controller.EditUser() as ViewResult).Model as UserInfoEditModel;

            Assert.AreEqual("testuser", result.Name);
            Assert.AreEqual("test@example.com", result.Email);
            Assert.AreEqual("0987654321", result.Phone);
            Assert.AreEqual(1, result.CityID);
            Assert.AreEqual(1, result.CountryID);
            Assert.AreEqual("111號", result.ShippingAddress);
        }

        [TestMethod]
        public void Can_Get_Initial_UserData_in_Create_User()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(false);
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = (controller.CreateUser() as ViewResult).Model;

            Assert.IsInstanceOfType(result, typeof(CreateAccountModel));
        }

        [TestMethod]
        public void Can_Edit_User()
        {
            var myMock = new MyMock();
            //不確認UserManager的功能是否正常
            myMock.UserManager.Setup(u => u.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            myMock.UserManager.Setup(u => u.UserValidator_ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            myMock.UserManager.Setup(u => u.PasswordValidator_ValidateAsync(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            myMock.UserManager.Setup(u => u.PasswordHasher_HashPassword(It.IsAny<string>())).Returns("password");
            UserInfoEditModel userInfo = new UserInfoEditModel
            {
                Name = myMock.AppUser.UserName,
                Email = myMock.AppUser.Email,
                Password = "666aa666",
                PasswordConfirm = "666aa666",
                Phone = "0123456789",
                CityID = 2,
                CountryID = 3,
                ShippingAddress = "999"
            };
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.EditUser(userInfo) as Task<ActionResult>;
            var viewresult = result.Result;

            myMock.UserManager.Verify(u => u.UserValidator_ValidateAsync(It.IsAny<AppUser>()), Times.Once());
            myMock.UserManager.Verify(u => u.PasswordValidator_ValidateAsync(It.IsAny<string>()), Times.Once());
            myMock.UserManager.Verify(u => u.PasswordHasher_HashPassword(It.IsAny<string>()), Times.Once());
            Assert.AreEqual("EditUser", (viewresult as RedirectToRouteResult).RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_Edit_User_With_Invalid_Value()
        {
            var myMock = new MyMock();
            //不確認UserManager的功能是否正常
            //密碼要求至少六位數
            myMock.UserManager.Setup(u => u.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            myMock.UserManager.Setup(u => u.UserValidator_ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(new IdentityResult("error"));
            myMock.UserManager.Setup(u => u.PasswordValidator_ValidateAsync(It.IsAny<string>())).ReturnsAsync(new IdentityResult("error"));
            myMock.UserManager.Setup(u => u.PasswordHasher_HashPassword(It.IsAny<string>())).Returns("password");
            UserInfoEditModel userInfo = new UserInfoEditModel
            {
                Name = myMock.AppUser.UserName,
                Email = myMock.AppUser.Email,
                Password = "123",
                PasswordConfirm = "123",
                Phone = "0123456789",
                CityID = 2,
                CountryID = 3,
                ShippingAddress = "999"
            };
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);

            var result = controller.EditUser(userInfo) as Task<ActionResult>;
            var viewresult = result.Result as ViewResult;

            Assert.AreEqual(false, viewresult.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Edit_User_When_ModelState_Invalid()
        {
            var myMock = new MyMock();
            //不確認UpdateAsync功能
            myMock.UserManager.Setup(u => u.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            UserInfoEditModel userInfo = new UserInfoEditModel
            {
                Name = myMock.AppUser.UserName,
                Email = myMock.AppUser.Email,
                Password = "123",
                PasswordConfirm = "123",
                Phone = "0123456789",
                CityID = 2,
                CountryID = 3,
                ShippingAddress = "999"
            };
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.ModelState.AddModelError("error", "error");

            var result = controller.EditUser(userInfo) as Task<ActionResult>;
            var viewresult = result.Result as ViewResult;

            Assert.AreEqual(userInfo, viewresult.Model);
        }

        private UrlHelper GetUrlHelper()
        {
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request.ApplicationPath).Returns("/tmp/testpath");
            context.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns("/mynewVirtualPath/");
            RequestContext requestContext = new RequestContext(context.Object, new RouteData());
            return new UrlHelper(requestContext);
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
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.Url = GetUrlHelper();

            var result = controller.CreateUser(userInfo) as Task<ActionResult>;
            var viewresult = result.Result;

            Assert.AreEqual("Account", (viewresult as RedirectToRouteResult).RouteValues["controller"]);
            Assert.AreEqual("Login", (viewresult as RedirectToRouteResult).RouteValues["action"]);
            //還搞不定Url.Action的問題
            //Assert.AreEqual("Login", (viewresult as RedirectToRouteResult).RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Cannot_Create_User_When_ModelState_Invalid()
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
            var controller = new HomeController(myMock.ProductRepository.Object, myMock.UserManager.Object);
            controller.ModelState.AddModelError("error", "error");
            controller.Url = GetUrlHelper();

            var result = controller.CreateUser(userInfo) as Task<ActionResult>;
            var viewresult = result.Result;

            Assert.AreEqual(userInfo, (viewresult as ViewResult).Model);
        }
    }
}
