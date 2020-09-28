using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;

namespace PracticeWeb.WebUI.Infrastructure
{
    public static class CityAndCountryPorvider
    {
        public static SelectListItem[] GetCitySelectListItem(IEnumerable<City> cities, int selectedID = 0)
        {
            if (cities == null)
                return null;
            return cities
                .Select(c => new SelectListItem { Text = c.Name, Value = c.ID.ToString(), Selected = c.ID == selectedID })
                .ToArray();
        }

        public static SelectListItem[] GetCountrySelectListItem(IEnumerable<Country> countries, int selectedCountryID = 0)
        {
            if (countries == null)
                return null;
            return countries
                .Select(c => new SelectListItem { Text = c.Name, Value = c.ID.ToString(), Selected = c.ID == selectedCountryID })
                .ToArray();
        }

        public static void SetSelectListToViewBag(ControllerBase controller, IProductRepository repo,
            int selectedCityID = 0, int selectedCountryID = 0)
        {
            controller.ViewBag.CitySelectList = GetCitySelectListItem(
                repo.Cities,
                selectedCityID);
            controller.ViewBag.CountrySelectList = GetCountrySelectListItem(
                repo.Countries.Where(c => c.CityID == selectedCityID),
                selectedCountryID);
        }

        public static  bool CheckIfSelectListOfViewBagCorrect(ControllerBase controller)
        {
            if (controller.ViewBag.CitySelectList == null ||
                controller.ViewBag.CountrySelectList == null ||
                (controller.ViewBag.CitySelectList as SelectListItem[]).Length == 0 ||
                (controller.ViewBag.CountrySelectList as SelectListItem[]).Length == 0)
                return false;
            return true;
        }
    }
}