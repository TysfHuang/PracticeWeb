using System.Threading.Tasks;
using System.Web.Mvc;
using PracticeWeb.WebUI.Models;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;
using Practice.Domain.Concrete;
using PracticeWeb.WebUI.Infrastructure;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace PracticeWeb.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserManager myUserManager;

        public AccountController(IUserManager userManager)
        {
            this.myUserManager = userManager;
        }

        [AllowAnonymous]    //整個class都要求認證，但個別設定AllowAnonymous即可無認證請求
        public PartialViewResult BtnOnNavBar()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (IsAuthenticated)
            {
                string userName = CurrentUser.UserName;
                if (myUserManager.IsInRole(CurrentUser.Id, "Administrators"))
                    data.Add("AccountLevel", 0);    //登入為最高權限者
                else
                    data.Add("AccountLevel", 1);    //登入為一般使用者
                data.Add("UserName", userName);
                return PartialView("_BtnOnNavBar", data);
            }
            data.Add("AccountLevel", -1);   //沒登入
            return PartialView("_BtnOnNavBar", data);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (IsAuthenticated)  //若已登入，代表user沒有請求的action權限
            {
                return View("Error", new string[] { "Access Denied" }); //導向錯誤頁面
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await myUserManager.FindAsync(details.Name, details.Password);
                if (user == null || user.UserName == null)
                {
                    ViewBag.returnUrl = returnUrl;
                    ModelState.AddModelError("", "無效的帳號或密碼！");
                }
                else
                {
                    // 創建瀏覽器在後續請求中發送的cookie，以顯示它們已通過身份驗證。
                    ClaimsIdentity ident = await myUserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);  //識別user
                    //ident.AddClaims(LocationClaimsProvider.GetClaims(ident));
                    //ident.AddClaims(ClaimsRoles.CreateRolesFromClaims(ident));
                    AuthManager.SignOut();  //先作廢任何的認證cookie
                    AuthManager.SignIn(new AuthenticationProperties //再創建新的
                    {
                        IsPersistent = false    //若為true，則認證cookie會持續存在，即為user新開session時不必重新認證
                    }, ident);
                    return Redirect(returnUrl);
                }
            }
            return View(details);
        }

        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", new { returnUrl = returnUrl })
            };
            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            AppUser user = await UserManager.FindAsync(loginInfo.Login);    //檢查user 是否為第一次登入，若為是的話則回傳null
            if (user == null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                    CityID = 1,
                    CountryID = 1
                };  //為該user 創建一個新的資料存到DB
                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }

            ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                DefaultAuthenticationTypes.ApplicationCookie);
            //ident.AddClaims(loginInfo.ExternalIdentity.Claims); //複製由Google 提供的Claims
            ident.AddClaims(ClaimsRoles.CreateRoleForGoogle());
            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);
            return Redirect(returnUrl ?? "/");
        }

        private IAuthenticationManager AuthManager
        {
            get { return myUserManager.AuthManager; }
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
    }
}