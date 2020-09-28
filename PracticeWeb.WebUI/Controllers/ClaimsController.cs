using PracticeWeb.WebUI.Infrastructure;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PracticeWeb.WebUI.Controllers
{
    public class ClaimsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            if (ident == null)
            {
                return View("Error", new string[] { "No claims available" });
            }
            else
            {
                return View(ident.Claims);
            }
        }

        //[ClaimsAccess(Issuer = "RemoteClaims", ClaimType = ClaimTypes.PostalCode, Value = "Taipei 100")]
        //public string OtherAction()
        //{
        //    return "This is the protected action";
        //}
    }
}