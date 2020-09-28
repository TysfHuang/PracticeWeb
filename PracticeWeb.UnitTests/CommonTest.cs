using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeWeb.WebUI.Controllers;
using PracticeWeb.WebUI.Infrastructure;
using Moq;
using System.Web.Mvc;

namespace PracticeWeb.UnitTests
{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void Can_Get_CityAndCountryPorvider_CitySelectListItem()
        {
            MyMock mock = new MyMock();

            SelectListItem[] result = CityAndCountryPorvider.GetCitySelectListItem(mock.ProductRepository.Object.Cities, 2);

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("City1", result[0].Text);
            Assert.AreEqual("1", result[0].Value);
            Assert.AreEqual("City3", result[2].Text);
            Assert.AreEqual("3", result[2].Value);
            Assert.IsTrue(result[1].Selected);
        }

        [TestMethod]
        public void Can_Get_CityAndCountryPorvider_CountrySelectListItem()
        {
            MyMock mock = new MyMock();

            SelectListItem[] result = CityAndCountryPorvider.GetCountrySelectListItem(mock.ProductRepository.Object.Countries, 2);

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("Country1", result[0].Text);
            Assert.AreEqual("1", result[0].Value);
            Assert.AreEqual("Country3", result[2].Text);
            Assert.AreEqual("3", result[2].Value);
            Assert.IsTrue(result[1].Selected);
        }

        [TestMethod]
        public void Can_Set_CityAndCountrySelectListItem_To_ViewBag_And_Check()
        {
            MyMock mock = new MyMock();
            var controller = new HomeController(mock.ProductRepository.Object, mock.UserManager.Object);

            CityAndCountryPorvider.SetSelectListToViewBag(controller, mock.ProductRepository.Object, 2);
            SelectListItem[] resultCity = controller.ViewBag.CitySelectList as SelectListItem[];
            SelectListItem[] resultCountry = controller.ViewBag.CountrySelectList as SelectListItem[];

            Assert.AreEqual(4, resultCity.Length);
            Assert.AreEqual(2, resultCountry.Length);
            Assert.IsTrue(CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(controller));
        }

        [TestMethod]
        public void Can_Get_BrandAndCategoryPorvider_CategorySelectListItem()
        {
            MyMock mock = new MyMock();

            SelectListItem[] result = BrandAndCategoryProvider.GetCategoryListItem(mock.ProductRepository.Object.Categories, 2);

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("Category1", result[0].Text);
            Assert.AreEqual("1", result[0].Value);
            Assert.AreEqual("Category3", result[2].Text);
            Assert.AreEqual("3", result[2].Value);
            Assert.IsTrue(result[1].Selected);
        }

        [TestMethod]
        public void Can_Get_BrandAndCategoryPorvider_BrandSelectListItem()
        {
            MyMock mock = new MyMock();

            SelectListItem[] result = BrandAndCategoryProvider.GetBrandListItem(mock.ProductRepository.Object.Brands, 2);

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("Brand1", result[0].Text);
            Assert.AreEqual("1", result[0].Value);
            Assert.AreEqual("Brand3", result[2].Text);
            Assert.AreEqual("3", result[2].Value);
            Assert.IsTrue(result[1].Selected);
        }

        [TestMethod]
        public void Can_Set_BrandAndCategorySelectListItem_To_ViewBag_And_Check()
        {
            MyMock mock = new MyMock();
            var controller = new HomeController(mock.ProductRepository.Object, mock.UserManager.Object);

            BrandAndCategoryProvider.SetSelectListToViewBag(controller, mock.ProductRepository.Object);
            SelectListItem[] resultCategory = controller.ViewBag.CategoryList as SelectListItem[];
            SelectListItem[] resultBrand = controller.ViewBag.BrandList as SelectListItem[];

            Assert.AreEqual(4, resultCategory.Length);
            Assert.AreEqual(4, resultBrand.Length);
            Assert.IsTrue(BrandAndCategoryProvider.CheckIfSelectListOfViewBagCorrect(controller));
        }
    }
}
