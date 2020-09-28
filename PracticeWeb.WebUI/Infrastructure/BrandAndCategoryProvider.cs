using Practice.Domain.Abstract;
using Practice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeWeb.WebUI.Infrastructure
{
    public class BrandAndCategoryProvider
    {
        public static SelectListItem[] GetCategoryListItem(IEnumerable<Category> categories, int selectedID = 0)
        {
            if (categories == null)
                return null;
            return categories
                .Select(c => new SelectListItem { Text = c.Name, Value = c.ID.ToString(), Selected = c.ID == selectedID })
                .ToArray();
        }

        public static SelectListItem[] GetBrandListItem(IEnumerable<Brand> brands, int selectedID = 0)
        {
            if (brands == null)
                return null;
            return brands
                .Select(c => new SelectListItem { Text = c.Name, Value = c.ID.ToString(), Selected = c.ID == selectedID })
                .ToArray();
        }

        public static void SetSelectListToViewBag(ControllerBase controller, IProductRepository repo,
            int selectedCategoryID = 0, int selectedBrandID = 0)
        {
            controller.ViewBag.CategoryList = GetCategoryListItem(
                repo.Categories,
                selectedCategoryID);
            controller.ViewBag.BrandList = GetBrandListItem(
                repo.Brands,
                selectedBrandID);
        }

        public static bool CheckIfSelectListOfViewBagCorrect(ControllerBase controller)
        {
            if (controller.ViewBag.CategoryList == null ||
                controller.ViewBag.BrandList == null ||
                (controller.ViewBag.CategoryList as SelectListItem[]).Length == 0 ||
                (controller.ViewBag.BrandList as SelectListItem[]).Length == 0)
                return false;
            return true;
        }
    }
}