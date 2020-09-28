using System.Web.Mvc;
using Practice.Domain.Abstract;

namespace PracticeWeb.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category)
        {
            ViewBag.SelectedCategory = category;
            return PartialView(repository.Categories);
        }
    }
}