using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Domain.Abstract;
using Practice.Domain.Concrete;
using Practice.Domain.Entities;
using PracticeWeb.WebUI.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using PracticeWeb.WebUI.Infrastructure;
using System.Security.Claims;
using System.Net;

namespace PracticeWeb.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository repository;
        private IUserManager myUserManager;

        public HomeController(IProductRepository productRepository, IUserManager userManager)
        {
            this.repository = productRepository;
            this.myUserManager = userManager;
        }

        public ViewResult Index(string searchString, string category, int page = 1)
        {
            return View();
        }

        public ActionResult ProductDetail(int productID)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ID == productID);
            if (product == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(product);
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

        private UserInfoEditModel GetUserInfoEditModel(AppUser user)
        {
            return new UserInfoEditModel
            {
                Name = user.UserName,
                Email = user.Email,
                Password = "",
                PasswordConfirm = "",
                Phone = user.PhoneNumber,
                CityID = user.CityID,
                CountryID = user.CountryID,
                ShippingAddress = user.ShippingAddress
            };
        }

        private bool CheckIfGoogleAccount()
        {
            try
            {
                ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
                if (ident.HasClaim(x => x.Type == ClaimTypes.Role && x.Value == "GoogleAccount"))
                    return true;
            }
            catch (NullReferenceException e)
            {
            }
            return false;
        }

        [Authorize]
        public ActionResult EditUser()
        {
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, CurrentUser.CityID, CurrentUser.CountryID);
            ViewBag.IsGoogleAccount = CheckIfGoogleAccount();
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(GetUserInfoEditModel(CurrentUser));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UserInfoEditModel userInfo)
        {
            userInfo.Name = CurrentUser.UserName;
            userInfo.Email = CurrentUser.Email;
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, userInfo.CityID, userInfo.CountryID);
            ViewBag.IsGoogleAccount = CheckIfGoogleAccount();
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (!ModelState.IsValid)
            {
                return View(userInfo);
            }
            // 確認phone有符合所訂要求
            IdentityResult validPhone = null;
            if (userInfo.Phone != string.Empty)
            {
                CurrentUser.PhoneNumber = userInfo.Phone; //因ValidateAsync只接受User物件，因此這邊直接更改user的屬性
                validPhone = await myUserManager.UserValidator_ValidateAsync(CurrentUser);
                if (!validPhone.Succeeded)
                {
                    AddErrorsFromResult(validPhone);
                }
            }
            // 確認密碼
            IdentityResult validPass = null;
            if (userInfo.Password != string.Empty && userInfo.Password != null)
            {
                validPass = await myUserManager.PasswordValidator_ValidateAsync(userInfo.Password);
                if (validPass.Succeeded)
                {
                    CurrentUser.PasswordHash = myUserManager.PasswordHasher_HashPassword(userInfo.Password);
                }
                else
                {
                    AddErrorsFromResult(validPass);
                }
            }
            if ((validPhone.Succeeded && validPass == null) ||
                (validPhone.Succeeded && userInfo.Password != string.Empty && validPass.Succeeded))
            {
                CurrentUser.CityID = userInfo.CityID;
                CurrentUser.CountryID = userInfo.CountryID;
                CurrentUser.ShippingAddress = userInfo.ShippingAddress;
                IdentityResult result = await myUserManager.UpdateAsync(CurrentUser);
                if (result.Succeeded)
                {
                    TempData["message"] = "修改資料成功！";
                    return RedirectToAction("EditUser");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(userInfo);
        }

        [Authorize]
        public ActionResult ViewOrder(int page = 1)
        {
            const int orderCountPerPage = 2;
            IEnumerable<ProductOrder> oriData = repository.ProductOrders
                .Where(p => p.AppUserID == CurrentUser.Id)
                .OrderByDescending(p => p.Date);
            int totalItem = oriData.Count();
            oriData = oriData
                .Skip((page - 1) * orderCountPerPage)
                .Take(orderCountPerPage);
            List<UserOrderViewModel> data = new List<UserOrderViewModel>(
                oriData.Select(p => new UserOrderViewModel
                {
                    Date = p.Date,
                    ProductList = p.GetDetailFromProductList(),
                    ShippingAddress = p.ShippingAddress,
                    ReceiverName = p.ReceiverName

                }));

            ViewBag.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = orderCountPerPage,
                TotalItems = totalItem
            };
            return View(data);
        }

        public ActionResult CreateUser()
        {
            if (IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            try
            {
                int defaultCityId = repository.Cities.OrderBy(c => c.ID).FirstOrDefault().ID;
                CityAndCountryPorvider.SetSelectListToViewBag(this, repository, defaultCityId);
                if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return View(new CreateAccountModel());
            }
            catch (NullReferenceException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                //page.310
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    CityID = model.CityID,
                    CountryID = model.CountryID,
                    ShippingAddress = model.ShippingAddress
                };
                IdentityResult result = await myUserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await UserManager.AddToRoleAsync(user.Id, "Users");  //新帳號預設Users權限
                    TempData["message"] = "創建帳號成功，請登入！";
                    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Home") });
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            CityAndCountryPorvider.SetSelectListToViewBag(this, repository, model.CityID, model.CountryID);
            if (!CityAndCountryPorvider.CheckIfSelectListOfViewBagCorrect(this))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return View(model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool IsAuthenticated
        {
            get { return myUserManager.IsAuthenticated; }
        }

        private AppUser CurrentUser
        {
            get { return myUserManager.CurrentUser; }
        }

        private AppUserManager UserManager
        {
            get { return myUserManager.UserManager; }
        }

        //private AppUser CurrentUser
        //{
        //    get
        //    {
        //        return UserManager.FindByName(HttpContext.User.Identity.Name);
        //    }
        //}

        //private AppUserManager UserManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
        //    }
        //}

        //[Authorize]
        //public ActionResult Test()
        //{
        //    return View(GetData("Test"));
        //}

        //[Authorize(Roles = "Users")]
        //public ActionResult OtherAction()
        //{
        //    return View("Test", GetData("OtherAction"));
        //}

        //[Authorize(Roles = "TaipeiStaff")]
        //public string AnotherAction()
        //{
        //    return "This is the protected action";
        //}

        //private Dictionary<string, object> GetData(string actionName)
        //{
        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    dict.Add("Action", actionName);
        //    dict.Add("User", HttpContext.User.Identity.Name);
        //    dict.Add("Authenticated", HttpContext.User.Identity.IsAuthenticated);
        //    dict.Add("Auth Type", HttpContext.User.Identity.AuthenticationType);
        //    dict.Add("In Users Role", HttpContext.User.IsInRole("Users"));
        //    return dict;
        //}

        //[Authorize]
        //public ActionResult UserProps()
        //{
        //    return View(CurrentUser);
        //}

        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult> UserProps()
        //{
        //    AppUser user = CurrentUser;
        //    await UserManager.UpdateAsync(user);
        //    return View(user);
        //}
    }
}