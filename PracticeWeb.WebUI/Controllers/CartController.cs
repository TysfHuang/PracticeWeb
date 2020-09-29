using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Practice.Domain.Abstract;
using Practice.Domain.Concrete;
using Practice.Domain.Entities;
using PracticeWeb.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using PracticeWeb.WebUI.Infrastructure;
using System.Net;

namespace PracticeWeb.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        private IOrderLogger orderLogger;
        private IUserManager myUserManager;

        public CartController(IProductRepository repo, IOrderProcessor proc, IOrderLogger logger, IUserManager userManager)
        {
            repository = repo;
            orderProcessor = proc;
            orderLogger = logger;
            myUserManager = userManager;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int Id, int quantity)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ID == Id);
            if (product != null)
            {
                cart.AddItem(product, quantity);
            }
            return RedirectToAction("ProductDetail", "Home", new { productID = Id});
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int Id, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ID == Id);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public JsonResult GetCountrySelectListItemJson(string cityID = "1")
        {
            IEnumerable<SelectListItem> ori = CityAndCountryPorvider.GetCountrySelectListItem(
                repository.Countries.Where(c => c.CityID == Convert.ToInt32(cityID)));
            var formattedData = ori.Select(p => new
            {
                Text = p.Text,
                Value = p.Value
            }).ToArray();
            return Json(formattedData, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Checkout(Cart cart)
        {
            if (cart.Lines.Count() == 0)
            {
                TempData["message"] = "抱歉, 您的購物車是空的!";
                return RedirectToAction("Index");
            }
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, CurrentUser.CityID, CurrentUser.CountryID);
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(new ShippingDetails { 
                CityID = CurrentUser.CityID,
                CountryID = CurrentUser.CountryID,
                ReceiverName = "",
                Line = CurrentUser.ShippingAddress });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "抱歉, 您的購物車是空的!");
            }
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, shippingDetails.CityID, shippingDetails.CountryID);
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                //orderProcessor.ProcessOrder(cart, shippingDetails);
                orderLogger.RecordOrder(cart, CurrentUser.Id, shippingDetails.ReceiverName,
                    repository.GetCityAndCountryName(shippingDetails.CountryID) + shippingDetails.Line);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        private AppUser CurrentUser
        {
            get { return myUserManager.CurrentUser; }
        }

        private AppUserManager UserManager
        {
            get { return myUserManager.UserManager; }
        }
    }
}