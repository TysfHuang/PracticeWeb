using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Practice.Domain.Entities;
using PracticeWeb.WebUI.Controllers;
using PracticeWeb.WebUI.Models;

namespace PracticeWeb.UnitTests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void Check_BtnOnNavBar_With_User_Not_Login()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(false);
            var controller = new AccountController(myMock.UserManager.Object);

            Dictionary<string, object> result = controller.BtnOnNavBar().Model as Dictionary<string, object>;

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(-1, result["AccountLevel"]);
        }

        [TestMethod]
        public void Check_BtnOnNavBar_With_User_Logined()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(true);
            myMock.UserManager.Setup(u => u.IsInRole(It.IsAny<string>(), "Administrators")).Returns(false);
            var controller = new AccountController(myMock.UserManager.Object);

            Dictionary<string, object> result = controller.BtnOnNavBar().Model as Dictionary<string, object>;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result["AccountLevel"]);
            Assert.AreEqual(myMock.AppUser.UserName, result["UserName"]);
        }

        [TestMethod]
        public void Check_BtnOnNavBar_With_Administrators_Logined()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(true);
            myMock.UserManager.Setup(u => u.IsInRole(It.IsAny<string>(), "Administrators")).Returns(true);
            var controller = new AccountController(myMock.UserManager.Object);

            Dictionary<string, object> result = controller.BtnOnNavBar().Model as Dictionary<string, object>;

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(0, result["AccountLevel"]);
            Assert.AreEqual(myMock.AppUser.UserName, result["UserName"]);
        }

        [TestMethod]
        public void Can_Notify_User_The_Page_Unauthorized()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(true);
            var controller = new AccountController(myMock.UserManager.Object);

            var result = controller.Login("returnUrl") as ViewResult;

            Assert.AreEqual("Error", result.ViewName);
            Assert.AreEqual("Access Denied", (result.Model as string[])[0]);
        }

        [TestMethod]
        public void Can_Go_To_Login_Page()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(false);
            var controller = new AccountController(myMock.UserManager.Object);

            var result = controller.Login("returnUrl") as ViewResult;

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual("returnUrl", result.ViewBag.returnUrl);
        }

        [TestMethod]
        public void Can_Login()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(false);
            myMock.UserManager.Setup(u => u.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(myMock.AppUser);
            myMock.UserManager.Setup(u => u.CreateIdentityAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(new ClaimsIdentity());
            myMock.UserManager.Setup(u => u.AuthManager.SignOut());
            LoginModel details = new LoginModel() { Name = myMock.AppUser.UserName, Password = myMock.AppUser.PasswordHash };
            var controller = new AccountController(myMock.UserManager.Object);

            var result = controller.Login(details, "returnUrl") as Task<ActionResult>;
            var viewresult = result.Result as RedirectResult;

            Assert.AreEqual("returnUrl", viewresult.Url);
        }

        [TestMethod]
        public void Cannot_Login_When_ModelState_Invalid()
        {
            var myMock = new MyMock();
            LoginModel details = new LoginModel() { Name = myMock.AppUser.UserName, Password = myMock.AppUser.PasswordHash };
            var controller = new AccountController(myMock.UserManager.Object);
            controller.ModelState.AddModelError("error", "error");

            var result = controller.Login(details, "returnUrl") as Task<ActionResult>;
            var viewresult = result.Result as ViewResult;

            Assert.AreEqual("", viewresult.ViewName);
            Assert.AreEqual(details, viewresult.Model);
            Assert.AreEqual(false, viewresult.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Login_When_User_Nonexistent()
        {
            var myMock = new MyMock();
            myMock.UserManager.Setup(u => u.IsAuthenticated).Returns(false);
            myMock.UserManager.Setup(u => u.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AppUser());
            LoginModel details = new LoginModel() { Name = myMock.AppUser.UserName, Password = myMock.AppUser.PasswordHash };
            var controller = new AccountController(myMock.UserManager.Object);

            var result = controller.Login(details, "returnUrl") as Task<ActionResult>;
            var viewresult = result.Result as ViewResult;

            Assert.AreEqual("returnUrl", viewresult.ViewBag.returnUrl);
            Assert.AreEqual(false, viewresult.ViewData.ModelState.IsValid);
        }
    }
}
